using InvoiceHub.Application.Dtos.Common;
using InvoiceHub.Application.Dtos.Invoice;
using InvoiceHub.Application.Interfaces;
using InvoiceHub.Application.Mappings;
using InvoiceHub.Domain.Entities;
using InvoiceHub.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace InvoiceHub.Application.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IConfiguration _configuration;

        public InvoiceService(IUnitOfWork unitOfWork, IInvoiceRepository invoiceRepository,
            IProductRepository productRepository, IClientRepository clientRepository, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _invoiceRepository = invoiceRepository;
            _productRepository = productRepository;
            _clientRepository = clientRepository;
            _configuration = configuration;
        }

        public async Task<InvoiceResponseDto> CreateAsync(InvoiceRequestDto dto)
        {
            var client = await _clientRepository.GetByIdAsync(dto.ClientId);

            if (client is null) throw new ArgumentException($"El cliente con ID {dto.ClientId} no existe.");
            if (!client.IsActive) throw new ArgumentException($"El cliente con ID {dto.ClientId} está inactivo.");
            
            var newInvoice = dto.ToEntityInvoice();
            newInvoice.Client = client;
            newInvoice.InvoiceNumber = $"INV-{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";

            decimal accumulatedSubtotal = 0;
            
            foreach (var detail in newInvoice.InvoiceDetails)
            {
                var product = await _productRepository.GetByIdAsync(detail.ProductId);

                if (product is null) throw new ArgumentException($"El producto {detail.ProductId} no existe.");
                if (!product.IsActive) throw new ArgumentException($"El producto {detail.ProductId} está inactivo.");

                if (product.Stock < detail.Quantity) throw new ArgumentException("Stock insuficiente");

                detail.Product = product;

                product.Stock -= detail.Quantity;

                _productRepository.Update(product);

                detail.UnitPrice = product.UnitPrice;
                detail.LineTotal = detail.Quantity * detail.UnitPrice;

                accumulatedSubtotal += detail.LineTotal;
            }
            newInvoice.SubTotal = accumulatedSubtotal;
            newInvoice.TaxPercentage = _configuration.GetValue<decimal>("TaxSettings:DefaultPercentage", 0.19m);


            newInvoice.TaxAmount = newInvoice.TaxPercentage * newInvoice.SubTotal;

            newInvoice.Total = newInvoice.SubTotal + newInvoice.TaxAmount;
                               
            _invoiceRepository.Add(newInvoice);

            await _unitOfWork.SaveChangesAsync();
            return newInvoice.ToDto();

        }

        public async Task<IEnumerable<InvoiceListResponseDto>> GetAllAsync()
        {
            var invoice = await _invoiceRepository.GetAllAsync();
            return invoice.Select(x => x.ToListDto());
        }

        public async Task<IEnumerable<InvoiceListResponseDto>> GetByClientIdAsync(
         int clientId)
        {
            var invoices =
                await _invoiceRepository.GetByClientIdAsync(clientId);

            return invoices
                .Select(x => x.ToListDto())
                .ToList();
        }

        public async Task<InvoiceResponseDto?> GetByIdAsync(int id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            return invoice?.ToDto();
        }

        public async Task<PagedResult<InvoiceListResponseDto>> GetPagedAsync(string? searchTerm, int page, int pageSize)
        {
            var (items, totalCount) = await _invoiceRepository.GetPagedAsync(searchTerm, page, pageSize);

            var dtos = items.Select(i => new InvoiceListResponseDto
            {
                Id = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                ClientName = i.Client?.Name ?? "N/A",
                Total = i.Total
            }).ToList();

            return new PagedResult<InvoiceListResponseDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize
            };
        }

    }
}
