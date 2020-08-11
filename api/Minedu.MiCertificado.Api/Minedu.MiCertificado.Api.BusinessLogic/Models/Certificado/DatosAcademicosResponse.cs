using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Certificado;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado
{
    public class DatosAcademicosResponse
    {
        public string fechaNacimiento { get; set; }
        public List<GradoCertificadoModel> grados { get; set; }
        public List<PDFNotaCertificado> cursoList { get; set; }
        public List<PDFNotaCertificado> tallerList { get; set; }
        public List<PDFNotaCertificado> competenciaList { get; set; }
        public List<ObservacionCertificadoEntity> observaciones { get; set; }
    }
    public class DatosEstudianteResponse
    {
        public Models.Siagie.EstudianteInfoPorNivel2 estudiante{get;set;}
        public List<GradoCertificadoModel> gradosList { get; set; }
        public List<PDFNotaCertificado> cursoList { get; set; }
        public List<PDFNotaCertificado> tallerList { get; set; }
        public List<PDFNotaCertificado> competenciaList { get; set; }
        public List<ObservacionCertificadoModel> observacionesList { get; set; }
    }
}
