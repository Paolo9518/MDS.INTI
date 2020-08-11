using System.ComponentModel.DataAnnotations;

namespace MDS.Inventario.Api.Application.Entities.Models.Constancia
{
    public class VerificacionRequest
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
