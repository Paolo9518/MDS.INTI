using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Siagie
{
    public class ColegioRequest
    {
        public string departamento { get; set; }
        public string provincia { get; set; }
        public string ubigeo { get; set; }
        public string cenEdu { get; set; }
        public string codMod { get; set; }
        public string anexo { get; set; }

        public string estado { get; set; }
        public string codUgel { get; set; }

        public int pageSize { get; set; } = 10;
        public int page { get; set; } = 0;
    }
}
