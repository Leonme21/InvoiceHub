using FluentValidation;
using InvoiceHub.Application.Dtos.Client;
using InvoiceHub.Application.Dtos.Common;
using InvoiceHub.Application.Interfaces;
using InvoiceHub.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IValidator<CreateClientDto> _createValidator;
        private readonly IValidator<UpdateClientDto> _updateValidator;

        public ClientController(
            IClientService clientService,
            IValidator<CreateClientDto> createValidator,
            IValidator<UpdateClientDto> updateValidator)
        {
            _clientService = clientService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }


        [HttpGet]
        public async Task<ActionResult<PagedResult<ClientListResponseDto>>> Get(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool? isActive = null)
        {
            var result = await _clientService.GetPagedAsync(
                search,
                page,
                pageSize,
                isActive);

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDetailResponseDto>> GetById(int id)
        {
            var client = await _clientService.GetByIdAsync(id);

            if (client is null)
            {
                return NotFound();
            }

            return Ok(client);
        }


        [HttpGet("document/{document}")]
        public async Task<ActionResult<ClientDetailResponseDto>> GetByDocument(
            string document)
        {
            var client =
                await _clientService.GetByDocumentAsync(document);

            if (client is null)
            {
                return NotFound();
            }

            return Ok(client);
        }


        [HttpPost]
        public async Task<ActionResult<ClientDetailResponseDto>> Create(
            CreateClientDto dto)
        {
            var validationResult =
                await _createValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var createdClient =
                await _clientService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdClient.Id },
                createdClient);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<ClientDetailResponseDto>> Update(
            int id,
            [FromBody] UpdateClientDto dto)
        {
            var validationResult =
                await _updateValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var updatedClient =
                await _clientService.UpdateAsync(id, dto);

            return Ok(updatedClient);
        }


        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(
            int id,
            [FromBody] ChangeClientStatusDto dto)
        {
            await _clientService.ChangeStatusAsync(
                id,
                dto.IsActive);

            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _clientService.DeleteAsync(id);

            return NoContent();
        }
    }
}