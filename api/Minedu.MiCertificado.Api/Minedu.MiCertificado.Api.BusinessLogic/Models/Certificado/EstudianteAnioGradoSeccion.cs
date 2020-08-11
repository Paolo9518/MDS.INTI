using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class EstudianteAnioGradoSeccion
    {
        public string codMod { get; set; }
        public string anexo { get; set; }
        public int idAnio { get; set; }
        public string idNivel { get; set; }
        public string idGrado { get; set; }
        public string idSeccion { get; set; }
        public int idPersona { get; set; }
        public string trasladoExterno { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string nombres { get; set; }
        public string nombreCompleto { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public string codigoEstudiante { get; set; }
        public int idMatricula { get; set; }
        public int estadoMatricula { get; set; }
        public DateTime fechaMatricula { get; set; }
        public string numeroDocumento { get; set; }
        public int validadoReniec { get; set; }
        public string idModulo { get; set; }
        public string descNivel { get; set; }
        public string idModalidad { get; set; }
        public string descModalidad { get; set; }
        public string abreModalidad { get; set; }
        public string descGrado { get; set; }
        public bool generar { get; set; }
        public string descEstadoSolicitud { get; set; }
        public int estadoInformacion { get; set; }
        public string codPersona { get; set; }
        public int estado { get; set; }
        public string estadoSolicitud { get; set; }
    }
}
