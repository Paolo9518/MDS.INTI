using System;

namespace MDS.Inventario.Api.DataAccess.Contracts.Entities.Certificado
{
    public class SolicitudExtend : SolicitudCertificadoEntity
    {
        public int ID_PERSONA { get; set; }
        public string ID_TIPO_DOCUMENTO { get; set; }
        public string TIPO_DOCUMENTO { get; set; }
        public string NUMERO_DOCUMENTO { get; set; }
        public string NOMBRE_ESTUDIANTE { get; set; }
        public string APELLIDO_PATERNO { get; set; }
        public string APELLIDO_MATERNO { get; set; }
        public string DSC_ESTADO_SOLICITUD { get; set; }
        public string NOMBRE { get; set; }
        public string CORREO_ELECTRONICO { get; set; }
        public int ULTIMO_ANIO { get; set; }
        public string DSC_MOTIVO { get; set; }
        public string FECHA_INICIO { get; set; }
        public string FECHA_FIN { get; set; }
        public string FECHA_SOLICITUD { get; set; }
        public DateTime FECHA_PROCESO { get; set; }
        public string COD_SOLICITUD { get; set; }
        public int ESTADO_INFORMACION { get; set; }
        public string DSC_ESTADO_ESTUDIANTE { get; set; }
        public int TIP_DOC_ESTUDIANTE { get; set; }
        public int NUM_DOC_ESTUDIANTE { get; set; }
        public int TIP_DOC_SOLICITANTE { get; set; }
        public int NUM_DOC_SOLICITANTE { get; set; }

        public int NUM_SOLICITUDES { get; set; }
        public string DESCRIPCION_MOTIVO { get;set;}

        public int rowsPerPage { get; set; }
        public int pageNumber { get; set; }
        public int TotalRegistros { get; set; }
        public string usuario { get; set; }


    }
}
