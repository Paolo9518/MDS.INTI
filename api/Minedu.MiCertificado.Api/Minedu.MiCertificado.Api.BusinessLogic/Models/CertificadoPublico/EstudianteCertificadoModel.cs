using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class EstudianteCertificadoModel
    {
        public int idEstudiante { get; set; }

        public int idPersona { get; set; }
        public string idTipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string nombres { get; set; }
        public string ubigeo { get; set; }
        public string departamento { get; set; }
        public string provincia { get; set; }
        public string distrito { get; set; }
        public string usuario { get; set; }
    }
}
