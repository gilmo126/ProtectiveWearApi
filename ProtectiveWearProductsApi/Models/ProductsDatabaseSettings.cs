using ProtectiveWearProductsApi.Interfaces;
namespace ProtectiveWearProductsApi.Models
{

    /// <summary>
    /// Clase encargada de tomar los valores del config para su validacion en la conexion con la base de datos
    /// Implementa de la interface IProductsDatabaseSettings.
    /// </summary>
    public class ProductsDatabaseSettings : IProductsDatabaseSettings
    {
        /// <summary>
        /// Captura el nombre de la tabla afectada, al iniciar la conexion.
        /// </summary>
        public string ProductsCollectionName { get; set; }
        /// <summary>
        /// Captura el nombre del host o la url junto con el encabezado de la base de datos, Ejemplo mongodb://
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// Captura el nombre de la base de datos a almacenar.
        /// </summary>
        public string DatabaseName { get; set; }
       
    }
       
}
