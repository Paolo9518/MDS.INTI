using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.DataAccess.Contracts.Entities
{
    public class CorreoEntity
    {
        public string PARA { get; set; }
        public string CC { get; set; }
        public string CCO { get; set; }
        public string ASUNTO { get; set; }
        public string MENSAJE { get; set; }
    }
}
