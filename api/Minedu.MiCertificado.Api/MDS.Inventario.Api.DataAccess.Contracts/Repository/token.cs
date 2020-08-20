using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.DataAccess.Contracts.Entities
{
    public class token
    {
        public int ID_TOKEN { get; set; }
        public int ID_ENTIDAD { get; set; }
        public string CLAVE { get; set; }
        public int ESTADO_ACTIVO { get; set; }
        public bool ESTADO_REGISTRO { get; set; }
        public string USUARIO_CREACION { get; set; }
        public string USUARIO_MODIFICACION { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public DateTime FECHA_MODIFICACION { get; set; }
    }
}
