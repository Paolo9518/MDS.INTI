using System;

namespace Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Constancia
{
    public class MotivoEntity
    {
        public int ID_MOTIVO { get; set; }

        public string DESCRIPCION { get; set; }

        public bool ACTIVO { get; set; }
        public string ESTADO { get; set; }
        public string USUARIO_CREACION { get; set; }
        public string USUARIO_ACTUALIZACION { get; set; }
        public bool REQUIERE_DETALLE { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public DateTime FECHA_ACTUALIZACION { get; set; }
    }
}
