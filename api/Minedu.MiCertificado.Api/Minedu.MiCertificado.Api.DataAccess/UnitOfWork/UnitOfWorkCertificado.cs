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
        public async Task<IEnumerable<SolicitudExtend>> GetCertificado(SolicitudExtend entity)
        {
            var parm = new Parameter[] {
                new Parameter("@NUMERO_DOCUMENTO" , entity.NUMERO_DOCUMENTO),
                new Parameter("@TIPO_DOCUMENTO" , entity.TIPO_DOCUMENTO),
                new Parameter("@NOMBRE_ESTUDIANTE" , entity.NOMBRE_ESTUDIANTE),
                new Parameter("@FECHA_SOLICITUD" , entity.FECHA_SOLICITUD),
                new Parameter("@ESTADO_SOLICITUD" , entity.ESTADO_SOLICITUD)
            };

            try
            {
                var result = this.ExecuteReader<SolicitudExtend>(
                    "USP_INTERNO_CERTIFICADO_LISTAR_SOLICITUD_CERTIFICADO"
                    , CommandType.StoredProcedure
                    , ref parm
                );

                return result;
            }
            catch(Exception ex)
            {
                throw;
            }


        }

        public async Task<int> UpdateCertificado(SolicitudExtend entity)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_SOLICITUD" , entity.ID_SOLICITUD),
                new Parameter("@ID_ESTUDIANTE" , entity.ID_ESTUDIANTE),
                new Parameter("@ID_SOLICITANTE" , entity.ID_SOLICITANTE),
                new Parameter("@ID_MOTIVO" , entity.ID_MOTIVO),
                new Parameter("@ID_MODALIDAD" , entity.ID_MODALIDAD),
                new Parameter("@ABR_MODALIDAD" , entity.ABR_MODALIDAD),
                new Parameter("@DSC_MODALIDAD" , entity.DSC_MODALIDAD),
                new Parameter("@ID_NIVEL" , entity.ID_NIVEL),
                new Parameter("@DSC_NIVEL" , entity.DSC_NIVEL),
                new Parameter("@ID_GRADO" , entity.ID_GRADO),
                new Parameter("@DSC_GRADO" , entity.DSC_GRADO),
                new Parameter("@ESTADO_SOLICITUD" , entity.ESTADO_SOLICITUD),
                new Parameter("@USUARIO" , entity.usuario)
            };

            try
            {
                var rows = this.ExecuteScalar<int>(
                    "dbo.USP_INTERNO_CERTIFICADO_EMISION_INSERT_UPDATE"
                    , CommandType.StoredProcedure
                    , ref parm
                );

                return rows;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<SolicitudExtend>> GetSolicitud(SolicitudExtend entity)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_SOLICITUD" , entity.ID_SOLICITUD),
                new Parameter("@ESTADO_SOLICITUD",entity.ESTADO_SOLICITUD)
            };

            try
            {
                var result = this.ExecuteReader<SolicitudExtend>(
                    "dbo.USP_INTERNO_CERTIFICADO_LISTAR_SOLICITUD"
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

        public async Task<IEnumerable<SolicitudExtend>> GetSolicitudCertificado(SolicitudExtend entity)
        {
            //int IdSolicitud = Convert.ToInt32(entity.ID_SOLICITUD);
            var parm = new Parameter[] {
                new Parameter("@ID_SOLICITUD" , entity.ID_SOLICITUD),
                new Parameter("@ESTADO_SOLICITUD",entity.ESTADO_SOLICITUD)
            };

            try
            {
                var result = this.ExecuteReader<SolicitudExtend>(
                    "dbo.USP_INTERNO_CERTIFICADO_LISTAR_SOLICITUD"
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

        public async Task<int> InsertarEstudianteCertificado(EstudianteCertificadoEntity entity)
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
                new Parameter("@DISTRITO" , entity.DISTRITO),
                new Parameter("@USUARIO" , entity.USUARIO),
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
        }
        
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
                new Parameter("@DISTRITO" , entity.DISTRITO),
                new Parameter("@USUARIO" , entity.USUARIO)
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
        }

        public async Task<int> InsertarSolicitudCertificado(SolicitudExtend entity)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_SOLICITANTE" , entity.ID_SOLICITANTE),
                new Parameter("@ID_ESTUDIANTE" , entity.ID_ESTUDIANTE),
                new Parameter("@ID_MOTIVO" , entity.ID_MOTIVO),
                new Parameter("@ID_MODALIDAD" , entity.ID_MODALIDAD),
                new Parameter("@ABR_MODALIDAD" , entity.ABR_MODALIDAD),
                new Parameter("@DSC_MODALIDAD" , entity.DSC_MODALIDAD),
                new Parameter("@ID_NIVEL" , entity.ID_NIVEL),
                new Parameter("@DSC_NIVEL" , entity.DSC_NIVEL),
                new Parameter("@ID_GRADO" , entity.ID_GRADO),
                new Parameter("@DSC_GRADO" , entity.DSC_GRADO),
                new Parameter("@ANIO_CULMINACION" , entity.ANIO_CULMINACION),
                new Parameter("@ESTADO_SOLICITUD" , entity.ESTADO_SOLICITUD),
                new Parameter("@CODIGO_MODULAR" , entity.CODIGO_MODULAR),
                new Parameter("@ANEXO" , entity.ANEXO),
                new Parameter("@ESTADO_ESTUDIANTE" , entity.ESTADO_ESTUDIANTE),
                new Parameter("@CICLO",entity.CICLO),
                new Parameter("@DIRECTOR", entity.DIRECTOR),
                new Parameter("@USUARIO", entity.usuario),
            };

            try
            {
                var result = this.ExecuteScalar<int>(
                    "dbo.USP_INTERNO_CERTIFICADO_SOLICITUD_INSERT"
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

        /*
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
                new Parameter("@ID_CONSTANCIA_GRADO" , entity.ID_CONSTANCIA_GRADO)
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
        */
        /*
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
                new Parameter("@ID_CONSTANCIA_NOTA" , entity.ID_CONSTANCIA_NOTA)
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
        */
        public async Task<int> InsertUpdateInstitucion(IEEntity entity)
        {
            var parm = new Parameter[] {
                new Parameter("@CODIGO_MODULAR" , entity.CODIGO_MODULAR),
                 new Parameter("@ANEXO" , entity.ANEXO),
                new Parameter("@NOMBRE_INSTITUCION" , entity.CENTRO_EDUCATIVO),
                new Parameter("@ID_NIVEL" , entity.NIVEL),
                new Parameter("@ESTADO" , entity.ESTADO),
                new Parameter("@USUARIO" , entity.USUARIO)
            };

            try
            {
                var rows = this.ExecuteScalar<int>(
                    "dbo.USP_INTERNO_INSTITUCION_INSERT_UPDATE"
                    , CommandType.StoredProcedure
                    , ref parm
                );

                return rows;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<IEEntity>> PostInstitucion(IEEntity entity)
        {
            var parm = new Parameter[] {
                new Parameter("@CODIGO_MODULAR" , entity.CODIGO_MODULAR),
                new Parameter("@ANEXO" , entity.ANEXO) 
            };

            try
            {
                var result = this.ExecuteReader<IEEntity>(
                    "USP_INTERNO_INSTITUCION_SELECT_X_COD_MODULAR_ANEXO"
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

        public async Task<IEnumerable<SolicitudExtend>> GetSolicitudRechazada(SolicitudExtend entity)
        {
            var parm = new Parameter[] {
                new Parameter("@NUMERO_DOCUMENTO" , entity.NUMERO_DOCUMENTO),                
                new Parameter("@NOMBRE_ESTUDIANTE" , entity.NOMBRE_ESTUDIANTE),
                new Parameter("@FECHA_INICIO" , entity.FECHA_INICIO),
                new Parameter("@FECHA_FIN" , entity.FECHA_FIN),
                new Parameter("@ULTIMO_ANIO" , entity.ULTIMO_ANIO),
                new Parameter("@pageNumber",entity.pageNumber),
                new Parameter("@rowsPerPage",entity.rowsPerPage),
                new Parameter("@CODIGO_MODULAR",entity.CODIGO_MODULAR),
                new Parameter("@ANEXO",entity.ANEXO)
            };

            try
            {
                var result = this.ExecuteReader<SolicitudExtend>(
                    "USP_INTERNO_CERTIFICADO_EMISION_LISTAR_SOLICITUDES_RECHAZADAS"
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

        public async Task<IEnumerable<SolicitudExtend>> GetCertificadosEmitidos(SolicitudExtend entity)
        {
            var parm = new Parameter[] {
                new Parameter("@NUMERO_DOCUMENTO" , entity.NUMERO_DOCUMENTO),
                new Parameter("@NOMBRE_ESTUDIANTE" , entity.NOMBRE_ESTUDIANTE),
                new Parameter("@FECHA_INICIO" , entity.FECHA_INICIO),
                new Parameter("@FECHA_FIN" , entity.FECHA_FIN),
                new Parameter("@ULTIMO_ANIO" , entity.ULTIMO_ANIO),
                new Parameter("@pageNumber",entity.pageNumber),
                new Parameter("@rowsPerPage",entity.rowsPerPage),
                new Parameter("@CODIGO_MODULAR",entity.CODIGO_MODULAR),
                new Parameter("@ANEXO",entity.ANEXO)

            };

            try
            {
                var result = this.ExecuteReader<SolicitudExtend>(
                    "USP_INTERNO_CERTIFICADO_EMISION_LISTAR_SOLICITUDES_APROBADAS"
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

        public async Task<IEnumerable<SolicitudExtend>> GetSolicitudesPendientes(SolicitudExtend entity)
        {
            var parm = new Parameter[] {
                new Parameter("@NUMERO_DOCUMENTO" , entity.NUMERO_DOCUMENTO),
                new Parameter("@NOMBRE_ESTUDIANTE" , entity.NOMBRE_ESTUDIANTE),
                new Parameter("@FECHA_INICIO" , entity.FECHA_INICIO),
                new Parameter("@FECHA_FIN" , entity.FECHA_FIN),
                new Parameter("@ULTIMO_ANIO" , entity.ULTIMO_ANIO),
                new Parameter("@pageNumber",entity.pageNumber),
                new Parameter("@rowsPerPage",entity.rowsPerPage),
                new Parameter("@CODIGO_MODULAR",entity.CODIGO_MODULAR),
                new Parameter("@ANEXO",entity.ANEXO)

            };

            try
            {
                var result = this.ExecuteReader<SolicitudExtend>(
                    "USP_INTERNO_CERTIFICADO_EMISION_LISTAR_SOLICITUDES_PENDIENTES"
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

        /*
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
                new Parameter("@ID_CERTIFICADO_OBSERVACION" , entity.ID_CERTIFICADO_OBSERVACION),
                new Parameter("@TIPO_OBS" , entity.TIPO_OBS),
                new Parameter("@ESTADO" , entity.ESTADO)
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
        }*/

        public async Task<IEnumerable<USP_INTERNO_CERTIFICADO_VALIDAR_SOLICITUD_SELECT_Result>> GetValidarSolicitud(string TIP_DOC_ESTUDIANTE, string NUM_DOC_ESTUDIANTE, string TIP_DOC_SOLICITANTE, string NUM_DOC_SOLICITANTE, int ANIO_CULMINACION)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_TIPO_DOC_ESTUDIANTE" , TIP_DOC_ESTUDIANTE),
                new Parameter("@NUM_DOC_ESTUDIANTE" , NUM_DOC_ESTUDIANTE),
                new Parameter("@ID_TIPO_DOCUMENTO" , TIP_DOC_SOLICITANTE),
                new Parameter("@NUM_DOC_SOLICITANTE" , NUM_DOC_SOLICITANTE),
                new Parameter("@ANIO_CULMINACION" , ANIO_CULMINACION)
            };
 
            try
            {            
                var result = this.ExecuteReader<USP_INTERNO_CERTIFICADO_VALIDAR_SOLICITUD_SELECT_Result>(
                   "dbo.USP_INTERNO_CERTIFICADO_VALIDAR_SOLICITUD_SELECT"
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

        public async void SolicitudEstadoEmitido(int ID_SOLICITUD, int ID_PERSONA, string ID_MODALIDAD, string ID_NIVEL, string DIRECTOR, string USUARIO)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_SOLICITUD" , ID_SOLICITUD),
                new Parameter("@ID_PERSONA" , ID_PERSONA),
                new Parameter("@ID_MODALIDAD" , ID_MODALIDAD),
                new Parameter("@ID_NIVEL" , ID_NIVEL),
                new Parameter("@DIRECTOR", DIRECTOR),
                new Parameter("@USUARIO", USUARIO)
            };

            try
            {
                this.ExecuteNonQuery(
                    "dbo.USP_INTERNO_CERTIFICADO_SOLICITUD_ESTADO_EMITIDO"
                    , CommandType.StoredProcedure
                    , ref parm
                );
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<SolicitudExtend>> ObtenerSolicitudCertificadoPorCodigoVirtual(string CODIGO_VIRTUAL, string ID_TIPO_DOCUMENTO, string NUMERO_DOCUMENTO)
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

        public async Task<IEnumerable<EstudianteCertificadoEntity>> ObtenerCertificadoEstudianteValidado(int ID_ESTUDIANTE)
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

        public async Task<IEnumerable<GradoCertificadoEntity>> ObtenerCertificadoGradosValidados(int ID_SOLICITUD)
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

        public async Task<IEnumerable<NotaCertificadoEntity>> ObtenerCertificadoNotasValidadas(int ID_SOLICITUD)
        {
            var parm = new Parameter[] {
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
        
        public async Task<IEnumerable<ObservacionCertificadoEntity>> ObtenerCertificadoObservacionesValidadas(int ID_SOLICITUD)
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
        
        public async Task<IEnumerable<SolicitudExtend>> ObtenerSolicitudesPendientes(string ID_TIPO_DOCUMENTO, string NUMERO_DOCUMENTO)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_TIPO_DOCUMENTO" , ID_TIPO_DOCUMENTO),
                new Parameter("@NUMERO_DOCUMENTO" , NUMERO_DOCUMENTO)
            };

            try
            {
                var result = this.ExecuteReader<SolicitudExtend>(
                    "dbo.USP_INTERNO_CERTIFICADO_SOLICITUD_ESTUDIANTE_SELECT"
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

        public async Task<int> InsertUpdateSolicitudCertificado(SolicitudExtend entity)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_SOLICITANTE" , entity.ID_SOLICITANTE),
                new Parameter("@ID_ESTUDIANTE" , entity.ID_ESTUDIANTE),
                new Parameter("@ID_MOTIVO" , entity.ID_MOTIVO),
                new Parameter("@ID_MODALIDAD" , entity.ID_MODALIDAD),
                new Parameter("@ABR_MODALIDAD" , entity.ABR_MODALIDAD),
                new Parameter("@DSC_MODALIDAD" , entity.DSC_MODALIDAD),
                new Parameter("@ID_NIVEL" , entity.ID_NIVEL),
                new Parameter("@DSC_NIVEL" , entity.DSC_NIVEL),
                new Parameter("@ID_GRADO" , entity.ID_GRADO),
                new Parameter("@DSC_GRADO" , entity.DSC_GRADO),
                new Parameter("@ANIO_CULMINACION" , entity.ANIO_CULMINACION),
                new Parameter("@ESTADO_SOLICITUD" , entity.ESTADO_SOLICITUD),
                new Parameter("@CODIGO_MODULAR" , entity.CODIGO_MODULAR),
                new Parameter("@ANEXO" , entity.ANEXO),
                new Parameter("@ID_SOLICITUD" , entity.ID_SOLICITUD),
                new Parameter("@ESTADO_ESTUDIANTE" , entity.ESTADO_ESTUDIANTE),
                new Parameter("@DIRECTOR", entity.DIRECTOR),
                new Parameter("@USUARIO", entity.usuario)
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

        public async Task<IEnumerable<SolicitudExtend>> ConsultarSolicitudPorPersona(int ID_PERSONA)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_PERSONA", ID_PERSONA)
            };

            try
            {
                var result = this.ExecuteReader<SolicitudExtend>(
                    "dbo.USP_INTERNO_CERTIFICADO_LISTAR_SOLICITUDES_POR_ESTUDIANTE"
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

        public async Task<IEnumerable<AreaCertificadoEntity>> GetValidarArea(AreaCertificadoEntity entity)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_TIPO_AREA" , entity.ID_TIPO_AREA),
                new Parameter("@ID_NIVEL" , entity.ID_NIVEL),
                new Parameter("@DSC_AREA" , entity.DSC_AREA)
            };

            try
            {
                var result = this.ExecuteReader<AreaCertificadoEntity>(
                   "dbo.USP_INTERNO_CERTIFICADO_VALIDAR_AREA_SELECT"
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

        public async Task<IEnumerable<AreaCertificadoEntity>> ObtenerAreasPorDisenio(int ID_ANIO, string ID_NIVEL)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_ANIO" , ID_ANIO),
                new Parameter("@ID_NIVEL" , ID_NIVEL)
            };

            try
            {
                var result = this.ExecuteReader<AreaCertificadoEntity>(
                    "dbo.USP_INTERNO_CERTIFICADO_EMISION_LISTAR_AREAS"
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

        public async Task<int> UpdateEstadoEstudianteCertificado(int ID_SOLICITUD, string ESTADO_ESTUDIANTE, string USUARIO)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_SOLICITUD" , ID_SOLICITUD),
                new Parameter("@ESTADO_ESTUDIANTE" , ESTADO_ESTUDIANTE),
                new Parameter("@USUARIO" , USUARIO)
            };

            try
            {
                var rows = this.ExecuteScalar<int>(
                    "dbo.USP_INTERNO_CERTIFICADO_EMISION_UPDATE_ESTADO_ESTUDIANTE_SOLICITUD"
                    , CommandType.StoredProcedure
                    , ref parm
                );

                return rows;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<NotaCertificadoEntity>> ObtenerNotasPorSolicitud(int ID_SOLICITUD)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_SOLICITUD" , ID_SOLICITUD)
            };

            try
            {
                var result = this.ExecuteReader<NotaCertificadoEntity>(
                    "dbo.USP_EXTERNO_CERTIFICADO_EMISION_NOTA_SELECT"
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

        public async Task<IEnumerable<GradoCertificadoEntity>> ObtenerGradosPorSolicitud(int ID_SOLICITUD)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_CONSTANCIA_GRADO" , 0),
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

        public async Task<int> UpdateEstadoCertificado(int ID_SOLICITUD, string ESTADO_SOLICITUD, string ESTADO_ESTUDIANTE, string DIRECTOR, string USUARIO)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_SOLICITUD" , ID_SOLICITUD),
                new Parameter("@ESTADO_SOLICITUD" , ESTADO_SOLICITUD),
                new Parameter("@ESTADO_ESTUDIANTE" , ESTADO_ESTUDIANTE),
                new Parameter("@DIRECTOR", DIRECTOR),
                new Parameter("@USUARIO", USUARIO)
            };

            try
            {
                var rows = this.ExecuteScalar<int>(
                    "dbo.USP_INTERNO_CERTIFICADO_EMISION_UPDATE_ESTADO_SOLICITUD"
                    , CommandType.StoredProcedure
                    , ref parm
                );

                return rows;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> ActualizarNotaCertificado(NotaCertificadoEntity entity)
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
                new Parameter("@USUARIO" , entity.USUARIO)
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

        public async Task<int> DeleteObservacionCertificado(int ID_CERTIFICADO_OBSERVACION)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_CERTIFICADO_OBSERVACION" , ID_CERTIFICADO_OBSERVACION)
            };

            try
            {
                var result = this.ExecuteScalar<int>(
                    "dbo.USP_INTERNO_CERTIFICADO_OBSERVACION_DELETE"
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

        public async Task<int> DeleteObservacionCertificadoAll(int ID_SOLICITUD)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_SOLICITUD" , ID_SOLICITUD)
            };

            try
            {
                var result = this.ExecuteScalar<int>(
                    "dbo.USP_INTERNO_CERTIFICADO_OBSERVACION_DELETE_ALL"
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

        public async Task<IEnumerable<NotaCertificadoEntity>> InactivarNota(int ID_CONSTANCIA_NOTA, int ID_SOLICITUD)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_CONSTANCIA_NOTA" , ID_CONSTANCIA_NOTA),
                new Parameter("@ID_SOLICITUD" , ID_SOLICITUD),
                new Parameter("@ACTIVO" , 0)
            };

            try
            {
                var result = this.ExecuteReader<NotaCertificadoEntity>(
                    "dbo.USP_INTERNO_CERTIFICADO_NOTA_ACTIVO_UPDATE"
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

        public async Task<IEnumerable<SolicitudExtend>> ObtenerSolicitudPorID(int ID_SOLICITUD)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_SOLICITUD" , ID_SOLICITUD)
            };

            try
            {
                var result = this.ExecuteReader<SolicitudExtend>(
                    "dbo.USP_INTERNO_CERTIFICADO_SOLICITUD_ID_SELECT"
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

        public async Task<IEnumerable<USP_INTERNO_CERTIFICADO_LISTAR_REGISTROS_ESTUDIANTE_Result>> ObtenerRegistroEstudiante(string ID_TIPO_DOCUMENTO, string NUMERO_DOCUMENTO)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_TIPO_DOCUMENTO" , ID_TIPO_DOCUMENTO),
                new Parameter("@NUMERO_DOCUMENTO" , NUMERO_DOCUMENTO)
            };

            try
            {
                var result = this.ExecuteReader<USP_INTERNO_CERTIFICADO_LISTAR_REGISTROS_ESTUDIANTE_Result>(
                    "dbo.USP_INTERNO_CERTIFICADO_LISTAR_REGISTROS_ESTUDIANTE"
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

        public async Task<IEnumerable<EstudianteCertificadoEntity>> ObtenerDatosEstudiante(string ID_TIPO_DOCUMENTO, string NUMERO_DOCUMENTO)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_TIPO_DOCUMENTO" , ID_TIPO_DOCUMENTO),
                new Parameter("@NUMERO_DOCUMENTO" , NUMERO_DOCUMENTO)
            };

            try
            {
                var result = this.ExecuteReader<EstudianteCertificadoEntity>(
                    "dbo.USP_INTERNO_CERTIFICADO_ESTUDIANTE_SELECT_POR_DOCUMENTO"
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

        public async void DesactivarSolicitudesEmitidas(int ID_SOLICITUD, int ID_PERSONA, string ID_MODALIDAD, string ID_NIVEL)
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

        public async void DesactivarSolicitudesEmitidasPorIdEstudiante(int ID_SOLICITUD, int ID_ESTUDIANTE, string ID_MODALIDAD, string ID_NIVEL)
        {
            var parm = new Parameter[] {
                new Parameter("@ID_SOLICITUD" , ID_SOLICITUD),
                new Parameter("@ID_ESTUDIANTE" , ID_ESTUDIANTE),
                new Parameter("@ID_MODALIDAD" , ID_MODALIDAD),
                new Parameter("@ID_NIVEL" , ID_NIVEL)
            };

            try
            {
                this.ExecuteNonQuery(
                    "dbo.USP_INTERNO_CERTIFICADO_SOLICITUD_DESACTIVAR_POR_ID_ESTUDIANTE"
                    , CommandType.StoredProcedure
                    , ref parm
                );
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
