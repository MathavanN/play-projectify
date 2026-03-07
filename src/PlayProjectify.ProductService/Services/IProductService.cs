using PlayProjectify.ProductService.Models.DTO;
using PlayProjectify.ServiceDefaults;

namespace PlayProjectify.ProductService.Services;

public interface IProductService
{
    Task<ProjectifyServiceResult<IEnumerable<GetProductDto>>> GetAll(CancellationToken cancellationToken);
    Task<ProjectifyServiceResult<GetProductDto>> Get(Guid id, CancellationToken cancellationToken);
    Task<ProjectifyServiceResult<ProductLookupDto>> GetByIds(IEnumerable<Guid> productIds, CancellationToken cancellationToken);
    Task<ProjectifyServiceResult<ProductDto>> Add(AddProductDto product, CancellationToken cancellationToken);
    Task<bool> Update(UpdateProductDto product, CancellationToken cancellationToken);
    Task<bool> Delete(Guid id, CancellationToken cancellationToken);
    Task<ProjectifyServiceResult> ReserveStock(ProductInventoryDto items, CancellationToken cancellationToken);
    Task<ProjectifyServiceResult> ReleaseStock(ProductInventoryDto items, CancellationToken cancellationToken);
}


public interface ICategoryService
{
    Task<ProjectifyServiceResult<IEnumerable<CategoryDto>>> GetAll(CancellationToken cancellationToken);
    Task<ProjectifyServiceResult<CategoryDto>> Get(Guid id, CancellationToken cancellationToken);
    Task<ProjectifyServiceResult<CategoryDto>> Add(AddCategoryDto category, CancellationToken cancellationToken);
    Task<bool> Update(UpdateCategoryDto category, CancellationToken cancellationToken);
    Task<bool> Delete(Guid id, CancellationToken cancellationToken);
}