using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class EstudianteModalidadNivelModularRequest
    {

        public string idPersonaEstudiante { get; set; }
        public string idModalidad { get; set; }
        public string idNivel { get; set; }

        public string codMod { get; set; }
        public string anexo { get; set; }
    }
}
