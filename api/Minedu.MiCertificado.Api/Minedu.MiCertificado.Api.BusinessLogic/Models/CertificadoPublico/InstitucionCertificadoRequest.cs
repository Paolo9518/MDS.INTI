using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class InstitucionCertificadoRequest
    {
        public string codMod { get; set; }

        public string anexo { get; set; }

        public string cenEdu { get; set; }

        public string idModalidad { get; set; }

        public string dscModalidad { get; set; }

        public string abrModalidad { get; set; }

        public string idNivel { get; set; }

        public string dscNivel { get; set; }

        public string ugel { get; set; }

        public string dre { get; set; }
    }
}
