using PlayProjectify.ProductService.Models.DTO;
using PlayProjectify.ServiceDefaults;

namespace PlayProjectify.ProductService.Services;

public interface IProductService
{
    Task<ProjectifyServiceResult<IEnumerable<GetProductDto>>> GetAll(CancellationToken cancellationToken = default);
    Task<ProjectifyServiceResult<GetProductDto>> Get(Guid id, CancellationToken cancellationToken = default);
    Task<ProjectifyServiceResult<ProductDto>> Add(AddProductDto product, CancellationToken cancellationToken = default);
    Task<bool> Update(UpdateProductDto product, CancellationToken cancellationToken = default);
    Task<bool> Delete(Guid id, CancellationToken cancellationToken = default);
}


public interface ICategoryService
{
    Task<ProjectifyServiceResult<IEnumerable<CategoryDto>>> GetAll(CancellationToken cancellationToken = default);
    Task<ProjectifyServiceResult<CategoryDto>> Get(Guid id, CancellationToken cancellationToken = default);
    Task<ProjectifyServiceResult<CategoryDto>> Add(AddCategoryDto category, CancellationToken cancellationToken = default);
    Task<bool> Update(UpdateCategoryDto category, CancellationToken cancellationToken = default);
    Task<bool> Delete(Guid id, CancellationToken cancellationToken = default);
}