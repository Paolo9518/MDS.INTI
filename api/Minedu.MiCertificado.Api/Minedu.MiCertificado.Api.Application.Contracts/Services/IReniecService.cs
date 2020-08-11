using Minedu.MiCertificado.Api.BusinessLogic.Models;
using System.Threading.Tasks;

namespace Minedu.MiCertificado.Api.Application.Contracts.Services
{
    public interface IReniecService
    {
        Task<ReniecPersona> ReniecConsultarPersona(string nroDocumento);
    }
}
