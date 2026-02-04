using PlayProjectify.ProductService.Models.DTO;
using PlayProjectify.ServiceDefaults;

namespace PlayProjectify.ProductService.Services;

public interface IProductService
{
    ProjectifyServiceResult<IEnumerable<ProductDto>> GetAll();
    ProjectifyServiceResult<ProductDto> Get(Guid id);
    ProjectifyServiceResult<ProductDto> Add(AddProductDto product);
    bool Update(UpdateProductDto product);
    bool Delete(Guid id);
}
