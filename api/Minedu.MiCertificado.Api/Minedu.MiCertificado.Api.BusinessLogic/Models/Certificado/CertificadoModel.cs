using System;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class CertificadoModel
    {
        public int IdSolicitud { get; set; }
        public int IdEstudiante { get; set; }
        public int? IdSolicitante { get; set; }
        public int IdMotivo { get; set; }
        public int IdPersona { get; set; }

        public string IdNivel { get; set; }
        public string IdGrado { get; set; }
        public string IdModalidad { get; set; }

        public string IdTipoDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }        
        public string NombresEstudiante { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Nombre { get; set; }
        public string CorreoElectronico { get; set; }
        public int UltimoAnio { get; set; }


        public string FechaSolicitud { get; set; }
        public string EstadoSolicitud { get; set; }
        public string DescripcionEstadoSolicitud { get; set; }              
        public string AbreviaturaModalidad { get; set; }
        public string DescripcionModalidad { get; set; }       
        public string DescripcionNivel { get; set; }        
        public string DescripcionGrado { get; set; }
        public DateTime FechaProceso { get; set; }
        public string CodigoVirtual { get; set; }
        public string DescripcionMotivo { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }

        public int rowsPerPage { get; set; }
        public int pageNumber { get; set; }
        public int TotalRegistros { get; set; }
        public string CodSolicitud { get; set; }
        public string CodMotivo { get; set; }
        public string CodigoModular { get; set; }
        public string Anexo { get; set; }

        public string EstadoEstudiante { get; set; }
        public string DscEstadoEstudiante { get; set; }
        public int Ciclo { get; set; }
        public string CodigoPersona { get; set; }
        public string usuario { get; set; }
    }
}
