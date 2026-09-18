using FluentValidation;
using InvoiceHub.Application.Dtos.Invoice;

namespace InvoiceHub.Application.Validators.Invoice
{
    public class InvoiceRequestDtoValidator :AbstractValidator<InvoiceRequestDto>
    {
        private readonly IValidator<InvoiceDetailRequestDto> _detailValidator;

        public InvoiceRequestDtoValidator(IValidator<InvoiceDetailRequestDto> detailValidator)
        {
            _detailValidator = detailValidator;

            RuleFor(x => x.ClientId)
                .GreaterThan(0).WithMessage("Debe seleccionar un cliente válido.");

            RuleFor(x => x.Observations)
                .MaximumLength(500).WithMessage("Las observaciones no pueden exceder los 500 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Observations));
            
            RuleFor(x => x.Details)
                .NotEmpty().WithMessage("La factura debe contener al menos un producto.");

            RuleForEach(x => x.Details)
                .SetValidator(detailValidator);
        }
    }
}
