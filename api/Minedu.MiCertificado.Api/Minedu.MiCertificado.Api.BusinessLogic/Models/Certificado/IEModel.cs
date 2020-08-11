namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class IEModel
    {
        public IEModel()
        {
        }

        public int tipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string idRol { get; set; }
        public string descripcionRol { get; set; }
        public string codigoModular { get; set; }
        public string anexo { get; set; }
        public int tipoSede { get; set; }
        public int? porDefecto { get; set; }
        public string nivel { get; set; }
        public string centroEducativo { get; set; }

        public string nombres_usuario { get; set; }
        public string fullNombres { get; set; }
        public bool usuarioAutenticacion_IIResult { get; set; }
        public string usr_login { get; set; }

        public string token { get; set; }
        //HJSH
        public string estado { get; set; }
        public string usuario { get; set; }

    }
}
