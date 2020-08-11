using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class InformacionCertificadoRequest
    {
        public string idGrado { get; set; }

        public string dscGrado { get; set; }

        public string anioCulminacion { get; set; }

        public string cicloCulminacion { get; set; }

        public string telefonoContacto { get; set; }

        public string correoElectronico { get; set; }

        public string idMotivo { get; set; }

        public string dscMotivo { get; set; }

        public string motivoOtros { get; set; }
    }
}
