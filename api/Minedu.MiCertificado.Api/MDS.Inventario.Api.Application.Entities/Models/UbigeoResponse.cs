using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models
{
    public class UbigeoResponse
    {
        public string codDepartamento { get; set; }
        public string departamento { get; set; }
        public string codProvincia { get; set; }
        public string provincia { get; set; }
        public string codUbigeo { get; set; }
        public string distrito { get; set; }
    }
}
