using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ProtectiveWearProductsApi.Models;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace ProtectiveWearProductsApi.Services
{
    public class ProductService
    {
        private readonly IMongoCollection<Product> _products;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setting"></param>
        public ProductService(IProductsDatabaseSettings setting)
        {
            var client = new MongoClient(setting.ConnectionString);
            var database = client.GetDatabase(setting.DatabaseName);
            _products = database.GetCollection<Product>(setting.ProductsCollectionName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Product> Get()
        {
            return _products.Find(prod => true).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Product>> GetAsync()
        {
            return await _products.FindAsync(prod => true).Result.ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Product> GetAsync(string id)
        {
            return await _products.FindAsync(prod => prod.Id == id).Result.FirstOrDefaultAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prod"></param>
        /// <returns></returns>
        public async Task<Product> CreateAsync(Product prod)
        {
           await _products.InsertOneAsync(prod);
            return prod;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prod"></param>
        /// <returns></returns>
        public Product Create(Product prod)
        {
             _products.InsertOne(prod);
            return prod;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="prodIn"></param>
        public  void Update(string id, Product prodIn)
        {
             _products.ReplaceOneAsync(car => car.Id == id, prodIn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="prodIn"></param>
        public async Task UpdateAsync(string id, Product prodIn)
        {
           await _products.ReplaceOneAsync(car => car.Id == id, prodIn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodIn"></param>
        /// <returns></returns>
        public async Task RemoveAsync(Product prodIn)
        {
           await _products.DeleteOneAsync(prod => prod.Id == prodIn.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodIn"></param>
        /// <returns></returns>
        public void Remove(Product prodIn)
        {
             _products.DeleteOneAsync(prod => prod.Id == prodIn.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task RemoveAsync(string id)
        {
           await _products.DeleteOneAsync(prod => prod.Id == id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void Remove(string id)
        {
             _products.DeleteOneAsync(prod => prod.Id == id);
        }
    }
}
