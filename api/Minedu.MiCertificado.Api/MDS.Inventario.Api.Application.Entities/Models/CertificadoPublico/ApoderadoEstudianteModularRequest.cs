using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
{
    public class ApoderadoEstudianteModularRequest
    {
        public int idPersonaApoderado { get; set; }
        public int idPersonaEstudiante { get; set; }
    }
}
