using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Siagie
{
    public class EstudiantePersonaMatriculaResponse
    {
        public int idPersonaEstudiante { get; set; }

        public int idMatricula { get; set; }

        public string codMod { get; set; }

        public string anexo { get; set; }
    }
}
