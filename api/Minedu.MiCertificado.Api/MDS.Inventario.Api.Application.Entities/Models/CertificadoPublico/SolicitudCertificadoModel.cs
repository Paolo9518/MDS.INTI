using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
{
    public class SolicitudCertificadoModel
    {
        public int idSolicitud { get; set; }

        public int idEstudiante { get; set; }
        public int? idSolicitante { get; set; }

        public int idMotivo { get; set; }
        public string idModalidad { get; set; }
        public string abrModalidad { get; set; }
        public string dscModalidad { get; set; }
        public string idNivel { get; set; }
        public string dscNivel { get; set; }
        public string idGrado { get; set; }
        public string dscGrado { get; set; }
        public string estadoSolicitud { get; set; }

        public string motivoOtros { get; set; }

        public string estadoEstudiante { get; set; }

        public string codigoVirtual { get; set; }

        public DateTime fechaCreacion { get; set; }
        public DateTime fechaActualizacion { get; set; }

        public string codigoModular { get; set; }

        public string anexo { get; set; }

        public int anioCulminacion { get; set; }

        public int tipDocEstudiante { get; set; }
        public int numDocEstudiante { get; set; }
        public int tipDocSolicitante { get; set; }
        public int numDocSolicitante { get; set; }
        public int ciclo { get; set; }
        public string descripcionMotivo { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string nombre { get; set; }
        public string correoElectronico { get; set; }
        public string director { get; set; }
        public string usuario { get; set; }
    }
}
