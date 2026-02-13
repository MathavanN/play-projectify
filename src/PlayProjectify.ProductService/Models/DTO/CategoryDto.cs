namespace PlayProjectify.ProductService.Models.DTO;

public interface ICategoryDto
{
    string CategoryName { get; }
    string CategoryDescription { get; }
}

public sealed record CategoryDto(Guid CategoryId, string CategoryName, string CategoryDescription);

public sealed record AddCategoryDto(string CategoryName, string CategoryDescription) : ICategoryDto;

public sealed record UpdateCategoryDto(Guid CategoryId, string CategoryName, string CategoryDescription) : ICategoryDto;