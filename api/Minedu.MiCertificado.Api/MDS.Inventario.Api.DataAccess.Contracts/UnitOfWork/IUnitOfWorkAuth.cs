using MDS.Inventario.Api.DataAccess.Contracts.Entities.Certificado;
using Minedu.Comun.IData;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDS.Inventario.Api.DataAccess.Contracts.UnitOfWork
{
    public partial interface IUnitOfWork : IBaseUnitOfWork
    {
        Task<IEnumerable<PersonalEntity>> ValidarUsuario(string usuario, string contrasenia)
    }
}
