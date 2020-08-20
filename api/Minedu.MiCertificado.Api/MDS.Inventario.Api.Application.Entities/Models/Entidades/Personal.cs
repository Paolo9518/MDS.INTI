using MDS.Inventario.Api.DataAccess.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models
{
    public class Personal
    {
        public int IdPersonal { get; set; }
        public int IdEntidad { get; set; }
        public int IdTipoDocumento {get;set;}
        public string NumeroDocumento { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Nombres { get; set; }
        public string NombreCompleto { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string NumLicenciaConducir { get; set; }
        public string Sexo { get; set; }
        public int IdCargo { get; set; }
        public int IdArea { get; set; }
        public string TelefonoPersonal { get; set; }
        public string TelefonoTrabajo { get; set; }

    }
}
