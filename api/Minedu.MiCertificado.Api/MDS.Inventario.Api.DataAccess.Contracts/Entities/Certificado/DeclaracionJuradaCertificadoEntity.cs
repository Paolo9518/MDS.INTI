using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.DataAccess.Contracts.Entities.Certificado
{
    public class DeclaracionJuradaCertificadoEntity
    {
        public int ID_DECLARACION { get; set; }
        public string DESCRIPCION { get; set; }
        public bool ACTIVO { get; set; }
        public string ESTADO { get; set; }
        public string USUARIO_CREACION { get; set; }
        public string USUARIO_ACTUALIZACION { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public DateTime FECHA_ACTUALIZACION { get; set; }
    }
}
