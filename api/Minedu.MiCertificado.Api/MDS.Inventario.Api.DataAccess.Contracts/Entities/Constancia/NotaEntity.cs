using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.DataAccess.Contracts.Entities.Constancia
{
    public class NotaEntity
    {
        public int ID_CONSTANCIA_NOTA { get; set; }
        public int ID_SOLICITUD { get; set; }

        public int ID_ANIO { get; set; }
        public string COD_MOD { get; set; }
        public string ANEXO { get; set; }
        public string ID_NIVEL { get; set; }
        public string DSC_NIVEL { get; set; }
        public string ID_GRADO { get; set; }
        public string DSC_GRADO { get; set; }
        public string ID_TIPO_AREA { get; set; }
        public string DSC_TIPO_AREA { get; set; }
        public string ID_AREA { get; set; }
        public string DSC_AREA { get; set; }
        public int ESCONDUCTA { get; set; }
        public string NOTA_FINAL_AREA { get; set; }

        public bool ACTIVO { get; set; }
        public string ESTADO { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public DateTime FECHA_ACTUALIZACION { get; set; }
    }
}
