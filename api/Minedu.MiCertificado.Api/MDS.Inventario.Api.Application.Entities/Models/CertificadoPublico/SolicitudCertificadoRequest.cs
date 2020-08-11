using MDS.Inventario.Api.Application.Entities.Models.Constancia;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
{
    public class SolicitudCertificadoRequest
    {
        [Required]
        public InstitucionCertificadoRequest ie { get; set; }

        [Required]
        public TipoSolicitudCertificadoRequest tipo { get; set; }

        [Required]
        public SolicitanteRequest solicitante { get; set; }

        [Required]
        public EstudianteCertificadoRequest estudiante { get; set; }

        [Required]
        public InformacionCertificadoRequest solicitud { get; set; }
    }
}
