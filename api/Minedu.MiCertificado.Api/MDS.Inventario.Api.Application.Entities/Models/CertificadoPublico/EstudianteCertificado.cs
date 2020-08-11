using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
{
    public class EstudianteCertificado
    {
        public SolicitudCertificadoModel solicitud { get; set; }

        public EstudianteCertificadoModel estudiante { get; set; }

        public List<GradoCertificadoModel> grados { get; set; }

        public List<NotaCertificadoModel> notas { get; set; }

        public List<ObservacionCertificadoModel> observaciones { get; set; }
    }
}
