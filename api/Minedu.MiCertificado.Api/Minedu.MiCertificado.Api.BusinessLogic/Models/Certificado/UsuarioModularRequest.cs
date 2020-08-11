using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Certificado;
using System.Collections.Generic;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class UsuarioModularRequest
    {
        public string usuarioLogin { get; set; }
        public string codigoModular { get; set; }
        public string anexo { get; set; }
        public string idRol { get; set; }
    }
}
