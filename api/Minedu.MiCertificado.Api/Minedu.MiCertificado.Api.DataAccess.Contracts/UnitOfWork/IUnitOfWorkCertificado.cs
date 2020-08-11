using Minedu.Comun.IData;
using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Certificado;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minedu.MiCertificado.Api.DataAccess.Contracts.UnitOfWork
{
    public partial interface IUnitOfWork : IBaseUnitOfWork
    {
        Task<IEnumerable<SolicitudExtend>> GetCertificado(SolicitudExtend entity);

        Task<int> UpdateCertificado(SolicitudExtend entity);

        Task<IEnumerable<SolicitudExtend>> GetSolicitud(SolicitudExtend entity);

        Task<IEnumerable<SolicitudExtend>> GetSolicitudCertificado(SolicitudExtend entity);

        Task<int> InsertarEstudianteCertificado(EstudianteCertificadoEntity entity);
        
        Task<int> InsertarSolicitanteCertificado(SolicitanteCertificadoEntity entity);

        Task<int> InsertarSolicitudCertificado(SolicitudExtend entity);

        //Task<int> InsertarGradoCertificado(GradoCertificadoEntity entity);

        //Task<int> InsertarNotaCertificado(NotaCertificadoEntity entity);

        Task<int> InsertUpdateInstitucion(IEEntity entity);

        Task<IEnumerable<IEEntity>> PostInstitucion(IEEntity entity);

        Task<IEnumerable<SolicitudExtend>> GetSolicitudRechazada(SolicitudExtend entity);

        Task<IEnumerable<SolicitudExtend>> GetCertificadosEmitidos(SolicitudExtend entity);

        Task<IEnumerable<SolicitudExtend>> GetSolicitudesPendientes(SolicitudExtend entity);

        //Task<int> InsertarObservacionCertificado(ObservacionCertificadoEntity entity);

        Task<IEnumerable<USP_INTERNO_CERTIFICADO_VALIDAR_SOLICITUD_SELECT_Result>> GetValidarSolicitud(string TIP_DOC_ESTUDIANTE, string NUM_DOC_ESTUDIANTE, string TIP_DOC_SOLICITANTE, string NUM_DOC_SOLICITANTE, int ANIO_CULMINACION);

        void SolicitudEstadoEmitido(int ID_SOLICITUD, int ID_PERSONA, string ID_MODALIDAD, string ID_NIVEL, string DIRECTOR, string USUARIO);

        //Task<IEnumerable<SolicitudCertificadoEntity>> ObtenerSolicitudCertificadoPorCodigoVirtual(string CODIGO_VIRTUAL, string ID_TIPO_DOCUMENTO, string NUMERO_DOCUMENTO);

        //Task<IEnumerable<EstudianteCertificadoEntity>> ObtenerCertificadoEstudianteValidado(int ID_ESTUDIANTE);

        //Task<IEnumerable<GradoCertificadoEntity>> ObtenerCertificadoGradosValidados(int ID_SOLICITUD);

        //Task<IEnumerable<NotaCertificadoEntity>> ObtenerCertificadoNotasValidadas(int ID_SOLICITUD);

        Task<IEnumerable<ObservacionCertificadoEntity>> ObtenerCertificadoObservacionesValidadas(int ID_SOLICITUD);

        Task<int> InsertUpdateSolicitudCertificado(SolicitudExtend entity);

        Task<IEnumerable<SolicitudExtend>> ObtenerSolicitudesPendientes(string ID_TIPO_DOCUMENTO, string NUMERO_DOCUMENTO);

        Task<IEnumerable<SolicitudExtend>> ConsultarSolicitudPorPersona(int ID_PERSONA);

        Task<IEnumerable<AreaCertificadoEntity>> GetValidarArea(AreaCertificadoEntity entity);

        Task<IEnumerable<AreaCertificadoEntity>> ObtenerAreasPorDisenio(int ID_ANIO, string ID_NIVEL);

        Task<int> UpdateEstadoEstudianteCertificado(int ID_SOLICITUD, string ESTADO_ESTUDIANTE,string USUARIO);

        Task<IEnumerable<NotaCertificadoEntity>> ObtenerNotasPorSolicitud(int ID_SOLICITUD);

        Task<IEnumerable<GradoCertificadoEntity>> ObtenerGradosPorSolicitud(int ID_SOLICITUD);

        Task<int> UpdateEstadoCertificado(int ID_SOLICITUD, string ESTADO_SOLICITUD, string ESTADO_ESTUDIANTE, string DIRECTOR, string USUARIO);

        Task<int> DeleteObservacionCertificado(int ID_CERTIFICADO_OBSERVACION);

        Task<int> DeleteObservacionCertificadoAll(int ID_SOLICITUD);

        Task<int> ActualizarNotaCertificado(NotaCertificadoEntity entity);

        Task<IEnumerable<NotaCertificadoEntity>> InactivarNota(int ID_CONSTANCIA_NOTA, int ID_SOLICITUD);

        Task<IEnumerable<EstudianteCertificadoEntity>> ObtenerCertificadoEstudianteValidado(int ID_ESTUDIANTE);

        Task<IEnumerable<SolicitudExtend>> ObtenerSolicitudPorID(int ID_SOLICITUD);

        Task<IEnumerable<USP_INTERNO_CERTIFICADO_LISTAR_REGISTROS_ESTUDIANTE_Result>> ObtenerRegistroEstudiante(string ID_TIPO_DOCUMENTO, string NUMERO_DOCUMENTO);

        Task<IEnumerable<EstudianteCertificadoEntity>> ObtenerDatosEstudiante(string ID_TIPO_DOCUMENTO, string NUMERO_DOCUMENTO);

        void DesactivarSolicitudesEmitidas(int ID_SOLICITUD, int ID_ESTUDIANTE, string ID_MODALIDAD, string ID_NIVEL);

        void DesactivarSolicitudesEmitidasPorIdEstudiante(int ID_SOLICITUD, int ID_ESTUDIANTE, string ID_MODALIDAD, string ID_NIVEL);
    }
}
