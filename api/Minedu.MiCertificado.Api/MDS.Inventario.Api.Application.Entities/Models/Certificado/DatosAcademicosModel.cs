using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
{
    public class DatosAcademicosModel
    {
        public string codigoSolicitud { get; set; }
        public NotasRequest[] notas {get;set;}
        public GradoCertificadoModel[] grados { get; set; }
        public ObservacionCertificadoModel[] observaciones { get; set; }
        public NotasRequest[] eliminados { get; set; }
        public string usuario { get; set; }
    }
}
