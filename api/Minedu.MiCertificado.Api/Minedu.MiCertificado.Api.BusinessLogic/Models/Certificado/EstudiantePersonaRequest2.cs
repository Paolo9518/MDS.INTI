using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
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