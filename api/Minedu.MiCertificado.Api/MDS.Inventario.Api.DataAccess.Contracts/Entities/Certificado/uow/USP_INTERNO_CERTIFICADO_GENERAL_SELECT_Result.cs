using System;

namespace MDS.Inventario.Api.DataAccess.Contracts.Entities.Certificado
{
    public class USP_INTERNO_CERTIFICADO_GENERAL_SELECT_Result
    {
        public int ID_SOLICITUD { get; set; }
        public int ID_ESTUDIANTE { get; set; }
        public int? ID_SOLICITANTE { get; set; }
        public int ID_MOTIVO { get; set; }
        public int ID_PERSONA { get; set; }

        public string ID_MODALIDAD { get; set; }
        public string ID_NIVEL { get; set; }
        public string ID_GRADO { get; set; }

        public string ID_TIPO_DOCUMENTO { get; set; }
        public string TIPO_DOCUMENTO { get; set; }
        public string NUMERO_DOCUMENTO { get; set; }
        public string NOMBRE_ESTUDIANTE { get; set; }
        public string APELLIDO_PATERNO { get; set; }
        public string APELLIDO_MATERNO { get; set; }
        public string ESTADO_SOLICITUD { get; set; }
        public string DSC_ESTADO_SOLICITUD { get; set; }
        public bool ACTIVO { get; set; }
        public string ESTADO { get; set; }
        public string FECHA_SOLICITUD { get; set; }
        public DateTime FECHA_ACTUALIZACION { get; set; }
        public string ABR_MODALIDAD { get; set; }
        public string DSC_MODALIDAD { get; set; }
        public string DSC_NIVEL { get; set; }
        public string DSC_GRADO { get; set; }
        public DateTime FECHA_PROCESO { get; set; }
        public string CODIGO_VIRTUAL { get; set; }
        public string NOMBRE { get; set; }
        public string CORREO_ELECTRONICO { get; set; }
        public int ULTIMO_ANIO { get; set; }
        public string DSC_MOTIVO { get; set; }

        public string FECHA_INICIO { get; set; }
        public string FECHA_FIN { get; set; }

        public int rowsPerPage { get; set; }
        public int pageNumber { get; set; }
        public int TotalRegistros { get; set; }

        public string COD_SOLICITUD { get; set; }
        public int ESTADO_INFORMACION { get; set; }
        public string CODIGO_MODULAR { get; set; }
        public string ANEXO { get; set; }

        public string ESTADO_ESTUDIANTE { get; set; }
        public string DSC_ESTADO_ESTUDIANTE { get; set; }
    }
}
