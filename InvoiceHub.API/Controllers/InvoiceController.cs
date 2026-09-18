using FluentValidation;
using InvoiceHub.Application.Dtos.Common;
using InvoiceHub.Application.Dtos.Invoice;
using InvoiceHub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IValidator<InvoiceRequestDto> _validator;

        public InvoiceController(
            IInvoiceService invoiceService,
            IValidator<InvoiceRequestDto> validator)
        {
            _invoiceService = invoiceService;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<InvoiceListResponseDto>>> GetPaged(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _invoiceService.GetPagedAsync(
                search,
                page,
                pageSize);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceResponseDto>> GetById(int id)
        {
            var invoice = await _invoiceService.GetByIdAsync(id);

            if (invoice is null)
            {
                return NotFound();
            }

            return Ok(invoice);
        }

        [HttpGet("client/{clientId}")]
        public async Task<ActionResult<IEnumerable<InvoiceListResponseDto>>> GetByClientId(
            int clientId)
        {
            var invoices =
                await _invoiceService.GetByClientIdAsync(clientId);

            return Ok(invoices);
        }

        [HttpPost]
        public async Task<ActionResult<InvoiceResponseDto>> Create(
            InvoiceRequestDto dto)
        {
            var validationResult =
                await _validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var createdInvoice =
                await _invoiceService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdInvoice.Id },
                createdInvoice);
        }
    }
}