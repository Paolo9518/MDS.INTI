using System;

namespace MDS.Inventario.Api.DataAccess.Contracts.Entities.Certificado
{
    //EDWX NO ES TABLA
    public class IEEntity
    {
        public int TIPO_DOCUMENTO { get; set; }
        public string NUMERO_DOCUMENTO { get; set; }
        public string ID_ROL { get; set; }
        public string DESCRIPCION_ROL { get; set; }
        public string CODIGO_MODULAR { get; set; }
        public string ANEXO { get; set; }
        public int TIPO_SEDE { get; set; }
        public int? POR_DEFECTO { get; set; }
        public string NIVEL { get; set; }
        public string CENTRO_EDUCATIVO { get; set; }

        //INCIO HJSH
        public string ESTADO { get; set; }
        public string USUARIO { get; set; }
    }
}
