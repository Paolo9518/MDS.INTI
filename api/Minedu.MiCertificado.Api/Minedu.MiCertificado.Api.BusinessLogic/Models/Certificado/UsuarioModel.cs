using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Certificado;
using System.Collections.Generic;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class UsuarioModel
    {
        public UsuarioModel()
        {
        }

        public int tipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string nombreUsuario { get; set; }
        public string fullNombres { get; set; }
        public List<IEEntity> roles { get; set; }
        public bool usuarioAutenticacion_IIResult { get; set; }
        public string rolActivo { get; set; }
        public string nombreIE { get; set; }
        public string codigoModular { get; set; }
        public string anexo { get; set; }
        public string UsuarioLogin { get; set; }

        public string token { get; set; }

    }
}
