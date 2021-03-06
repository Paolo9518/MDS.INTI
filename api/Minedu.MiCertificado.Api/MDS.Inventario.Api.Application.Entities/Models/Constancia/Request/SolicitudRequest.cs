﻿using System.ComponentModel.DataAnnotations;

namespace MDS.Inventario.Api.Application.Entities.Models.Constancia
{
    public class SolicitudRequest
    {
        /*[Required]
        public string request { get; set; }*/

        [Required]
        public TipoSolicitudRequest tipo { get; set; }

        [Required]
        public SolicitanteRequest solicitante { get; set; }

        [Required]
        public EstudianteRequest estudiante { get; set; }

        [Required]
        public InformacionRequest solicitud { get; set; }
    }
}
