using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class PersonaModel
    {
        public string numDoc { get; set; }

        public string apellidoMaterno { get; set; }
        public string apellidoPaterno { get; set; }
        public string nombres { get; set; }

        public string fecNacimiento { get; set; }

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
        public string codModular { get; set; }

        public bool IdemNivel { get; set; }

        public string NumDocApoderado { get; set; }
        public string idGrado { get; set; }
        public string dscGrado { get; set; }

        public string codigoError { get; set; }
        public string mensaje { get; set; }
    }
}

 
 
