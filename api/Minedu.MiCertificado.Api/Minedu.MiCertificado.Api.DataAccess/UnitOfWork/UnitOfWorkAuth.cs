using Minedu.Comun.Data;
using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Certificado;
using Minedu.MiCertificado.Api.DataAccess.Contracts.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Minedu.MiCertificado.Api.DataAccess.UnitOfWork
{
    public partial class UnitOfWork : BaseUnitOfWork, IUnitOfWork
    {
        /*public async Task<IEnumerable<IEEntity>> GetIE(IEEntity entity)
        {
            var parm = new Parameter[] {
                new Parameter("@NUMERO_DOCUMENTO" , entity.NUMERO_DOCUMENTO),
                new Parameter("@TIPO_DOCUMENTO" , entity.TIPO_DOCUMENTO)
            };

            try
            {
                var result = this.ExecuteReader<IEEntity>(
                    "dbo.USP_INTERNO_CERTIFICADO_LISTAR_IE_ASOCIADAS_DIRECTOR"
                    , CommandType.StoredProcedure
                    , ref parm
                );

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }*/

        /*public async Task<IEnumerable<IEEntity>> ObtenerRolActivo(IEEntity entity)
        {
            var parm = new Parameter[] {
                new Parameter("@NUMERO_DOCUMENTO" , entity.NUMERO_DOCUMENTO),
                new Parameter("@TIPO_DOCUMENTO" , entity.TIPO_DOCUMENTO)
            };

            try
            {
                var result = this.ExecuteReader<IEEntity>(
                    "dbo.USP_INTERNO_CERTIFICADO_LISTAR_IE_ASOCIADAS_DIRECTOR"
                    , CommandType.StoredProcedure
                    , ref parm
                );

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }*/

        public async Task<IEnumerable<MenuNivelRolEntity>> ObtenerMenuNivelPorRol(string ID_ROL)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_ROL" , ID_ROL)
            };

            try
            {
                var result = this.ExecuteReader<MenuNivelRolEntity>(
                    "dbo.USP_INTERNO_CERTIFICADO_LISTAR_MENU_NIVEL_X_ROL"
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
