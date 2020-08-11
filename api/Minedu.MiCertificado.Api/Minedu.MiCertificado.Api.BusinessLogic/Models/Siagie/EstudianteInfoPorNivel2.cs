using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Siagie
{
    public class EstudianteInfoPorNivel2
    {
        public int idPersona { get; set; }
        public string idTipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string nombres { get; set; }
        public int idMatricula { get; set; }
        public int idAnio { get; set; }
        public string codMod { get; set; }
        public string anexo { get; set; }
        public string cenEdu { get; set; }
        public string idGrado { get; set; }
        public string dscGrado { get; set; }
        public string idNivel { get; set; }
        public string dscNivel { get; set; }
        public string idModalidad { get; set; }
        public string abrModalidad { get; set; }
        public string dscModalidad { get; set; }
        public string codigoPersona { get; set; }
        public string fechaNacimiento { get; set; }
    }
}
