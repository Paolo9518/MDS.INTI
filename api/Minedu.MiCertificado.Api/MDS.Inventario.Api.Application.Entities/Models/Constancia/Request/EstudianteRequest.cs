using System.ComponentModel.DataAnnotations;

namespace MDS.Inventario.Api.Application.Entities.Models.Constancia
{
    public class EstudianteRequest
    {
        //[Required]
        public string idPersonaEstudiante { get; set; }

        //[Required]
        //[StringLength(1, MinimumLength = 1, ErrorMessage = "Tipo de Documento inválido")]
        public string tipoDocEstudiante { get; set; }

        //[Required]
        //[StringLength(8, MinimumLength = 8, ErrorMessage = "Documento de Identidad inválido")]
        public string nroDocEstudiante { get; set; }

        //public string departamentoEstudiante { get; set; }

        //public string provinciaEstudiante { get; set; }

        //[Required]
        //[StringLength(6, MinimumLength = 6, ErrorMessage = "Ubigeo inválido")]
        public string ubigeoEstudiante { get; set; }
    }
}