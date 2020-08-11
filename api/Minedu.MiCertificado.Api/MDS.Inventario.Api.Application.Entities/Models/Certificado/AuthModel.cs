namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
{
    public class AuthModel
    {
        public AuthModel()
        {
        }

        public string usuario { get; set; }
        public string contrasenia { get; set; }
        public string token { get; set; }
        //public string usr_log_usr { get; set; }
        //public string fecha_inicio_conexion { get; set; }
        //public string id_sistema_id { get; set; }
        //public string nombre_estacion { get; set; }
        //public string mac_address { get; set; }

    }

    public class AuthUserModel
    {
        public string token { get; set; }
    }
}
