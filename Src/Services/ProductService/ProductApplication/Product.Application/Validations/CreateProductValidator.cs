using FluentValidation;

namespace Product.Application.Validations;
public class CreateProductValidator : AbstractValidator<Domain.Entities.Product>
{
    public CreateProductValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .NotNull()
                .WithMessage("ürün adını boş geçmeyiniz.")
            .MaximumLength(150)
            .MinimumLength(5)
                .WithMessage("ürün adını 5 ile 150 karakter arasında giriniz.");

        RuleFor(p => p.Price)
            .NotEmpty()
            .NotNull()
                .WithMessage("fiyat bilgisini boş geçmeyiniz.")
            .Must(s => s >= 0)
                .WithMessage("bilgisi eksi tutar olamaz!");
    }
}
