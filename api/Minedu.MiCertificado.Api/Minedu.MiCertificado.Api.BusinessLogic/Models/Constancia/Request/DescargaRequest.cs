using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Constancia
{
    public class DescargaRequest
    {
        [Required]
        public string codigoVirtual { get; set; }

        [Required]
        public string tipoDocumento { get; set; }

        [Required]
        public string numeroDocumento { get; set; }
    }
}
