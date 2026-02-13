using FluentValidation;

namespace PlayProjectify.ProductService.Models.DTO;

public interface IProductDto
{
    string ProductName { get; }
    string ProductDescription { get; }
    decimal ProductPrice { get; }
}
public sealed record ProductDto(Guid ProductId, string ProductName, string ProductDescription, decimal ProductPrice, int ProductQuantity, Guid CategoryId);
public sealed record GetProductDto(Guid ProductId, string ProductName, string ProductDescription, decimal ProductPrice, int ProductQuantity, Guid CategoryId, string CategoryName);

public sealed record AddProductDto(string ProductName, string ProductDescription, decimal ProductPrice, int ProductQuantity, Guid CategoryId) : IProductDto;

public sealed record UpdateProductDto(Guid ProductId, string ProductName, string ProductDescription, decimal ProductPrice, int ProductQuantity, Guid CategoryId) : IProductDto;

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