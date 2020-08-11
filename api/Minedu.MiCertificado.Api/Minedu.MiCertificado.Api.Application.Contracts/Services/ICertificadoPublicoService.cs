using Minedu.Comun.Helper;
using Models = Minedu.MiCertificado.Api.BusinessLogic.Models;
using System.Threading.Tasks;


namespace Minedu.MiCertificado.Api.Application.Contracts.Services
{
    public interface ICertificadoPublicoService
    {
        Task<StatusResponse> ObtenerToken();
        Task<StatusResponse> ObtenerIIEE(Models.Siagie.ColegioRequest encryptedRequest);
        StatusResponse ValidarDJ(Models.Constancia.DeclaracionRequest encryptedRequest);
        Task<StatusResponse> ValidarApoderado(Models.Certificado.ApoderadoPersonaModularRequest encryptedRequest);
        Task<StatusResponse> ValidarEstudiante(Models.Certificado.EstudiantePersonaModularRequest encryptedRequest);
        Task<StatusResponse> ObtenerUltimoColegioEstudiante(Models.Certificado.EstudianteModalidadNivelModularRequest encryptedRequest);
        Task<StatusResponse> ObtenerSolicitudesPendientesEstudiante(Models.Certificado.PersonaModalidadNivelRequest encryptedRequest);

        Task<byte[]> VistaPreviaPDFCertificado(Models.Constancia.EstudianteModalidadNivelPersonaRequest encryptedRequest);
        Task<StatusResponse> ValidarEstudianteNotas(Models.Constancia.EstudianteModalidadNivelRequest encryptedRequest);
        Task<StatusResponse> GenerarPDFCertificado(Models.Certificado.SolicitudCertificadoRequest encryptedRequest);
        Task<StatusResponse> DescargarPDFCertificado(Models.Constancia.DescargaRequest encryptedRequest);
        Task<StatusResponse> ValidarPDFCertificado(Models.Certificado.VerificacionCertificadoRequest encryptedRequest);
    }
}
