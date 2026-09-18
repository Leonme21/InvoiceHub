using FluentValidation;
using InvoiceHub.Application.Dtos.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceHub.Application.Validators.Product
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("El código del producto es obligatorio.")
                .MaximumLength(50).WithMessage("El código no puede exceder los 50 caracteres.");
            
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del producto es obligatorio.")
                .MaximumLength(150).WithMessage("El nombre no puede exceder los 150 caracteres.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("La descripción es demasiado larga (máximo 500 caracteres).")
                .When(x => !string.IsNullOrEmpty(x.Description)); 

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("El precio unitario debe ser mayor a cero.");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo.");
        }
    }
}
