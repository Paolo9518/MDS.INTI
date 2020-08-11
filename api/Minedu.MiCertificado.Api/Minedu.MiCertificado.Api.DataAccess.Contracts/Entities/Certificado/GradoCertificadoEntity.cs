using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Certificado
{
    public class GradoCertificadoEntity
    {
        public int ID_CONSTANCIA_GRADO { get; set; }
        public int ID_SOLICITUD { get; set; }
        public string ID_GRADO { get; set; }
        public string DSC_GRADO { get; set; }
        public string CORR_ESTADISTICA { get; set; }
        public int ID_ANIO { get; set; }
        public string COD_MOD { get; set; }
        public string ANEXO { get; set; }
        public string SITUACION_FINAL { get; set; }
        public int CICLO { get; set; }

        public bool ACTIVO { get; set; }
        public string ESTADO { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public DateTime FECHA_ACTUALIZACION { get; set; }
        public string USUARIO { get; set; }
    }
}
