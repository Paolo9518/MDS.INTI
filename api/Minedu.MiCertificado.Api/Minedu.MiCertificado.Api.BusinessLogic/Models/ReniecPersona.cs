using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models
{
    public class ReniecPersona
    {
        public string numDoc { get; set; }

        public string apellidoMaterno { get; set; }
        public string apellidoPaterno { get; set; }
        public string nombres { get; set; }

        public string fecNacimiento { get; set; }

        public string nombreMadre { get; set; }
        public string nroDocMadre { get; set; }
        public string nombrePadre { get; set; }
        public string nroDocPadre { get; set; }

        public string ubigeoDomicilio { get; set; }

        public string dptoDomicilio { get; set; }
        public string provDomicilio { get; set; }
        public string distDomicilio { get; set; }

        public bool fecFallecimientoSpecified { get; set; }
    }
}
