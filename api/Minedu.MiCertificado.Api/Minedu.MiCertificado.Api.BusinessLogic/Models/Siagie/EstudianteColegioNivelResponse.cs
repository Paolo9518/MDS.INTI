using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Siagie
{
    public class EstudianteColegioNivelResponse
    {
        //public int idMatricula { get; set; }
        public string idMatricula { get; set; }
        public int idAnio { get; set; }
        public string codigoModular { get; set; }
        public string anexo { get; set; }
        public string nombreIE { get; set; }
        public string idGrado { get; set; }
        public string descripcionGrado { get; set; }
        public int estadoActa { get; set; }
    }
}
