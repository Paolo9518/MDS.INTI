namespace MDS.Inventario.Api.Application.Entities.Models.Central
{
    public class BEUsuarioPermisoResponse
    {
        //public string usrLogin { get; set; }
        //public string nombresUsuario { get; set; }
        //public string apellidoPaternoUsuario { get; set; }
        //public string apellidoMaternoUsuario { get; set; }

        //public short tipoDocumento { get; set; }
        //public string numeroDocumento { get; set; }

        public string idRol { get; set; }
        public string rolDescripcion { get; set; }

        //public string csts { get; set; }
        public string codigo { get; set; }
        public int tipoSede { get; set; }

        public string idSede { get; set; }
        public string idSedeAnx { get; set; }
        public string cenEdu { get; set; }
        public string dre { get; set; }
        public string codigel { get; set; }

        public int porDefecto { get; set; }

        //public string correoElectronico { get; set; }
        
        public bool descentralizadoUp { get; set; }
        public short estadoUsuarioPermiso { get; set; }

        //extras
        public string idModalidad { get; set; }
        public string idNivel { get; set; }
        public string dscNivel { get; set; }
        public string idSistema { get; set; }
    }
}
