using Minedu.Comun.IData;
using MDS.Inventario.Api.DataAccess.Contracts.Entities.Certificado;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDS.Inventario.Api.DataAccess.Contracts.UnitOfWork
{
    public partial interface IUnitOfWork : IBaseUnitOfWork
    {
        //Task<IEnumerable<IEEntity>> GetIE(IEEntity entity);
        //Task<IEnumerable<IEEntity>> ObtenerRolActivo(IEEntity entity);
        Task<IEnumerable<MenuNivelRolEntity>> ObtenerMenuNivelPorRol(string ID_ROL);
    }
}
