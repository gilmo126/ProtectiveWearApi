using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtectiveWearProductsApi.Models
{

    public class ProductsDatabaseSettings : IProductsDatabaseSettings
    {
        public string ProductsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string Server { get; set; }
    }


    public interface IProductsDatabaseSettings
    {
        string ProductsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        public string Server { get; set; }
    }
}
