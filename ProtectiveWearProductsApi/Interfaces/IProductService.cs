using ProtectiveWearProductsApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProtectiveWearProductsApi.Interfaces
{
    public interface IProductService
    {
        public List<ProductApi> Get();
        public Product Get(string id);
        public Product Create(Product model);
        public void Update(string id, Product model);
        public void Remove(string id);


        public Task<List<ProductApi>> GetAsync();
        public Task<Product> GetAsync(string id);
        public Task<Product> CreateAsync(Product model);
        public Task UpdateAsync(string id, Product model);
        public Task RemoveAsync(string id);
    }
}
