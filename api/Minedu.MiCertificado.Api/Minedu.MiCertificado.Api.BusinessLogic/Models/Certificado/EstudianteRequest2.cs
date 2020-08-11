using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class EstudianteRequest2
    {
        public int idPersonaEstudiante { get; set; }
        public string codEstudiante { get; set; }
        public string tipoDocEstudiante { get; set; }
        public string nroDocEstudiante { get; set; }
        public string nombresEstudiante { get; set; }
        public string apellidoPaternoEstudiante { get; set; }
        public string apellidoMaternoEstudiante { get; set; }
        public string departamentoEstudiante { get; set; }
        public string provinciaEstudiante { get; set; }
        public string ubigeoEstudiante { get; set; }
        public string idNivel { get; set; }
        public string idGrado { get; set; }
        public string dscGrado { get; set; }
        public string codigoPersona { get; set; }
        public string numDocEnvio { get; set; }
        public string tipDocEnvio { get; set; }
    }
}