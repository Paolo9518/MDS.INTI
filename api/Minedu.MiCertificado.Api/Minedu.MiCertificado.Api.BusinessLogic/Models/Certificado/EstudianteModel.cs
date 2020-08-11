using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class EstudianteModel
    {
        public string numeroDocumento { get; set; }
        public string codigoEstudiante { get; set; }

        public string apellidoMaterno { get; set; }
        public string apellidoPaterno { get; set; }
        public string nombres { get; set; }
        public string fechaNacimiento { get; set; }
        public string nombreMadre { get; set; }
        public string nombrePadre { get; set; }

        public string ubigeoDomicilio { get; set; }
        public string dptoDomicilio { get; set; }
        public string provDomicilio { get; set; }
        public string distDomicilio { get; set; }

        public int ultimoAnio { get; set; }
        public string idNivel { get; set; }
        public string idPersona { get; set; }
        public string idModalidad { get; set; }
        public string codigoModular { get; set; }
        public string idGrado { get; set; }
        public string dscGrado { get; set; }

        public string idMotivo { get; set; }
        public string correoElectronico { get; set; }
        public string ciclo { get; set; }

        public string numeroDocumentoApoderado { get; set; }       

        public string codigoError { get; set; }
        public string mensaje { get; set; }
        public int esMenor { get; set; }
    }
}

 
 
