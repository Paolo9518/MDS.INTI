using Minedu.Comun.Data;
using Entities = Minedu.MiCertificado.Api.DataAccess.Contracts.Entities;
using Minedu.MiCertificado.Api.DataAccess.Contracts.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Certificado;

namespace Minedu.MiCertificado.Api.DataAccess.UnitOfWork
{
    public partial class UnitOfWork : BaseUnitOfWork, IUnitOfWork
    {
        public async Task<IEnumerable<Entities.Certificado.MenuCertificadoEntity>> ObtenerCertificadoMenu(Entities.Certificado.MenuCertificadoEntity entity)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_MENU" , entity.ID_MENU)
            };

            try
            {
                var result = this.ExecuteReader<Entities.Certificado.MenuCertificadoEntity>(
                    "dbo.USP_INTERNO_CERTIFICADO_MENU_SELECT"
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

        public async Task<IEnumerable<Entities.Certificado.DeclaracionJuradaCertificadoEntity>> ObtenerCertificadoDeclaracionJurada()
        {
            var parm = new Parameter[] { };

            try
            {
                var result = this.ExecuteReader<Entities.Certificado.DeclaracionJuradaCertificadoEntity>(
                    "dbo.USP_INTERNO_CERTIFICADO_DECLARACION_JURADA_SELECT"
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

        public async Task<IEnumerable<Entities.Certificado.MotivoCertificadoEntity>> ObtenerCertificadoMotivo(Entities.Certificado.MotivoCertificadoEntity entity)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_MOTIVO" , entity.ID_MOTIVO)
            };

            try
            {
                var result = this.ExecuteReader<Entities.Certificado.MotivoCertificadoEntity>(
                    "dbo.USP_INTERNO_CERTIFICADO_MOTIVO_SELECT"
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

        public async Task<bool> EnviarCorreoCertificado(Entities.CorreoEntity entity)
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
                    "dbo.USP_INTERNO_CERTIFICADO_ENVIAR_MAIL"
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

        public async Task<IEnumerable<AreaCertificadoEntity>> ObtenerArea(string codigoTipoArea, string nivel)
        {
            var parm = new Parameter[] {
                new Parameter("@COD_TIPOAREA" , codigoTipoArea),
                new Parameter("@NIVEL" , nivel)
            };

            try
            {
                var result = this.ExecuteReader<AreaCertificadoEntity>(
                    "dbo.USP_INTERNO_CERTIFICADO_AREA_SELECT"
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

        public async Task<string> InsertArea(string codigoTipoArea, string nivel, string descripcionArea, string usuario, string modalidad)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_TIPO_AREA" , codigoTipoArea),
                new Parameter("@ID_NIVEL" , nivel),
                new Parameter("@DSC_AREA" , descripcionArea),
                new Parameter("@USUARIO" , usuario),
                new Parameter("@ID_MODALIDAD" , modalidad)
            };

            try
            {

                var result = this.ExecuteScalar<string>(
                   "dbo.USP_INTERNO_CERTIFICADO_AREA_INSERT"
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

        public async Task<IEnumerable<SolicitudExtend>> ObtenerAniosSolicitud(string IdNivel, string CodigoModular, string Anexo, string EstadoSolicitud)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_NIVEL" , IdNivel),
                new Parameter("@CODIGO_MODULAR" , CodigoModular),
                new Parameter("@ANEXO" , Anexo),
                new Parameter("@ESTADO_SOLICITUD" , EstadoSolicitud)
            };

            try
            {
                var result = this.ExecuteReader<SolicitudExtend>(
                   "dbo.USP_INTERNO_CERTIFICADO_SOLICITUD_ANIOS_SELECT"
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