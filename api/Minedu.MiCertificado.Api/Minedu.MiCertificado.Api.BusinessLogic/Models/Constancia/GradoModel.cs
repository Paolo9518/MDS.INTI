using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Constancia
{
    public class GradoModel
    {
        public GradoModel()
        {

        }

        public int idConstanciaGrado { get; set; }
        public int idSolicitud { get; set; }

        public string idGrado { get; set; }
        public string dscGrado { get; set; }
        public string corrEstadistica { get; set; }
        public int idAnio { get; set; }
        public string codMod { get; set; }
        public string anexo { get; set; }
        public string situacionFinal { get; set; }
    }
}
