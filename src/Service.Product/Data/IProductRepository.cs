using PlayProjectify.Core.ApiResult;
using Service.Product.Controllers;

namespace Service.Product.Data
{
    public interface IProductRepository
    {
        ApiResult<IEnumerable<Product>> GetAll();
        ApiResult<Product> Get(int id);
        Product Add(Product product);
        bool Update(Product product);
        bool Delete(int id);
    }
}
