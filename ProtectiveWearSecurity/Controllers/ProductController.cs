using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProtectiveWearSecurity.Models;
using ProtectiveWearSecurity.Services;

namespace ProtectiveWearSecurity.Controllers
{
    /// <summary>
    /// Controlador y rutas de la Uri.
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        /// <response code="401">Unauthorized. Requested resource requires authentication.</response>              
        /// <response code="200">OK. Solicitud exitosa.</response>        
        /// <response code="404">NotFound. Requested resource does not exist on the server.</response> 
        [HttpGet]
        public async Task<ActionResult<List<ProductApiModel>>> Products()
        {
            return await _productService.GetProductAsync();
        }
        /// <summary>
        /// Proceso para consultar el detalle de un producto.
        /// </summary>
        /// <param name="id">Identificación de un producto.</param>
        /// <response code="401">Unauthorized. Requested resource requires authentication.</response>              
        /// <response code="200">OK. Solicitud exitosa.</response>        
        /// <response code="404">NotFound. Requested resource does not exist on the server.</response> 
        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<ActionResult<ProductApiModel>> GetProduct(string id)
        {
            return await _productService.GetProductByIdAsync(id);
        }
        /// <summary>
        /// Proceso de creación de un producto.
        /// </summary>
        /// <param name="model">Objeto de tipo producto.</param>
        /// <response code="401">Unauthorized. Requested resource requires authentication.</response>              
        /// <response code="201">Created. Se ha creado el objeto.</response>        
        /// <response code="400">BadRequest. Request could not be understood by the server.</response> 
        [HttpPost]
        public async Task<ActionResult<ProductApiModel>> Create([FromBody]Product model)
        {
            return await _productService.CreateProductAsync(model);
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
            await _productService.UpdateProductAsync(model);
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
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }

    }
}