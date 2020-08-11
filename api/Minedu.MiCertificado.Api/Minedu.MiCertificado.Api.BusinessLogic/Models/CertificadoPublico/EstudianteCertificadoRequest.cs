using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class EstudianteCertificadoRequest
    {
        public string idMatricula { get; set; }

        public string idPersonaEstudiante { get; set; }

        public string tipoDocEstudiante { get; set; }

        public string nroDocEstudiante { get; set; }

        public string ubigeoEstudiante { get; set; }
    }
}
