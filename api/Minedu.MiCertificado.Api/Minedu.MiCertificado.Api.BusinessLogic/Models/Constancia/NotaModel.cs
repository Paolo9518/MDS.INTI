using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Constancia
{
    public class NotaModel
    {
        public int idConstanciaNota { get; set; }
        public int idSolicitud { get; set; }

        public int idAnio { get; set; }
        public string codMod { get; set; }
        public string anexo { get; set; }
        public string idNivel { get; set; }
        public string dscNivel { get; set; }
        public string idGrado { get; set; }
        public string dscGrado { get; set; }
        public string idTipoArea { get; set; }
        public string dscTipoArea { get; set; }
        public string idArea { get; set; }
        public string dscArea { get; set; }
        public int esconducta { get; set; }
        public string notaFinalArea { get; set; }
    }
}
