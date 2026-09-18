using FluentValidation;
using InvoiceHub.Application.Dtos.Client;

namespace InvoiceHub.Application.Validators.Client
{
    public class UpdateClientDtoValidator : AbstractValidator<UpdateClientDto>
    {
        public UpdateClientDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("El nombre del cliente es obligatorio.")
                .MinimumLength(3)
                    .WithMessage("El nombre del cliente debe tener al menos 3 caracteres.")
                .MaximumLength(100)
                    .WithMessage("El nombre del cliente no debe superar los 100 caracteres.");

            RuleFor(x => x.Email)
                .MaximumLength(150)
                    .WithMessage("El correo no puede exceder los 150 caracteres.")
                .EmailAddress()
                    .WithMessage("El formato del correo electrónico no es válido.")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.Phone)
                .GreaterThanOrEqualTo(0)
                    .WithMessage("El teléfono no puede ser negativo.")
                .LessThanOrEqualTo(9999999999)
                    .WithMessage("El teléfono no puede tener más de 10 dígitos.")
                .When(x => x.Phone.HasValue);

            RuleFor(x => x.Address)
                .MaximumLength(250)
                    .WithMessage("La dirección es demasiado larga (máximo 250 caracteres).")
                .When(x => !string.IsNullOrWhiteSpace(x.Address));
        }
    }
}