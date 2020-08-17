using Minedu.Comun.Helper;
using MDS.Inventario.Api.Application.Entities.Models;
using System.Threading.Tasks;

namespace MDS.Inventario.Api.Application.Contracts.Services
{
    public interface IAuthService
    {
        Task<StatusResponse> Login(UsuarioExtends request);
    }
}
