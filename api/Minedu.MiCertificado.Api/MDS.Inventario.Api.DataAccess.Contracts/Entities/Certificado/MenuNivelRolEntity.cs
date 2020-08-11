using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.DataAccess.Contracts.Entities.Certificado
{
    //EDWX NO ES TABLA
    public class MenuNivelRolEntity
    {
        public string ID_ROL { get; set; }
        public int ID_MENU_NIVEL { get; set; }
        public string DSC_MENU_NIVEL { get; set; }
        public string DSC_ROL { get; set; }
    }
}
