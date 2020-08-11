using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Constancia
{
    public class SolicitudResponse
    {
        public string codigoVirtual { get; set; }
        //public string nombresEstudiante { get; set; }
        public string nombresSolicitante { get; set; }
        //public string correoElectronico { get; set; }
        public string nroDocEstudiante { get; set; }
        public string fechaCreacion { get; set; }
    }
}
