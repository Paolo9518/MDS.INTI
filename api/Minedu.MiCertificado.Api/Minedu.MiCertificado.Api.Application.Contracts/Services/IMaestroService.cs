using Minedu.Comun.Helper;
using Models = Minedu.MiCertificado.Api.BusinessLogic.Models;
using System.Threading.Tasks;

namespace Minedu.MiCertificado.Api.Application.Contracts.Services
{
    public interface IMaestroService
    {
        #region CONSTANCIA
        Task<StatusResponse> ObtenerConstanciaMenu();
        Task<StatusResponse> ObtenerConstanciaDeclaracionJurada();
        Task<StatusResponse> ObtenerConstanciaMotivos();
        Task<StatusResponse> ObtenerConstanciaModalidades();
        #endregion CONSTANCIA

        Task<StatusResponse> ObtenerReniecDepartamentos();
        Task<StatusResponse> ObtenerReniecProvincias(Models.DepartamentoRequest request);
        Task<StatusResponse> ObtenerReniecDistritos(Models.ProvinciaRequest request);

        Task<StatusResponse> ObtenerSiagieDepartamentos();
        Task<StatusResponse> ObtenerSiagieProvincias(Models.DepartamentoRequest request);
        Task<StatusResponse> ObtenerSiagieDistritos(Models.ProvinciaRequest request);

        #region CERTIFICADO_PÚBLICO
        Task<StatusResponse> ObtenerCertificadoMenus();
        Task<StatusResponse> ObtenerCertificadoDeclaracionJurada();
        Task<StatusResponse> ObtenerCertificadoModalidades();
        Task<StatusResponse> ObtenerCertificadoMotivos();
        Task<StatusResponse> ObtenerCertificadoGrados(Models.ModalidadNivelRequest encryptedRequest);
        #endregion CERTIFICADO_PÚBLICO

        Task<StatusResponse> ObtenerDRE();
        Task<StatusResponse> ObtenerUGEL(Models.Certificado.UgelRequest request);
    }    
}
