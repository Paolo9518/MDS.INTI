using System;

namespace MDS.Inventario.Api.DataAccess.Contracts.Entities.Certificado
{
    public class USP_INTERNO_CERTIFICADO_LISTAR_REGISTROS_ESTUDIANTE_Result
    {
        public int ID_ANIO { get; set; }
        public int CICLO { get; set; }
        public string ID_GRADO { get; set; }
        public string ID_NIVEL { get; set; }
        public string ID_MODALIDAD { get; set; }
        public string COD_MOD { get; set; }
        public string ANEXO { get; set; }
        public int ID_SOLICITUD { get; set; }
    }
}
