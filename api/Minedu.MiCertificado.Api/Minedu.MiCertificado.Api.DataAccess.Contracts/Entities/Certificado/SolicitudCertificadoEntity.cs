using System;

namespace Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Certificado
{
    public class SolicitudCertificadoEntity
    {
        public int ID_SOLICITUD { get; set; }

        public int ID_ESTUDIANTE { get; set; }
        public int? ID_SOLICITANTE { get; set; }

        public int ID_MOTIVO { get; set; }
        public string MOTIVO_OTROS { get; set; }

        public string ID_MODALIDAD { get; set; }
        public string ABR_MODALIDAD { get; set; }
        public string DSC_MODALIDAD { get; set; }
        public string ID_NIVEL { get; set; }
        public string DSC_NIVEL { get; set; }
        public string ID_GRADO { get; set; }
        public string DSC_GRADO { get; set; }
        public string ESTADO_SOLICITUD { get; set; }
        public string ESTADO_ESTUDIANTE { get; set; }

        public string CODIGO_VIRTUAL { get; set; }

        public bool ACTIVO { get; set; }
        public string ESTADO { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public DateTime FECHA_ACTUALIZACION { get; set; }

        public string CODIGO_MODULAR { get; set; }
        public string ANEXO { get; set; }
        public int ANIO_CULMINACION { get; set; }
        public int CICLO { get; set; }
        public string DIRECTOR { get; set; }
    }
}
