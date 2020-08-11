using System.Collections.Generic;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class PDFNotaCertificado
    {
        public string IdArea { get; set; }
        public string DscArea { get; set; }
        public string IdTipoArea { get; set; }
        public string DscTipoArea { get; set; }
        public List<PDFGradoNotaCertificado> GradoNotas { get; set; }
        public string Estado { get; set; }
        public int EsAreaSiagie { get; set; }
        public int IdAnio { get; set; }
        public int AnioInicio { get; set; }
        public int AnioFin { get; set; }
    }
}
