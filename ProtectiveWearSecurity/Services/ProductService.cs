using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProtectiveWearSecurity.Exceptions;
using ProtectiveWearSecurity.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProtectiveWearSecurity.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductService
    {
        private readonly HttpClient client;
        private HttpResponseMessage response;
        private readonly string _HostProduct;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Connstructor de la clase.
        /// </summary>
        public ProductService(IConfiguration configuration)
        {
            client = new HttpClient();
            _configuration = configuration;

            /// for testing
            //_HostProduct = _configuration.GetConnectionString("HostProducts");
            _HostProduct = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING_PRODUCTS");
        }
        /// <summary>
        ///Proceso de creación de un producto, asíncrono.
        /// </summary>
        /// <param name="model">Objeto de tipo producto.</param>
        /// <returns>Retorna el nevo objeto creado con su id</returns>
        public async Task<ProductApiModel> CreateProductAsync([FromBody]ProductCreated model)
        {
            ProductApiModel pro = null;

            response = await client.PostAsJsonAsync($"{_HostProduct}v1/api/product", model);
            var jsonSerialize = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                string mensaje = JsonConvert.SerializeObject(jsonSerialize);
                throw new HttpException(new List<string> { mensaje }, response.StatusCode);
            }
            //response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            pro = JsonConvert
             .DeserializeObject<ProductApiModel>(jsonSerialize.ToString()
             , new JsonSerializerSettings()
             {
                 MissingMemberHandling =
                     MissingMemberHandling.Ignore
             });

            pro.CheckIsNotNull();

            return pro;
        }
        /// <summary>
        /// Proceso que consulta una lista de productos, de forma asíncrono.
        /// </summary>
        /// <returns>Retorna una lista de objetos de tipo producto.</returns>
        public async Task<List<ProductApiModel>> GetProductAsync()
        {
            List<ProductApiModel> model = null;

            var res = await client.GetAsync($"{_HostProduct}v1/api/product");



            HttpResponseMessage response = res;

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                using (StreamReader sr = new StreamReader(stream))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    model = serializer.Deserialize<List<ProductApiModel>>(reader);
                }
            }


            var jsonSerialize = await response.Content.ReadAsStringAsync();

            model = JsonConvert
                        .DeserializeObject<List<ProductApiModel>>(jsonSerialize.ToString()
                        , new JsonSerializerSettings()
                        {
                            MissingMemberHandling =
                                MissingMemberHandling.Ignore
                        });

            model.CheckIsNotNull();

            return model;
        }

        /// <summary>
        /// Proceso para consultar el detalle de un producto.
        /// </summary>
        /// <param name="id">dentificación de un producto.</param>
        /// <returns>Retorna la información de tallada de un producto.</returns>
        public async Task<Product> GetProductByIdAsync(string id)
        {
            Product _product = null;
            response = await client.GetAsync($"{_HostProduct}v1/api/product/{id}");

            if (response.IsSuccessStatusCode)
            {
                _product = await response.Content.ReadAsAsync<Product>();
            }
            _product.CheckIsNotNull();
            return _product;
        }
        /// <summary>
        /// Proceso de actualización de un producto.
        /// </summary>
        /// <param name="model">Objeto de tipo producto.</param>
        /// <returns>Retorna un valor vacio con resultado ok.</returns>
        public async Task UpdateProductAsync(string id, [FromBody]Product model)
        {
           
            response = await client.PutAsJsonAsync($"{_HostProduct}v1/api/product/{id}", model);

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage errorMessage = null;
                var jsonSerialize = await response.Content.ReadAsStringAsync();
                errorMessage = JsonConvert
                 .DeserializeObject<ErrorMessage>(jsonSerialize.ToString()
                 , new JsonSerializerSettings()
                 {
                     MissingMemberHandling =
                         MissingMemberHandling.Ignore
                 });                

                throw new HttpException(errorMessage.messages, errorMessage.code);
            }
            response.EnsureSuccessStatusCode();

        }
        /// <summary>
        /// Proceso de eliminación de un producto.
        /// </summary>
        /// <param name="id">Identificación de un producto.</param>
        /// <returns>Retorna un valor vacio con resultado ok.</returns>
        public async Task<HttpStatusCode> DeleteProductAsync(string id)
        {
            response = await client.DeleteAsync($"{_HostProduct}v1/api/product/{id}");
            return response.StatusCode;
        }

    }
}
