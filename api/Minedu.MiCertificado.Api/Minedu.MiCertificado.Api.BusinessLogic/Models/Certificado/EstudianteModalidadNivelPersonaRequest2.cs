using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class EstudianteModalidadNivelPersonaRequest2
    {
        //public string request { get; set; }

        public string idPersona { get; set; }
        public string idModalidad { get; set; }
        public string idNivel { get; set; }
        public string idTipoDocumento { get; set; }
        public string numeroDocumento { get; set; }

        public string idSolicitud { get; set; }
        public string codigoModular { get; set; }
        public string anexo { get; set; }
        public string idMotivo { get; set; }
        public string idSolicitante { get; set; }
        public int anioCulminacion { get; set; }
        public string codPersona { get; set; }

        public int? tieneApoderado { get; set; }
        public string numeroDocumentoApoderado { get; set; }
        public string tipoDocumentoApoderado { get; set; }
        public int idPersonaApoderado { get; set; }
        public string codMod { get; set; }
        public int idAnio { get; set; }
        public string codigoSolicitud { get; set; }
        public string estadoSolicitud { get; set; }
        public string correo { get; set; }
        public string usuario { get; set; }
        public string emisionDirecta { get; set; }
    }
}
