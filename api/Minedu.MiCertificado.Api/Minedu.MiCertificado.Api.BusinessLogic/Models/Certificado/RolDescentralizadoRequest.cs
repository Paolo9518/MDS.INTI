using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class RolDescentralizadoRequest
    {
        public string usuarioLogin { get; set; }
        public string idRol { get; set; }
        public string csts { get; set; }
        public string codigo { get; set; }
        public string tipoSede { get; set; }
        public string codigoModular { get; set; }
    }
}
