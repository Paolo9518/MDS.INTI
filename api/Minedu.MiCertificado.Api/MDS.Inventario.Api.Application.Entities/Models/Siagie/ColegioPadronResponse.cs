using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models.Siagie
{
    public class ColegioPadronResponse
    {
        public string codMod { get; set; }
        public string anexo { get; set; }
        public string cenEdu { get; set; }
        public string idNivel { get; set; }
        public string dscNivel { get; set; }
        public string idModalidad { get; set; }
        public string dscModalidad { get; set; }
        public string abrModalidad { get; set; }
        public string departamento { get; set; }
        public string provincia { get; set; }
        public string distrito { get; set; }
        public string dirCen { get; set; }
        public string ugel { get; set; }
        public string dre { get; set; }
        public string estado { get; set; }
        public string dscEstado { get; set; }
        public int total { get; set; }
    }
}
