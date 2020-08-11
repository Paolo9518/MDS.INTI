using Minedu.MiCertificado.Api.BusinessLogic.Models;
using Minedu.MiCertificado.Api.BusinessLogic.Models.Siagie;
using System.Threading.Tasks;

namespace Minedu.MiCertificado.Api.Application.Contracts.Services
{
    public interface ISiagieService
    {
        Task<T> GetServiceResponseById<T>(string controller, int id);
        Task<T> PostServiceToken<T>(TokenRequest request);
        Task<T> GetServiceByQueryAndToken<T, Y>(string token, string method, Y filter);
    }
}
