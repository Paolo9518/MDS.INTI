using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
{
    public class UsuariosRolPermisoRequest
    {
        public List<UsuarioPermisoResponse> usuarios { get; set; }
    }
}
