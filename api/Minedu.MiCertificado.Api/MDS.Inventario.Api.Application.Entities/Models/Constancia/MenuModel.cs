namespace MDS.Inventario.Api.Application.Entities.Models.Constancia
{
    public class MenuModel
    {
        public MenuModel()
        {
        }

        public string idMenu { get; set; }

        public string ruta { get; set; }
        public string nombreIcono { get; set; }
        public string descripcionCorta { get; set; }
        public string descripcion { get; set; }
    }
}
