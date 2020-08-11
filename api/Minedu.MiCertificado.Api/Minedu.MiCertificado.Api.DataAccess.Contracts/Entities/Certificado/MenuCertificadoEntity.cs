using System;

namespace Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Certificado
{
    public class MenuCertificadoEntity
    {
        public int ID_MENU { get; set; }

        public string URL { get; set; }
        public string NOMBRE_ICONO { get; set; }
        public string DESCRIPCION_CORTA { get; set; }
        public string DESCRIPCION { get; set; }

        public bool ACTIVO { get; set; }
        public string ESTADO { get; set; }
        public string USUARIO_CREACION { get; set; }
        public string USUARIO_ACTUALIZACION { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public DateTime FECHA_ACTUALIZACION { get; set; }
    }
}
