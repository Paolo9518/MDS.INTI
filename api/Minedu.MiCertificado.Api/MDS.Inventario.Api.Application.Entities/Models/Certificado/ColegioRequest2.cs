using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
{
    public class ColegioRequest2
    {
        public string nombre { get; set; }
        public string modular { get; set; }
        public string anexo { get; set; }
        public string ultimoGrado { get; set; }
    }
}