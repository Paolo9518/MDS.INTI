using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.DataAccess.Contracts.Entities
{
    public class entidad
    {
        public int ID_ENTIDAD { get; set; }
        public string RAZON_SOCIAL { get; set; }
        public string RUC { get; set; }
        public string DIRECCION { get; set; }
        public int ESTADO_ACTIVO { get; set; }
        public bool ESTADO_REGISTRO { get; set; }
        public string USUARIO_CREACION { get; set; }
        public string USUARIO_MODIFICACION { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public DateTime FECHA_MODIFICACION { get; set; }
    }
}
