using MDS.Inventario.Api.DataAccess.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models.Helpers
{
    public class DatosUsuarioHelper
    {
        public string usuario { get; set; }
        public string nombresCompleto { get; set; }
        public string tipoDocumento {get;set;}
        public string numeroDocumento { get; set; }
        public string idRol { get; set; }
        public string dscRol { get; set; }
        public string token { get; set; }
    }
}
