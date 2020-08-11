using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Constancia
{
    public class InformacionRequest
    {
        //[Required]
        public string idModalidad { get; set; }

        //[Required]
        public string idNivel { get; set; }

        //[Phone]
        //[RegularExpression(@"^(?:\d{9})?$", ErrorMessage = "Número celular inválido")]
        //[Required(AllowEmptyStrings = true)]
        //[DisplayFormat(ConvertEmptyStringToNull = false)]
        public string telefonoContacto { get; set; }

        //[Required]
        //[RegularExpression(@"^\s*[\w\-\+_']+(\.[\w\-\+_']+)*\@[A-Za-z0-9]([\w\.-]*[A-Za-z0-9])?\.[A-Za-z][A-Za-z\.]*[A-Za-z]$", ErrorMessage = "Correo electrónico invalido")]
        public string correoElectronico { get; set; }

        //[Required]
        public string idMotivo { get; set; }

        //[MaxLength(150, ErrorMessage = "Ha sobrepasado el límite de caracteres permitido")]
        //[Required(AllowEmptyStrings = true)]
        //[DisplayFormat(ConvertEmptyStringToNull = false)]
        public string motivoOtros { get; set; }

        //public ColegioRequest colegio { get; set; }
    }
}
