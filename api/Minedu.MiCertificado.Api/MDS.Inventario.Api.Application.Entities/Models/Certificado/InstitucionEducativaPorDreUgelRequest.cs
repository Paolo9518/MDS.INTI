namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
{
    public class InstitucionEducativaPorDreUgelRequest
    {
        public string CodigoDre { get; set; }
        public string CodigoUgel { get; set; }
        public string CodigoModular { get; set; }
        public string NombreIE { get; set; }
        public string anexo { get; set; }
        public string IdNivel { get; set; }
        public int pageNumber { get; set; }
        public int rowsPerPage { get; set; }

        
    }

}