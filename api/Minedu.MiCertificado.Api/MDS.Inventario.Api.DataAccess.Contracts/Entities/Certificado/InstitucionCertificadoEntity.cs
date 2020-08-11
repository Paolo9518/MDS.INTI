using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.DataAccess.Contracts.Entities.Certificado
{
    public class InstitucionCertificadoEntity
    {
        public int ID_INSTITUCION { get; set; }
        public string CODIGO_MODULAR { get; set; }
        public string ANEXO { get; set; }
        public string NOMBRE_INSTITUCION { get; set; }
        public string ID_NIVEL { get; set; }
        public string DRE { get; set; }
        public string ESTADO { get; set; }

        public DateTime FECHA_CREACION { get; set; }
        public DateTime FECHA_ACTUALIZACION { get; set; }
    }
}
