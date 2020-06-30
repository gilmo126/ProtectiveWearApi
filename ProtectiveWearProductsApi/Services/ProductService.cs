using System.Collections.Generic;
using MongoDB.Driver;
using ProtectiveWearProductsApi.Models;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using ProtectiveWearProductsApi.Exceptions;
using ProtectiveWearProductsApi.Interfaces;
using System.Net;

namespace ProtectiveWearProductsApi.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IMongoDatabase _productsDB;

        

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="setting">Toma una extesion de los parametros configurables de la cadena de conexion</param>
        public ProductService(IProductsDatabaseSettings setting)
        {
            var client = new MongoClient(setting.ConnectionString);
            if (client != null)
            {
                _productsDB = client.GetDatabase(setting.DatabaseName);
            }
        }

        /// <summary>
        /// propiedad de tipo IMongoCollection, para tomar la coleccion a mostrar
        /// </summary>
        public IMongoCollection<Product> Products
        {
            get
            {
                return _productsDB.GetCollection<Product>("Product");
            }
        }
        /// <summary>
        /// Proceso que consulta una lista de productos, de forma síncrono.
        /// </summary>
        /// <returns>Retorna una lista de objetos de tipo producto</returns>
        public List<ProductApi> Get()
        {
            return Products
                  .AsQueryable()
                  .Select(x => new ProductApi
                  {
                      Id = x.Id,
                      Nombre = x.Nombre,
                      Presentacion = x.Presentacion,
                      Descripcion = x.Descripcion,
                      Precio = x.Precio,
                      FechaCreacion = x.FechaCreacion

                  })
                  .ToList();
        }

        /// <summary>
        /// Proceso que consulta una lista de productos, de forma asíncrono.
        /// </summary>
        /// <returns>Retorna una lista de objetos de tipo producto.</returns>
        public async Task<List<ProductApi>> GetAsync()
        {
            return await Products
                  .AsQueryable()
                  .Select(x => new ProductApi
                  {
                      Id = x.Id,
                      Nombre = x.Nombre,
                      Presentacion = x.Presentacion,
                      Descripcion = x.Descripcion,
                      Precio = x.Precio,
                      FechaCreacion = x.FechaCreacion

                  })
                  .ToListAsync();
        }

        /// <summary>
        /// Proceso para consultar el detalle de un producto, de forma asíncrono.
        /// </summary>
        /// <param name="id">Identificación de un producto</param>
        /// <returns>Retorna la información de tallada de un producto</returns>
        public async Task<ProductApi> GetAsync(string id)
        {
            var result = await Products
                        .AsQueryable()
                        .Where(prod => prod.Id == id)
                        .Select(p => new ProductApi
                        {
                            Id = p.Id,
                            Nombre = p.Nombre,
                            Presentacion = p.Presentacion,
                            Descripcion = p.Descripcion,
                            Precio = p.Precio,
                            FechaCreacion = p.FechaCreacion
                        })
                       .FirstOrDefaultAsync();

            result.CheckIsNotNull();

            return result;
        }

        /// <summary>
        /// Proceso para consultar el detalle de un producto, de forma síncrono.
        /// </summary>
        /// <param name="id">Identificación de un producto</param>
        /// <returns>Retorna la información de tallada de un producto</returns>
        public ProductApi Get(string id)
        {
            var result = Products
                      .AsQueryable()
                      .Where(prod => prod.Id == id)
                      .Select(p => new ProductApi
                      {
                          Id = p.Id,
                          Nombre = p.Nombre,
                          Presentacion = p.Presentacion,
                          Descripcion = p.Descripcion,
                          Precio = p.Precio,
                          FechaCreacion = p.FechaCreacion
                      })
                      .FirstOrDefault();

            result.CheckIsNotNull();

            return result;

        }

        /// <summary>
        /// Proceso de creación de un producto, asíncrono.
        /// </summary>
        /// <param name="model">Objeto de tipo producto.</param>
        /// <returns>Retorna el nevo objeto creado con su id.</returns>
        public async Task<Product> CreateAsync(Product model)
        {
            if (model is null)
            {
                throw new HttpException(new List<string> { "Datos de productos no diligenciados" }, HttpStatusCode.Gone);
            }

            await Products.InsertOneAsync(model);
            return model;
        }

        /// <summary>
        /// Proceso de creación de un producto, síncrono.
        /// </summary>
        /// <param name="model">Objeto de tipo producto</param>
        /// <returns>Retorna el nevo objeto creado con su id</returns>
        public Product Create(Product model)
        {
            if (model is null)
            {
                throw new HttpException(new List<string> { "Datos de productos no diligenciados" }, HttpStatusCode.Gone);
            }
            Products.InsertOne(model);
            return model;
        }

        /// <summary>
        /// Proceso de actualización de un producto, síncrono.
        /// </summary>
        /// <param name="id">Identificación de un producto</param>
        /// <param name="model">Objeto de tipo producto</param>
        public void Update(string id, Product model)
        {
            var result = Get(id);
            if(result == null)
                throw new HttpException(new List<string> { "Producto no encontrado para actualizar" }, HttpStatusCode.NotFound);

            Products.ReplaceOne(car => car.Id == id, model);
        }

        /// <summary>
        /// Proceso de actualización de un producto, asíncrono.
        /// </summary>
        /// <param name="id">Identificación de un producto</param>
        /// <param name="model">Objeto de tipo producto</param>
        public async Task UpdateAsync(string id, Product model)
        {
            var result = Get(id);
            if (result == null)
                throw new HttpException(new List<string> { "Producto no encontrado para actualizar" }, HttpStatusCode.NotFound);

            await Products.ReplaceOneAsync(car => car.Id == id, model);
        }

        /// <summary>
        /// Proceso de eliminación de un producto, asíncrono.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retorna un valor vacio con resultado ok</returns>
        public async Task RemoveAsync(string id)
        {
            await Products.DeleteOneAsync(prod => prod.Id == id);
        }
        /// <summary>
        /// Proceso de eliminación de un producto, síncrono.
        /// </summary>
        /// <param name="id">Identificación de un producto</param>
        /// <returns>Retorna un valor vacio con resultado ok</returns>
        public void Remove(string id)
        {
            Products.DeleteOne(prod => prod.Id == id);
        }

       
    }
}
