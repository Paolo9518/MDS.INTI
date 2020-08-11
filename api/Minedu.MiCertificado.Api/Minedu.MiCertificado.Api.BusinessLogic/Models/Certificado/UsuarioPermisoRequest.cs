using System;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class UsuarioPermisoRequest
    {
        public string usr_login { get; set; }
        public string id_sistema { get; set; }
        public int? tipo_sede { get; set; }
        public string id_sede { get; set; }
        public string id_sede_anx { get; set; }
        public string codigo { get; set; }
        public int? por_defecto { get; set; }
        public int? nivel { get; set; }
        public string verificacion { get; set; }
        public DateTime? fechaprimeringreso { get; set; }
        public string idrol { get; set; }
        public string doc_referencia { get; set; }
        public short estado_usuario_permiso { get; set; }
        public string cen_edu { get; set; }
        public bool? descentralizado_up { get; set; }
        public string usuario_registro { get; set; }
        public DateTime? fecha_registro { get; set; }
        public string usuario_modificador { get; set; }
        public DateTime? fecha_modificacion { get; set; }

        public string id_sistema_id { get; set; }
    }
}