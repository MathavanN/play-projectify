namespace PlayProjectify.ProductService.Models.DTOs;

public record ProductDto(Guid ProductId, string ProductName, string ProductDescription, decimal ProductPrice);

public record AddProductDto(string ProductName, string ProductDescription, decimal ProductPrice);

public record UpdateProductDto(Guid ProductId, string ProductName, string ProductDescription, decimal ProductPrice);