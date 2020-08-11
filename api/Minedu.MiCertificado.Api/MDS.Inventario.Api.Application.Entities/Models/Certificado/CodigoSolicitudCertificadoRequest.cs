using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
{
    public class CodigoSolicitudCertificadoRequest
    {
        public string codigoSolicitud { get; set; }
        public string codigoModular { get; set; }
        public string anexo { get; set; }
        public string estadoSolicitud { get; set; }
        public string estadoEstudiante { get; set; }
        public string usuario { get; set; }
        public string idTipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string idNivel { get; set; }
    }
}
