using Minedu.Comun.IData;
using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Constancia;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minedu.MiCertificado.Api.DataAccess.Contracts.UnitOfWork
{
    public partial interface IUnitOfWork : IBaseUnitOfWork
    {
        Task<int> InsertarEstudiante(EstudianteEntity entity);
        Task<int> InsertarSolicitante(SolicitanteEntity entity);
        Task<int> InsertarSolicitud(SolicitudEntity entity);
        void DesactivarSolicitudes(int ID_SOLICITUD, int ID_PERSONA, string ID_MODALIDAD, string ID_NIVEL);
        Task<int> InsertarGrado(GradoEntity entity);
        Task<int> InsertarNota(NotaEntity entity);
        Task<int> InsertarObservacion(ObservacionEntity entity);

        Task<IEnumerable<SolicitudEntity>> ObtenerSolicitud(SolicitudEntity entity);
        Task<IEnumerable<SolicitudEntity>> ObtenerSolicitudPorPersona(int ID_PERSONA, string ID_MODALIDAD, string ID_NIVEL);
        Task<IEnumerable<SolicitudEntity>> ObtenerSolicitudPorCodigoVirtual(string CODIGO_VIRTUAL, string ID_TIPO_DOCUMENTO, string NUMERO_DOCUMENTO);
        Task<IEnumerable<EstudianteEntity>> ObtenerEstudianteValidado(int ID_ESTUDIANTE);
        Task<IEnumerable<GradoEntity>> ObtenerGradosValidados(int ID_SOLICITUD);
        Task<IEnumerable<NotaEntity>> ObtenerNotasValidadas(int ID_SOLICITUD);
        Task<IEnumerable<ObservacionEntity>> ObtenerObservacionesValidadas(int ID_SOLICITUD);
    }
}
