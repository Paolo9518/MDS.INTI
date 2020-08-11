using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Constancia
{
    public class DeclaracionJuradaEntity
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
