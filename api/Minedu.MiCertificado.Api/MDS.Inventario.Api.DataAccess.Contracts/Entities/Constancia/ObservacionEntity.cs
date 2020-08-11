using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.DataAccess.Contracts.Entities.Constancia
{
    public class ObservacionEntity
    {
        public int ID_CONSTANCIA_OBSERVACION { get; set; }
        public int ID_SOLICITUD { get; set; }
        public string ID_NIVEL { get; set; }
        public int ID_ANIO { get; set; }
        public string RESOLUCION { get; set; }
        public int TIPO_SOLICITUD { get; set; }
        public string MOTIVO { get; set; }
        public int ID_TIPO { get; set; }
        public string DSC { get; set; }
        public int TIPO_OBS { get; set; }

        public bool ACTIVO { get; set; }
        public string ESTADO { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public DateTime FECHA_ACTUALIZACION { get; set; }
    }
}
