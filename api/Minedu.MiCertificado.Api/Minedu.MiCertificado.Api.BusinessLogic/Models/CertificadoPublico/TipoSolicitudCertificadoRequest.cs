using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class TipoSolicitudCertificadoRequest
    {
        public string tipoSolicitante { get; set; }
        public string notasIncompletas { get; set; }
    }
}
