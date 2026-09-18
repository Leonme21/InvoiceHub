using InvoiceHub.Application.Dtos.Client;
using InvoiceHub.Application.Dtos.Common;
using InvoiceHub.Application.Dtos.Invoice;
using InvoiceHub.Application.Interfaces;
using InvoiceHub.Application.Mappings;
using InvoiceHub.Domain.Interfaces;

namespace InvoiceHub.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ClientService(
            IClientRepository clientRepository,
            IUnitOfWork unitOfWork)
        {
            _clientRepository = clientRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ClientDetailResponseDto> CreateAsync(CreateClientDto dto)
        {
            var existingClient =
                await _clientRepository.GetByDocumentAsync(dto.Document);

            if (existingClient != null)
            {
                throw new ArgumentException(
                    "El documento ya se encuentra registrado en el sistema.");
            }

            var newClient = dto.ToEntity();

            _clientRepository.Add(newClient);

            await _unitOfWork.SaveChangesAsync();

            return newClient.ToDetailDto();
        }

        public async Task<IEnumerable<ClientListResponseDto>> GetAllAsync()
        {
            var clients = await _clientRepository.GetAllAsync();

            return clients.Select(x => x.ToListDto());
        }

        public async Task<ClientDetailResponseDto?> GetByDocumentAsync(
            string document)
        {
            var client =
                await _clientRepository.GetByDocumentAsync(document);

            return client?.ToDetailDto();
        }

        public async Task<ClientDetailResponseDto?> GetByIdAsync(int id)
        {
            var client =
                await _clientRepository.GetByIdAsync(id);

            return client?.ToDetailDto();
        }

        public async Task<ClientDetailResponseDto> UpdateAsync(
            int id,
            UpdateClientDto dto)
        {
            var existingClient =
                await _clientRepository.GetByIdAsync(id);

            if (existingClient is null)
            {
                throw new ArgumentException(
                    "El cliente no existe.");
            }

            dto.UpdateEntity(existingClient);

            _clientRepository.Update(existingClient);

            await _unitOfWork.SaveChangesAsync();

            return existingClient.ToDetailDto();
        }

        public async Task<PagedResult<ClientListResponseDto>> GetPagedAsync(
            string? search,
            int page,
            int pageSize,
            bool? isActive)
        {
            var (items, totalCount) =
                await _clientRepository.GetPagedAsync(
                    search,
                    page,
                    pageSize,
                    isActive);

            return new PagedResult<ClientListResponseDto>
            {
                Items = items
                    .Select(c => c.ToListDto())
                    .ToList(),

                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize
            };
        }

        public async Task ChangeStatusAsync(
            int id,
            bool isActive)
        {
            var updated =
                await _clientRepository.ChangeStatusAsync(
                    id,
                    isActive);

            if (!updated)
            {
                throw new ArgumentException(
                    "El cliente no existe.");
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await ChangeStatusAsync(id, false);
        }
    }
}