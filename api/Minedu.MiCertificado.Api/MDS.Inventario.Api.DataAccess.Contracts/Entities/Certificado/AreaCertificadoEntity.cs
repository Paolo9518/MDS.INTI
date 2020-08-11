using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.DataAccess.Contracts.Entities.Certificado
{
    public class AreaCertificadoEntity
    {
        public string ID_TIPO_AREA { get; set; }
        public string ID_DISENIO { get; set; }
        public int ID_ANIO { get; set; }
        public string ID_NIVEL { get; set; }
        public string ID_AREA { get; set; }
        public string DSC_AREA { get; set; }
        public int NUM_AREAS { get; set; }
        public int ANIO_INICIO { get; set; }
        public int ANIO_FIN { get; set; }

    }
}
