using MDS.Inventario.Api.DataAccess.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public int Idpersonal { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasenia { get; set; }
        public int IdRol { get; set; }
    }
}
