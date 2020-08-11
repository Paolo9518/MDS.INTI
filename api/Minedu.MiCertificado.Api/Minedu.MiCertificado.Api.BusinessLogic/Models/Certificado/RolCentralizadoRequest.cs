using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class RolCentralizadoRequest
    {
        public string usuarioLogin { get; set; }
        public string idRol { get; set; }
        public string codigo { get; set; }
        public string tipoSede { get; set; }
        public string codigoModular { get; set; }
    }
}
