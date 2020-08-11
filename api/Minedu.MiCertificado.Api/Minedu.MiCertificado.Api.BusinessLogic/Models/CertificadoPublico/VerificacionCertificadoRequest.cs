using System.ComponentModel.DataAnnotations;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
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
