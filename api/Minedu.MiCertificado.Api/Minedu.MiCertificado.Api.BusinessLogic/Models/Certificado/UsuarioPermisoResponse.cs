namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class UsuarioPermisoResponse
    {
        public string usr_login { get; set; }
        public string id_sistema { get; set; }
        public int? tipo_sede { get; set; }
        public string id_sede { get; set; }
        public string id_sede_anx { get; set; }
        public int? por_defecto { get; set; }
        public int? nivel { get; set; }
        public string cen_edu { get; set; }
        public bool? descentralizado_up { get; set; }

        public string fullname { get; set; }
        public string rolDescripcion { get; set; }

        //public UsuarioPermiso certificado { get; set; }
        public bool certificado { get; set; }
    }
}
