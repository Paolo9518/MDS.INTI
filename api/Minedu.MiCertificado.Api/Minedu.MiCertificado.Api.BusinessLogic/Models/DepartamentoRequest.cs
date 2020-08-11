using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models
{
    public class DepartamentoRequest
    {
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Código de departamento inválido")]
        public string codDepartamento { get; set; }
    }
}
