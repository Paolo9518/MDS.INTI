using System.ComponentModel.DataAnnotations;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Constancia
{
    public class SolicitanteRequest
    {
        //[Required]
        public string idPersonaApoderado { get; set; }

        //[Required]
        //[StringLength(1, MinimumLength = 1, ErrorMessage = "Tipo de Documento inválido")]
        public string tipoDocApoderado { get; set; }

        //[Required]
        //[StringLength(8, MinimumLength = 8, ErrorMessage = "Documento de Identidad inválido")]
        public string nroDocApoderado { get; set; }

        //public string departamentoApoderado { get; set; }

        //public string provinciaApoderado { get; set; }

        //[Required]
        //[StringLength(6, MinimumLength = 6, ErrorMessage = "Ubigeo inválido")]
        public string ubigeoApoderado { get; set; }
    }
}
