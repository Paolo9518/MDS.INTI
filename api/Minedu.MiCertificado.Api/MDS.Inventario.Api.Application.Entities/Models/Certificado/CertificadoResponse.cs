using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
{
    public class CertificadoResponse
    {
        public string codigoVirtual { get; set; }
        public string nombresSolicitante { get; set; }
        public string correoElectronico { get; set; }
        public string nroDocEstudiante { get; set; }
        public string fechaCreacion { get; set; }

        public string idSolicitud { get; set; }
        public int num { get; set; }
        public string estado { get; set; }
        public string idPersona { get; set; }
        public string idModalidad { get; set; }
    }
}
