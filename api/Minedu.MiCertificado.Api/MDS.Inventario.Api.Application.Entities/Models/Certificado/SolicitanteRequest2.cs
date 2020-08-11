using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
{
    public class SolicitanteRequest2
    {
        public int idPersonaApoderado { get; set; }

        public string tipoDocApoderado { get; set; }
        public string nroDocApoderado { get; set; }
        public string nombresApoderado { get; set; }
        public string apellidoPaternoApoderado { get; set; }
        public string apellidoMaternoApoderado { get; set; }
        public string departamentoApoderado { get; set; }
        public string provinciaApoderado { get; set; }
        public string ubigeoApoderado { get; set; }


    }

    public class SolicitanteModel
    {
        public string numeroDocumento { get; set; }
    }
}
