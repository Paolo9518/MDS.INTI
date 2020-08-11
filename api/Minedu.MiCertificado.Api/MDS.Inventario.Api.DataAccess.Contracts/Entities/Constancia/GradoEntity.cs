using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.DataAccess.Contracts.Entities.Constancia
{
    public class GradoEntity
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

        public bool ACTIVO { get; set; }
        public string ESTADO { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public DateTime FECHA_ACTUALIZACION { get; set; }
    }
}
