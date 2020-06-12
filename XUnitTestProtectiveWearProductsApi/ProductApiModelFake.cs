using ProtectiveWearProductsApi.Interfaces;
using ProtectiveWearProductsApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XUnitTestProtectiveWearProductsApi
{
    public class ProductApiModelFake : IProductService
    {
        private readonly List<Product> productApis;

        public ProductApiModelFake()
        {
            productApis = new List<Product>
            {
               new Product{
                            Id = Guid.NewGuid().ToString(),
                            Nombre = "Orange Juice",
                            Descripcion ="Just for test Orange Juice",
                            Presentacion ="box x 12 units",
                            Precio = 22222
               },
                new Product{
                            Id = Guid.NewGuid().ToString(),
                            Nombre = "Diary Milk",
                            Descripcion ="Just for test Diary Milk",
                            Presentacion ="box x 24 units",
                            Precio = 234567
               }

            };
        }

        public Product Create(Product model)
        {
            model.Id = Guid.NewGuid().ToString();
            productApis.Add(model);
            return model;
        }

        public async Task<Product> CreateAsync(Product model)
        {
            model.Id = Guid.NewGuid().ToString();
            productApis.Add(model);
            return model;
        }

        public List<ProductApi> Get()
        {
            throw new NotImplementedException();
        }

        public ProductApi Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductApi>> GetAsync()
        {
            var result = productApis
                .Select(x => new ProductApi
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    Presentacion = x.Presentacion,
                    Precio = x.Precio
                }

                ).ToList();
            return result;
        }

        public async Task<ProductApi> GetAsync(string id)
        {
            var result = productApis
                .Where(x => x.Id == id)
                .Select(p => new ProductApi
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Presentacion = p.Presentacion,
                    Precio = p.Precio
                }
                ).FirstOrDefault();
            return result;
        }

        public void Remove(string id)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAsync(string id)
        {
            var result = productApis.FirstOrDefault(p => p.Id == id);
            productApis.Remove(result);
        }

        public void Update(string id, Product model)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(string id, Product model)
        {
            throw new NotImplementedException();
        }
    }
}
