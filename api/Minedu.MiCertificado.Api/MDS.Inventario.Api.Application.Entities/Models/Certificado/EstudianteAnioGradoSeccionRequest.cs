using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
{
    public class EstudianteAnioGradoSeccionRequest
    {
        public string codMod { get; set; }
        public string anexo { get; set; }
        public int idAnio { get; set; }
        public string idNivel { get; set; }
        public string idGrado { get; set; }
        public string idSeccion { get; set; }
        public string numeroDocumento { get; set; }
        public string nombresEstudiante { get; set; }
        public string estadoSolicitud { get; set; }
    }
}
