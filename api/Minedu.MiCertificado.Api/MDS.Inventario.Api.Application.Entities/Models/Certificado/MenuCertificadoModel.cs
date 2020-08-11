namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
{
    public class MenuCertificadoModel
    {
        public MenuCertificadoModel()
        {
        }

        public string idMenu { get; set; }

        public string ruta { get; set; }
        public string nombreIcono { get; set; }
        public string descripcionCorta { get; set; }
        public string descripcion { get; set; }
    }
}
