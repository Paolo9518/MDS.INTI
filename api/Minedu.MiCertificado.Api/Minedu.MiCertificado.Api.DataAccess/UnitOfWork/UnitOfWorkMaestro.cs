using Minedu.Comun.Data;
using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Constancia;
using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities;
using Minedu.MiCertificado.Api.DataAccess.Contracts.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Minedu.MiCertificado.Api.DataAccess.UnitOfWork
{
    public partial class UnitOfWork : BaseUnitOfWork, IUnitOfWork
    {
        public async Task<IEnumerable<MenuEntity>> ObtenerMenu(MenuEntity entity)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_MENU" , entity.ID_MENU)
            };

            try
            {
                var result = this.ExecuteReader<MenuEntity>(
                    "dbo.USP_EXTERNO_CERTIFICADO_MENU_SELECT"
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

        public async Task<IEnumerable<DeclaracionJuradaEntity>> ObtenerDeclaracionJurada()
        {
            var parm = new Parameter[] { };

            try
            {
                var result = this.ExecuteReader<DeclaracionJuradaEntity>(
                    "dbo.USP_EXTERNO_CERTIFICADO_DECLARACION_JURADA_SELECT"
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

        public async Task<IEnumerable<MotivoEntity>> ObtenerMotivo(MotivoEntity entity)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_MOTIVO" , entity.ID_MOTIVO)
            };

            try
            {
                var result = this.ExecuteReader<MotivoEntity>(
                    "dbo.USP_EXTERNO_CERTIFICADO_MOTIVO_SELECT"
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

        public async Task<bool> EnviarCorreo(CorreoEntity entity)
        {
            var parm = new Parameter[] {
                new Parameter("@PARA" , entity.PARA),
                new Parameter("@CC" , entity.CC),
                new Parameter("@CCO" , entity.CCO),
                new Parameter("@ASUNTO" , entity.ASUNTO),
                new Parameter("@MENSAJE" , entity.MENSAJE)
            };

            try
            {
                var result = this.ExecuteScalar<int>(
                    "dbo.USP_EXTERNO_CERTIFICADO_ENVIAR_MAIL"
                    , CommandType.StoredProcedure
                    , ref parm
                );

                return result == 1;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
