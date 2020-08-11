using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models.Siagie
{
    public class EstudiantePersonaMatriculaResponse
    {
        public int idPersonaEstudiante { get; set; }

        public int idMatricula { get; set; }

        public string codMod { get; set; }

        public string anexo { get; set; }
    }
}
