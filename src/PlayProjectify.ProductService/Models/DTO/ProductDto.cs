using FluentValidation;

namespace PlayProjectify.ProductService.Models.DTO;

public record ProductDto(Guid ProductId, string ProductName, string ProductDescription, decimal ProductPrice);

public sealed record AddProductDto(string ProductName, string ProductDescription, decimal ProductPrice);

public record UpdateProductDto(Guid ProductId, string ProductName, string ProductDescription, decimal ProductPrice);

public sealed class AddProductDtoValidator : AbstractValidator<AddProductDto>
{
    public AddProductDtoValidator()
    {
        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product Name is required.")
            .MaximumLength(100).WithMessage("Code maximum length is 100.");

        RuleFor(x => x.ProductDescription)
            .NotEmpty().WithMessage("Product Name is required.");

        RuleFor(x => x.ProductPrice)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Discount must be a positive value."); ;
    }
}

public sealed class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .Sh
            .NotEmpty().WithMessage("Product Id is required.");
        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product Name is required.")
            .MaximumLength(100).WithMessage("Code maximum length is 100.");

        RuleFor(x => x.ProductDescription)
            .NotEmpty().WithMessage("Product Name is required.");

        RuleFor(x => x.ProductPrice)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Discount must be a positive value."); ;
    }
}