using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Constancia
{
    public class ObservacionModel
    {
        public ObservacionModel()
        {

        }

        public int idConstanciaObservacion { get; set; }
        public int idSolicitud { get; set; }

        public string idNivel { get; set; }
        public int idAnio { get; set; }
        public string resolucion { get; set; }
        public int tipoSolicitud { get; set; }
        public string motivo { get; set; }
        public int idTipo { get; set; }
        public string dsc { get; set; }
        public int tipoObs { get; set; }
    }
}
