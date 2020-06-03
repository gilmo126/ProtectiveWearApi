using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProtectiveWearProductsApi.Models;
using ProtectiveWearProductsApi.Services;

namespace ProtectiveWearProductsApi.Controllers
{
    /// <summary>
    /// Controlador encargado de realizar las operaciones crud sobre el objeto productos.
    /// </summary>
    [Route("v1/api/product")]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        /// <summary>
        /// Propiedad que permite aplicar una injeccion en el constructor para invocar los metodos del servicio.
        /// </summary>
        private readonly ProductService _productService;

        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="productService"></param>
        public ProductController(ProductService productService)
        {
            this._productService = productService;
        }

        /// <summary>
        /// Proceso que consulta una lista de productos.
        /// </summary>
        /// <returns>Retorna una lista de objetos de tipo producto</returns>
        [HttpGet]
        public async Task<ActionResult<List<ProductApi>>> Products()
        {
            return await _productService.GetAsync();
        }

        /// <summary>
        /// Proceso para consultar el detalle de un producto.
        /// </summary>
        /// <param name="id">Identificación de un producto.</param>
        /// <returns>Retorna la información de tallada de un producto.</returns>
        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<ActionResult<ProductApi>> GetProduct(string id)
        {
            return Ok(await _productService.GetAsync(id));
        }


        /// <summary>
        /// Proceso de creación de un producto.
        /// </summary>
        /// <param name="model">Objeto de tipo producto.</param>
        /// <returns>Retorna el nevo objeto creado con su id.</returns>
        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromBody]Product model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _productService.CreateAsync(model);
            return CreatedAtRoute("GetProductById", new { id = model.Id.ToString() }, model);
        }


        /// <summary>
        /// Proceso de actualización de un producto.
        /// </summary>
        /// <param name="id">Identificación de un producto.</param>
        /// <param name="model">Objeto de tipo producto.</param>
        /// <returns>Retorna un valor vacio con resultado ok.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Product model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _productService.UpdateAsync(id, model);
            return NoContent();
        }

        /// <summary>
        /// Proceso de eliminación de un producto.
        /// </summary>
        /// <param name="id">Identificación de un producto</param>
        /// <returns>Retorna un valor vacio con resultado ok.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _productService.RemoveAsync(id);
            return NoContent();
        }
    }
}