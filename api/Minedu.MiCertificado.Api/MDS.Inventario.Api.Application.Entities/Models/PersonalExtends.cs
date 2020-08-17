using MDS.Inventario.Api.DataAccess.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models
{
    public class PersonalExtends: Personal
    {
        public int IdRol { get; set; }
        public string DscRol { get; set; }
    }
}
