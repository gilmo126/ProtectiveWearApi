
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

    /// <summary>
    /// Interface que implementa las propiedades basicas para identificar la cadena de conexion.
    /// </summary>
    public interface IProductsDatabaseSettings
    {
        /// <summary>
        /// Captura el nombre de la tabla afectada, al iniciar la conexion.
        /// </summary>
        string ProductsCollectionName { get; set; }
        /// <summary>
        /// aptura el nombre del host o la url junto con el encabezado de la base de datos, Ejemplo mongodb://
        /// </summary>
        /// </summary>
        string ConnectionString { get; set; }
        /// <summary>
        /// Captura el nombre de la base de datos a almacenar.
        /// </summary>
        string DatabaseName { get; set; }
        
    }
}
