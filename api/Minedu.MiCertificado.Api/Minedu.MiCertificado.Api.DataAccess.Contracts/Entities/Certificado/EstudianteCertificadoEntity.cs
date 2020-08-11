using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Certificado
{
    public class EstudianteCertificadoEntity
    {
        public int ID_ESTUDIANTE { get; set; }

        public int ID_PERSONA { get; set; }
        public string ID_TIPO_DOCUMENTO { get; set; }
        public string NUMERO_DOCUMENTO { get; set; }
        public string APELLIDO_PATERNO { get; set; }
        public string APELLIDO_MATERNO { get; set; }
        public string NOMBRES { get; set; }
        public string UBIGEO { get; set; }
        public string DEPARTAMENTO { get; set; }
        public string PROVINCIA { get; set; }
        public string DISTRITO { get; set; }

        public DateTime FECHA_CREACION { get; set; }
        public DateTime FECHA_ACTUALIZACION { get; set; }
        public string USUARIO { get; set; }
    }
}
