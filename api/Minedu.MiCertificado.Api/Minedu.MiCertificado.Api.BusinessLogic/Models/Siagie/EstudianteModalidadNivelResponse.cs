using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Siagie
{
    public class EstudianteModalidadNivelResponse
    {
        public int? idAnio { get; set; }
        public string codMod { get; set; }
        public string anexo { get; set; }
        public int idPersona { get; set; }
        public string idNivel { get; set; }
        public string dscNivel { get; set; }
        public string idGrado { get; set; }
        public string dscGrado { get; set; }
        public string idTipoArea { get; set; }
        public string dscTipoArea { get; set; }
        public int esConducta { get; set; }
        public string notaFinalArea { get; set; }
        public string idArea { get; set; }
        public string dscArea { get; set; }
        public string registroNota { get; set; }
        public int? estado { get; set; }
    }
}
