using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models.Constancia
{
    public class EstudianteModalidadNivelPersonaRequest
    {
        //public string request { get; set; }

        public string idPersonaEstudiante { get; set; }
        public string idModalidad { get; set; }
        public string idNivel { get; set; }
        public string idTipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
    }
}
