using System;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Constancia
{
    public class SolicitudModel
    {
        public int idSolicitud { get; set; }

        public int idEstudiante { get; set; }
        public int idSolicitante { get; set; }

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

        public string codigoVirtual { get; set; }

        public DateTime fechaCreacion { get; set; }

    }
}
