using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class NotasRequest
    {
        public int IdConstanciaNota { get; set; }
        public int IdSolicitud { get; set; }
        public int IdAnio { get; set; }
        public string CodigoModular { get; set; }
        public string Anexo { get; set; }
        public string IdNivel { get; set; }
        public string DscNivel { get; set; }
        public string IdGrado { get; set; }
        public string DscGrado { get; set; }
        public string IdTipoArea { get; set; }
        public string DscTipoArea { get; set; }
        public string IdArea { get; set; }
        public string DscArea { get; set; }
        public int EsConducta { get; set; }
        public string NotaFinal { get; set; }
        public string CodigoSolicitud { get; set; }
        public string Estado { get; set; }
        public int Ciclo { get; set; }
        public int EsAreaSiagie { get; set; }
        public bool Activo { get; set; }
        public string usuario { get; set; }
    }
}

 
 
