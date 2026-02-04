using FluentValidation;

namespace PlayProjectify.ProductService.Models.DTO;

public interface IProductDto
{
    string ProductName { get; }
    string ProductDescription { get; }
    decimal ProductPrice { get; }
}
public record ProductDto(Guid ProductId, string ProductName, string ProductDescription, decimal ProductPrice);

public sealed record AddProductDto(string ProductName, string ProductDescription, decimal ProductPrice) : IProductDto;

public record UpdateProductDto(Guid ProductId, string ProductName, string ProductDescription, decimal ProductPrice) : IProductDto;

public sealed class AddProductDtoValidator : ProductBaseValidator<AddProductDto> { }

public sealed class UpdateProductDtoValidator : ProductBaseValidator<UpdateProductDto>
{
    public UpdateProductDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product Id is required.");
    }
}

public abstract class ProductBaseValidator<T> : AbstractValidator<T> where T : IProductDto
{
    protected ProductBaseValidator()
    {
        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product Name is required.")
            .MaximumLength(100).WithMessage("Product Name maximum length is 100.");

        RuleFor(x => x.ProductDescription)
            .NotEmpty().WithMessage("Product Description is required.");

        RuleFor(x => x.ProductPrice)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Product Price must be a positive value.");
    }
}