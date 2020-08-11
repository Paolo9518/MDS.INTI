//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class FileModel
    {
        public string data { get; set; }
        public string nombre { get; set; }
        public string extension { get; set; }
        public IFormFile image { get; set; }
    }

}