using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
{
    public class EstudiantePersonaRequest2
    {
        public string tipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string codigoModular { get; set; }
        public string anexo { get; set; }
        public string idNivel { get; set; }
    }
}