using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Central
{
    public class BEUsuarioResponse
    {
        public string usrLogin { get; set; }
        public string usrLogUsr { get; set; }
        public string nombresUsuario { get; set; }
        public string apellidoPaternoUsuario { get; set; }
        public string apellidoMaternoUsuario { get; set; }
        public string fullNombre { get; set; }
        
        public short tipoDocumento { get; set; }
        public string numeroDocumento { get; set; }

        public string correoElectronico { get; set; }

        public short estadoUsuario { get; set; }

        public bool resultado { get; set; }
        public string mensaje { get; set; }

    }
}
