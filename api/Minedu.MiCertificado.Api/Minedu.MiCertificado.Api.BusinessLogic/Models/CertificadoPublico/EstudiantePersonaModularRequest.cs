namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class EstudiantePersonaModularRequest
    {
        public string tipoSolicitante { get; set; }

        //public string idPersonaApoderado { get; set; }
        public string tipoDocApoderado { get; set; }
        public string nroDocApoderado { get; set; }

        public string tipoDocEstudiante { get; set; }
        public string nroDocEstudiante { get; set; }
        public string nombrePadreEstudiante { get; set; }
        public string nombreMadreEstudiante { get; set; }
        public string fechaNacimientoEstudiante { get; set; }
        public string ubigeoEstudiante { get; set; }

        public string codModular { get; set; }
        public string anexo { get; set; }
        public string idModalidad { get; set; }
        public string idNivel { get; set; }
    }
}
