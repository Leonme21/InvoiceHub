using FluentValidation;
using InvoiceHub.Application.Dtos.Common;
using InvoiceHub.Application.Dtos.Product;
using InvoiceHub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IValidator<CreateProductDto> _createValidator;
        private readonly IValidator<UpdateProductDto> _updateValidator;

        public ProductController(
            IProductService productService,
            IValidator<CreateProductDto> createValidator,
            IValidator<UpdateProductDto> updateValidator)
        {
            _productService = productService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<ProductListResponseDto>>> GetPaged(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool? isActive = null)
        {
            var result =
                await _productService.GetPagedAsync(
                    search,
                    page,
                    pageSize,
                    isActive);

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDetailResponseDto>> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product is null) return NotFound();
            return Ok(product);
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<ProductDetailResponseDto>> GetByCode(string code)
        {
            var product = await _productService.GetByCodeAsync(code);
            if (product is null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDetailResponseDto>> Create(CreateProductDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var createdProduct = await _productService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDetailResponseDto>> Update(int id, [FromBody] UpdateProductDto dto)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var updatedProduct = await _productService.UpdateAsync(id, dto);
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(
            int id,
            [FromBody] ChangeProductStatusDto dto)
        {
            await _productService.ChangeStatusAsync(
                id,
                dto.IsActive);

            return NoContent();
        }
    }
}
