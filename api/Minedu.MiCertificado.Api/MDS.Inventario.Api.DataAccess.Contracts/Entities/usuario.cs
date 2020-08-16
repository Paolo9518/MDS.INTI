using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.DataAccess.Contracts.Entities
{
    public class usuario
    {

        public int ID_USUARIO { get; set; }
        public int ID_PERSONAL { get; set; }
        public string USUARIO { get; set; }
        public string CONTRASENIA { get; set; }
        public int ID_ROL { get; set; }
        public int ESTADO_ACTIVO { get; set; }
        public bool ESTADO_REGISTRO { get; set; }
        public string USUARIO_CREACION { get; set; }
        public string USUARIO_MODIFICACION { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public DateTime FECHA_MODIFICACION { get; set; }
    }
}
