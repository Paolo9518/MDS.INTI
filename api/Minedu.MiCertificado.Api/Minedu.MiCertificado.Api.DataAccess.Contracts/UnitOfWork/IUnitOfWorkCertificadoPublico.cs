using Minedu.Comun.IData;
using Entities = Minedu.MiCertificado.Api.DataAccess.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Minedu.MiCertificado.Api.DataAccess.Contracts.UnitOfWork
{
    public partial interface IUnitOfWork : IBaseUnitOfWork
    {
        Task<IEnumerable<Entities.Certificado.InstitucionCertificadoEntity>> ObtenerCertificadoInstitucion(string CODIGO_MODULAR);
        Task<IEnumerable<Entities.Certificado.SolicitudExtend>> ObtenerSolicitudesCertificadoPorEstudiante(string ID_TIPO_DOCUMENTO, string NUMERO_DOCUMENTO, string ID_MODALIDAD, string ID_NIVEL, string ESTADO_SOLICITUD);

        //Task<int> InsertarEstudianteCertificado(Entities.Certificado.EstudianteCertificadoEntity entity);
        //Task<int> InsertarSolicitanteCertificado(Entities.Certificado.SolicitanteCertificadoEntity entity);
        //Task<int> InsertarSolicitudCertificado(Entities.Certificado.SolicitudCertificadoEntity entity);
        void DesactivarSolicitudesCertificado(int ID_SOLICITUD, int ID_PERSONA, string ID_MODALIDAD, string ID_NIVEL);
        Task<int> InsertarGradoCertificado(Entities.Certificado.GradoCertificadoEntity entity);
        Task<int> InsertarNotaCertificado(Entities.Certificado.NotaCertificadoEntity entity);
        Task<int> InsertarObservacionCertificado(Entities.Certificado.ObservacionCertificadoEntity entity);
        Task<IEnumerable<Entities.Certificado.SolicitudExtend>> ObtenerSolicitudCertificado(Entities.Certificado.SolicitudExtend entity);

        Task<IEnumerable<Entities.Certificado.SolicitudExtend>> ObtenerSolicitudCertificadoPorCodigoVirtual2(string CODIGO_VIRTUAL, string ID_TIPO_DOCUMENTO, string NUMERO_DOCUMENTO);
        Task<IEnumerable<Entities.Certificado.EstudianteCertificadoEntity>> ObtenerEstudianteCertificadoValidado(int ID_ESTUDIANTE);
        Task<IEnumerable<Entities.Certificado.GradoCertificadoEntity>> ObtenerGradosCertificadoValidados(int ID_SOLICITUD);
        Task<IEnumerable<Entities.Certificado.NotaCertificadoEntity>> ObtenerNotasCertificadoValidadas(int ID_SOLICITUD);
        Task<IEnumerable<Entities.Certificado.ObservacionCertificadoEntity>> ObtenerObservacionesCertificadoValidadas(int ID_SOLICITUD);
    }
}
