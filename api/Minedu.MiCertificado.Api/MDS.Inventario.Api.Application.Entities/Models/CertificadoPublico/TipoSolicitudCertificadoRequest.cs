using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
{
    public class TipoSolicitudCertificadoRequest
    {
        public string tipoSolicitante { get; set; }
        public string notasIncompletas { get; set; }
    }
}
