using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models
{
    public class ProvinciaRequest
    {
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Código de provincia inválida")]
        public string codProvincia { get; set; }
    }
}
