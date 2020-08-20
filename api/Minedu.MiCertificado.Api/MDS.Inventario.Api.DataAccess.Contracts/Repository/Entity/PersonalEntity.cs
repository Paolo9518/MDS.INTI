using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.DataAccess.Contracts.Entities.Certificado
{
    public class PersonalEntity : personal
    {
        public int ID_ROL { get; set; }
        public string DSC_ROL { get; set; }
    }
}
