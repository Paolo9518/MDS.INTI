namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class MotivoCertificadoModel
    {
        public MotivoCertificadoModel()
        {

        }

        public string idMotivo { get; set; }

        public string descripcion { get; set; }

        public bool requiereDetalle { get; set; }
    }
}
