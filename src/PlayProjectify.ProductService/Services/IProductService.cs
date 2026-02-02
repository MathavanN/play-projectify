using PlayProjectify.ProductService.Models.DTO;

namespace PlayProjectify.ProductService.Services;

public interface IProductService
{
    IEnumerable<ProductDto> GetAll();
    ProductDto? Get(Guid id);
    ProductDto Add(AddProductDto product);
    bool Update(UpdateProductDto product);
    bool Delete(Guid id);
}
