using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class ApoderadoEstudianteRequest
    {
        public int idPersonaApoderado { get; set; }
        public int idPersonaEstudiante { get; set; }
    }
}
