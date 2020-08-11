using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models.Siagie
{
    public class InstitucionEducativaPorDreUgelResponse
    {
        public string nombreIE { get; set; }
        public string codMod { get; set; }
        public string anexo { get; set; }
        public string codDre { get; set; }
        public string nombreDre { get; set; }
        public string codUgel { get; set; }
        public string nombreUgel { get; set; }
        public string departamento { get; set; }
        public string provincia { get; set; }
        public string distrito { get; set; }
        public string nombreDirector { get; set; }
        public int TotalRegistros { get; set; }
    }
}
