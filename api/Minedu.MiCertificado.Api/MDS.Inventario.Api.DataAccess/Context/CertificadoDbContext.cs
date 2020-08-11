using Microsoft.EntityFrameworkCore;
using Minedu.Comun.Data;

namespace MDS.Inventario.Api.DataAccess.Context
{
    public partial class CertificadoDbContext : DbContext, IDbContext
    {
        private readonly string _connstr;

        public CertificadoDbContext(string connstr)
        {
            this._connstr = connstr;
        }

        public CertificadoDbContext(DbContextOptions<CertificadoDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrWhiteSpace(this._connstr))
            {
                optionsBuilder.UseSqlServer(this._connstr, b => b.UseRowNumberForPaging());
            }
        }
    }
}
