using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class ColegioRequest2
    {
        public string nombre { get; set; }
        public string modular { get; set; }
        public string anexo { get; set; }
        public string ultimoGrado { get; set; }
    }
}