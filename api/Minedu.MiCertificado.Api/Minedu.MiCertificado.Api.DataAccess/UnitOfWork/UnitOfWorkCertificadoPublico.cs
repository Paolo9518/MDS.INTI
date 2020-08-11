using Minedu.Comun.Data;
using Minedu.MiCertificado.Api.DataAccess.Contracts.Entities.Certificado;
using Minedu.MiCertificado.Api.DataAccess.Contracts.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Minedu.MiCertificado.Api.DataAccess.UnitOfWork
{
    public partial class UnitOfWork : BaseUnitOfWork, IUnitOfWork
    {
        public async Task<IEnumerable<InstitucionCertificadoEntity>> ObtenerCertificadoInstitucion(string CODIGO_MODULAR)
        {
            var parm = new Parameter[] {
                new Parameter("@CODIGO_MODULAR" , CODIGO_MODULAR)
            };

            try
            {
                var result = this.ExecuteReader<InstitucionCertificadoEntity>(
                    "dbo.USP_INTERNO_CERTIFICADO_INSTITUCION_SELECT"
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

        public async Task<IEnumerable<SolicitudExtend>> ObtenerSolicitudesCertificadoPorEstudiante(string ID_TIPO_DOCUMENTO, string NUMERO_DOCUMENTO, string ID_MODALIDAD, string ID_NIVEL, string ESTADO_SOLICITUD)
        {
            var parm = new Parameter[] {
                    new Parameter("@ID_TIPO_DOCUMENTO" , ID_TIPO_DOCUMENTO),
                    new Parameter("@NUMERO_DOCUMENTO" , NUMERO_DOCUMENTO),
                    new Parameter("@ID_MODALIDAD" , ID_MODALIDAD),
                    new Parameter("@ID_NIVEL" , ID_NIVEL),
                    new Parameter("@ESTADO_SOLICITUD" , ESTADO_SOLICITUD)
                };

            try
            {
                var result = this.ExecuteReader<SolicitudExtend>(
                    "dbo.USP_INTERNO_CERTIFICADO_SOLICITUD_SELECT"
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

        /*public async Task<int> InsertarEstudianteCertificado(EstudianteCertificadoEntity entity)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_PERSONA" , entity.ID_PERSONA),
                new Parameter("@ID_TIPO_DOCUMENTO" , entity.ID_TIPO_DOCUMENTO),
                new Parameter("@NUMERO_DOCUMENTO" , entity.NUMERO_DOCUMENTO),
                new Parameter("@APELLIDO_PATERNO" , entity.APELLIDO_PATERNO),
                new Parameter("@APELLIDO_MATERNO" , entity.APELLIDO_MATERNO),
                new Parameter("@NOMBRES" , entity.NOMBRES),
                new Parameter("@UBIGEO" , entity.UBIGEO),
                new Parameter("@DEPARTAMENTO" , entity.DEPARTAMENTO),
                new Parameter("@PROVINCIA" , entity.PROVINCIA),
                new Parameter("@DISTRITO" , entity.DISTRITO)
            };

            try
            {
                var result = this.ExecuteScalar<int>(
                    "dbo.USP_INTERNO_CERTIFICADO_ESTUDIANTE_INSERT_UPDATE"
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
        /*
        public async Task<int> InsertarSolicitanteCertificado(SolicitanteCertificadoEntity entity)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_PERSONA" , entity.ID_PERSONA),
                new Parameter("@ID_TIPO_DOCUMENTO" , entity.ID_TIPO_DOCUMENTO),
                new Parameter("@NUMERO_DOCUMENTO" , entity.NUMERO_DOCUMENTO),
                new Parameter("@APELLIDO_PATERNO" , entity.APELLIDO_PATERNO),
                new Parameter("@APELLIDO_MATERNO" , entity.APELLIDO_MATERNO),
                new Parameter("@NOMBRES" , entity.NOMBRES),
                new Parameter("@TELEFONO_CELULAR" , entity.TELEFONO_CELULAR),
                new Parameter("@CORREO_ELECTRONICO" , entity.CORREO_ELECTRONICO),
                new Parameter("@UBIGEO" , entity.UBIGEO),
                new Parameter("@DEPARTAMENTO" , entity.DEPARTAMENTO),
                new Parameter("@PROVINCIA" , entity.PROVINCIA),
                new Parameter("@DISTRITO" , entity.DISTRITO)
            };

            try
            {
                var result = this.ExecuteScalar<int>(
                    "dbo.USP_INTERNO_CERTIFICADO_SOLICITANTE_INSERT_UPDATE"
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
        /*
        public async Task<int> InsertarSolicitudCertificado(SolicitudCertificadoEntity entity)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_SOLICITANTE" , entity.ID_SOLICITANTE),
                new Parameter("@ID_ESTUDIANTE" , entity.ID_ESTUDIANTE),
                new Parameter("@ID_MOTIVO" , entity.ID_MOTIVO),
                new Parameter("@MOTIVO_OTROS" , entity.MOTIVO_OTROS),
                new Parameter("@ID_MODALIDAD" , entity.ID_MODALIDAD),
                new Parameter("@ABR_MODALIDAD" , entity.ABR_MODALIDAD),
                new Parameter("@DSC_MODALIDAD" , entity.DSC_MODALIDAD),
                new Parameter("@ID_NIVEL" , entity.ID_NIVEL),
                new Parameter("@DSC_NIVEL" , entity.DSC_NIVEL),
                new Parameter("@ID_GRADO" , entity.ID_GRADO),
                new Parameter("@DSC_GRADO" , entity.DSC_GRADO),
                new Parameter("@CODIGO_MODULAR" , entity.CODIGO_MODULAR),
                new Parameter("@ANEXO" , entity.ANEXO),
                new Parameter("@ANIO_CULMINACION" , entity.ANIO_CULMINACION),
                new Parameter("@ESTADO_ESTUDIANTE" , entity.ESTADO_ESTUDIANTE),
            };

            try
            {
                var result = this.ExecuteScalar<int>(
                    "dbo.USP_INTERNO_CERTIFICADO_SOLICITUD_INSERT_UPDATE"
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
        */
        public async void DesactivarSolicitudesCertificado(int ID_SOLICITUD, int ID_PERSONA, string ID_MODALIDAD, string ID_NIVEL)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_SOLICITUD" , ID_SOLICITUD),
                new Parameter("@ID_PERSONA" , ID_PERSONA),
                new Parameter("@ID_MODALIDAD" , ID_MODALIDAD),
                new Parameter("@ID_NIVEL" , ID_NIVEL)
            };

            try
            {
                this.ExecuteNonQuery(
                    "dbo.USP_INTERNO_CERTIFICADO_SOLICITUD_DESACTIVAR"
                    , CommandType.StoredProcedure
                    , ref parm
                );
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> InsertarGradoCertificado(GradoCertificadoEntity entity)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_SOLICITUD" , entity.ID_SOLICITUD),
                new Parameter("@ID_GRADO" , entity.ID_GRADO),
                new Parameter("@DSC_GRADO" , entity.DSC_GRADO),
                new Parameter("@CORR_ESTADISTICA" , entity.CORR_ESTADISTICA),
                new Parameter("@ID_ANIO" , entity.ID_ANIO),
                new Parameter("@COD_MOD" , entity.COD_MOD),
                new Parameter("@ANEXO" , entity.ANEXO),
                new Parameter("@SITUACION_FINAL" , entity.SITUACION_FINAL),
                new Parameter("@ID_CONSTANCIA_GRADO" , entity.ID_CONSTANCIA_GRADO),
                new Parameter("@CICLO", entity.CICLO),
                new Parameter("@USUARIO", entity.USUARIO)
            };

            try
            {
                var result = this.ExecuteScalar<int>(
                    "dbo.USP_INTERNO_CERTIFICADO_GRADO_INSERT_UPDATE"
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

        public async Task<int> InsertarNotaCertificado(NotaCertificadoEntity entity)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_SOLICITUD" , entity.ID_SOLICITUD),
                new Parameter("@ID_ANIO" , entity.ID_ANIO),
                new Parameter("@COD_MOD" , entity.COD_MOD),
                new Parameter("@ANEXO" , entity.ANEXO),
                new Parameter("@ID_NIVEL" , entity.ID_NIVEL),
                new Parameter("@DSC_NIVEL" , entity.DSC_NIVEL),
                new Parameter("@ID_GRADO" , entity.ID_GRADO),
                new Parameter("@DSC_GRADO" , entity.DSC_GRADO),
                new Parameter("@ID_TIPO_AREA" , entity.ID_TIPO_AREA),
                new Parameter("@DSC_TIPO_AREA" , entity.DSC_TIPO_AREA),
                new Parameter("@ID_AREA" , entity.ID_AREA),
                new Parameter("@DSC_AREA" , entity.DSC_AREA),
                new Parameter("@ESCONDUCTA" , entity.ESCONDUCTA),
                new Parameter("@NOTA_FINAL_AREA" , entity.NOTA_FINAL_AREA),
                new Parameter("@ESTADO" , entity.ESTADO),
                new Parameter("@ID_CONSTANCIA_NOTA" , entity.ID_CONSTANCIA_NOTA),
                new Parameter("@ES_AREA_SIAGIE", entity.ES_AREA_SIAGIE),
                new Parameter("@ACTIVO", entity.ACTIVO),
                new Parameter("@USUARIO", entity.USUARIO)
            };

            try
            {
                var result = this.ExecuteScalar<int>(
                    "dbo.USP_INTERNO_CERTIFICADO_NOTA_INSERT_UPDATE"
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

        public async Task<int> InsertarObservacionCertificado(ObservacionCertificadoEntity entity)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_SOLICITUD" , entity.ID_SOLICITUD),
                new Parameter("@ID_NIVEL" , entity.ID_NIVEL),
                new Parameter("@ID_ANIO" , entity.ID_ANIO),
                new Parameter("@RESOLUCION" , entity.RESOLUCION),
                new Parameter("@TIPO_SOLICITUD" , entity.TIPO_SOLICITUD),
                new Parameter("@MOTIVO" , entity.MOTIVO),
                new Parameter("@ID_TIPO" , entity.ID_TIPO),
                new Parameter("@DSC" , entity.DSC),
                new Parameter("@TIPO_OBS" , entity.TIPO_OBS),
                new Parameter("@ID_CERTIFICADO_OBSERVACION" , entity.ID_CERTIFICADO_OBSERVACION),
                new Parameter("@ESTADO" , entity.ESTADO),
                new Parameter("@USUARIO" , entity.USUARIO)
            };

            try
            {
                var result = this.ExecuteScalar<int>(
                    "dbo.USP_INTERNO_CERTIFICADO_OBSERVACION_INSERT_UPDATE"
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

        public async Task<IEnumerable<SolicitudExtend>> ObtenerSolicitudCertificado(SolicitudExtend entity)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_SOLICITUD" , entity.ID_SOLICITUD)
            };

            try
            {
                var result = this.ExecuteReader<SolicitudExtend>(
                    "dbo.USP_INTERNO_CERTIFICADO_SOLICITUD_SELECT"
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

        public async Task<IEnumerable<SolicitudExtend>> ObtenerSolicitudCertificadoPorCodigoVirtual2(string CODIGO_VIRTUAL, string ID_TIPO_DOCUMENTO, string NUMERO_DOCUMENTO)
        {
            var parm = new Parameter[] {
                new Parameter("@CODIGO_VIRTUAL" , CODIGO_VIRTUAL),
                new Parameter("@ID_TIPO_DOCUMENTO" , ID_TIPO_DOCUMENTO),
                new Parameter("@NUMERO_DOCUMENTO" , NUMERO_DOCUMENTO)
            };

            try
            {
                var result = this.ExecuteReader<SolicitudExtend>(
                    "dbo.USP_INTERNO_CERTIFICADO_SOLICITUD_SELECT"
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

        public async Task<IEnumerable<EstudianteCertificadoEntity>> ObtenerEstudianteCertificadoValidado(int ID_ESTUDIANTE)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_ESTUDIANTE" , ID_ESTUDIANTE)
            };

            try
            {
                var result = this.ExecuteReader<EstudianteCertificadoEntity>(
                    "dbo.USP_INTERNO_CERTIFICADO_ESTUDIANTE_SELECT"
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

        public async Task<IEnumerable<GradoCertificadoEntity>> ObtenerGradosCertificadoValidados(int ID_SOLICITUD)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_SOLICITUD" , ID_SOLICITUD)
            };

            try
            {
                var result = this.ExecuteReader<GradoCertificadoEntity>(
                    "dbo.USP_INTERNO_CERTIFICADO_GRADO_SELECT"
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

        public async Task<IEnumerable<NotaCertificadoEntity>> ObtenerNotasCertificadoValidadas(int ID_SOLICITUD)
        {
            var parm = new Parameter[] {                
                new Parameter("@ID_CONSTANCIA_NOTA", 0),
                new Parameter("@ID_SOLICITUD" , ID_SOLICITUD)
            };

            try
            {
                var result = this.ExecuteReader<NotaCertificadoEntity>(
                    "dbo.USP_INTERNO_CERTIFICADO_NOTA_SELECT"
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

        public async Task<IEnumerable<ObservacionCertificadoEntity>> ObtenerObservacionesCertificadoValidadas(int ID_SOLICITUD)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_SOLICITUD" , ID_SOLICITUD)
            };

            try
            {
                var result = this.ExecuteReader<ObservacionCertificadoEntity>(
                    "dbo.USP_INTERNO_CERTIFICADO_OBSERVACION_SELECT"
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
