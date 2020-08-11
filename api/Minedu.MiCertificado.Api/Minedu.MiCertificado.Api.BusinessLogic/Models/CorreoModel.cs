using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models
{
    public class CorreoModel
    {
        public string para { get; set; }
        public string cc { get; set; }
        public string cco { get; set; }
        public string asunto { get; set; }
        public string mensaje { get; set; }
    }
}
