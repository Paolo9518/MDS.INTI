using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
{
    public class GradoCertificadoResponse
    {
        public string idConstanciaGrado { get; set; }
        public string idSolicitud { get; set; }
        public string idGrado { get; set; }
        public string estado { get; set; }
        public int ciclo { get; set; }

        public string dscGrado { get; set; }
        public string corrEstadistica { get; set; }
        public string idAnio { get; set; }
        public string codMod { get; set; }
        public string anexo { get; set; }
        public string situacionFinal { get; set; }
    }
}