using FluentValidation;

namespace Domain.Model
{
    public class UpdateCardRequestValidator : AbstractValidator<UpdateCardRequest>
    {
        public UpdateCardRequestValidator()
        {
            RuleFor(s => s.Consumer_id)
              .NotNull().WithMessage("Id do consumidor é obrigatório.")
              .NotEmpty().WithMessage("Id do consumidor  é obrigatório.");

            RuleFor(s => s.Number)
              .NotNull().WithMessage("Número do cartão é obrigatório")
              .NotEmpty().WithMessage("Número do cartão é obrigatório");

            RuleFor(s => s.Validity)
              .NotNull().WithMessage("Validade é obrigatório.")
              .NotEmpty().WithMessage("Validade é obrigatório.");

            RuleFor(s => s.Document)
              .NotNull().WithMessage("Documento é obrigatório.")
              .NotEmpty().WithMessage("Documento do cartão é obrigatório.");

            RuleFor(s => s.Name)
              .NotNull().WithMessage("Nome é obrigatório.")
              .NotEmpty().WithMessage("Nome do cartão é obrigatório.");

        }
    }
}
