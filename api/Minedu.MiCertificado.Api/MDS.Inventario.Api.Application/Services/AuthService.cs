using Minedu.Comun.Helper;
using MDS.Inventario.Api.Application.Contracts.Security;
using MDS.Inventario.Api.Application.Contracts.Services;
using Mappers = MDS.Inventario.Api.Application.Mappers;
using MDS.Inventario.Api.DataAccess.Contracts.UnitOfWork;
using Models = MDS.Inventario.Api.Application.Entities.Models;
using SeguridadWSService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using MDS.Inventario.Api.Application.Utils;
using Newtonsoft.Json;
using MDS.Inventario.Api.Application.Security;

namespace MDS.Inventario.Api.Application.Services
{

    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEncryptionServerSecurity _encryptionServerSecurity;
        private readonly ISiagieService _siagieService;
        private readonly IConfiguration _configuration;
        private readonly ISeguridadService _seguridadService;
        public readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IUnitOfWork unitOfWork,
            IEncryptionServerSecurity encryptionServerSecurity,
            ISiagieService siagieService,
            IConfiguration configuration,
            ISeguridadService seguridadService,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _unitOfWork = unitOfWork;
            _encryptionServerSecurity = encryptionServerSecurity;
            _siagieService = siagieService;
            _configuration = configuration;
            _seguridadService = seguridadService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<StatusResponse> ObtenerUsuario(Models.Certificado.AuthModel encryptedRequest)
        {
            var response = new StatusResponse();
            SeguridadServicesClient seguridadProxy = new SeguridadServicesClient();
            List<Models.Certificado.IEModel> lstIEModel = new List<Models.Certificado.IEModel>();

            var request = new Models.Certificado.AuthModel()
            {
                usuario = encryptedRequest.usuario,
                contrasenia = encryptedRequest.contrasenia
            };

            var idSistemaSiagie = _configuration.GetSection("SeguridadService:IdSistemaSiagie").Value.ToString();
            var idSistemaCertificado = _configuration.GetSection("SeguridadService:IdSistemaCertificado").Value.ToString();

            try
            {
                #region TOKEN_SIAGIE
                //Construir request;
                var tokenRequest = new Models.Siagie.TokenRequest()
                {
                    ServerName = _configuration.GetSection("SiagieService:ServerName").Value,
                    Ticket = _configuration.GetSection("SiagieService:Ticket").Value,
                    UserId = _configuration.GetSection("SiagieService:UserId").Value,
                    UserName = _configuration.GetSection("SiagieService:UserName").Value
                };

                //Obtener token
                var tokenSiagieResponse = await _siagieService.PostServiceToken<string>(tokenRequest);

                //Verificar token
                if (tokenSiagieResponse == null || tokenSiagieResponse.Equals(""))
                {
                    response.Success = false;
                    response.Data = null;
                    response.Messages.Add("Ocurrió un problema al procesar su solicitud, intentelo nuevamente.");
                    return response;
                }
                #endregion TOKEN_SIAGIE

                #region TOKEN_CERTIFICADO
                //Encapsular token de Siagie en token de Certificado
                var secretKey = _configuration.GetSection("JwT:Key").Value;
                var key = Encoding.ASCII.GetBytes(secretKey);

                var claimsConfig = new[]
                {
                    new Claim("siagie", _encryptionServerSecurity.Encrypt(tokenSiagieResponse.ToString()))
                };

                var issuerConfig = _configuration.GetSection("JwT:Issuer").Value;
                var audienceConfig = _configuration.GetSection("JwT:Audience").Value;
                var expiresConfig = DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration.GetSection("JwT:Time").Value));
                var credsConfig = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

                JwtSecurityToken tokenCertificado = new JwtSecurityToken
                (
                    issuer: issuerConfig,
                    audience: audienceConfig,
                    claims: claimsConfig,
                    expires: expiresConfig,
                    signingCredentials: credsConfig
                );
                #endregion TOKEN_CERTIFICADO

                //Login con Central
                var usuarioResponse = await _seguridadService.SeguridadConsultarDatos(request, idSistemaSiagie);



