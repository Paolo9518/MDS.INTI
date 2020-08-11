namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class BEUsuarioAutenticacionRequest
    {
        public string usr_log_usr { get; set; }
        public string usr_login { get; set; }
        public string usr_password { get; set; }
    }

    public class BEusuarioAutenticacionResponse
    {
        public short? TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string NombreCompleto { get; set; }
        public string UsuarioLogin { get; set; }

        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string NombresUsuario { get; set; }
        public string CentroEducativo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUgel { get; set; }
        public string IdNivelEducativo { get; set; }
        public string DscNivelEducativo { get; set; }
        public string IdModalidad { get; set; }
        public string IdSede { get; set; }
        public int? TipoSede { get; set; }
        public string Anexo { get; set; }
        public string IdRol { get; set; }
        public string DescripcionRol { get; set; }
        public int? PorDefecto { get; set; }

        public string Mensaje { get; set; }
        public bool Success { get; set; }

    }
}
