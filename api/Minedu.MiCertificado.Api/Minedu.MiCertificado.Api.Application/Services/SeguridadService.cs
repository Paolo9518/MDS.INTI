using Microsoft.Extensions.Configuration;
using Minedu.Comun.Helper;
using Minedu.MiCertificado.Api.Application.Contracts.Services;
using Minedu.MiCertificado.Api.BusinessLogic.Models.Central;
using Minedu.MiCertificado.Api.BusinessLogic.Models.Certificado;
using SeguridadWSService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minedu.MiCertificado.Api.Application.Services
{
    public class SeguridadService : ISeguridadService
    {
        private readonly IConfiguration _configuration;

        public SeguridadService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private System.ServiceModel.ChannelFactory<T> init<T>()
        {
            var binding = new System.ServiceModel.BasicHttpBinding(System.ServiceModel.BasicHttpSecurityMode.None);
            //binding.MaxReceivedMessageSize = Int32.MaxValue;
            //binding.MaxBufferSize = Int32.MaxValue;

            binding.MaxReceivedMessageSize = Int32.MaxValue;
            binding.MaxBufferSize = Int32.MaxValue;

            string url = _configuration.GetSection("SeguridadService:BaseUrl").Value;

            var endpoint = new System.ServiceModel.EndpointAddress(url);
            var channelFactory = new System.ServiceModel.ChannelFactory<T>(binding, endpoint);
            return channelFactory;
        }

        
        public async Task<BEUsuarioResponse> SeguridadConsultarDatos(AuthModel request, string idSistema)
        {
            SeguridadServicesClient seguridadProxy = new SeguridadServicesClient();
            BEUsuarioResponse result = null;

            var oAcceso = new BELogAcceso
            {
                usr_log_usr = request.usuario,
                fecha_inicio_conexion = DateTime.Now,
                id_sistema_id = _configuration.GetSection("SeguridadService:IdSistemaSiagie").Value,
                nombre_estacion = "10.10.10.10",
                mac_address = ""
            };

            var oUsuario = new BEUsuario
            {
                usr_login = request.usuario,
                usr_password = request.contrasenia
            };

            var oUsuarioAuth = new UsuarioAutenticacion_IIRequest
            {
                oBELogAcceso = oAcceso,
                oBEUsuario = oUsuario,
                sDBName = _configuration.GetSection("SeguridadService:DataBase").Value,
                sMensaje = ""
            };

            try
            {
                using (var clientService = init<ISeguridadServicesChannel>().CreateChannel())
                {
                    var response = await seguridadProxy.UsuarioAutenticacion_IIAsync(oUsuarioAuth);

                    if (response != null)
                    {
                        result = new BEUsuarioResponse
                        {
                            usrLogin = response.oBEUsuario.usr_login,
                            usrLogUsr = response.oBEUsuario.usr_log_usr,
                            nombresUsuario = response.oBEUsuario.nombres_usuario,
                            apellidoPaternoUsuario = response.oBEUsuario.apellido_paterno_usuario,
                            apellidoMaternoUsuario = response.oBEUsuario.apellido_materno_usuario,
                            fullNombre = response.oBEUsuario.fullnombre,
                            tipoDocumento = response.oBEUsuario.tipo_documento.GetValueOrDefault(),
                            numeroDocumento = response.oBEUsuario.numero_documento,
                            correoElectronico = response.oBEUsuario.correo_electronico,
                            estadoUsuario = response.oBEUsuario.estado_usuario.GetValueOrDefault(),
                            resultado = response.UsuarioAutenticacion_IIResult,
                            mensaje = response.sMensaje
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        public async Task<BEUsuarioPermisoResponse> UsuarioPermisoTraerPorDefecto(string usrLogUsr, string id_sistema_id)
        {
            SeguridadServicesClient seguridadProxy = new SeguridadServicesClient();
            BEUsuarioPermisoResponse result = null;

            var request = new UsuarioPermisotraerporDefectoRequest()
            {
                sLogin = usrLogUsr,
                sSistema = id_sistema_id
            };

            try
            {
                using (var clientService = init<ISeguridadServicesChannel>().CreateChannel())
                {
                    var response = await seguridadProxy.UsuarioPermisotraerporDefectoAsync(request);

                    if (response != null)
                    {
                        result = new BEUsuarioPermisoResponse()
                        {
                            idRol = response.UsuarioPermisotraerporDefectoResult.idrol,
                            rolDescripcion = response.UsuarioPermisotraerporDefectoResult.rolDescripcion,

                            codigo = response.UsuarioPermisotraerporDefectoResult.codigo,
                            tipoSede = response.UsuarioPermisotraerporDefectoResult.tipo_sede.GetValueOrDefault(),

                            //dre = response.UsuarioPermisotraerporDefectoResult.dre,
                            //codigel = response.UsuarioPermisotraerporDefectoResult.dre,

                            idSede = response.UsuarioPermisotraerporDefectoResult.id_sede,
                            idSedeAnx = response.UsuarioPermisotraerporDefectoResult.id_sede_anx,
                            cenEdu = response.UsuarioPermisotraerporDefectoResult.cen_edu,

                            porDefecto = response.UsuarioPermisotraerporDefectoResult.por_defecto.GetValueOrDefault(),

                            descentralizadoUp = response.UsuarioPermisotraerporDefectoResult.descentralizado_up.GetValueOrDefault(),
                            estadoUsuarioPermiso = response.UsuarioPermisotraerporDefectoResult.estado_usuario_permiso
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        public async Task<BEUsuarioPermisoResponse> UsuarioPermisoLeerPorSistema(string usrLogUsr, string id_sistema_id)
        {
            SeguridadServicesClient seguridadProxy = new SeguridadServicesClient();
            BEUsuarioPermisoResponse result = null;

            var request = new UsuarioPermisoLeerPorSistemaRequest()
            {
                sLogin = usrLogUsr,
                IdSistema = id_sistema_id//_configuration.GetSection("SeguridadService:IdSistemaSiagie").Value
            };

            try
            {
                using (var clientService = init<ISeguridadServicesChannel>().CreateChannel())
                {
                    var response = await seguridadProxy.UsuarioPermisoLeerPorSistemaAsync(request);

                    if (response != null)
                    {
                        result = new BEUsuarioPermisoResponse()
                        {
                            idRol = response.UsuarioPermisoLeerPorSistemaResult.idrol,
                            rolDescripcion = response.UsuarioPermisoLeerPorSistemaResult.rolDescripcion,

                            codigo = response.UsuarioPermisoLeerPorSistemaResult.codigo,
                            tipoSede = response.UsuarioPermisoLeerPorSistemaResult.tipo_sede.GetValueOrDefault(),

                            //dre = response.UsuarioPermisoLeerPorSistemaResult.dre,
                            //codigel = response.UsuarioPermisoLeerPorSistemaResult.dre,

                            idSede = response.UsuarioPermisoLeerPorSistemaResult.id_sede,
                            idSedeAnx = response.UsuarioPermisoLeerPorSistemaResult.id_sede_anx,
                            cenEdu = response.UsuarioPermisoLeerPorSistemaResult.cen_edu,

                            porDefecto = response.UsuarioPermisoLeerPorSistemaResult.por_defecto.GetValueOrDefault(),

                            descentralizadoUp = response.UsuarioPermisoLeerPorSistemaResult.descentralizado_up.GetValueOrDefault(),
                            estadoUsuarioPermiso = response.UsuarioPermisoLeerPorSistemaResult.estado_usuario_permiso
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        public async Task<List<BEUsuarioPermisoResponse>> UsuarioPermisoBuscar(string usrLogUsr, string id_sistema_id)
        {
            SeguridadServicesClient seguridadProxy = new SeguridadServicesClient();
            List<BEUsuarioPermisoResponse> result = null;

            var request = new UsuarioPermisoBuscarRequest()
            {
                usrLogUsr = usrLogUsr,
                idSistema = id_sistema_id,
                pageIndex = 1,
                pageSize = 10
            };

            try
            {
                using (var clienteService = init<ISeguridadServicesChannel>().CreateChannel())
                {
                    var response = await seguridadProxy.UsuarioPermisoBuscarAsync(request);

                    if (response != null)
                    {
                        result = response.UsuarioPermisoBuscarResult.Select(x => new BEUsuarioPermisoResponse()
                        {
                            idRol = x.id_rol,
                            rolDescripcion = x.rolDescripcion,

                            codigo = x.codigo,
                            tipoSede = x.tipo_sede.GetValueOrDefault(),

                            idSede = x.id_sede,
                            idSedeAnx = x.id_sede_anx,
                            cenEdu = x.cen_edu,

                            idNivel = x.niv_mod,
                            dscNivel = x.niv_mod_detalle,

                            porDefecto = x.por_defecto.GetValueOrDefault(),

                            descentralizadoUp = x.descentralizado_up.GetValueOrDefault()
                        }).ToList();
                    }
                        
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        public async Task<List<BEUsuarioPermisoResponse>> UsuarioPermisoListar(string usrLogUsr)
        {
            SeguridadServicesClient seguridadProxy = new SeguridadServicesClient();
            List<BEUsuarioPermisoResponse> result = null;
            
            var request = new UsuarioPermisoListarRequest()
            {
                usuario = usrLogUsr
            };

            try
            {
                using (var clienteService = init<ISeguridadServicesChannel>().CreateChannel())
                {
                    var response = await seguridadProxy.UsuarioPermisoListarAsync(request);

                    if (response != null)
                    {
                        result = response.UsuarioPermisoListarResult.Select(x => new BEUsuarioPermisoResponse()
                        {
                            idRol = x.idrol,
                            rolDescripcion = x.rolDescripcion,

                            codigo = x.codigo,
                            tipoSede = x.tipo_sede.GetValueOrDefault(),

                            idSede = x.id_sede,
                            idSedeAnx = x.id_sede_anx,
                            cenEdu = x.cen_edu,

                            porDefecto = x.por_defecto.GetValueOrDefault(),

                            descentralizadoUp = x.descentralizado_up.GetValueOrDefault(),
                            estadoUsuarioPermiso = x.estado_usuario_permiso,

                            idSistema = x.id_sistema
                        }).ToList();
                    }   
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        public async Task<BEUsuarioPermisoResponse> UsuarioPermisosLlenar(string anexo, string idSede, string idSistema, string usrLogUsr)
        {
            SeguridadServicesClient seguridadProxy = new SeguridadServicesClient();
            BEUsuarioPermisoResponse result = null;

            UsuarioPermisoObtenerPorSedeRequest request = new UsuarioPermisoObtenerPorSedeRequest
            {
                idSede = idSede,
                anexo = anexo,
                idSistema = idSistema,
                usrLogUsr = usrLogUsr
            };

            try
            {
                using (var clientService = init<ISeguridadServicesChannel>().CreateChannel())
                {
                    var response = await seguridadProxy.UsuarioPermisoObtenerPorSedeAsync(request);

                    if (response != null)
                    {

                        result = new BEUsuarioPermisoResponse()
                        {
                            idRol = response.UsuarioPermisoObtenerPorSedeResult.idrol,
                            rolDescripcion = response.UsuarioPermisoObtenerPorSedeResult.rolDescripcion,

                            codigo = response.UsuarioPermisoObtenerPorSedeResult.codigel,
                            tipoSede = response.UsuarioPermisoObtenerPorSedeResult.tipo_sede.GetValueOrDefault(),

                            idSede = response.UsuarioPermisoObtenerPorSedeResult.id_sede,
                            idSedeAnx = response.UsuarioPermisoObtenerPorSedeResult.id_sede_anx,
                            cenEdu = response.UsuarioPermisoObtenerPorSedeResult.cen_edu,

                            porDefecto = response.UsuarioPermisoObtenerPorSedeResult.por_defecto.GetValueOrDefault(),

                            //idModalidad = response.UsuarioPermisoObtenerPorSedeResult.id_modalidad,
                            //idNivel = response.UsuarioPermisoObtenerPorSedeResult.niv_mod,
                            //dscNivel = response.UsuarioPermisoObtenerPorSedeResult.niv_mod_detalle
                        };
                    }
                };
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        public async Task<List<UsuarioPermiso>> UsuarioPermisoListarSede(string cCodModular, string cCodAnexo, string idSistema)
        {
            SeguridadServicesClient seguridadProxy = new SeguridadServicesClient();
            List<UsuarioPermiso> result = new List<UsuarioPermiso>();

            UsuarioPermisoListarSedeRequest request = new UsuarioPermisoListarSedeRequest()
            {
              oBEUsuarioPermiso = new BEUsuarioPermiso()
              {
                  id_sistema = idSistema,
                  //usr_log_usr = string.Empty,
                  //usr_login = string.Empty,
                  tipo_sede = 1,
                  id_sede = cCodModular,
                  id_sede_anx = cCodAnexo,
                  //accion = "0",
                  limit_inicio = 0,
                  limit_registros = 500,
                  //CriterioFiltro = ""
              },
              descentralizado = true
            };

            try
            {
                using (var clientService = init<ISeguridadServicesChannel>().CreateChannel())
                {
                    var response = await seguridadProxy.UsuarioPermisoListarSedeAsync(request);

                    result = response.UsuarioPermisoListarSedeResult.Select(x => new UsuarioPermiso()
                    {
                        usr_login = x.usr_login,
                        id_sistema = x.id_sistema,
                        tipo_sede = x.tipo_sede,
                        id_sede = x.id_sede,
                        id_sede_anx = x.id_sede_anx,
                        //codigo = x.codigo,
                        por_defecto = x.por_defecto,
                        nivel = x.nivel,
                        //verificacion = x.verificacion,
                        //fechaprimeringreso = x.fechaprimeringreso,
                        //idrol = x.idrol,
                        //doc_referencia = x.doc_referencia,
                        //estado_usuario_permiso = x.estado_usuario_permiso,
                        cen_edu = x.cen_edu,
                        descentralizado_up = x.descentralizado_up,
                        //usuario_registro = x.usuario_registro,
                        //fecha_registro = x.fecha_registro,
                        //usuario_modificador = x.usuario_modificador,
                        //fecha_modificacion = x.fecha_modificacion,
                        
                        fullname = x.fullname,
                        rolDescripcion = x.rolDescripcion
                    }).ToList();
                };

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<UsuarioPermiso> UsuarioPermisoLeerPorKey(UsuarioPermisoResponse request)
        {
            SeguridadServicesClient seguridadProxy = new SeguridadServicesClient();

            UsuarioPermiso result = new UsuarioPermiso();

            UsuarioPermisoLeerPorKeyRequest req = new UsuarioPermisoLeerPorKeyRequest()
            {
                oBEUsuarioPermiso = new BEUsuarioPermiso()
                {
                    id_sistema = request.id_sistema,
                    id_sede = request.id_sede,
                    id_sede_anx = request.id_sede_anx,
                    usr_login = request.usr_login,
                    tipo_sede = request.tipo_sede,
                    cen_edu = request.cen_edu,
                    id_sistema_id = request.id_sistema,
                    usr_log_usr = request.usr_login,
                    por_defecto = request.por_defecto,
                    nivel = request.nivel,
                    descentralizado_up = request.descentralizado_up
                }
            };

            try
            {
                using (var clientService = init<ISeguridadServicesChannel>().CreateChannel())
                {
                    var response = await seguridadProxy.UsuarioPermisoLeerPorKeyAsync(req);

                    result = new UsuarioPermiso()
                    {
                        usr_login = response.UsuarioPermisoLeerPorKeyResult.usr_login,
                        id_sistema = response.UsuarioPermisoLeerPorKeyResult.id_sistema,
                        tipo_sede = response.UsuarioPermisoLeerPorKeyResult.tipo_sede,
                        id_sede = response.UsuarioPermisoLeerPorKeyResult.id_sede,
                        id_sede_anx = response.UsuarioPermisoLeerPorKeyResult.id_sede_anx,
                        codigo = response.UsuarioPermisoLeerPorKeyResult.codigo,
                        por_defecto = response.UsuarioPermisoLeerPorKeyResult.por_defecto,
                        nivel = response.UsuarioPermisoLeerPorKeyResult.nivel,
                        verificacion = response.UsuarioPermisoLeerPorKeyResult.verificacion,
                        fechaprimeringreso = response.UsuarioPermisoLeerPorKeyResult.fechaprimeringreso,
                        idrol = response.UsuarioPermisoLeerPorKeyResult.idrol,
                        doc_referencia = response.UsuarioPermisoLeerPorKeyResult.doc_referencia,
                        estado_usuario_permiso = response.UsuarioPermisoLeerPorKeyResult.estado_usuario_permiso,
                        cen_edu = response.UsuarioPermisoLeerPorKeyResult.cen_edu,
                        descentralizado_up = response.UsuarioPermisoLeerPorKeyResult.descentralizado_up,
                        usuario_registro = response.UsuarioPermisoLeerPorKeyResult.usuario_registro,
                        fecha_registro = response.UsuarioPermisoLeerPorKeyResult.fecha_registro,
                        usuario_modificador = response.UsuarioPermisoLeerPorKeyResult.usuario_modificador,
                        fecha_modificacion = response.UsuarioPermisoLeerPorKeyResult.fecha_modificacion
                    };
                };

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async void UsuarioPermisoActualizar(UsuarioPermisoRequest request)
        {
            SeguridadServicesClient seguridadProxy = new SeguridadServicesClient();

            UsuarioPermisoActualizarRequest req = new UsuarioPermisoActualizarRequest()
            {
                oBEUsuarioPermiso = new BEUsuarioPermiso()
                {
                    usr_login = request.usr_login,
                    id_sistema = request.id_sistema,
                    tipo_sede = request.tipo_sede,
                    id_sede = request.id_sede,
                    id_sede_anx = request.id_sede_anx,
                    codigo = request.codigo,
                    por_defecto = request.por_defecto,
                    nivel = request.nivel,
                    verificacion = request.verificacion,
                    fechaprimeringreso = request.fechaprimeringreso,
                    idrol = request.idrol,
                    doc_referencia = request.doc_referencia,
                    estado_usuario_permiso = request.estado_usuario_permiso,
                    cen_edu = request.cen_edu,
                    descentralizado_up = request.descentralizado_up,
                    usuario_registro = request.usuario_registro,
                    fecha_registro = request.fecha_registro,
                    usuario_modificador = request.usuario_modificador,
                    fecha_modificacion = request.fecha_modificacion,
                    id_sistema_id = request.id_sistema_id
                }
            };

            try
            {
                using (var clientService = init<ISeguridadServicesChannel>().CreateChannel())
                {
                    await seguridadProxy.UsuarioPermisoActualizarAsync(req);

                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async void UsuarioPermisoInsertar(UsuarioPermisoRequest request)
        {
            SeguridadServicesClient seguridadProxy = new SeguridadServicesClient();

            UsuarioPermisoInsertarRequest req = new UsuarioPermisoInsertarRequest()
            {
                oBEUsuarioPermiso = new BEUsuarioPermiso()
                {
                    usr_login = request.usr_login,
                    id_sistema = request.id_sistema,
                    tipo_sede = request.tipo_sede,
                    id_sede = request.id_sede,
                    id_sede_anx = request.id_sede_anx,
                    codigo = request.codigo,
                    por_defecto = request.por_defecto,
                    nivel = request.nivel,
                    verificacion = request.verificacion,
                    fechaprimeringreso = request.fechaprimeringreso,
                    idrol = request.idrol,
                    doc_referencia = request.doc_referencia,
                    estado_usuario_permiso = request.estado_usuario_permiso,
                    cen_edu = request.cen_edu,
                    descentralizado_up = request.descentralizado_up,
                    usuario_registro = request.usuario_registro,
                    fecha_registro = request.fecha_registro,
                    usuario_modificador = request.usuario_modificador,
                    fecha_modificacion = request.fecha_modificacion
                }
            };

            try
            {
                using (var clientService = init<ISeguridadServicesChannel>().CreateChannel())
                {
                    await seguridadProxy.UsuarioPermisoInsertarAsync(req);

                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