                //desde aqui
                if (usuarioResponse == null)
                {
                    throw new ArgumentException("Ocurrió un problema al procesar su solicitud, intentelo nuevamente.");
                }

                if (!usuarioResponse.resultado)
                {
                    throw new ArgumentException("Ocurrió un problema al procesar su solicitud, intentelo nuevamente.");
                }

                var usuarioPermisoResponse = await _seguridadService.UsuarioPermisoTraerPorDefecto(usuarioResponse.usrLogUsr, idSistemaSiagie);

                if (usuarioPermisoResponse == null)
                {
                    usuarioPermisoResponse = await _seguridadService.UsuarioPermisoLeerPorSistema(usuarioResponse.usrLogUsr, idSistemaSiagie);

                    if (usuarioPermisoResponse == null)
                    {
                        throw new ArgumentException("Ocurrió un problema al procesar su solicitud, intentelo nuevamente.");
                    }
                }

                Models.Certificado.MenuNivelRolModel menuNivel = null;
                Models.Siagie.ColegioPadronResponse iieePadron = null;
                string idRolTemporal = "0";
                string estado = "1";

                if (usuarioPermisoResponse.tipoSede == 1) //Cole
                {
                    if (usuarioPermisoResponse.descentralizadoUp)
                    {
                        var usuarioCertificadoResponse = await _seguridadService.UsuarioPermisosLlenar(usuarioPermisoResponse.idSedeAnx,
                            usuarioPermisoResponse.idSede,
                            idSistemaCertificado,
                            request.usuario);

                        if (usuarioCertificadoResponse != null)
                        {
                            idRolTemporal = usuarioPermisoResponse.idRol;
                        }
                        else
                        {
                            estado = "0";
                        }
                    }
                    else
                    {
                        idRolTemporal = usuarioPermisoResponse.idRol;
                    }

                    //DATOS PADRÓN SIAGIE
                    var siagieRequest = new Models.Siagie.ColegioRequest()
                    {
                        codMod = usuarioPermisoResponse.idSede,
                        anexo = usuarioPermisoResponse.idSedeAnx
                    };

                    var statusResponse = await _siagieService
                            .GetServiceByQueryAndToken<StatusResponse,
                            Models.Siagie.ColegioRequest>(tokenSiagieResponse, "iiee", siagieRequest);

                    //Consulta de iiee en Siagie: FAIL?
                    if (!statusResponse.Success)
                    {
                        throw new ArgumentException("No se logró obtener la información de la Institución Educativa por defecto.");
                    }

                    iieePadron = JsonConvert
                        .DeserializeObject<List<Models.Siagie.ColegioPadronResponse>>(statusResponse.Data.ToString())
                        .FirstOrDefault();

                    if (iieePadron == null)
                    {
                        throw new ArgumentException("No se logró obtener la información de la Institución Educativa por defecto.");
                    }
                }

                //MENU NIVEL X ROL
                var resultMenuNivel = await _unitOfWork.ObtenerMenuNivelPorRol(idRolTemporal);

                if (resultMenuNivel != null && resultMenuNivel.ToList().Count > 0)
                {
                    menuNivel = Mappers.Certificado.MenuNivelRolMapper.Map(resultMenuNivel.FirstOrDefault());
                }

