using Microsoft.Extensions.Configuration;
using Minedu.Comun.Data;
using Minedu.MiCertificado.Api.DataAccess.Contracts.UnitOfWork;
using System.Data;
using System.Data.Common;

namespace Minedu.MiCertificado.Api.DataAccess.UnitOfWork
{
    public partial class UnitOfWork : BaseUnitOfWork, IUnitOfWork
    {
        public UnitOfWork(IDbContext context) : base(context, true)
        {

        }
    }
}
