using System.Collections.Generic;

namespace MDS.Inventario.Api.Application.Entities.Models
{
    public class PDFNota
    {
        //public string IdArea { get; set; }
        public string DscArea { get; set; }
        public List<PDFGradoNota> GradoNotas { get; set; }
    }
}
