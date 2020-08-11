namespace MDS.Inventario.Api.Application.Entities.Models.Constancia
{
    public class MotivoModel
    {
        public MotivoModel()
        {

        }

        public string idMotivo { get; set; }

        public string descripcion { get; set; }

        public bool requiereDetalle { get; set; }
    }
}
