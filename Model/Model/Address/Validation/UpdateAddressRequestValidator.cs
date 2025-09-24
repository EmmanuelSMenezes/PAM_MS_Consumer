using FluentValidation;

namespace Domain.Model
{
  public class UpdateAddressRequestValidator : AbstractValidator<UpdateAddressRequest>
  {
    public UpdateAddressRequestValidator()
    {
      RuleFor(s => s.Street)
        .NotNull().WithMessage("Logradouro é obrigatório.")
        .NotEmpty().WithMessage("Logradouro é obrigatório.");
      
      RuleFor(s => s.Number)
        .NotNull().WithMessage("Numero do imóvel é obrigatório.")
        .NotEmpty().WithMessage("Numero do imóvel é obrigatório.");

      RuleFor(s => s.District)
        .NotNull().WithMessage("Bairro é obrigatório.")
        .NotEmpty().WithMessage("Bairro é obrigatório.");

      RuleFor(s => s.City)
        .NotNull().WithMessage("Cidade é obrigatório.")
        .NotEmpty().WithMessage("Cidade é obrigatório.");

      RuleFor(s => s.State)
        .NotNull().WithMessage("Estado é obrigatório.")
        .NotEmpty().WithMessage("Estado é obrigatório.");

      RuleFor(s => s.Zip_code)
      .NotNull().WithMessage("CEP é obrigatório.")
      .NotEmpty().WithMessage("CEP é obrigatório.");

      RuleFor(s => s.Latitude)
      .NotNull().WithMessage("Latitude é obrigatório.")
      .NotEmpty().WithMessage("Latitude é obrigatório.");

      RuleFor(s => s.Longitude)
      .NotNull().WithMessage("Longitude é obrigatório.")
      .NotEmpty().WithMessage("Longitude é obrigatório.");

      RuleFor(s => s.Consumer_id)
        .NotNull().WithMessage("Id do Consumidor é obrigatório.")
        .NotEmpty().WithMessage("Id do Consumidor é obrigatório.");
      
      RuleFor(s => s.Address_id)
        .NotNull().WithMessage("Id do Endereço é obrigatório.")
        .NotEmpty().WithMessage("Id do Endereço é obrigatório.");

    }
  }
}
