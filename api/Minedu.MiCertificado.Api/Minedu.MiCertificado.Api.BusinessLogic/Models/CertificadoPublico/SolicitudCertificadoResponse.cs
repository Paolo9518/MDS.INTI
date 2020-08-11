using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class SolicitudCertificadoResponse
    {
        public string nroDocSolicitante { get; set; }
        public string nombresSolicitante { get; set; }
        public string apellidosSolicitante { get; set; }
        public string motivoSolicitud { get; set; }
        public string correoSolicitante { get; set; }
        public string telefonoSolicitante { get; set; }

        public string anioCulminacion { get; set; }
        public string dre { get; set; }
        public string ugel { get; set; }
        public string cenEdu { get; set; }
        public string codMod { get; set; }
        public string anexo { get; set; }

        public string codigoVirtual { get; set; }
        public string horaCreacion { get; set; }
    }
}