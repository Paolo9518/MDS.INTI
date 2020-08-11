using Minedu.Comun.Helper;
using System.Threading.Tasks;
using Models = Minedu.MiCertificado.Api.BusinessLogic.Models;

namespace Minedu.MiCertificado.Api.Application.Contracts.Services
{
    public interface ICertificadoMaestroService
    {
        Task<StatusResponse> ObtenerGradoSeccion(Models.Certificado.ParametroModel objetoEncriptado);
        Task<StatusResponse> ObtenerInstitucionEducativa(Models.Certificado.InstitucionEducativaRequest request);
        Task<StatusResponse> ObtenerTipoDeArea();
        Task<StatusResponse> ObtenerAnios(Models.Certificado.ParametroModel objetoEncriptado);
        //Task<StatusResponse> ObtenerDatosInstitucionEducativa(Models.Certificado.InstitucionEducativaPorDreUgelRequest request);
        Task<StatusResponse> ObtenerDatosInstitucionEducativaxCodigoModular(Models.Certificado.ParametroModel objetoEncriptado);
        Task<StatusResponse> ObtenerArea(Models.Certificado.ParametroModel objetoEncriptado);
        Task<StatusResponse> InsertArea(Models.Certificado.AreaRequest request);
        Task<StatusResponse> ObtenerGradosPorNivel(Models.Certificado.GradoSeccionRequest request);
        Task<StatusResponse> ObtenerAniosSolicitud(Models.Certificado.ParametroModel objetoEncriptado);
    }
}
