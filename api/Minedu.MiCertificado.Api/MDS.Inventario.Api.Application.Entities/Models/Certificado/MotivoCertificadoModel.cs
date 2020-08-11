namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
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
