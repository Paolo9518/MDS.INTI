using Minedu.Comun.Helper;
using MDS.Inventario.Api.Application.Entities.Models.Certificado;
using System.Threading.Tasks;

namespace MDS.Inventario.Api.Application.Contracts.Services
{
    public interface IAuthService
    {
        Task<StatusResponse> ObtenerUsuario(AuthModel request);
        Task<StatusResponse> ObtenerRolCentralizado(RolCentralizadoRequest encryptedRequest);
        Task<StatusResponse> ObtenerRolesDescentralizados(RolDescentralizadoRequest encryptedRequest);
        Task<StatusResponse> ObtenerRolPorModular(UsuarioModularRequest encryptedRequest);
        Task<StatusResponse> ObtenerMenuNivelPorRol(RolRequest encryptedRequest);
        Task<StatusResponse> Login(AuthUserModel request);
    }
}
