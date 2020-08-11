using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models.Siagie
{
    public class UsuarioResponse
    {
        public string codModular { get; set; }
        public string anexo { get; set; }
        public string numeroDocumento { get; set; }
        public string idNivel { get; set; }
        public string dscNivel { get; set; }
        public string idRol { get; set; }

        public string nombreRol { get; set; }
        public string loginUsuario { get; set; }
        public string tipoSede { get; set; }
        public string nombreCompleto { get; set; }
        public string ugel { get; set; }
        public string nombreIE { get; set; }
        public string idModalidad { get; set; }

        /*EBA MODEL*/
        public string dni { get; set; }
        public string descNivel { get; set; }
        public string nombre { get; set; }
        public string apePaterno { get; set; }
        public string apeMaterno { get; set; }
        public string idTipoSede { get; set; }
    }
}
