using System.ComponentModel.DataAnnotations;

namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
{
    public class VerificacionCertificadoRequest
    {
        [Required]
        public string codigoVirtual { get; set; }

        [Required]
        public string tipoDocumento { get; set; }

        [Required]
        public string numeroDocumento { get; set; }

        [Required]
        public string captcha { get; set; }
    }
}
