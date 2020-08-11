using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Entities.Models.Certificado
{
    public class InformacionRequest2
    {
        public string modalidad { get; set; }
        public string nivel { get; set; }
        public string telefonoContacto { get; set; }
        public string correoElectronico { get; set; }
        public string idMotivo { get; set; }
        public string motivo { get; set; }
        public string codigoModular { get; set; }
        public string anexo { get; set; }
        public int anioCulminacion { get; set; }
        public string solicitante { get; set; }
        public string estadoSolicitud { get; set; }
        public int ciclo { get; set; }
        public string idNivel { get; set; }
        public ColegioRequest2 colegio { get; set; }
    }
}