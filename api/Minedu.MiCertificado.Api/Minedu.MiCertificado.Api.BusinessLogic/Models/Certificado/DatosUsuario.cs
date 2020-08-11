using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class DatosUsuario
    {
        public string usuarioLogin { get; set; }
        //public string nombresUsuario { get; set; }
        public string nombresCompleto { get; set; }
        
        //public string tipoDocumento { get; set; }
        public string numeroDocumento { get; set; }

        public string idRol { get; set; }
        public string dscRol { get; set; }

        //public string csts { get; set; }
        //public string codigo { get; set; }
        public string tipoSede { get; set; }

        //public string codigoModular { get; set; }
        //public string anexo { get; set; }
        public string nombreIE { get; set; }
        
        public string idNivel { get; set; }
        public string dscNivel { get; set; }
        public string idModalidad { get; set; }

        public string idMenuNivel { get; set; }
        public string dscMenuNivel { get; set; }

        public string token { get; set; }

        //public string estado { get; set; }

        /**Siagie**/
        //public string dni { get; set; }
        //public string nombre { get; set; }
        //public string apePaterno { get; set; }
        //public string apeMaterno { get; set; }
        //public string codModular { get; set; }
        public string anexo { get; set; }
        ///public bool esDirector { get; set; }
        //public bool esTutor { get; set; }
        //public bool esDocente { get; set; }
        //public int anioId { get; set; }
        //public string ugel { get; set; }


        public string codigoModular { get; set; }
        // int dotacion { get; set; }
        //public string rol { get; set; }
    }
}
