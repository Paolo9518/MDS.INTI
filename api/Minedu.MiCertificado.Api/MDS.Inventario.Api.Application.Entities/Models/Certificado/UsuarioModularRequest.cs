using MDS.Inventario.Api.DataAccess.Contracts.Entities.Certificado;
using System.Collections.Generic;

namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
{
    public class UsuarioModularRequest
    {
        public string usuarioLogin { get; set; }
        public string codigoModular { get; set; }
        public string anexo { get; set; }
        public string idRol { get; set; }
    }
}
