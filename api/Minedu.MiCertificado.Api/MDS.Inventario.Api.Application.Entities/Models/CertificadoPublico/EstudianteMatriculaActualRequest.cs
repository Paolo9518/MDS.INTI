﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
{
    public class EstudianteMatriculaActualRequest
    {

        public string tipoDocumento { get; set; }
        public string nroDocumento { get; set; }
        public string idNivel { get; set; }

        public string codMod { get; set; }
        public string anexo { get; set; }
    }
}
