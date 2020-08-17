using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.DataAccess.Contracts.Entities
{
    public class personal
    {
        public int ID_PERSONAL { get; set; }
        public int ID_ENTIDAD { get; set; }
        public int ID_TIPO_DOCUMENTO { get; set; }
        public string NUMERO_DOCUMENTO { get; set; }
        public string APELLIDO_PATERNO { get; set; }
        public string APELLIDO_MATERNO { get; set; }
        public string NOMBRES { get; set; }
        public string NOMBRE_COMPLETO { get; set; }
        public DateTime FECHA_NACIMIENTO { get; set; }
        public DateTime FECHA_INGRESO { get; set; }
        public string NUM_LICENCIA_CONDUCIR { get; set; }
        public string SEXO { get; set; }
        public int ID_CARGO { get; set; }
        public int ID_AREA { get; set; }
        public string TELEFONO_PERSONAL { get; set; }
        public string TELEFONO_TRABAJO { get; set; }
        public int ESTADO_ACTIVO { get; set; }
        public bool ESTADO_REGISTRO { get; set; }
        public string USUARIO_CREACION { get; set; }
        public string USUARIO_MODIFICACION { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public DateTime FECHA_MODIFICACION { get; set; }
    }
}
