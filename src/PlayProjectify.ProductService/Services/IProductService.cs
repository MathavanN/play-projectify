using PlayProjectify.ProductService.Models.DTO;
using PlayProjectify.ServiceDefaults;

namespace PlayProjectify.ProductService.Services;

public interface IProductService
{
    Task<ProjectifyServiceResult<IEnumerable<GetProductDto>>> GetAll();
    Task<ProjectifyServiceResult<GetProductDto>> Get(Guid id);
    Task<ProjectifyServiceResult<ProductDto>> Add(AddProductDto product);
    Task<bool> Update(UpdateProductDto product);
    Task<bool> Delete(Guid id);
}


public interface ICategoryService
{
    Task<ProjectifyServiceResult<IEnumerable<CategoryDto>>> GetAll();
    Task<ProjectifyServiceResult<CategoryDto>> Get(Guid id);
    Task<ProjectifyServiceResult<CategoryDto>> Add(AddCategoryDto category);
    Task<bool> Update(UpdateCategoryDto category);
    Task<bool> Delete(Guid id);
}