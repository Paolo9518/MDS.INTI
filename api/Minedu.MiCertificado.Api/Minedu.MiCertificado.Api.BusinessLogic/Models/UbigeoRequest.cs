using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models
{
    public class UbigeoRequest
    {
        //public string request { get; set; }

        [StringLength(2, MinimumLength = 2, ErrorMessage = "Código de departamento inválido")]
        public string codDepartamento { get; set; }

        [StringLength(4, MinimumLength = 4, ErrorMessage = "Código de provincia inválida")]
        public string codProvincia { get; set; }

        [StringLength(6, MinimumLength = 6, ErrorMessage = "Código de ubigeo inválido")]
        public string codUbigeo { get; set; }
    }
}
