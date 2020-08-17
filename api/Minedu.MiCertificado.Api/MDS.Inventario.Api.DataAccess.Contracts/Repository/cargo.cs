using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.DataAccess.Contracts.Entities
{
    public class cargo
    {
        public int ID_CARGO { get; set; }
        public string DESCRIPCION_CARGO { get; set; }
        public int ESTADO_ACTIVO { get; set; }
        public bool ESTADO_REGISTRO { get; set; }
        public string USUARIO_CREACION { get; set; }
        public string USUARIO_MODIFICACION { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public DateTime FECHA_MODIFICACION { get; set; }
    }
}
