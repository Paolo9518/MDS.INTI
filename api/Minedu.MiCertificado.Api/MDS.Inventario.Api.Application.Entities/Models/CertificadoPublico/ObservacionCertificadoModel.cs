using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
{
    public class ObservacionCertificadoModel
    {
        public ObservacionCertificadoModel()
        {

        }

        public int idConstanciaObservacion { get; set; }
        public int idCertificadoObservacion { get; set; }
        public int idSolicitud { get; set; }

        public string idNivel { get; set; }
        public int idAnio { get; set; }
        public string resolucion { get; set; }
        public int tipoSolicitud { get; set; }
        public string motivo { get; set; }
        public int idTipo { get; set; }
        public string dsc { get; set; }
        public int tipoObs { get; set; }
        public string estado { get; set; }
        public string usuario { get; set; }
    }
}
