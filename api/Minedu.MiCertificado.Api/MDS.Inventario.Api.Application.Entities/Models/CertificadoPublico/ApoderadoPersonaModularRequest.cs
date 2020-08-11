namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
{
    public class ApoderadoPersonaModularRequest
    {
        public string tipoDocApoderado { get; set; }
        public string nroDocApoderado { get; set; }
        public string nombrePadreApoderado { get; set; }
        public string nombreMadreApoderado { get; set; }
        public string fechaNacimientoApoderado { get; set; }
        public string ubigeoApoderado { get; set; }

        public string codModular { get; set; }
        public string anexo { get; set; }
    }
}
