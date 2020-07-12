using System.Collections.Generic;
using System.Net;

namespace ProtectiveWearProductsApi.Models
{
    public class ErrorMessage
    {
        public HttpStatusCode code { get; set; }

        public List<string> messages { get; set; }
    }
}
