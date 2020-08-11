using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models.Constancia
{
    public class SolicitanteModel
    {
        public int idSolicitante { get; set; }

        public int idPersona { get; set; }
        public string idTipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string nombres { get; set; }
        public string telefonoCelular { get; set; }
        public string correoElectronico { get; set; }
        public string ubigeo { get; set; }
        public string departamento { get; set; }
        public string provincia { get; set; }
        public string distrito { get; set; }
    }
}
