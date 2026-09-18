using FluentValidation;
using InvoiceHub.Application.Dtos.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceHub.Application.Validators.Invoice
{
    public class InvoiceDetailRequestDtoValidator : AbstractValidator<InvoiceDetailRequestDto>
    {
        public InvoiceDetailRequestDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Debe seleccionar un producto valido");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("La cantidad a comprar debe ser mayor a cero.");

        }
    }
}
