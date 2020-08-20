using Minedu.Comun.Data;
using MDS.Inventario.Api.DataAccess.Contracts.Entities.Certificado;
using MDS.Inventario.Api.DataAccess.Contracts.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using MDS.Inventario.Api.Application.Entities.Models;

namespace MDS.Inventario.Api.DataAccess.UnitOfWork
{
    public partial class UnitOfWork : BaseUnitOfWork, IUnitOfWork
    {
        public async Task<IEnumerable<PersonalEntity>> ValidarUsuario(string usuario, string contrasenia)
        {
            var parm = new Parameter[] {
                new Parameter("@USUARIO" , usuario),
                new Parameter("@CONTRASENIA" , contrasenia)
            };

            try
            {
                var result = this.ExecuteReader<PersonalEntity>(
                    "dbo.USP_MDS_INVENTARIO_USUARIO_VALIDO"
                    , CommandType.StoredProcedure
                    , ref parm
                );

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
