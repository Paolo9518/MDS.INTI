using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models
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
