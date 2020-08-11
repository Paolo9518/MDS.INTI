//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;

namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
{
    public class FileModel
    {
        public string data { get; set; }
        public string nombre { get; set; }
        public string extension { get; set; }
        public IFormFile image { get; set; }
    }

}