                response.Data = new Models.Certificado.DatosUsuario()
                {
                    usuarioLogin = _encryptionServerSecurity.Encrypt(usuarioResponse.usrLogin),
                    //nombresUsuario = usuarioResponse.nombresUsuario,
                    nombresCompleto = usuarioResponse.fullNombre,

                    //tipoDocumento = _encryptionServerSecurity.Encrypt(usuarioResponse.tipoDocumento.ToString()),
                    numeroDocumento = _encryptionServerSecurity.Encrypt(usuarioResponse.numeroDocumento),

                    idRol = _encryptionServerSecurity.Encrypt(usuarioPermisoResponse.idRol),
                    dscRol = usuarioPermisoResponse.rolDescripcion,

                    // codigo = _encryptionServerSecurity.Encrypt(usuarioPermisoResponse.codigo),
                    tipoSede = ReactEncryptationSecurity.Encrypt(usuarioPermisoResponse.tipoSede.ToString()),

                    codigoModular = usuarioPermisoResponse.idSede,
                    anexo = usuarioPermisoResponse.idSedeAnx,
                    nombreIE = usuarioPermisoResponse.cenEdu,

                    idNivel = iieePadron != null ? _encryptionServerSecurity.Encrypt(iieePadron.idNivel) : null,
                    dscNivel = iieePadron != null ? iieePadron.dscNivel : null,
                    idModalidad = iieePadron != null ? ReactEncryptationSecurity.Encrypt(iieePadron.idModalidad) : null,

                    idMenuNivel = menuNivel != null ? ReactEncryptationSecurity.Encrypt(menuNivel.idMenuNivel.ToString()) : null,
                    dscMenuNivel = menuNivel != null ? menuNivel.dscMenuNivel : null,

                    token = new JwtSecurityTokenHandler().WriteToken(tokenCertificado),
                    //estado = ReactEncryptationSecurity.Encrypt(estado)
                };
                response.Success = true;
                response.Messages.Add("Inicio de Sesión exitoso.");
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.Success = false;
                response.Messages.Add("Error general:" + ex);
            }

            return response;
        }

        public async Task<StatusResponse> ObtenerRolCentralizado(Models.Certificado.RolCentralizadoRequest encryptedRequest)
        {
            var response = new StatusResponse();
            var siagie = _encryptionServerSecurity.Decrypt<string>(ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", ""), "");

            var request = new Models.Certificado.RolCentralizadoRequest()
            {
                usuarioLogin = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.usuarioLogin, ""),
                idRol = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.idRol, ""),
                codigo = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.codigo, ""),
                tipoSede = ReactEncryptationSecurity.Decrypt<string>(encryptedRequest.tipoSede, "0"),
                codigoModular = encryptedRequest.codigoModular
            };

            try
            {
                var siagieRequest = new Models.Siagie.ColegioRequest()
                {
                    codMod = request.codigoModular,
                    codUgel = request.idRol.Equals("031") ? request.codigo : "",
                    estado = request.idRol.Equals("031") ? "2" : ""
                };

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        Models.Siagie.ColegioRequest>(siagie, "iiee", siagieRequest);

                //Consulta de iiee en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    throw new ArgumentException("No se logró obtener la relación de Instituciones Educativas.");
                }

                List<Models.Siagie.ColegioPadronResponse> iieePadron = new List<Models.Siagie.ColegioPadronResponse>();
                iieePadron.AddRange(JsonConvert
                    .DeserializeObject<List<Models.Siagie.ColegioPadronResponse>>(statusResponse.Data.ToString())
                    .ToList());

                response.Data = iieePadron
                    .Select(y => new
                    {
                        codigoModular = y.codMod,
                        y.anexo,
                        centroEducativo = y.cenEdu,
                        codigo = encryptedRequest.codigo,
                        idNivel = _encryptionServerSecurity.Encrypt(y.idNivel),
                        descripcionNivel = y.dscNivel,
                        idModalidad = ReactEncryptationSecurity.Encrypt(y.idModalidad),
                        request.idRol
                    }).ToList();
                response.Success = true;
                response.Messages.Add("Consulta exitosa.");
            }
            catch (Exception ex)
            {
                response.Data = "";
                response.Success = false;
                response.Messages.Add("Error: " + ex.Message);
            }

            return response;
        }

        public async Task<StatusResponse> ObtenerRolesDescentralizados(Models.Certificado.RolDescentralizadoRequest encryptedRequest)
        {
            var response = new StatusResponse();

            string idSistemaSiagie = _configuration.GetSection("SeguridadService:IdSistemaSiagie").Value.ToString();

            var request = new Models.Certificado.RolDescentralizadoRequest()
            {
                usuarioLogin = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.usuarioLogin, ""),
                //idRol = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.idRol, "000"),
                //tipoSede = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.tipoSede, "0"),
                codigoModular = encryptedRequest.codigoModular
            };

            try
            {
                var responseRoles = await _seguridadService.UsuarioPermisoBuscar(request.usuarioLogin, idSistemaSiagie);

                response.Data = responseRoles
                    .Where(x => x.idSede != request.codigoModular)
                    .Select(y => new
                    {
                        codigoModular = y.idSede,
                        anexo = y.idSedeAnx,
                        centroEducativo = y.cenEdu,
                        codigo = _encryptionServerSecurity.Encrypt(y.codigo),
                        idNivel = _encryptionServerSecurity.Encrypt(y.idNivel),
                        descripcionNivel = y.dscNivel,
                        idModalidad = ConsultarModalidad(y.idNivel),
                        idRol = _encryptionServerSecurity.Encrypt(y.idRol)
                    }).ToList();
                /*.Select(z => new {
                    z.codigoModular,
                    z.anexo,
                    z.centroEducativo,
                    idNivel = _encryptionServerSecurity.Encrypt(z.nivel.idNivel),
                    z.nivel.descripcionNivel,
                    idModalidad = ReactEncryptationSecurity.Encrypt(z.nivel.idModalidad)
                }).ToList();*/
                response.Success = true;
                response.Messages.Add("Consulta exitosa.");
            }
            catch (Exception ex)
            {
                response.Data = "";
                response.Success = false;
                response.Messages.Add("Error: " + ex.Message);
            }
            return response;
        }

        private string ConsultarModalidad(string idNivel)
        {
            var result = "";

            switch (idNivel)
            {
                case "A1":
                case "A2":
                case "A3":
                case "A5":
                case "B0":
                case "C0":
                case "F0":
                case "G0":
                    result = "01";
                    break;
                case "D0":
                case "D1":
                case "D2":
                    result = "03";
                    break;
                case "E0":
                case "E1":
                case "E2":
                    result = "04";
                    break;
            }

            return result;
        }

        public async Task<StatusResponse> ObtenerRolPorModular(Models.Certificado.UsuarioModularRequest request)
        {
            var response = new StatusResponse();

            string idSistemaSiagie = _configuration.GetSection("SeguridadService:IdSistemaSiagie").Value.ToString();

            /*var request = new Models.Certificado.UsuarioModularRequest()
            {
                usuarioLogin = ReactEncryptationSecurity.Decrypt<string>(encryptedRequest.usuarioLogin, ""),
                codigoModular = encryptedRequest.codigoModular,
                anexo = encryptedRequest.anexo,
                idRol = ReactEncryptationSecurity.Decrypt<string>(encryptedRequest.idRol, "")
            };*/

            try
            {
                var usuarioPermisosResponse = await _seguridadService.UsuarioPermisoListar(request.usuarioLogin);

                if (usuarioPermisosResponse == null || usuarioPermisosResponse.Count == 0)
                {
                    throw new ArgumentException("Ocurrió un problema al asignar la IE seleccionada o no tiene permisos activos para dicha IE.");
                }

                var permisoPorModular = usuarioPermisosResponse
                        .Where(x => x.idSede == request.codigoModular
                        && x.idSedeAnx == request.anexo
                        && x.estadoUsuarioPermiso == 1
                        && x.idSistema == idSistemaSiagie)
                        .FirstOrDefault();

                if (permisoPorModular == null)
                {
                    throw new ArgumentException("Ocurrió un problema al asignar la IE seleccionada o no tiene permisos activos para dicha IE.");
                }

                string idRolTemporal = "0";
                string estado = "1";

                if (permisoPorModular.descentralizadoUp)
                {
                    var idSistemaCertificado = _configuration.GetSection("SeguridadService:IdSistemaCertificado").Value.ToString();

                    var responseRoles = await _seguridadService.UsuarioPermisoBuscar(request.usuarioLogin, idSistemaCertificado);



                    var result = responseRoles.Where(x => x.idSede == request.codigoModular
                                                            && x.idSedeAnx == request.anexo
                                                            && x.idRol == request.idRol
                                                            && x.estadoUsuarioPermiso == 1)
                        .FirstOrDefault();

                    if (result != null)
                    {
                        idRolTemporal = permisoPorModular.idRol;
                    }
                    else
                    {
                        estado = "0";
                    }
                }
                else
                {
                    idRolTemporal = permisoPorModular.idRol;
                }

                var resultMenuNivel = await _unitOfWork.ObtenerMenuNivelPorRol(idRolTemporal);

                if (resultMenuNivel == null)
                {
                    throw new ArgumentException("No se pudo asignar el Menú de opciones para el rol de la IE seleccionada.");
                }

                var menuNivel = Mappers.Certificado.MenuNivelRolMapper.Map(resultMenuNivel.FirstOrDefault());

                var datosUsuario = new
                {
                    idRol = ReactEncryptationSecurity.Encrypt(permisoPorModular.idRol),
                    dscRol = permisoPorModular.rolDescripcion,

                    tipoSede = ReactEncryptationSecurity.Encrypt(permisoPorModular.tipoSede.ToString()),

                    idMenuNivel = menuNivel != null ? ReactEncryptationSecurity.Encrypt(menuNivel.idMenuNivel.ToString()) : null,
                    dscMenuNivel = menuNivel != null ? menuNivel.dscMenuNivel : null,

                    estado = ReactEncryptationSecurity.Encrypt(estado)
                };
                //Enviar los datos de usuario como StatusResponse
                response.Data = datosUsuario;
                response.Success = true;
                response.Messages.Add("Consulta Exitosa");
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.Success = true;
                response.Messages.Add(ex.Message);
            }

            return response;
        }

        public async Task<Models.Certificado.MenuCertificadoModel> GetMenu(string id)
        {
            var result = await _unitOfWork.ObtenerCertificadoMenu(Mappers.Certificado.MenuMapper.Map(new Models.Certificado.MenuCertificadoModel()
            {
                idMenu = id
            }, _encryptionServerSecurity));

            return result.Select(x => Mappers.Certificado.MenuMapper.Map(x, _encryptionServerSecurity)).FirstOrDefault();
        }

        public async Task<StatusResponse> ObtenerMenuNivelPorRol(Models.Certificado.RolRequest encryptedRequest)
        {
            var result = new StatusResponse();

            var request = new Models.Certificado.RolRequest()
            {
                idRol = ReactEncryptationSecurity.Decrypt<string>(encryptedRequest.idRol, "")
            };

            try
            {
                if (request.idRol.Length != 3)
                {
                    throw new ArgumentException("Rol inválido.");
                }

                var resultMenuNivel = await _unitOfWork.ObtenerMenuNivelPorRol(request.idRol);

                if (resultMenuNivel == null || resultMenuNivel.ToList().Count == 0)
                {
                    throw new ArgumentException("No se encontró un nivel para el Rol seleccionado.");
                }

                var menuNivel = Mappers.Certificado.MenuNivelRolMapper.Map(resultMenuNivel.FirstOrDefault());

                result.Success = true;
                result.Data = new
                {
                    idMenuNivel = ReactEncryptationSecurity.Encrypt(menuNivel.idMenuNivel.ToString()),
                    menuNivel.dscMenuNivel
                };
                result.Messages.Add("Consulta de Menu Nivel, exitosa.");
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Data = null;
                result.Messages.Add(ex.GetType().IsAssignableFrom(typeof(ArgumentException))
                    ? ex.Message
                    : "Se presentó un inconveniente al obtener la información solicitada.");
                return result;
            }
        }

        private Models.Siagie.UsuarioResponse GetToken(string token)
        {
            var jsonUsuario = new Models.Siagie.UsuarioResponse();
            var ok = true;
            byte[] key = Encoding.ASCII.GetBytes(_configuration.GetSection("JwT:KeySiagie").Value);
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var validations = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                var claims = handler.ValidateToken(token, validations, out var tokenSecure);

                jsonUsuario = JsonConvert.DeserializeObject<Models.Siagie.UsuarioResponse>(claims.Claims.FirstOrDefault().Value);

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return null;
            }
            return jsonUsuario;
        }

        public async Task<StatusResponse> Login(Models.Certificado.AuthUserModel request)
        {
            var response = new StatusResponse();
            string idSistemaSiagie = _configuration.GetSection("SeguridadService:IdSistemaSiagie").Value.ToString();
            string idSistemaCertificado = _configuration.GetSection("SeguridadService:IdSistemaCertificado").Value.ToString();
            string idRolTemporal = "0";
            string estado = "1";
            var usuarioOACIGED = _configuration.GetSection("Roles:OACIGED").Value.ToString();
            var usuarioUGEL = _configuration.GetSection("Roles:Ugel").Value.ToString();

            try
            {
                var usuarioRequest = GetToken(request.token);
                if (usuarioRequest == null)
                {
                    response.Success = false;
                    response.Data = null;
                    response.Messages.Add("Usted no cuenta con permisos para ingresar al sistema. Se redireccionará a Siagie");
                    return response;
                }

                if (usuarioRequest.codModular == null)
                {
                    response.Success = false;
                    response.Data = null;
                    response.Messages.Add("Usted no ha seleccionado una Institución Educativa. Se redireccionará a Siagie");
                    return response;
                }


                #region TOKEN_SIAGIE
                //Construir request;
                var tokenRequest = new Models.Siagie.TokenRequest()
                {
                    ServerName = _configuration.GetSection("SiagieService:ServerName").Value,
                    Ticket = _configuration.GetSection("SiagieService:Ticket").Value,
                    UserId = _configuration.GetSection("SiagieService:UserId").Value,
                    UserName = _configuration.GetSection("SiagieService:UserName").Value
                };

                //Obtener token
                var tokenSiagieResponse = await _siagieService.PostServiceToken<string>(tokenRequest);

                //Verificar token
                if (tokenSiagieResponse == null || tokenSiagieResponse.Equals(""))
                {
                    response.Success = false;
                    response.Data = null;
                    response.Messages.Add("Ocurrió un problema al procesar su solicitud, intentelo nuevamente.");
                    return response;
                }
                #endregion TOKEN_SIAGIE

                #region TOKEN_CERTIFICADO
                //Encapsular token de Siagie en token de Certificado
                var secretKey = _configuration.GetSection("JwT:Key").Value;
                var key = Encoding.ASCII.GetBytes(secretKey);

                // encriptando datos de la sesion de usuario
                var userInfo = string.Format("{0}|{1}|{2}", usuarioRequest.numeroDocumento, usuarioRequest.codModular, usuarioRequest.anexo);

                var claimsConfig = new[]
                {
                    new Claim("siagie", _encryptionServerSecurity.Encrypt(tokenSiagieResponse.ToString())),
                    new Claim("certificado", _encryptionServerSecurity.Encrypt(userInfo.ToString())),
                };

                var issuerConfig = _configuration.GetSection("JwT:Issuer").Value;
                var audienceConfig = _configuration.GetSection("JwT:Audience").Value;
                var expiresConfig = DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration.GetSection("JwT:Time").Value));
                var credsConfig = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

                JwtSecurityToken tokenCertificado = new JwtSecurityToken
                (
                    issuer: issuerConfig,
                    audience: audienceConfig,
                    claims: claimsConfig,
                    expires: expiresConfig,
                    signingCredentials: credsConfig
                );
                #endregion TOKEN_CERTIFICADO

                #region IE_ACTIVO
                var InstitucionEducativa = new Models.Certificado.InstitucionEducativaPorDreUgelRequest
                {
                    CodigoModular = usuarioRequest.codModular,
                    anexo = usuarioRequest.anexo
                };
                var institucionResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        Models.Certificado.InstitucionEducativaPorDreUgelRequest>(tokenSiagieResponse, "datosinstitucioneducativa", InstitucionEducativa);

                var institucionEducativa = JsonConvert
                               .DeserializeObject<List<Models.Certificado.InstitucionEducativaPorDreUgelResponse>>(institucionResponse.Data.ToString())
                               .FirstOrDefault();

                var especialistaUgel = _configuration.GetSection("Roles:Ugel").Value.ToString();
                if (especialistaUgel == usuarioRequest.idRol)
                {
                    if (institucionEducativa.Estado == "1")
                    {
                        response.Success = false;
                        response.Data = null;
                        response.Messages.Add("Usted no tiene permiso para emitir Certificado de Estudios en una I.E. activa.");
                        return response;
                    }
                }
                else
                {
                    if (institucionEducativa.Estado == "2")
                    {
                        response.Success = false;
                        response.Data = null;
                        response.Messages.Add("Usted no tiene permiso para emitir Certificado de Estudios en una I.E. inactiva.");
                        return response;
                    }
                }
                #endregion IE_ACTIVO

                #region PERMISOS

                var requestPermisos = new Models.Certificado.UsuarioModularRequest()
                {
                    usuarioLogin = usuarioRequest.loginUsuario,
                    codigoModular = usuarioRequest.codModular,
                    anexo = usuarioRequest.anexo,
                    idRol = usuarioRequest.idRol
                };

                var usuarioPermisosResponse = await _seguridadService.UsuarioPermisoListar(usuarioRequest.loginUsuario);

                if (usuarioPermisosResponse == null || usuarioPermisosResponse.Count() == 0)
                {
                    throw new ArgumentException("Ocurrió un problema al asignar la IE seleccionada o no tiene permisos activos para la IE.");
                }

                if (usuarioRequest.idRol != usuarioUGEL && usuarioRequest.idRol != usuarioOACIGED)
                {//OACIGED
                    var permisoPorModular = usuarioPermisosResponse
                            .Where(x => x.idSede == usuarioRequest.codModular
                            && x.idSedeAnx == usuarioRequest.anexo
                            && x.estadoUsuarioPermiso == 1
                            && x.idSistema == idSistemaSiagie)
                            .FirstOrDefault();

                    if (permisoPorModular == null)
                    {
                        throw new ArgumentException("Ocurrió un problema al asignar la IE seleccionada o no tiene permisos activos para dicha IE.");
                    }


                    if (permisoPorModular.descentralizadoUp)
                    {
                        var responseRoles = await _seguridadService.UsuarioPermisoBuscar(usuarioRequest.loginUsuario, idSistemaCertificado);

                        if (responseRoles == null)
                        {
                            response.Data = null;
                            response.Success = false;
                            response.Messages.Add("Usted no tiene permiso para emitir Certificado de Estudios en esta I.E.");
                        }

                        var result = responseRoles.Where(x => x.idSede == usuarioRequest.codModular
                                                                && x.idSedeAnx == usuarioRequest.anexo
                                                                && x.idRol == usuarioRequest.idRol
                                                                && x.estadoUsuarioPermiso == 1);

                        if (result != null)
                        {
                            idRolTemporal = permisoPorModular.idRol;
                        }
                        else
                        {
                            response.Data = null;
                            response.Success = false;
                            response.Messages.Add("Usted no cuenta con permisos asignados a la plataforma - Mi Certificado.");
                        }
                    }
                    else
                    {
                        idRolTemporal = permisoPorModular.idRol;
                    }
                }
                else //if (usuarioRequest.idRol == usuarioUGEL)
                {
                    var requestUgel = new Models.Certificado.RolCentralizadoRequest()
                    {
                        usuarioLogin = usuarioRequest.loginUsuario,
                        idRol = usuarioRequest.idRol,
                        codigo = usuarioPermisosResponse.FirstOrDefault().codigo,
                        tipoSede = usuarioPermisosResponse.FirstOrDefault().tipoSede.ToString(),
                        codigoModular = usuarioRequest.codModular
                    };

                    var siagieRequest = new Models.Siagie.ColegioRequest()
                    {
                        codMod = usuarioRequest.codModular,
                        codUgel = usuarioRequest.idRol.Equals(usuarioUGEL) ? usuarioPermisosResponse.FirstOrDefault().codigo : "",
                        estado = usuarioRequest.idRol.Equals(usuarioUGEL) ? "2" : ""
                    };

                    var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        Models.Siagie.ColegioRequest>(tokenSiagieResponse, "iiee", siagieRequest);

                    //Consulta de iiee en Siagie: FAIL?
                    if (statusResponse.Data==null)
                    {
                        response.Data = null;
                        response.Success = false;
                        response.Messages.Add("Usted no tiene permiso para emitir Certificado de Estudios en esta I.E.");
                        return response;
                    }

                    List<Models.Siagie.ColegioPadronResponse> iieePadron = new List<Models.Siagie.ColegioPadronResponse>();
                    iieePadron.AddRange(JsonConvert
                        .DeserializeObject<List<Models.Siagie.ColegioPadronResponse>>(statusResponse.Data.ToString())
                        .ToList());

                    var validacionIIEE = iieePadron.Where(w => w.codMod == usuarioRequest.codModular).ToList();
                    if (validacionIIEE.Count() == 0)
                    {
                        response.Data = null;
                        response.Success = false;
                        response.Messages.Add("Usted no tiene permiso para emitir Certificado de Estudios en esta I.E.");
                        return response;
                    }
                    idRolTemporal = usuarioRequest.idRol;
                }

                #endregion PERMISOS

                Models.Certificado.MenuNivelRolModel menuNivel = null;

                //MENU NIVEL X ROL
                //var resultMenuNivel = await _unitOfWork.ObtenerMenuNivelPorRol(usuarioRequest.idRol);
                var resultMenuNivel = await _unitOfWork.ObtenerMenuNivelPorRol(idRolTemporal);

                if (resultMenuNivel != null && resultMenuNivel.ToList().Count > 0)
                {
                    menuNivel = Mappers.Certificado.MenuNivelRolMapper.Map(resultMenuNivel.FirstOrDefault());
                }

                response.Data = new Models.Certificado.DatosUsuario()
                {
                    usuarioLogin = ReactEncryptationSecurity.Encrypt(usuarioRequest.loginUsuario),
                    nombresCompleto = usuarioRequest.nombreCompleto == null ? ReactEncryptationSecurity.Encrypt(usuarioRequest.nombre + " " + usuarioRequest.apePaterno + " " + usuarioRequest.apeMaterno) : ReactEncryptationSecurity.Encrypt(usuarioRequest.nombreCompleto),
                    numeroDocumento = ReactEncryptationSecurity.Encrypt(usuarioRequest.numeroDocumento == null ? usuarioRequest.dni : usuarioRequest.numeroDocumento),
                    idRol = ReactEncryptationSecurity.Encrypt(usuarioRequest.idRol),
                    dscRol = menuNivel.dscRol.Trim(),
                    tipoSede = ReactEncryptationSecurity.Encrypt(usuarioRequest.tipoSede == null ? usuarioRequest.idTipoSede : usuarioRequest.tipoSede.Trim()),
                    codigoModular = ReactEncryptationSecurity.Encrypt(usuarioRequest.codModular.Trim()),
                    anexo = ReactEncryptationSecurity.Encrypt(usuarioRequest.anexo.Trim()),
                    nombreIE = usuarioRequest.nombreIE.Trim(),
                    idNivel = ReactEncryptationSecurity.Encrypt(usuarioRequest.idNivel),
                    dscNivel = usuarioRequest.dscNivel == null ? usuarioRequest.descNivel : usuarioRequest.dscNivel.Trim(),
                    idModalidad = ReactEncryptationSecurity.Encrypt(usuarioRequest.idModalidad),
                    idMenuNivel = ReactEncryptationSecurity.Encrypt(menuNivel.idMenuNivel.ToString()),
                    dscMenuNivel = menuNivel.dscMenuNivel,
                    token = new JwtSecurityTokenHandler().WriteToken(tokenCertificado)
                };
                response.Success = true;
                response.Messages.Add("Inicio de Sesión exitoso.");
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.Success = false;
                response.Messages.Add("Error general:" + ex);
            }

            return response;

        }
    }
}
