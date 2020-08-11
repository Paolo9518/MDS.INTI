namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class GradoSeccionRequest
    {
        public string CodigoModular { get; set; }

        public string Anexo { get; set; }

        public int IdAnio { get; set; }

        public string IdNivel { get; set; }

        public string IdGrado { get; set; }

        public string IdSeccion { get; set; }

        public string IdFase { get; set; }

        public string EstadoSolicitud { get; set; }
    }

}