using Microsoft.AspNetCore.Mvc;
using Moq;
using ProtectiveWearProductsApi.Controllers;
using System.Threading.Tasks;
using Xunit;
using ProtectiveWearProductsApi.Models;
using System.Collections.Generic;
using ProtectiveWearProductsApi.Interfaces;
using System;

namespace XUnitTestProtectiveWearProductsApi
{
    public class ProductControllerTest
    {

        #region Test All products
        /// <summary>
        /// Test for get in all products exists
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ProductsTestAllItems()
        {
            // arrange
            var collection = new List<ProductApi>
            {
               new ProductApi{
                            Id = Guid.NewGuid().ToString(),
                            Nombre = "Orange Juice",
                            Descripcion ="Just for test Orange Juice",
                            Presentacion ="box x 12 units",
                            Precio = 22222
               },
                new ProductApi{
                            Id = Guid.NewGuid().ToString(),
                            Nombre = "Diary Milk",
                            Descripcion ="Just for test Diary Milk",
                            Presentacion ="box x 24 units",
                            Precio = 234567
               }

            };
            var mock = new Mock<IProductService>();
            mock.Setup(p => p.GetAsync())
                .ReturnsAsync(collection);
            // act
            var model = new ProductController(mock.Object);
            var mod = (await model.Products());

            // assert
            var items = Assert.IsType<List<ProductApi>>(mod.Value);
            Assert.Equal(2, items.Count);
        }
        /// <summary>
        /// Test for all products not exist
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ProductsTestNotAllItems()
        {
            // arrange
            var collection = new List<ProductApi>();

            var mock = new Mock<IProductService>();
            mock.Setup(p => p.GetAsync())
                .ReturnsAsync(collection);

            // act
            var prod = new ProductController(mock.Object);
            var mod = (await prod.Products());

            // assert
            var items = Assert.IsType<List<ProductApi>>(mod.Value);
            Assert.Empty(items);
        }
        #endregion

        #region Test only one product
        /// <summary>
        /// Test for especific product id unique identification.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetProductTestById()
        {

            // arrange
            var id = Guid.NewGuid().ToString();
            var productApiModel = new ProductApi
            {
                Id = id,
                Nombre = "cadena dos",
                Descripcion = "lo que sea",
                Presentacion = "prueba unitex",
                Precio = 00
            };
            var mock = new Mock<IProductService>();
            mock.Setup(p => p.GetAsync(id))
                .ReturnsAsync(productApiModel);
            var prod = new ProductController(mock.Object);

            // act
            var result = (await prod.GetProduct(id)).Result as OkObjectResult;


            //var data = await prod.GetProduct(id);
            //IActionResult actionResult = data.Result;
            //OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
            //ProductApi model = Assert.IsType<ProductApi>(okObjectResult.Value);

            // Assert
            //Assert.IsType<OkObjectResult>(data.Result);
            //Assert.Equal(id, model.Id);
            Assert.IsType<ProductApi>(result.Value);
            Assert.Equal(id, (result.Value as ProductApi).Id);
        }
        /// <summary>
        /// Test for especific product Id diferent identification.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetProductNoResultTestById()
        {
            // arrange
            var id = Guid.NewGuid().ToString();
            var mock = new Mock<IProductService>();
            mock.Setup(p => p.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null)
                ;
            var prod = new ProductController(mock.Object);

            // act
            var data = await prod.GetProduct(id);
            IActionResult actionResult = data.Result;
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(actionResult);

            ProductApi model = null;

            if (okObjectResult.Value != null)
                model = Assert.IsType<ProductApi>(okObjectResult.Value);

            // assert
            Assert.Null(model);
        }

        #endregion

    }
}
