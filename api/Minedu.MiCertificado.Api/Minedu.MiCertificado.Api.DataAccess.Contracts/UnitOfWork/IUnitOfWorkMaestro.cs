using Minedu.Comun.IData;
using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Constancia;
using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minedu.MiCertificado.Api.DataAccess.Contracts.UnitOfWork
{
    public partial interface IUnitOfWork : IBaseUnitOfWork
    {
        Task<IEnumerable<MenuEntity>> ObtenerMenu(MenuEntity entity);
        Task<IEnumerable<DeclaracionJuradaEntity>> ObtenerDeclaracionJurada();
        Task<IEnumerable<MotivoEntity>> ObtenerMotivo(MotivoEntity entity);
        Task<bool> EnviarCorreo(CorreoEntity entity);
    }
}
