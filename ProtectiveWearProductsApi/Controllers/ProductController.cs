using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ProtectiveWearProductsApi.Exceptions;
using ProtectiveWearProductsApi.Interfaces;
using ProtectiveWearProductsApi.Models;

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

        private readonly IProductService _productService;

        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="productService"></param>
        public ProductController(IProductService productService)
        {
            this._productService = productService;
        }

        /// <summary>
        /// Proceso que consulta una lista de productos.
        /// </summary>
        /// <response code="401">Unauthorized. Requested resource requires authentication.</response>              
        /// <response code="200">OK. Solicitud exitosa.</response>        
        /// <response code="404">NotFound. Requested resource does not exist on the server.</response> 
        [HttpGet]
        public async Task<ActionResult<List<ProductApi>>> Products()
        {
            return await _productService.GetAsync();
        }

        /// <summary>
        /// Proceso para consultar el detalle de un producto.
        /// </summary>
        /// <param name="id">Identificación de un producto.</param>
        /// <response code="401">Unauthorized. Requested resource requires authentication.</response>              
        /// <response code="200">OK. Solicitud exitosa.</response>        
        /// <response code="404">NotFound. Requested resource does not exist on the server.</response> 
        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            return Ok(await _productService.GetAsync(id));
        }


        /// <summary>
        /// Proceso de creación de un producto.
        /// </summary>
        /// <param name="model">Objeto de tipo producto.</param>
        /// <response code="401">Unauthorized. Requested resource requires authentication.</response>              
        /// <response code="201">Created. Se ha creado el objeto.</response>        
        /// <response code="400">BadRequest. Request could not be understood by the server.</response> 
        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromBody]ProductCreated model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    new
                    {
                        Error = new { Message = "Invalid product created." },
                        ModelState.Values
                    }
                    );
            }
            var prod = new Product
            {
                Nombre = model.Nombre,
                Presentacion = model.Presentacion,
                Descripcion = model.Descripcion,
                Precio = model.Precio,
                FechaCreacion = model.FechaCreacion,
                ImageUrl = model.ImageUrl,
                Id = ""

            };

            await _productService.CreateAsync(prod);
            return CreatedAtRoute("GetProductById", new { id = prod.Id.ToString() }, prod);
        }


        /// <summary>
        /// Proceso de actualización de un producto.
        /// </summary>
        /// <param name="id">Identificación de un producto.</param>
        /// <param name="model">Objeto de tipo producto.</param>
        /// <response code="401">Unauthorized. Requested resource requires authentication.</response>              
        /// <response code="204">Created. No se devuelve el objeto solicitado.</response>        
        /// <response code="400">BadRequest. Request could not be understood by the server.</response>        
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
        /// <response code="401">Unauthorized. Requested resource requires authentication.</response>              
        /// <response code="204">OK. No se devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. Requested resource does not exist on the server.</response> 
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _productService.RemoveAsync(id);
            return NoContent();
        }
    }
}