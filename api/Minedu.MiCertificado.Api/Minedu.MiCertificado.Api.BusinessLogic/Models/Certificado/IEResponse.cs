namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class IEResponse
    {
        public string codigoModular { get; set; }
        public string anexo { get; set; }
        
        //1public string IdRol { get; set; }
        //public string DescripcionRol { get; set; }        
        public string centroEducativo { get; set; }

        /*public string idNivel { get; set; }
        public string descripcionNivel { get; set; }
        public string idModalidad { get; set; }*/
        public NivelResponse nivel { get; set; }

    }
}
