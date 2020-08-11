using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models
{
    public class PersonaRequest
    {
        public string tipoDocumento { get; set; }

        public string nroDocumento { get; set; }
    }
}
