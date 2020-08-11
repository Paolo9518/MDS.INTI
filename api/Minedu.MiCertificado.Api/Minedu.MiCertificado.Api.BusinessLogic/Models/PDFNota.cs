using System.Collections.Generic;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models
{
    public class PDFNota
    {
        //public string IdArea { get; set; }
        public string DscArea { get; set; }
        public List<PDFGradoNota> GradoNotas { get; set; }
    }
}
