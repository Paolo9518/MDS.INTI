using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Minedu.Comun.Helper;
using Minedu.MiCertificado.Api.Application.Contracts.Security;
using Minedu.MiCertificado.Api.Application.Contracts.Services;
using Minedu.MiCertificado.Api.DataAccess.Contracts.UnitOfWork;
using Models = Minedu.MiCertificado.Api.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Minedu.MiCertificado.Api.Application.Utils;
using Newtonsoft.Json;
using System.Linq;
using Minedu.MiCertificado.Api.Application.Security;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Mappers = Minedu.MiCertificado.Api.Application.Mappers;
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Layout.Element;
using iText.Layout.Properties;
using Minedu.MiCertificado.Api.Application.Constants;
using iText.Layout.Borders;
using iText.Barcodes.Qrcode;
using iText.Barcodes;
using iText.Kernel.Pdf.Xobject;
using iText.Kernel.Events;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Extgstate;
using iText.Kernel.Colors;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Minedu.MiCertificado.Api.Application.Services
{
    public class CertificadoPublicoService : ICertificadoPublicoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReniecService _reniecService;
        private readonly ISiagieService _siagieService;
        private readonly IConfiguration _configuration;
        private readonly IEncryptionServerSecurity _encryptionServerSecurity;
        public readonly IHttpContextAccessor _httpContextAccessor;

        public CertificadoPublicoService(IUnitOfWork unitOfWork,
            IReniecService reniecService,
            ISiagieService siagieService,
            IConfiguration configuration,
            IEncryptionServerSecurity encryptionServerSecurity,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _reniecService = reniecService;
            _siagieService = siagieService;
            _configuration = configuration;
            _encryptionServerSecurity = encryptionServerSecurity;
            _httpContextAccessor = httpContextAccessor;
        }

        //TOKEN
        public async Task<StatusResponse> ObtenerToken()
        {
            var result = new StatusResponse();

            try
            {
                //Obtener token
                var tokenRequest = new Models.Siagie.TokenRequest()
                {
                    ServerName = _configuration.GetSection("SiagieService:ServerName").Value,
                    Ticket = _configuration.GetSection("SiagieService:Ticket").Value,
                    UserId = _configuration.GetSection("SiagieService:UserId").Value,
                    UserName = _configuration.GetSection("SiagieService:UserName").Value
                };

                var tokenSiagie = await _siagieService.PostServiceToken<string>(tokenRequest);

                //Verificar token
                if (tokenSiagie == null || tokenSiagie.Equals(""))
                {
                    throw new ArgumentException("Ocurrió un problema al procesar su solicitud, intentelo nuevamente.");
                }

                var secretKey = _configuration.GetSection("JwT:Key").Value;
                var key = Encoding.ASCII.GetBytes(secretKey);

                var claimsConfig = new[]
                {
                    new Claim("siagie", _encryptionServerSecurity.Encrypt(tokenSiagie.ToString()))
                };

                var issuerConfig = _configuration.GetSection("JwT:Issuer").Value;
                var audienceConfig = _configuration.GetSection("JwT:Audience").Value;
                var expiresConfig = DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration.GetSection("JwT:Time").Value));
                var credsConfig = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);


                JwtSecurityToken token = new JwtSecurityToken
                (
                    issuer: issuerConfig,
                    audience: audienceConfig,
                    claims: claimsConfig,
                    expires: expiresConfig,
                    signingCredentials: credsConfig
                );

                //Token conforme
                result.Success = true;
                result.Data = new Models.TokenResponse()
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                };
                result.Messages.Add("Generación de Token, conforme.");
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Data = null;
                result.Messages.Add(ex.GetType().IsAssignableFrom(typeof(ArgumentException))
                    ? ex.Message
                    : "Se presentó un inconveniente al procesar su solicitud.");
                return result;
            }
        }

        //STEP 1 IIEE PADRÓN
        public async Task<StatusResponse> ObtenerIIEE(Models.Siagie.ColegioRequest request)
        {
            var result = new StatusResponse();

            try
            {
                //var siagie = "";
                var siagie = _encryptionServerSecurity.Decrypt<string>(ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", ""), "");
                /*var request = new
                {
                    departamento = ReactEncryptationSecurity.Decrypt<string>(encryptedRequest.departamento, ""),
                    provincia = ReactEncryptationSecurity.Decrypt<string>(encryptedRequest.provincia, ""),
                    ubigeo = ReactEncryptationSecurity.Decrypt<string>(encryptedRequest.ubigeo, ""),
                    cenEdu = ReactEncryptationSecurity.Decrypt<string>(encryptedRequest.cenEdu, ""),
                    codMod = ReactEncryptationSecurity.Decrypt<string>(encryptedRequest.codMod, ""),
                };*/

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        object>(siagie, "iiee", request);

                //Consulta de iiee en Siagie: sin resultados
                if (statusResponse.Success && statusResponse.Data == null)
                {
                    var message = "No se encontraron resultados para la búsqueda de Instituciones Educativas.";

                    if (request.codMod.Length > 0)
                    {
                        message = "La Institución Educativa no se encuentra activa o no existe.";
                    }

                    result.Success = true;
                    result.Data = null;
                    result.Messages.Add(message);
                    return result;
                }

                //Consulta de iiee en Siagie: OK
                List<Models.Siagie.ColegioPadronResponse> iieePadron = new List<Models.Siagie.ColegioPadronResponse>();
                iieePadron.AddRange(JsonConvert
                    .DeserializeObject<List<Models.Siagie.ColegioPadronResponse>>(statusResponse.Data.ToString())
                    .ToList());

                for (int i = 0; i < iieePadron.Count; i++)
                {
                    var ie = await _unitOfWork.ObtenerCertificadoInstitucion(iieePadron[i].codMod);
                    if (ie.ToList().Count > 0)
                    {
                        string estado = ie.FirstOrDefault().ESTADO;
                        iieePadron[i].estado = estado;
                        iieePadron[i].dscEstado = estado.Equals("0") ? "Inhabilitado" : "Habilitado";
                    }
                    else
                    {
                        iieePadron[i].estado = "0";
                        iieePadron[i].dscEstado = "Inhabilitado";
                    }
                }

                result.Success = true;
                result.Data = iieePadron.Select(x => new
                {
                    //codMod = ReactEncryptationSecurity.Encrypt(x.codMod),
                    //cenEdu = ReactEncryptationSecurity.Encrypt(x.cenEdu),
                    //dscNivel = ReactEncryptationSecurity.Encrypt(x.dscNivel),
                    //departamento = ReactEncryptationSecurity.Encrypt(x.departamento),
                    //provincia = ReactEncryptationSecurity.Encrypt(x.provincia),
                    //distrito = ReactEncryptationSecurity.Encrypt(x.distrito),
                    //dirCen = ReactEncryptationSecurity.Encrypt(x.dirCen),
                    //ugel = ReactEncryptationSecurity.Encrypt(x.ugel),
                    //dre = ReactEncryptationSecurity.Encrypt(x.dre),
                    x.codMod,
                    x.anexo,
                    x.cenEdu,
                    idNivel = _encryptionServerSecurity.Encrypt(x.idNivel),
                    x.dscNivel,
                    idModalidad = _encryptionServerSecurity.Encrypt(x.idModalidad),
                    x.dscModalidad,
                    x.abrModalidad,
                    x.departamento,
                    x.provincia,
                    x.distrito,
                    x.dirCen,
                    x.ugel,
                    x.dre,
                    x.estado,
                    x.dscEstado,
                    x.total
                }).ToList();
                //result.Data = iieePadron;
                result.Messages.Add("Relación de Instituciones Educativas, conforme.");
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

        //STEP 2 DJ
        public StatusResponse ValidarDJ(Models.Constancia.DeclaracionRequest encryptedRequest)
        {
            var result = new StatusResponse();

            //Validación de términos y condiciones
            var check = ReactEncryptationSecurity.Decrypt<string>(encryptedRequest.check, "");

            if (!check.Equals("1"))
            {
                result.Success = false;
                result.Messages.Add("Estimado usuario para poder continuar usted debe aceptar los Términos y Condiciones.");
                result.Data = null;
            }
            else
            {
                result.Success = true;
                result.Messages.Add("Estimado usuario para poder continuar usted debe aceptar los Términos y Condiciones.");
                result.Data = _encryptionServerSecurity.Encrypt(check);
            }

            return result;
        }

        //STEP 4 APODERADO
        public async Task<StatusResponse> ValidarApoderado(Models.Certificado.ApoderadoPersonaModularRequest encryptedRequest)
        {
            var result = new StatusResponse();

            var request = new Models.Certificado.ApoderadoPersonaModularRequest()
            {
                tipoDocApoderado = ReactEncryptationSecurity.Decrypt(encryptedRequest.tipoDocApoderado, ""),
                nroDocApoderado = ReactEncryptationSecurity.Decrypt(encryptedRequest.nroDocApoderado, ""),
                nombrePadreApoderado = ReactEncryptationSecurity.Decrypt(encryptedRequest.nombrePadreApoderado, ""),
                nombreMadreApoderado = ReactEncryptationSecurity.Decrypt(encryptedRequest.nombreMadreApoderado, ""),
                fechaNacimientoApoderado = ReactEncryptationSecurity.Decrypt(encryptedRequest.fechaNacimientoApoderado, ""),
                ubigeoApoderado = ReactEncryptationSecurity.Decrypt(encryptedRequest.ubigeoApoderado, ""),
                codModular = ReactEncryptationSecurity.Decrypt(encryptedRequest.codModular, ""),
                anexo = ReactEncryptationSecurity.Decrypt(encryptedRequest.anexo, "")
            };

            try
            {
                if (!request.tipoDocApoderado.Equals("2")
                    || request.nroDocApoderado.Length != 8
                    || request.fechaNacimientoApoderado.Equals("")
                    || request.ubigeoApoderado.Length != 6
                    || request.codModular.Length != 7
                    || request.anexo.Length != 1)
                {
                    throw new ArgumentException("Se presentó un inconveniente al validar la información registrada. Verifique la información digitada.");
                }

                var siagie = _encryptionServerSecurity.Decrypt<string>(
                    ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", "")
                    , "");

                var reniecResult = await ValidarApoderadoReniec(request);

                //Validación con Reniec: FAIL?
                if (!reniecResult.result)
                {
                    throw new ArgumentException(reniecResult.error);
                }

                result.Success = true;
                result.Data = new
                {
                    //idPersonaApoderado = _encryptionServerSecurity.Encrypt("0"),
                    nombreApoderado = ReactEncryptationSecurity.Encrypt(reniecResult.persona.nombres.ToUpper())

                };
                result.Messages.Add("apoderado, sus datos fueron validados satisfactoriamente.");
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Data = null;
                result.Messages.Add(ex.GetType().IsAssignableFrom(typeof(ArgumentException))
                    ? ex.Message
                    : "Se presentó un inconveniente al validar la información registrada.");
                return result;
            }
        }

        private async Task<Models.ValidationResult> ValidarApoderadoReniec(Models.Certificado.ApoderadoPersonaModularRequest model)
        {
            var result = new Models.ValidationResult();

            var persona = new Models.ReniecPersona()
            {
                numDoc = model.nroDocApoderado,
                nombrePadre = model.nombrePadreApoderado,
                nombreMadre = model.nombreMadreApoderado,
                fecNacimiento = model.fechaNacimientoApoderado,
                ubigeoDomicilio = model.ubigeoApoderado
            };

            var personaResult = await _reniecService.ReniecConsultarPersona(persona.numDoc);

            try
            {
                if (personaResult == null)
                {
                    throw new ArgumentException("El servicio de RENIEC no se encuentra disponible vuelva a internar en unos momentos.");
                }

                if (personaResult.nombrePadre != null
                    && !personaResult.nombrePadre.Trim().ToLower().Equals(persona.nombrePadre.Trim().ToLower()))
                {
                    throw new ArgumentException("Los datos ingresados no coinciden con los datos registrados en RENIEC.");
                }

                if (personaResult.nombreMadre != null
                    && !personaResult.nombreMadre.Trim().ToLower().Equals(persona.nombreMadre.Trim().ToLower()))
                {
                    throw new ArgumentException("Los datos ingresados no coinciden con los datos registrados en RENIEC.");
                }

                if (!personaResult.fecNacimiento.Equals(persona.fecNacimiento)
                    || !personaResult.ubigeoDomicilio.Equals(persona.ubigeoDomicilio))
                {
                    throw new ArgumentException("Los datos ingresados no coinciden con los datos registrados en RENIEC.");
                }

                DateTime nacimiento = DateTime.Parse(personaResult.fecNacimiento); //Fecha de nacimiento
                int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;

                if (edad < 18)
                {
                    throw new ArgumentException("Los datos ingresados pertenecen a un menor de edad.");
                }

                result.result = true;
                result.persona = personaResult;
                return result;
            }
            catch (Exception ex)
            {
                result.result = false;
                result.error = ex.GetType().IsAssignableFrom(typeof(ArgumentException))
                    ? ex.Message
                    : "Se presentó un inconveniente al validar la información registrada.";
                result.persona = null;
                return result;
            }
        }

        //STEP 4
        public async Task<StatusResponse> ValidarEstudiante(Models.Certificado.EstudiantePersonaModularRequest encryptedRequest)
        {
            var result = new StatusResponse();

            try
            {
                var request = new Models.Certificado.EstudiantePersonaModularRequest()
                {
                    tipoSolicitante = ReactEncryptationSecurity.Decrypt(encryptedRequest.tipoSolicitante, ""),
                    //idPersonaApoderado = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.idPersonaApoderado, "0"),
                    tipoDocApoderado = ReactEncryptationSecurity.Decrypt(encryptedRequest.tipoDocApoderado, ""),
                    nroDocApoderado = ReactEncryptationSecurity.Decrypt(encryptedRequest.nroDocApoderado, ""),

                    tipoDocEstudiante = ReactEncryptationSecurity.Decrypt(encryptedRequest.tipoDocEstudiante, ""),
                    nroDocEstudiante = ReactEncryptationSecurity.Decrypt(encryptedRequest.nroDocEstudiante, ""),
                    nombrePadreEstudiante = ReactEncryptationSecurity.Decrypt(encryptedRequest.nombrePadreEstudiante, ""),
                    nombreMadreEstudiante = ReactEncryptationSecurity.Decrypt(encryptedRequest.nombreMadreEstudiante, ""),
                    fechaNacimientoEstudiante = ReactEncryptationSecurity.Decrypt(encryptedRequest.fechaNacimientoEstudiante, ""),
                    ubigeoEstudiante = ReactEncryptationSecurity.Decrypt(encryptedRequest.ubigeoEstudiante, ""),

                    codModular = ReactEncryptationSecurity.Decrypt(encryptedRequest.codModular, ""),
                    anexo = ReactEncryptationSecurity.Decrypt(encryptedRequest.anexo, ""),
                    idModalidad = _encryptionServerSecurity.Decrypt(encryptedRequest.idModalidad, ""),
                    idNivel = _encryptionServerSecurity.Decrypt(encryptedRequest.idNivel, "")
                };

                if (request.tipoSolicitante.Length != 1

                    //|| !request.tipoDocEstudiante.Equals("2")
                    || request.nroDocEstudiante.Length < 8
                    || request.nroDocEstudiante.Length > 14
                    || request.fechaNacimientoEstudiante.Equals("")
                    || request.ubigeoEstudiante.Length != 6

                    || request.codModular.Length != 7
                    || request.anexo.Length != 1
                    || request.idModalidad.Length != 2
                    || request.idNivel.Length != 2)
                {
                    throw new ArgumentException("Se presentó un inconveniente al validar la información registrada. Verifique la información digitada.");
                }

                var siagie = _encryptionServerSecurity.Decrypt<string>(
                    ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", "")
                    , "");

                var reniecResult = await ValidarEstudianteReniec(request);

                //Validación con Reniec: FAIL?
                if (!reniecResult.result)
                {
                    result.Success = false;
                    result.Data = null;
                    result.Messages.Add(reniecResult.error);
                    return result;
                }

                //Validación con Reniec: OK               
                var paramsMatricula = new Models.Certificado.EstudianteMatriculaActualRequest()
                {
                    tipoDocumento = request.tipoDocEstudiante,
                    nroDocumento = request.nroDocEstudiante,
                    idNivel = request.idNivel,
                    codMod = request.codModular,
                    anexo = request.anexo
                };

                var statusResponseMatriculaActual = await _siagieService
                    .GetServiceByQueryAndToken<StatusResponse, Models.Certificado.EstudianteMatriculaActualRequest>(siagie, "estudiante/matricula/actual", paramsMatricula);

                if (statusResponseMatriculaActual.Data != null)
                {
                    var responseMatriculaActualList = JsonConvert
                    .DeserializeObject<List<Models.Siagie.EstudianteMatriculaActualResponse>>(statusResponseMatriculaActual.Data.ToString());
                    if (responseMatriculaActualList.FirstOrDefault().estado == 0)
                        throw new ArgumentException("El estudiante cuenta con una matrícula en SIAGIE pero aún no cuenta " +
                            "con información de evaluación de un periodo lectivo completo registrada. Deberá esperar a " +
                            "contar con evaluaciones finales registradas para solicitar el Certificado de Estudios.");
                }

                var personaRequest = new Models.Certificado.PersonaSistemaRequest()
                {
                    tipoDocumento = request.tipoDocEstudiante,
                    nroDocumento = request.nroDocEstudiante,
                    idSistema = "1"
                };
                var statusResponse1 = await _siagieService
                    .GetServiceByQueryAndToken<StatusResponse, Models.Certificado.PersonaSistemaRequest>(siagie, "estudiante/matricula/concluida", personaRequest);

                if (statusResponse1.Data == null)
                {
                    throw new ArgumentException(statusResponse1.Messages[0]);
                }

                //Validación de estudiante en Siagie: OK
                var responseEstudiantesList = JsonConvert
                .DeserializeObject<List<Models.Siagie.EstudiantePersonaMatriculaResponse>>(statusResponse1.Data.ToString());

                var responseEstudiante = responseEstudiantesList.Where(x => x.idPersonaEstudiante == 0 || x.idMatricula == 0).FirstOrDefault();

                if (responseEstudiante == null)
                {
                    responseEstudiante = responseEstudiantesList.Where(x => x.codMod == request.codModular && x.anexo == request.anexo).FirstOrDefault();

                    if (responseEstudiante == null)
                    {
                        throw new ArgumentException("La I.E 0000000 seleccionada no coincide con la ultima matricula concluida, deberá de seleccionar la I.E correspondiente.");
                    }
                }

                string idPersonaApoderado = _encryptionServerSecurity.Encrypt("0");
                string idPersonaEstudiante = _encryptionServerSecurity.Encrypt(responseEstudiante.idPersonaEstudiante.ToString());
                string idMatricula = ReactEncryptationSecurity.Encrypt(responseEstudiante.idMatricula.ToString());
                string nombreEstudiante = ReactEncryptationSecurity.Encrypt(reniecResult.persona.nombres.ToUpper());

                if (responseEstudiante.idPersonaEstudiante == 0)
                {
                    result.Success = false;
                    result.Data = new
                    {
                        idPersonaApoderado,
                        idPersonaEstudiante = ReactEncryptationSecurity.Encrypt(responseEstudiante.idPersonaEstudiante.ToString()),
                        idMatricula,
                        nombreEstudiante,
                    };
                    result.Messages.Add("El Solicitante no cuenta con registro en el Sistema de SIAGIE " +
                    "(Sistema de Información de Apoyo a la Gestión de la Institución Educativa), " +
                    "por lo que se procederá a la Regularización de sus Notas con la Institución Educativa " +
                    "o la UGEL correspondiente, cuando ingrese la información requerida en el siguiente formulario.");
                    return result;
                }

                if (responseEstudiante.idMatricula == 0)
                {
                    result.Success = false;
                    result.Data = new
                    {
                        idPersonaApoderado,
                        idPersonaEstudiante = ReactEncryptationSecurity.Encrypt(responseEstudiante.idPersonaEstudiante.ToString()),
                        idMatricula,
                        nombreEstudiante,
                    };
                    result.Messages.Add("Verificar que haya seleccionado correctamente la IE.");
                    return result;
                }

                //Si es con apoderado
                if (request.tipoSolicitante.Equals("1") && !reniecResult.esPadreMadre)
                {
                    personaRequest = new Models.Certificado.PersonaSistemaRequest()
                    {
                        tipoDocumento = request.tipoDocEstudiante,
                        nroDocumento = request.nroDocEstudiante,
                        idSistema = "1"
                    };

                    var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse, Models.Certificado.PersonaSistemaRequest>(siagie, "apoderado", personaRequest);

                    //Validación de apoderado en Siagie: FAIL?
                    if (!statusResponse.Success)
                    {
                        throw new ArgumentException((statusResponse.Messages.Count > 0)
                            ? statusResponse.Messages[0]
                            : "El solicitante no se encuentra registrado como padre o madre en el RENIEC ni como apoderado en el SIAGIE. Si considera que el registro de apoderado no se encuentra actualizado, podrá acercarse a la institución educativa para actualizarlo en el SIAGIE.");
                    }

                    //Validación de apoderado en Siagie: OK
                    var responseApoderado = JsonConvert
                        .DeserializeObject<List<Models.Siagie.ApoderadoPersonaResponse>>(statusResponse.Data.ToString())
                        .First();

                    var apoderadoEstudianteRequest = new Models.Certificado.ApoderadoEstudianteModularRequest()
                    {
                        idPersonaApoderado = responseApoderado.idPersonaApoderado,
                        idPersonaEstudiante = responseEstudiante.idPersonaEstudiante
                    };

                    var statusResponse2 = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        Models.Certificado.ApoderadoEstudianteModularRequest>(siagie, "apoderado/estudiante", apoderadoEstudianteRequest);

                    //Validación de relación apoderado-estudiante en Siagie: FAIL?
                    if (!statusResponse2.Success)
                    {
                        throw new ArgumentException((statusResponse2.Messages.Count > 0)
                            ? statusResponse2.Messages[0]
                            : "El solicitante no se encuentra registrado como padre o madre en el RENIEC ni como apoderado en el SIAGIE.Si considera que el registro de apoderado no se encuentra actualizado, podrá acercarse a la institución educativa para actualizarlo en el SIAGIE");
                    }

                    idPersonaApoderado = _encryptionServerSecurity.Encrypt(responseApoderado.idPersonaApoderado.ToString());
                }

                result.Success = true;
                result.Data = new
                {
                    idPersonaApoderado,
                    idPersonaEstudiante,
                    idMatricula,
                    nombreEstudiante
                };
                result.Messages.Add(request.tipoSolicitante.Equals("1")
                    ? "Los datos de estudiante fueron validados satisfactoriamente."
                    : "estudiante, sus datos fueron validados satisfactoriamente.");
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Data = null;
                result.Messages.Add(ex.GetType().IsAssignableFrom(typeof(ArgumentException))
                    ? ex.Message
                    : "Se presentó un inconveniente al validar la información registrada.");
                return result;
            }
        }

        private async Task<Models.ValidationResult> ValidarEstudianteReniec(Models.Certificado.EstudiantePersonaModularRequest model)
        {
            var result = new Models.ValidationResult();

            var persona = new Models.ReniecPersona()
            {
                numDoc = model.nroDocEstudiante,
                nombrePadre = model.nombrePadreEstudiante,
                nombreMadre = model.nombreMadreEstudiante,
                fecNacimiento = model.fechaNacimientoEstudiante,
                ubigeoDomicilio = model.ubigeoEstudiante
            };

            try
            {
                var personaResult = await _reniecService.ReniecConsultarPersona(persona.numDoc);
                if (personaResult == null)
                {
                    throw new ArgumentException("El servicio de RENIEC no se encuentra disponible vuelva a internar en unos momentos.");
                }

                if (personaResult.nombrePadre != null
                    && !personaResult.nombrePadre.Trim().ToLower().Equals(persona.nombrePadre.Trim().ToLower()))
                {
                    throw new ArgumentException("Los datos ingresados no coinciden con los datos registrados en RENIEC.");
                }

                if (personaResult.nombreMadre != null
                    && !personaResult.nombreMadre.Trim().ToLower().Equals(persona.nombreMadre.Trim().ToLower()))
                {
                    throw new ArgumentException("Los datos ingresados no coinciden con los datos registrados en RENIEC.");
                }


                if (!personaResult.fecNacimiento.Equals(persona.fecNacimiento)
                    || !personaResult.ubigeoDomicilio.Equals(persona.ubigeoDomicilio))
                {
                    throw new ArgumentException("Los datos ingresados no coinciden con los datos registrados en RENIEC.");
                }

                if (!personaResult.fecNacimiento.Equals(persona.fecNacimiento)
                    || !personaResult.ubigeoDomicilio.Equals(persona.ubigeoDomicilio))
                {
                    throw new ArgumentException("Los datos ingresados no coinciden con los datos registrados en RENIEC.");
                }

                DateTime nacimiento = DateTime.Parse(personaResult.fecNacimiento); //Fecha de nacimiento
                int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;

                //Tienes apoderado y eres mayor de edad
                //if (model.idPersonaApoderado > 0 && edad >= 18)
                if (model.tipoSolicitante.Equals("1"))
                {
                    if (edad >= 18)
                    {
                        throw new ArgumentException("Los datos del estudiante corresponden a una persona mayor de edad. Ella o él deberá solicitar el certificado mediante la opción “A título personal”.");
                    }

                    if (personaResult.nroDocMadre.Equals(model.nroDocApoderado)
                        || personaResult.nroDocPadre.Equals(model.nroDocApoderado))
                    {
                        result.esPadreMadre = true;
                    }
                }
                else if (model.tipoSolicitante.Equals("2"))
                {
                    if (edad < 18)
                    {
                        throw new ArgumentException("Los datos ingresados NO corresponden a una persona mayor de edad," +
                                                " deberá solicitar el certificado mediante la opción “Apoderado”.");
                    }
                    else if (edad >= 18 && personaResult.fecFallecimientoSpecified)
                    {
                        throw new ArgumentException("Es un estudiante fallecido.");
                    }
                }

                //No tienes apoderado y eres menor de edad
                //if (model.idPersonaApoderado <= 0 && edad < 18)
                /*if (model.idPersonaApoderado.Equals("0") && edad < 18)
                {
                    throw new ArgumentException("Los datos ingresados NO corresponden a una persona mayor de edad," +
                        " deberá solicitar el certificado mediante la opción: Apoderado.");
                }*/

                result.result = true;
                result.persona = personaResult;
                return result;
            }
            catch (Exception ex)
            {
                result.result = false;
                result.error = "Se presentó un inconveniente al validar la información solicitada. EL DNI no se encuentra registrado en RENIEC:";
                result.persona = null;
                return result;
            }
        }

        public async Task<StatusResponse> ObtenerSolicitudesPendientesEstudiante(Models.Certificado.PersonaModalidadNivelRequest encryptedRequest)
        {
            var result = new StatusResponse();

            var request = new Models.Certificado.PersonaModalidadNivelRequest()
            {
                tipoDocumento = ReactEncryptationSecurity.Decrypt<string>(encryptedRequest.tipoDocumento, ""),
                nroDocumento = ReactEncryptationSecurity.Decrypt<string>(encryptedRequest.nroDocumento, ""),
                idModalidad = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.idModalidad, ""),
                idNivel = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.idNivel, "")
            };

            try
            {
                if (request.tipoDocumento.Length != 1)
                {
                    throw new ArgumentException("Tipo de Documento inválido.");
                }

                if (request.nroDocumento.Length != 8)
                {
                    throw new ArgumentException("Número de Documento inválido.");
                }

                if (request.idModalidad.Length != 2)
                {
                    throw new ArgumentException("Modalidad de Estudios seleccionada, inválida.");
                }

                if (request.idNivel.Length != 2)
                {
                    throw new ArgumentException("Nivel Educativo seleccionado, inválido.");
                }

                var resultSolicitud = await _unitOfWork.ObtenerSolicitudesCertificadoPorEstudiante(request.tipoDocumento, request.nroDocumento, request.idModalidad, request.idNivel, "1");
                if (resultSolicitud == null)
                {
                    throw new ArgumentException("Ocurrió un problema al verificar si tiene otras solicitudes.");
                }

                if (resultSolicitud.ToList().Count > 0)
                {
                    Models.Certificado.SolicitudCertificadoModel solicitud = Mappers.Certificado.SolicitudCertificadoMapper.Map(resultSolicitud.FirstOrDefault());

                    throw new ArgumentException("Se verifica que el estudiante cuenta con una solicitud de emisión pendiente de aprobación, " +
                        "deberá de comunicarse con el directivo de la Institución Educativa.");
                }

                result.Success = true;
                result.Data = new
                {
                    countSolicitudes = ReactEncryptationSecurity.Encrypt("0")
                };
                result.Messages.Add("Sin solicitudes pendientes");
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

        //STEP 5 > DATOS DE LA ÚLTIMA IE
        public async Task<StatusResponse> ObtenerUltimoColegioEstudiante(Models.Certificado.EstudianteModalidadNivelModularRequest encryptedRequest)
        {
            var result = new StatusResponse();

            try
            {
                var siagie = _encryptionServerSecurity.Decrypt<string>(ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", ""), "");
                var decrypted = new
                {
                    idPersona = _encryptionServerSecurity.Decrypt<int>(encryptedRequest.idPersonaEstudiante, 0),
                    idModalidad = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.idModalidad, ""),
                    idNivel = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.idNivel, ""),
                    codMod = ReactEncryptationSecurity.Decrypt<string>(encryptedRequest.codMod, ""),
                    anexo = ReactEncryptationSecurity.Decrypt<string>(encryptedRequest.anexo, "")
                };

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        object>(siagie, "colegio/datos/modular", decrypted);

                //Consulta de niveles en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    throw new ArgumentException("No se logró obtener los datos de las IIEE del Estudiante.");
                }

                //Consulta de niveles en Siagie: OK
                var estudiantesColegioNivel = JsonConvert
                    .DeserializeObject<List<Models.Siagie.EstudianteColegioNivelModalidadResponse>>(statusResponse.Data.ToString())
                    .ToList();

                var anioActual = DateTime.Today.Year.ToString();

                var estudianteColegio = estudiantesColegioNivel
                    .Where(x => /*!x.idAnio.ToString().Equals(anioActual) &&*/ x.estadoActa != 1)
                    .OrderByDescending(y => y.idAnio)
                    .FirstOrDefault();

                if (estudianteColegio != null)
                {
                    result.Success = false;
                    result.Data = estudianteColegio;
                    result.Messages.Add(string.Format("El acta de evaluación para la Institución Educativa (I.E.) {0} " +
                        "en el año lectivo {1} y grado {2} se encuentra en revisión o rectificación, " +
                        "por favor comunicarse con el directivo de la I.E.",
                        estudianteColegio.nombreIE,
                        estudianteColegio.idAnio.ToString(),
                        estudianteColegio.descripcionGrado));
                    return result;
                }

                result.Success = true;
                result.Data = estudiantesColegioNivel.Select(x => new
                {
                    idMatricula = _encryptionServerSecurity.Encrypt(x.idMatricula.ToString()),
                    idAnio = ReactEncryptationSecurity.Encrypt(x.idAnio.ToString()),
                    codigoModular = ReactEncryptationSecurity.Encrypt(x.codigoModular),
                    anexo = ReactEncryptationSecurity.Encrypt(x.anexo),
                    nombreIE = ReactEncryptationSecurity.Encrypt(x.nombreIE),
                    modalidad = ReactEncryptationSecurity.Encrypt(x.dscModalidad),
                    nivel = ReactEncryptationSecurity.Encrypt(x.dscNivel),
                    idGrado = ReactEncryptationSecurity.Encrypt(x.idGrado),
                    descripcionGrado = ReactEncryptationSecurity.Encrypt(x.descripcionGrado),
                    estadoActa = ReactEncryptationSecurity.Encrypt(x.estadoActa.ToString())
                }).FirstOrDefault();
                result.Messages.Add("Datos de IIEE del Estudiante, conforme.");
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

        //STEP 5 > VISTA PREVIA
        public async Task<byte[]> VistaPreviaPDFCertificado(Models.Constancia.EstudianteModalidadNivelPersonaRequest encryptedRequest)
        {
            //StatusResponse result = new StatusResponse();

            try
            {
                var request = new Models.Constancia.EstudianteModalidadNivelPersonaRequest()
                {
                    idPersonaEstudiante = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.idPersonaEstudiante, "0"),
                    idModalidad = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.idModalidad, ""),
                    idNivel = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.idNivel, ""),
                    idTipoDocumento = ReactEncryptationSecurity.Decrypt(encryptedRequest.idTipoDocumento, ""),
                    numeroDocumento = ReactEncryptationSecurity.Decrypt(encryptedRequest.numeroDocumento, "")
                };

                if (request.idPersonaEstudiante.Equals("0"))
                {
                    throw new ArgumentException("Datos de Estudiante inválidos");
                }

                if (request.idModalidad.Equals(""))
                {
                    throw new ArgumentException("Modalidad de Estudios inválida");
                }

                if (request.idNivel.Equals(""))
                {
                    throw new ArgumentException("Nivel Educativo inválido");
                }

                if (!request.idTipoDocumento.Equals("2"))
                {
                    throw new ArgumentException("Tipo de Documento inválido");
                }

                if (request.numeroDocumento.Length != 8)
                {
                    throw new ArgumentException("Documento de Identidad inválido");
                }

                var siagie = _encryptionServerSecurity.Decrypt<string>(
                    ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", "")
                    , "");

                var estudianteCertificado = await SiagieEstudiante(siagie, request);
                if (estudianteCertificado == null)
                {
                    return null;
                }



                return PDFCertificadoInit(true, estudianteCertificado);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<Models.Certificado.EstudianteCertificado> SiagieEstudiante(string siagie, Models.Constancia.EstudianteModalidadNivelPersonaRequest request)
        {
            Models.Certificado.EstudianteCertificado estudianteCertificado = new Models.Certificado.EstudianteCertificado();

            //OBTENER DATA DE SIAGIE
            var infoEstudiante = await SiagieInfoEstudiante(siagie, request);
            if (infoEstudiante == null)
            {
                return null;
            }

            estudianteCertificado.solicitud = new Models.Certificado.SolicitudCertificadoModel()
            {
                idModalidad = infoEstudiante.idModalidad,
                abrModalidad = infoEstudiante.abrModalidad,
                dscModalidad = infoEstudiante.dscModalidad,
                idNivel = infoEstudiante.idNivel,
                dscNivel = infoEstudiante.dscNivel,
                idGrado = infoEstudiante.idGrado,
                dscGrado = infoEstudiante.dscGrado,
                estadoSolicitud = "1"
            };

            estudianteCertificado.estudiante = new Models.Certificado.EstudianteCertificadoModel()
            {
                idPersona = infoEstudiante.idPersona,
                idTipoDocumento = infoEstudiante.idTipoDocumento,
                numeroDocumento = infoEstudiante.numeroDocumento,
                apellidoPaterno = infoEstudiante.apellidoPaterno,
                apellidoMaterno = infoEstudiante.apellidoMaterno,
                nombres = infoEstudiante.nombres
            };

            var requestModalidadNivel = new Models.Constancia.EstudianteModalidadNivelRequest()
            {
                //request = request.request,
                idPersonaEstudiante = request.idPersonaEstudiante,
                idModalidad = request.idModalidad,
                idNivel = request.idNivel
            };

            estudianteCertificado.grados = await SiagieGradosEstudiante(siagie, requestModalidadNivel);
            if (estudianteCertificado.grados == null || estudianteCertificado.grados.Count == 0)
            {
                return null;
            }

            //List<NotaModel> notasList = await SiagieNotasEstudiante(idPersona, idModalidad, idNivel);
            estudianteCertificado.notas = await SiagieNotasEstudiante(siagie, requestModalidadNivel);
            if (estudianteCertificado.notas == null || estudianteCertificado.notas.Count == 0)
            {
                return null;
            }

            estudianteCertificado.observaciones = await SiagieObservacionesEstudiante(siagie, requestModalidadNivel);
            /*if (estudianteConstancia.observaciones == null || estudianteConstancia.observaciones.Count == 0)
            {
                return null;
            }*/

            return estudianteCertificado;
        }

        private async Task<Models.Siagie.EstudianteInfoPorNivel> SiagieInfoEstudiante(string siagie, Models.Constancia.EstudianteModalidadNivelPersonaRequest requestModalidadNivelPersona)
        {
            try
            {
                var request = new
                {
                    idPersona = requestModalidadNivelPersona.idPersonaEstudiante,
                    requestModalidadNivelPersona.idModalidad,
                    requestModalidadNivelPersona.idNivel,
                    requestModalidadNivelPersona.idTipoDocumento,
                    requestModalidadNivelPersona.numeroDocumento,
                    idSistema = "1"
                };

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        object>(siagie, "pdf/info", request);

                //Información del estudiante en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    return null;
                }

                //Información del estudiante en Siagie: OK
                var result = JsonConvert
                    .DeserializeObject<List<Models.Siagie.EstudianteInfoPorNivel>>(statusResponse.Data.ToString())
                    .FirstOrDefault();

                return result;
            }
            catch
            {
                return null;
            }
        }

        private async Task<List<Models.Certificado.GradoCertificadoModel>> SiagieGradosEstudiante(string siagie, Models.Constancia.EstudianteModalidadNivelRequest requestModalidadNivel)
        {
            try
            {
                var request = new
                {
                    idPersona = requestModalidadNivel.idPersonaEstudiante,
                    requestModalidadNivel.idModalidad,
                    requestModalidadNivel.idNivel,
                    idSistema = "1"
                };

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        object>(siagie, "pdf/grados", request);

                //Información de grados del estudiante en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    return null;
                }

                //Información de grados del estudiante en Siagie: OK
                var result = JsonConvert
                    .DeserializeObject<List<Models.Certificado.GradoCertificadoModel>>(statusResponse.Data.ToString())
                    .ToList();

                return result;
            }
            catch
            {
                return null;
            }
        }

        private async Task<List<Models.Certificado.NotaCertificadoModel>> SiagieNotasEstudiante(string siagie, Models.Constancia.EstudianteModalidadNivelRequest requestModalidadNivel)
        {
            try
            {
                var request = new
                {
                    idPersona = requestModalidadNivel.idPersonaEstudiante,
                    requestModalidadNivel.idModalidad,
                    requestModalidadNivel.idNivel,
                    idSistema = "1"
                };

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        object>(siagie, "pdf/notas", request);

                //Información de notas del estudiante en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    return null;
                }

                //Información de notas del estudiante en Siagie: OK
                var result = JsonConvert
                    .DeserializeObject<List<Models.Certificado.NotaCertificadoModel>>(statusResponse.Data.ToString())
                    .ToList();

                return result;
            }
            catch
            {
                return null;
            }
        }

        // ORDENAR NOTAS CON LINQ
        private List<Models.PDFNota> OrdenarNotasEstudiante(List<Models.Certificado.NotaCertificadoModel> notasList, List<Models.Certificado.GradoCertificadoModel> gradosList, string idTipoArea)
        {
            List<Models.PDFNota> constanciaNotasList = new List<Models.PDFNota>();
            constanciaNotasList = notasList
                                   .Where(z => z.idTipoArea == idTipoArea)
                                   .GroupBy(c => new { c.dscArea })
                                   .Select(g => new Models.PDFNota
                                   {
                                       DscArea = g.Key.dscArea,
                                       GradoNotas = gradosList
                                                       .GroupBy(a => new { a.idGrado, a.dscGrado })
                                                       .Select(b => new Models.PDFGradoNota
                                                       {
                                                           IdGrado = b.Key.idGrado,
                                                           DscGrado = b.Key.dscGrado,
                                                           NotaFinalArea = notasList
                                                                               .Where(w => w.idGrado == b.Key.idGrado && w.dscArea == g.Key.dscArea)
                                                                               .Select(h => h.notaFinalArea)
                                                                               .FirstOrDefault()
                                                       }).ToList()
                                   })
                                   .OrderBy(y => y.DscArea)
                                   .ToList();

            return constanciaNotasList;
        }

        private async Task<List<Models.Certificado.ObservacionCertificadoModel>> SiagieObservacionesEstudiante(string siagie, Models.Constancia.EstudianteModalidadNivelRequest requestModalidadNivel)
        {
            try
            {
                var request = new
                {
                    idPersona = requestModalidadNivel.idPersonaEstudiante,
                    requestModalidadNivel.idModalidad,
                    requestModalidadNivel.idNivel,
                    idSistema = "1"
                };

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        object>(siagie, "pdf/observaciones", request);

                //Información de grados del estudiante en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    return null;
                }

                //Información de grados del estudiante en Siagie: OK
                var result = JsonConvert
                    .DeserializeObject<List<Models.Certificado.ObservacionCertificadoModel>>(statusResponse.Data.ToString())
                    //.Where(x => x.tipoObs > 2)
                    .ToList();

                return result;
            }
            catch
            {
                return null;
            }
        }

        private List<Models.PDFObs> ConcatenarObservaciones(List<Models.Certificado.ObservacionCertificadoModel> observaciones)
        {
            List<Models.PDFObs> obsList = new List<Models.PDFObs>();

            try
            {
                if (observaciones != null)
                {
                    obsList = observaciones
                    .GroupBy(g => new { g.tipoObs })
                    .Select(y => new Models.PDFObs
                    {
                        codTipo = y.Key.tipoObs,
                        obs = observaciones
                            .Where(w => w.tipoObs == y.Key.tipoObs)
                            .Select(x => String.Format("{0} - {1} - {2}", x.idAnio.ToString(), x.resolucion.Trim().ToUpper(), x.motivo.Trim().ToUpper()))
                                       .Aggregate(new StringBuilder(), (current, next) => current.Append(next).Append(Environment.NewLine))
                                       .ToString()
                    })
                    .OrderBy(j => j.codTipo)
                    .ToList();
                }
            }
            catch (Exception ex)
            {

            }

            return obsList;
        }


        private bool validarEmail(string email)
        {
            var regex = @"^\s*[\w\-\+_']+(\.[\w\-\+_']+)*\@[A-Za-z0-9]([\w\.-]*[A-Za-z0-9])?\.[A-Za-z][A-Za-z\.]*[A-Za-z]$";
            var match = Regex.Match(email, regex, RegexOptions.IgnoreCase);

            return match.Success;
        }

        private bool validarTelefono(string email)
        {
            var regex = @"^(?:\d{9})?$";
            var match = Regex.Match(email, regex, RegexOptions.IgnoreCase);

            return match.Success;
        }


        //STEP 6 > VALIDAR NOTAS ANTES DE GENERAR
        public async Task<StatusResponse> ValidarEstudianteNotas(Models.Constancia.EstudianteModalidadNivelRequest encryptedRequest)
        {
            var result = new StatusResponse();

            try
            {
                var request = new
                {
                    idPersona = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.idPersonaEstudiante, "0"),
                    idModalidad = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.idModalidad, ""),
                    idNivel = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.idNivel, ""),
                    idSistema = "1"
                };

                if (request.idPersona.Equals("0"))
                {
                    throw new ArgumentException("Datos de Estudiante inválidos");
                }

                if (request.idModalidad.Equals(""))
                {
                    throw new ArgumentException("Modalidad de Estudios inválida");
                }

                if (request.idNivel.Equals(""))
                {
                    throw new ArgumentException("Nivel Educativo inválido");
                }

                var siagie = _encryptionServerSecurity.Decrypt<string>(ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", ""), "");

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        object>(siagie, "pdf/grados", request);

                //Consulta de niveles en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    throw new ArgumentException("No se logró obtener los datos de las IIEE del Estudiante.");
                }

                //Consulta de niveles en Siagie: OK
                var estudiantesColegioNivel = JsonConvert
                    .DeserializeObject<List<Models.Certificado.GradoCertificadoModel>>(statusResponse.Data.ToString())
                    .ToList();

                var countEstudianteColegio = estudiantesColegioNivel
                    .Where(x => x.estado == 0)
                    .Count();

                if (countEstudianteColegio > 0)
                {
                    result.Success = false;
                    result.Data = new
                    {
                        iCountNotas = ReactEncryptationSecurity.Encrypt(countEstudianteColegio.ToString())
                    };
                    result.Messages.Add("El estudiante no cuenta con todas las notas registradas en el SIAGIE " +
                        "(Sistema de Información de Apoyo a la Gestión de la Institución Educativa), " +
                        "por lo que se procederá a la regularización de sus notas con la Institución " +
                        "Educativa o la UGEL correspondiente.");
                    return result;
                }

                result.Success = true;
                result.Data = new
                {
                    iCountNotas = ReactEncryptationSecurity.Encrypt("0")
                };
                result.Messages.Add("Notas de IIEE del Estudiante, conforme.");
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Data = null;
                result.Messages.Add(ex.GetType().IsAssignableFrom(typeof(ArgumentException))
                    ? ex.Message
                    : "Se presentó un inconveniente al procesar su solicitud.");
                return result;
            }
        }

        //STEP 6 > GENERAR SOLICITUD + PDF
        public async Task<StatusResponse> GenerarPDFCertificado(Models.Certificado.SolicitudCertificadoRequest encryptedRequest)
        {
            var result = new StatusResponse();

            try
            {
                //Decrypt
                var request = new Models.Certificado.SolicitudCertificadoRequest()
                {
                    ie = new Models.Certificado.InstitucionCertificadoRequest()
                    {
                        codMod = ReactEncryptationSecurity.Decrypt(encryptedRequest.ie.codMod, ""),
                        anexo = ReactEncryptationSecurity.Decrypt(encryptedRequest.ie.anexo, ""),
                        cenEdu = ReactEncryptationSecurity.Decrypt(encryptedRequest.ie.cenEdu, ""),
                        idModalidad = _encryptionServerSecurity.Decrypt(encryptedRequest.ie.idModalidad, ""),
                        dscModalidad = ReactEncryptationSecurity.Decrypt(encryptedRequest.ie.dscModalidad, ""),
                        abrModalidad = ReactEncryptationSecurity.Decrypt(encryptedRequest.ie.abrModalidad, ""),
                        idNivel = _encryptionServerSecurity.Decrypt(encryptedRequest.ie.idNivel, ""),
                        dscNivel = ReactEncryptationSecurity.Decrypt(encryptedRequest.ie.dscNivel, ""),
                        ugel = ReactEncryptationSecurity.Decrypt(encryptedRequest.ie.ugel, ""),
                        dre = ReactEncryptationSecurity.Decrypt(encryptedRequest.ie.dre, "")
                    },
                    tipo = new Models.Certificado.TipoSolicitudCertificadoRequest()
                    {
                        tipoSolicitante = ReactEncryptationSecurity.Decrypt(encryptedRequest.tipo.tipoSolicitante, ""),
                        notasIncompletas = ReactEncryptationSecurity.Decrypt(encryptedRequest.tipo.notasIncompletas, ""),
                    },
                    solicitante = new Models.Constancia.SolicitanteRequest()
                    {
                        idPersonaApoderado = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.solicitante.idPersonaApoderado, "0"),
                        //idPersonaApoderado = encryptedRequest.solicitante.idPersonaApoderado,
                        tipoDocApoderado = ReactEncryptationSecurity.Decrypt(encryptedRequest.solicitante.tipoDocApoderado, ""),
                        nroDocApoderado = ReactEncryptationSecurity.Decrypt(encryptedRequest.solicitante.nroDocApoderado, ""),
                        ubigeoApoderado = ReactEncryptationSecurity.Decrypt(encryptedRequest.solicitante.ubigeoApoderado, ""),
                    },
                    estudiante = new Models.Certificado.EstudianteCertificadoRequest()
                    {
                        idMatricula = ReactEncryptationSecurity.Decrypt(encryptedRequest.estudiante.idMatricula, "0"),
                        idPersonaEstudiante = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.estudiante.idPersonaEstudiante, "0"),
                        //idPersonaEstudiante = encryptedRequest.estudiante.idPersonaEstudiante,
                        tipoDocEstudiante = ReactEncryptationSecurity.Decrypt(encryptedRequest.estudiante.tipoDocEstudiante, ""),
                        nroDocEstudiante = ReactEncryptationSecurity.Decrypt(encryptedRequest.estudiante.nroDocEstudiante, ""),
                        ubigeoEstudiante = ReactEncryptationSecurity.Decrypt(encryptedRequest.estudiante.ubigeoEstudiante, ""),
                    },
                    solicitud = new Models.Certificado.InformacionCertificadoRequest()
                    {
                        idGrado = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.solicitud.idGrado, ""),
                        dscGrado = ReactEncryptationSecurity.Decrypt<string>(encryptedRequest.solicitud.dscGrado, ""),
                        anioCulminacion = ReactEncryptationSecurity.Decrypt<string>(encryptedRequest.solicitud.anioCulminacion, "0"),
                        cicloCulminacion = ReactEncryptationSecurity.Decrypt<string>(encryptedRequest.solicitud.cicloCulminacion, "0"),
                        telefonoContacto = ReactEncryptationSecurity.Decrypt(encryptedRequest.solicitud.telefonoContacto, ""),
                        correoElectronico = ReactEncryptationSecurity.Decrypt(encryptedRequest.solicitud.correoElectronico, ""),
                        idMotivo = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.solicitud.idMotivo, ""),
                        dscMotivo = ReactEncryptationSecurity.Decrypt<string>(encryptedRequest.solicitud.dscMotivo, ""),
                        motivoOtros = ReactEncryptationSecurity.Decrypt(encryptedRequest.solicitud.motivoOtros, "")
                    }
                };

                //Validate
                if (request.ie.codMod.Length != 7)
                {
                    throw new ArgumentException("Código Modular no válido");
                }

                if (request.ie.anexo.Length != 1)
                {
                    throw new ArgumentException("Anexo no válido");
                }

                if (request.ie.idModalidad.Equals("") || request.ie.dscModalidad.Equals(""))
                {
                    throw new ArgumentException("Modalidad de Estudios inválida");
                }

                if (request.ie.idNivel.Equals("") || request.ie.dscNivel.Equals(""))
                {
                    throw new ArgumentException("Nivel Educativo inválido");
                }


                if (request.tipo.tipoSolicitante.Length != 1)
                {
                    throw new ArgumentException("Tipo de solicitante no válido");
                }

                if (request.tipo.notasIncompletas.Length != 1)
                {
                    throw new ArgumentException("Ocurrió un problema al validar las notas del Estudiante");
                }


                /*if (!request.estudiante.idMatricula.Equals("0")
                    && request.solicitante.idPersonaApoderado.Equals("0"))
                {
                    throw new ArgumentException("Datos de apoderado no válidos");
                }*/

                if (request.solicitante.tipoDocApoderado.Length != 1)
                {
                    throw new ArgumentException("Tipo de Documento inválido");
                }

                if (request.solicitante.nroDocApoderado.Length != 8)
                {
                    throw new ArgumentException("Documento de Identidad inválido");
                }

                if (request.solicitante.ubigeoApoderado.Length != 6)
                {
                    throw new ArgumentException("Ubigeo inválido");
                }


                if (request.estudiante.idMatricula.Equals(""))
                {
                    throw new ArgumentException("Información de matrícula de estudiante, inválido");
                }

                if (!request.estudiante.idMatricula.Equals("0")
                    && request.estudiante.idPersonaEstudiante.Equals("0"))
                {
                    throw new ArgumentException("Datos de estudiante no válidos");
                }

                if (request.estudiante.tipoDocEstudiante.Length != 1)
                {
                    throw new ArgumentException("Tipo de Documento inválido");
                }

                if (request.estudiante.nroDocEstudiante.Length != 8)
                {
                    throw new ArgumentException("Documento de Identidad inválido");
                }

                if (request.estudiante.ubigeoEstudiante.Length != 6)
                {
                    throw new ArgumentException("Ubigeo inválido");
                }


                if (request.estudiante.idMatricula.Equals("0")
                    && (request.solicitud.idGrado.Equals("") || request.solicitud.dscGrado.Equals("")))
                {
                    throw new ArgumentException("Grado no válido");
                }

                if (request.estudiante.idMatricula.Equals("0") && request.solicitud.anioCulminacion.Equals("0"))
                {
                    throw new ArgumentException("Año de culminación no válido");
                }

                if (!request.solicitud.telefonoContacto.Equals("")
                    && !validarTelefono(request.solicitud.telefonoContacto))
                {
                    throw new ArgumentException("Número celular inválido");
                }

                if (request.solicitud.correoElectronico.Equals("")
                    || !validarEmail(request.solicitud.correoElectronico))
                {
                    throw new ArgumentException("Correo electrónico no válido");
                }

                if (request.solicitud.idMotivo.Equals(""))
                {
                    throw new ArgumentException("Motivo no válido");
                }

                if (!request.solicitud.motivoOtros.Equals("")
                    && request.solicitud.motivoOtros.Length > 150)
                {
                    throw new ArgumentException("Ha sobrepasado el límite de caracteres permitido");
                }


                var siagie = _encryptionServerSecurity.Decrypt<string>(
                    ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", "")
                    , "");

                int idSolicitud = 0;

                Models.ReniecPersona estudiantePersona;
                Models.ReniecPersona apoderadoPersona;

                if (!request.estudiante.idMatricula.Equals("0"))
                {

                    //Validación de Estudiante
                    var personaRequest = new Models.Certificado.PersonaSistemaRequest()
                    {
                        tipoDocumento = request.estudiante.tipoDocEstudiante,
                        nroDocumento = request.estudiante.nroDocEstudiante,
                        idSistema = "1"
                    };

                    var statusResponseE = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse, Models.Certificado.PersonaSistemaRequest>(siagie, "estudiante", personaRequest);

                    if (statusResponseE.Data == null)
                    {
                        throw new ArgumentException(statusResponseE.Messages[0]);
                    }

                    //Con Apoderado
                    if (request.tipo.tipoSolicitante.Equals("1") && !request.solicitante.idPersonaApoderado.Equals("0"))
                    {
                        personaRequest = new Models.Certificado.PersonaSistemaRequest()
                        {
                            tipoDocumento = request.estudiante.tipoDocEstudiante,
                            nroDocumento = request.estudiante.nroDocEstudiante,
                            idSistema = "1"
                        };

                        var statusResponse = await _siagieService
                            .GetServiceByQueryAndToken<StatusResponse, Models.Certificado.PersonaSistemaRequest>(siagie, "apoderado", personaRequest);

                        //Validación de apoderado en Siagie: FAIL?
                        if (!statusResponse.Success)
                        {

                            throw new ArgumentException((statusResponse.Messages.Count > 0)
                                ? statusResponse.Messages[0]
                                : "El solicitante no se encuentra registrado como padre o madre en el RENIEC ni como apoderado en el SIAGIE. Si considera que el registro de apoderado no se encuentra actualizado, podrá acercarse a la institución educativa para actualizarlo en el SIAGIE.");
                        }

                        //Validación de Relación entre Apoderado y Estudiante
                        var apoderadoEstudianteRequest = new Models.Certificado.ApoderadoEstudianteModularRequest()
                        {
                            //idPersonaApoderado = _encryptionServerSecurity.Decrypt<int>(encryptedRequest.solicitante.idPersonaApoderado, 0),
                            idPersonaApoderado = int.Parse(request.solicitante.idPersonaApoderado),
                            //idPersonaEstudiante = _encryptionServerSecurity.Decrypt<int>(encryptedRequest.estudiante.idPersonaEstudiante, 0),
                            idPersonaEstudiante = int.Parse(request.estudiante.idPersonaEstudiante)
                        };

                        var statusResponseEA = await _siagieService
                            .GetServiceByQueryAndToken<StatusResponse,
                            Models.Certificado.ApoderadoEstudianteModularRequest>(siagie, "apoderado/estudiante", apoderadoEstudianteRequest);

                        //Validación de relación apoderado-estudiante en Siagie: FAIL?
                        if (!statusResponseEA.Success)
                        {
                            string message = (statusResponseEA.Messages.Count > 0)
                                ? statusResponseEA.Messages[0]
                                : "El estudiante no tiene vinculo registrado con el apoderado indicado. " +
                                "Por favor asegúrese de haber ingresado los datos correctos del apoderado o del estudiante, " +
                                "si persiste el inconveniente deberá de comunicarse con la Institución Educativa.";
                            throw new ArgumentException(message);
                        }
                    }
                    /*}
                    catch (Exception ex)
                    {
                        result.Success = false;
                        //result.Data = new Models.Siagie.EstudiantePersonaResponse { idPersonaEstudiante = 0 };
                        result.Data = null;
                        result.Messages.Add("Ocurrió un problema al procesar la información del solicitante y/o estudiante.");
                        return result;
                    }*/

                    var estudianteModalidadNivelPersona = new Models.Constancia.EstudianteModalidadNivelPersonaRequest()
                    {
                        //request = request.request,
                        idPersonaEstudiante = request.estudiante.idPersonaEstudiante,
                        idModalidad = request.ie.idModalidad,
                        idNivel = request.ie.idNivel,
                        idTipoDocumento = request.estudiante.tipoDocEstudiante,
                        numeroDocumento = request.estudiante.nroDocEstudiante
                    };

                    Models.Certificado.EstudianteCertificado estudianteCertificado = await SiagieEstudiante(siagie, estudianteModalidadNivelPersona);
                    //UPDATE ANIO CULMINACIÓN
                    request.solicitud.anioCulminacion = estudianteCertificado.grados.Where(x => x.idAnio != 0).OrderByDescending(y => y.idAnio).FirstOrDefault().idAnio.ToString();

                    estudiantePersona = await _reniecService.ReniecConsultarPersona(request.estudiante.nroDocEstudiante);
                    apoderadoPersona = await _reniecService.ReniecConsultarPersona(request.solicitante.nroDocApoderado);
                    if (estudiantePersona == null || apoderadoPersona == null)
                    {
                        throw new ArgumentException("El servicio de RENIEC no se encuentra disponible vuelva a internar en unos momentos.");
                    }

                    _unitOfWork.BeginTransaction();

                    int resultEstudiante = await _unitOfWork.InsertarEstudianteCertificado(Mappers.Certificado.EstudianteCertificadoMapper.Map(new Models.Certificado.EstudianteCertificadoModel()
                    {
                        idEstudiante = 0,
                        //idPersona = request.estudiante.idPersonaEstudiante,
                        idPersona = int.Parse(request.estudiante.idPersonaEstudiante),
                        idTipoDocumento = request.estudiante.tipoDocEstudiante,
                        numeroDocumento = request.estudiante.nroDocEstudiante,
                        apellidoPaterno = estudiantePersona.apellidoPaterno,
                        apellidoMaterno = estudiantePersona.apellidoMaterno,
                        nombres = estudiantePersona.nombres,
                        ubigeo = estudiantePersona.ubigeoDomicilio,
                        departamento = estudiantePersona.dptoDomicilio,
                        provincia = estudiantePersona.provDomicilio,
                        distrito = estudiantePersona.distDomicilio
                    }));

                    if (resultEstudiante <= 0)
                    {
                        throw new ArgumentException("Ocurrió un problema al registrar la información del estudiante.");
                    }

                    int resultSolicitante = await _unitOfWork.InsertarSolicitanteCertificado(Mappers.Certificado.SolicitanteCertificadoMapper.Map(new Models.Certificado.SolicitanteCertificadoModel()
                    {
                        idSolicitante = 0,
                        //idPersona = request.solicitante.idPersonaApoderado,
                        idPersona = int.Parse(request.solicitante.idPersonaApoderado),
                        idTipoDocumento = request.solicitante.tipoDocApoderado,
                        numeroDocumento = request.solicitante.nroDocApoderado,
                        apellidoPaterno = apoderadoPersona.apellidoPaterno,
                        apellidoMaterno = apoderadoPersona.apellidoMaterno,
                        nombres = apoderadoPersona.nombres,
                        telefonoCelular = request.solicitud.telefonoContacto,
                        correoElectronico = request.solicitud.correoElectronico,
                        ubigeo = apoderadoPersona.ubigeoDomicilio,
                        departamento = apoderadoPersona.dptoDomicilio,
                        provincia = apoderadoPersona.provDomicilio,
                        distrito = apoderadoPersona.distDomicilio
                    }));

                    if (resultSolicitante <= 0)
                    {
                        throw new ArgumentException("Ocurrió un problema al registrar la información del solicitante.");
                    }

                    idSolicitud = await _unitOfWork.InsertarSolicitudCertificado(Mappers.Certificado.SolicitudCertificadoMapper.Map(new Models.Certificado.SolicitudCertificadoModel()
                    {
                        idSolicitud = 0,
                        idEstudiante = resultEstudiante,
                        idSolicitante = resultSolicitante,
                        idMotivo = int.Parse(request.solicitud.idMotivo),
                        idModalidad = estudianteCertificado.solicitud.idModalidad,
                        abrModalidad = estudianteCertificado.solicitud.abrModalidad,
                        dscModalidad = estudianteCertificado.solicitud.dscModalidad,
                        idNivel = estudianteCertificado.solicitud.idNivel,
                        dscNivel = estudianteCertificado.solicitud.dscNivel,
                        idGrado = estudianteCertificado.solicitud.idGrado,
                        dscGrado = estudianteCertificado.solicitud.dscGrado,
                        motivoOtros = request.solicitud.motivoOtros,
                        codigoModular = request.ie.codMod,
                        anexo = request.ie.anexo,
                        anioCulminacion = int.Parse(request.solicitud.anioCulminacion),
                        ciclo = int.Parse(request.solicitud.cicloCulminacion),
                        //anioCulminacion = estudianteCertificado.grados.Where(x => x.idAnio != 0).OrderByDescending(y => y.idAnio).FirstOrDefault().idAnio,
                        estadoEstudiante = request.tipo.notasIncompletas.Equals("1") ? "2" : "3",
                        estadoSolicitud = "1"
                    }));

                    if (idSolicitud <= 0)
                    {
                        throw new ArgumentException("Ocurrió un problema al registrar su solicitud (1).");
                    }

                    if (request.tipo.notasIncompletas.Equals("0"))
                    {
                        int countGrados = 0;
                        foreach (Models.Certificado.GradoCertificadoModel grado in estudianteCertificado.grados)
                        {
                            grado.idSolicitud = idSolicitud;
                            var idConstanciaGrado = await _unitOfWork.InsertarGradoCertificado(Mappers.Certificado.GradoCertificadoMapper.Map(grado)); //TODO

                            if (idConstanciaGrado > 0)
                            {
                                countGrados++;
                            }
                        }

                        if (countGrados != estudianteCertificado.grados.Count)
                        {
                            throw new ArgumentException("Ocurrió un problema al registrar su solicitud (2).");
                        }

                        int countNotas = 0;
                        foreach (Models.Certificado.NotaCertificadoModel nota in estudianteCertificado.notas)
                        {
                            nota.idSolicitud = idSolicitud;
                            var idConstanciaNota = await _unitOfWork.InsertarNotaCertificado(Mappers.Certificado.NotaCertificadoMapper.Map(nota)); //TODO

                            if (idConstanciaNota > 0)
                            {
                                countNotas++;
                            }
                        }

                        if (countNotas != estudianteCertificado.notas.Count)
                        {
                            throw new ArgumentException("Ocurrió un problema al registrar su solicitud (3).");
                        }

                        if (estudianteCertificado.observaciones != null)
                        {
                            int countObs = 0;
                            foreach (Models.Certificado.ObservacionCertificadoModel obs in estudianteCertificado.observaciones)
                            {
                                obs.idSolicitud = idSolicitud;
                                var idConstanciaObservacion = await _unitOfWork.InsertarObservacionCertificado(Mappers.Certificado.ObservacionCertificadoMapper.Map(obs)); //TODO

                                if (idConstanciaObservacion > 0)
                                {
                                    countObs++;
                                }
                            }

                            if (countObs != estudianteCertificado.observaciones.Count)
                            {
                                throw new ArgumentException("Ocurrió un problema al registrar su solicitud (4).");
                            }
                        }
                    }
                }
                else
                {
                    estudiantePersona = await _reniecService.ReniecConsultarPersona(request.estudiante.nroDocEstudiante);
                    apoderadoPersona = await _reniecService.ReniecConsultarPersona(request.solicitante.nroDocApoderado);
                    if (estudiantePersona == null || apoderadoPersona == null)
                    {
                        throw new ArgumentException("El servicio de RENIEC no se encuentra disponible vuelva a internar en unos momentos.");
                    }

                    _unitOfWork.BeginTransaction();

                    int resultEstudiante = await _unitOfWork.InsertarEstudianteCertificado(Mappers.Certificado.EstudianteCertificadoMapper.Map(new Models.Certificado.EstudianteCertificadoModel()
                    {
                        idEstudiante = 0,
                        //idPersona = request.estudiante.idPersonaEstudiante,
                        idPersona = int.Parse(request.estudiante.idPersonaEstudiante),
                        idTipoDocumento = request.estudiante.tipoDocEstudiante,
                        numeroDocumento = request.estudiante.nroDocEstudiante,
                        apellidoPaterno = estudiantePersona.apellidoPaterno,
                        apellidoMaterno = estudiantePersona.apellidoMaterno,
                        nombres = estudiantePersona.nombres,
                        ubigeo = estudiantePersona.ubigeoDomicilio,
                        departamento = estudiantePersona.dptoDomicilio,
                        provincia = estudiantePersona.provDomicilio,
                        distrito = estudiantePersona.distDomicilio
                    }));

                    if (resultEstudiante <= 0)
                    {
                        throw new ArgumentException("Ocurrió un problema al registrar la información del estudiante.");
                    }

                    int resultSolicitante = await _unitOfWork.InsertarSolicitanteCertificado(Mappers.Certificado.SolicitanteCertificadoMapper.Map(new Models.Certificado.SolicitanteCertificadoModel()
                    {
                        idSolicitante = 0,
                        //idPersona = request.solicitante.idPersonaApoderado,
                        idPersona = int.Parse(request.solicitante.idPersonaApoderado),
                        idTipoDocumento = request.solicitante.tipoDocApoderado,
                        numeroDocumento = request.solicitante.nroDocApoderado,
                        apellidoPaterno = apoderadoPersona.apellidoPaterno,
                        apellidoMaterno = apoderadoPersona.apellidoMaterno,
                        nombres = apoderadoPersona.nombres,
                        telefonoCelular = request.solicitud.telefonoContacto,
                        correoElectronico = request.solicitud.correoElectronico,
                        ubigeo = apoderadoPersona.ubigeoDomicilio,
                        departamento = apoderadoPersona.dptoDomicilio,
                        provincia = apoderadoPersona.provDomicilio,
                        distrito = apoderadoPersona.distDomicilio
                    }));

                    if (resultSolicitante <= 0)
                    {
                        throw new ArgumentException("Ocurrió un problema al registrar la información del solicitante.");
                    }

                    idSolicitud = await _unitOfWork.InsertarSolicitudCertificado(Mappers.Certificado.SolicitudCertificadoMapper.Map(new Models.Certificado.SolicitudCertificadoModel()
                    {
                        idSolicitud = 0,
                        idEstudiante = resultEstudiante,
                        idSolicitante = resultSolicitante,
                        idMotivo = int.Parse(request.solicitud.idMotivo),
                        idModalidad = request.ie.idModalidad,
                        abrModalidad = request.ie.abrModalidad,
                        dscModalidad = request.ie.dscModalidad,
                        idNivel = request.ie.idNivel,
                        dscNivel = request.ie.dscNivel,
                        idGrado = request.solicitud.idGrado,
                        dscGrado = request.solicitud.dscGrado,
                        motivoOtros = request.solicitud.motivoOtros,
                        codigoModular = request.ie.codMod,
                        anexo = request.ie.anexo,
                        anioCulminacion = int.Parse(request.solicitud.anioCulminacion),
                        ciclo = int.Parse(request.solicitud.cicloCulminacion),
                        estadoEstudiante = "1",
                        estadoSolicitud = "1"
                    }));

                    if (idSolicitud <= 0)
                    {
                        throw new ArgumentException("Ocurrió un problema al registrar su solicitud (1).");
                    }
                }

                _unitOfWork.DesactivarSolicitudes(idSolicitud
                    , int.Parse(request.estudiante.idPersonaEstudiante)
                    , request.ie.idModalidad
                    , request.ie.idNivel);

                _unitOfWork.Commit();

                var resultSolicitud = await _unitOfWork.ObtenerSolicitudCertificado(Mappers.Certificado.SolicitudCertificadoMapper.Map(new Models.Certificado.SolicitudCertificadoModel()
                {
                    idSolicitud = idSolicitud
                }));

                Models.Certificado.SolicitudCertificadoModel solicitud = Mappers.Certificado.SolicitudCertificadoMapper.Map(resultSolicitud.FirstOrDefault());

                var correo = PrepararCorreo(solicitud, apoderadoPersona.nombres, request.solicitud.correoElectronico);
                var correoResult = await EnviarCorreo(correo);

                result.Success = true;
                result.Data = new Models.Certificado.SolicitudCertificadoResponse()
                {
                    nroDocSolicitante = ReactEncryptationSecurity.Encrypt(apoderadoPersona.numDoc),
                    nombresSolicitante = ReactEncryptationSecurity.Encrypt(apoderadoPersona.nombres),
                    apellidosSolicitante = ReactEncryptationSecurity.Encrypt(apoderadoPersona.apellidoPaterno + " " + apoderadoPersona.apellidoMaterno),
                    motivoSolicitud = encryptedRequest.solicitud.dscMotivo,
                    correoSolicitante = encryptedRequest.solicitud.correoElectronico,
                    telefonoSolicitante = encryptedRequest.solicitud.telefonoContacto,

                    anioCulminacion = ReactEncryptationSecurity.Encrypt(request.solicitud.anioCulminacion),
                    dre = encryptedRequest.ie.dre,
                    ugel = encryptedRequest.ie.ugel,
                    cenEdu = encryptedRequest.ie.cenEdu,
                    codMod = encryptedRequest.ie.codMod,
                    anexo = encryptedRequest.ie.anexo,

                    codigoVirtual = ReactEncryptationSecurity.Encrypt(solicitud.codigoVirtual),
                    horaCreacion = ReactEncryptationSecurity.Encrypt(solicitud.fechaCreacion.ToString("HH:mm", CultureInfo.CreateSpecificCulture("es-PE")))
                };
                result.Messages.Add("Solicitud registrada exitosamente.");
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Data = null;
                result.Messages.Add(ex.GetType().IsAssignableFrom(typeof(ArgumentException))
                    ? ex.Message
                    : "Se presentó un inconveniente al procesar su solicitud.");
                return result;
            }
        }

        private Models.CorreoModel PrepararCorreo(Models.Certificado.SolicitudCertificadoModel solicitud, string nombresApoderado, string correoElectronico)
        {
            string mensaje = "<html><body>Estimad@ <strong>" + nombresApoderado.ToUpper() + "</strong><br/><br/>" +
                   "Su solicitud ha sido registrada con <strong>código virtual N°. " + solicitud.codigoVirtual + "</strong> a las " +
                   "<strong>" + solicitud.fechaCreacion.ToString("HH:mm", CultureInfo.CreateSpecificCulture("es-PE")) + " hrs</strong>. De aprobarse la " +
                   "solicitud de Certificado Oficial de Estudios por el Directivo de la I.E., se le remitirá un correo con la confirmación " +
                   "de aprobación. Posterior a ello, a fin de dar por culminado el proceso de emisión del Certificado Oficial de Estudios deberá apersonarse a la I.E. " +
                   "para la entrega de dicho documento.<br/><br/>" +
                   "Ministerio de Educación<br/></body></html>";

            mensaje.Replace("'", "''");

            return new Models.CorreoModel()
            {
                para = correoElectronico,
                asunto = "Solicitud de Certificado Oficial de Estudios - MINEDU",
                mensaje = mensaje
            };
        }

        private async Task<bool> EnviarCorreo(Models.CorreoModel model)
        {
            try
            {
                return await _unitOfWork.EnviarCorreo(Mappers.CorreoMapper.Map(model));
            }
            catch
            {
                return false;
            }
        }

        //STEP 6 + RESULT > DESCARGAR CONSTANCIA
        public async Task<StatusResponse> DescargarPDFCertificado(Models.Constancia.DescargaRequest encryptedRequest)
        {
            var result = new StatusResponse();

            var request = new Models.Constancia.DescargaRequest()
            {
                codigoVirtual = ReactEncryptationSecurity.Decrypt(encryptedRequest.codigoVirtual, ""),
                tipoDocumento = ReactEncryptationSecurity.Decrypt(encryptedRequest.tipoDocumento, ""),
                numeroDocumento = ReactEncryptationSecurity.Decrypt(encryptedRequest.numeroDocumento, "")
            };

            if (request.codigoVirtual.Length != 8)
            {
                throw new ArgumentException("Código Virtual Inválido");
            }

            if (!request.tipoDocumento.Equals("2"))
            {
                throw new ArgumentException("Tipo de Documento inválido");
            }

            if (request.numeroDocumento.Length != 8)
            {
                throw new ArgumentException("Documento de Identidad inválido");
            }

            try
            {
                var pdf = await PDFCertificadoDescarga(request.codigoVirtual, request.tipoDocumento, request.numeroDocumento);

                if (pdf != null)
                {
                    result.Success = true;
                    //result.Data = File(pdf, "application/pdf");
                    result.Data = pdf;
                    result.Messages.Add("Constancia generada correctamente.");
                }
                else
                {
                    result.Success = false;
                    result.Data = null;
                    result.Messages.Add("Ocurrió un problema al procesar su solicitud");
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Data = null;
                result.Messages.Add(ex.GetType().IsAssignableFrom(typeof(ArgumentException))
                    ? ex.Message
                    : "Se presentó un inconveniente al obtener la información solicitada.");
            }

            return result;
        }

        //VALIDAR CERTIFICADO
        public async Task<StatusResponse> ValidarPDFCertificado(Models.Certificado.VerificacionCertificadoRequest encryptedRequest)
        {
            StatusResponse result = new StatusResponse();

            var request = new Models.Certificado.VerificacionCertificadoRequest()
            {
                codigoVirtual = ReactEncryptationSecurity.Decrypt(encryptedRequest.codigoVirtual, ""),
                tipoDocumento = ReactEncryptationSecurity.Decrypt(encryptedRequest.tipoDocumento, ""),
                numeroDocumento = ReactEncryptationSecurity.Decrypt(encryptedRequest.numeroDocumento, ""),
                captcha = encryptedRequest.captcha
            };
            try
            {

                if (request.numeroDocumento.Equals("") || !validarReCaptcha(request.captcha))
                {
                    throw new ArgumentException("El Código Captcha generado es inválido o posiblemente haya caducado. Inténtelo nuevamente.");
                }
                if (request.codigoVirtual.Length != 8)
                {
                    throw new ArgumentException("Código Virtual Inválido");
                }

                if (!request.tipoDocumento.Equals("2"))
                {
                    throw new ArgumentException("Tipo de Documento inválido");
                }

                if (!(request.numeroDocumento.Length >= 8 && request.numeroDocumento.Length <= 14))
                {
                    throw new ArgumentException("Documento de Identidad inválido");
                }

                var pdf = await PDFCertificadoDescarga(request.codigoVirtual, request.tipoDocumento, request.numeroDocumento);

                if (pdf != null)
                {
                    result.Success = true;
                    //result.Data = File(pdf, "application/pdf");
                    result.Data = pdf;
                    result.Messages.Add("Constancia generada correctamente.");
                }
                else
                {
                    result.Success = false;
                    result.Data = null;
                    result.Messages.Add("Ocurrió un problema al procesar su solicitud");
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Data = null;
                result.Messages.Add(ex.GetType().IsAssignableFrom(typeof(ArgumentException))
                    ? ex.Message
                    : "Se presentó un inconveniente al obtener la información solicitada.");
            }

            return result;
        }

        private async Task<byte[]> PDFCertificadoDescarga(string codigoVirtual, string tipoDocumento, string numeroDocumento)
        {
            try
            {
                Models.Certificado.EstudianteCertificado estudianteCertificado = new Models.Certificado.EstudianteCertificado();
                //*Cambio Bug:16288
                IEnumerable<DataAccess.Contracts.Entities.Certificado.SolicitudExtend> resultSolicitud = new List<DataAccess.Contracts.Entities.Certificado.SolicitudExtend>();
                if (numeroDocumento.Length == 8)
                {
                    resultSolicitud = await _unitOfWork.ObtenerSolicitudCertificadoPorCodigoVirtual2(codigoVirtual, tipoDocumento, numeroDocumento);

                    if (resultSolicitud == null || resultSolicitud.ToList().Count == 0)
                    {
                        resultSolicitud = await _unitOfWork.ObtenerSolicitudCertificadoPorCodigoVirtual2(codigoVirtual, "1", numeroDocumento);
                    }
                }
                else
                {
                    resultSolicitud = await _unitOfWork.ObtenerSolicitudCertificadoPorCodigoVirtual2(codigoVirtual, "1", numeroDocumento);
                }
                //*Cambio Bug:16288
                if (resultSolicitud == null || resultSolicitud.ToList().Count == 0)
                {
                    throw new ArgumentException("No se encontró una solicitud de Certificado de Estudios que coincida con los datos ingresados.");
                }
                estudianteCertificado.solicitud = Mappers.Certificado.SolicitudCertificadoMapper.Map(resultSolicitud.FirstOrDefault());

                if (estudianteCertificado.solicitud.estadoSolicitud.Equals("1"))
                {
                    throw new ArgumentException("Estimado usuario, su solicitud de emisión de certificado se encuentra en estado Generado, " +
                        "para mayor información comunicarse con el directivo de su Institución Educativa.");
                }

                if (estudianteCertificado.solicitud.estadoSolicitud.Equals("3"))
                {
                    throw new ArgumentException("Estimado usuario, su solicitud de emisión de certificado se encuentra en estado Rechazado, " +
                        "para mayor información comunicarse con el directivo de su Institución Educativa.");
                }

                var resultEstudiante = await _unitOfWork.ObtenerEstudianteCertificadoValidado(estudianteCertificado.solicitud.idEstudiante);
                if (resultEstudiante == null || resultEstudiante.ToList().Count == 0)
                {
                    throw new ArgumentException("Ocurrió un problema al procesar su solicitud");
                }
                estudianteCertificado.estudiante = Mappers.Certificado.EstudianteCertificadoMapper.Map(resultEstudiante.FirstOrDefault());

                var resultGrados = await _unitOfWork.ObtenerGradosCertificadoValidados(estudianteCertificado.solicitud.idSolicitud);
                if (resultGrados == null || resultGrados.ToList().Count == 0)
                {
                    throw new ArgumentException("Ocurrió un problema al procesar su solicitud");
                }
                estudianteCertificado.grados = resultGrados.Select(Mappers.Certificado.GradoCertificadoMapper.Map).ToList();

                var resultNotas = await _unitOfWork.ObtenerNotasCertificadoValidadas(estudianteCertificado.solicitud.idSolicitud);
                if (resultNotas == null || resultNotas.ToList().Count == 0)
                {
                    throw new ArgumentException("Ocurrió un problema al procesar su solicitud");
                }
                estudianteCertificado.notas = resultNotas.Select(Mappers.Certificado.NotaCertificadoMapper.Map).ToList();

                var resultObservaciones = await _unitOfWork.ObtenerObservacionesCertificadoValidadas(estudianteCertificado.solicitud.idSolicitud);
                if (resultObservaciones != null)
                {
                    estudianteCertificado.observaciones = resultObservaciones.Select(Mappers.Certificado.ObservacionCertificadoMapper.Map).ToList();
                }

                return PDFCertificadoInit(false, estudianteCertificado);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private byte[] PDFCertificadoInit(bool vistaPrevia, Models.Certificado.EstudianteCertificado estudianteCertificado)
        {
            try
            {
                List<Models.PDFNota> cursoList = OrdenarNotasEstudiante(estudianteCertificado.notas, estudianteCertificado.grados, "001");
                List<Models.PDFNota> competenciaList = OrdenarNotasEstudiante(estudianteCertificado.notas, estudianteCertificado.grados, "003");
                List<Models.PDFNota> tallerList = OrdenarNotasEstudiante(estudianteCertificado.notas, estudianteCertificado.grados, "002");
                List<Models.PDFObs> obsList = ConcatenarObservaciones(estudianteCertificado.observaciones);

                return PDFCertificado(vistaPrevia, estudianteCertificado, cursoList, competenciaList, tallerList, obsList).ToArray();
            }
            catch
            {
                return null;
            }
        }

        private MemoryStream PDFCertificado(bool vistaPrevia, Models.Certificado.EstudianteCertificado estudianteConstancia,
            List<Models.PDFNota> cursoList,
            List<Models.PDFNota> competenciaList,
            List<Models.PDFNota> tallerList,
            List<Models.PDFObs> obsList)
        {
            /* Variables */
            List<Models.Certificado.GradoCertificadoModel> gradosList = estudianteConstancia.grados;

            string correlativo = String.Format("{0:D8}", estudianteConstancia.solicitud.idSolicitud);
            string hashQRCode = _configuration.GetSection("PDF:QRCodeUrlCertificado").Value + "/validate/" + estudianteConstancia.solicitud.codigoVirtual;

            int totalGrados = gradosList.Count;
            string gradosConcatenados = "";

            string[] gradosCursados = gradosList.Where(w => w.idAnio > 0).Select(x => x.dscGrado).ToArray();
            gradosConcatenados = ConcatenarGrados(gradosCursados);

            int cursos = cursoList.Count;
            int competencias = competenciaList.Count;
            int talleres = tallerList.Count; //16

            //string ubigeo = "Lima";
            string fechaSolicitud = estudianteConstancia.solicitud.fechaCreacion.ToString("dd 'de' MMMM 'del' yyyy", CultureInfo.CreateSpecificCulture("es-PE"));
            string horaSolicitud = estudianteConstancia.solicitud.fechaCreacion.ToString("HH:mm:ss", CultureInfo.CreateSpecificCulture("es-PE"));
            string director = estudianteConstancia.solicitud.director;

            using (MemoryStream memoryStream = new MemoryStream())
            using (PdfWriter writer = new PdfWriter(memoryStream))
            using (PdfDocument pdf = new PdfDocument(writer))
            using (Document document = new Document(pdf, PageSize.A4, false))
            {
                float sizeFont = 7;
                PdfFont font = PdfFontFactory.CreateFont(StandardFonts.COURIER);
                PdfFont fontRegular = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                PdfFont fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                #region LogoMinedu
                byte[] imgdata = null;

                var PathNotProfile = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Public", "Images", "logo_minedu.png");
                imgdata = File.ReadAllBytes(PathNotProfile);

                // Load image from disk
                ImageData imageData = ImageDataFactory.Create(imgdata);
                // Create layout image object and provide parameters. Page number = 1
                Image image = new Image(imageData)
                    //.SetAutoScale(true);
                    .ScaleToFit(130, 400)
                    .SetHorizontalAlignment(HorizontalAlignment.CENTER);
                #endregion LogoMinedu

                int sizeMarginBottom = (vistaPrevia ? 36 : 110);
                document.SetMargins(18, 18, sizeMarginBottom, 18);

                #region Header
                Table table = new Table(UnitValue.CreatePercentArray(new float[] { 25, 50, 25 }));
                table.SetWidth(UnitValue.CreatePercentValue(100));
                table.SetHorizontalAlignment(HorizontalAlignment.CENTER);

                table.AddCell(new Cell(4, 1).Add(image).SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                table.AddCell(PDFMinedu.getCell("MINISTERIO DE EDUCACIÓN", 9, false, 1, 1, TextAlignment.CENTER, null, false));
                table.AddCell(PDFMinedu.getCell(" ", 9, false, 1, 1, TextAlignment.CENTER, null, false));
                table.AddCell(PDFMinedu.getCell("CERTIFICADO OFICIAL DE ESTUDIOS", 10, true, 1, 1, TextAlignment.CENTER, null, false));
                table.AddCell(PDFMinedu.getCell(vistaPrevia ? " " : "CÓDIGO VIRTUAL", 9, false, 1, 1, TextAlignment.CENTER, null, false));
                table.AddCell(PDFMinedu.getCell(estudianteConstancia.solicitud.dscModalidad, 9, false, 1, 1, TextAlignment.CENTER, null, false));
                table.AddCell(PDFMinedu.getCell(vistaPrevia ? " " : estudianteConstancia.solicitud.codigoVirtual, 11, true, 1, 1, TextAlignment.CENTER, null, false));
                table.AddCell(PDFMinedu.getCell("NIVEL " + estudianteConstancia.solicitud.dscNivel, 9, false, 1, 1, TextAlignment.CENTER, null, false));
                table.AddCell(PDFMinedu.getCell(" ", 9, false, 1, 1, TextAlignment.CENTER, null, false));
                document.Add(table);
                #endregion Header

                PDFMinedu.addEspacios(document, sizeFont);

                #region SubHeader
                table = new Table(UnitValue.CreatePercentArray(new float[] { 100 }));
                table.SetWidth(UnitValue.CreatePercentValue(100));
                table.SetHorizontalAlignment(HorizontalAlignment.CENTER);

                //table.AddCell(PDFMinedu.getCell("EL MINISTERIO DE EDUCACIÓN DEL PERÚ", 9, true, 1, 1, TextAlignment.CENTER, null, false));

                Text text1 = new Text("Que " + estudianteConstancia.estudiante.nombres + " " + estudianteConstancia.estudiante.apellidoPaterno + " " +
                estudianteConstancia.estudiante.apellidoMaterno + ", con DNI/Código del estudiante N.° " + estudianteConstancia.estudiante.numeroDocumento + ", ha concluido " +
                    "estudios correspondiente(s) a ").SetFont(fontRegular);
                Text text2 = new Text(gradosConcatenados + " ").SetFont(fontBold); // BOLD
                Text text3 = new Text((estudianteConstancia.solicitud.idNivel.Equals("B0")
                    || estudianteConstancia.solicitud.idNivel.Equals("F0")) ? "grado" : "").SetFont(fontRegular);
                Text text4 = new Text(" de " + estudianteConstancia.solicitud.abrModalidad + ", nivel ").SetFont(fontRegular);
                Text text5 = new Text((estudianteConstancia.solicitud.idNivel.Equals("B0") || estudianteConstancia.solicitud.idNivel.Equals("F0")) ? "de educación" : "").SetFont(fontRegular);
                Text text6 = new Text(" " + estudianteConstancia.solicitud.dscNivel).SetFont(fontBold); // BOLD
                Text text7 = new Text(", con los siguientes resultados, según consta en las actas de evaluación respectivas:").SetFont(fontRegular);

                Paragraph paragraph = new Paragraph().Add(text1).Add(text2).Add(text3).Add(text4).Add(text5).Add(text6).Add(text7);
                Cell cell = new Cell().Add(paragraph)
                    .SetFontSize(8)
                    .SetTextAlignment(TextAlignment.JUSTIFIED)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetBorder(Border.NO_BORDER);

                table.AddCell(cell);

                document.Add(table);
                #endregion SubHeader

                PDFMinedu.addEspacios(document, sizeFont);

                #region Body

                #region Notas_Parametros
                float[] withColumns = new float[totalGrados + 2];

                withColumns[0] = 3;
                //withColumns[1] = 25;
                withColumns[1] = 25 + 18;

                int withGrados = 54 / totalGrados;
                for (int i = 2; i < totalGrados + 2; i++)
                {
                    withColumns[i] = withGrados;
                }
                //withColumns[(totalGrados + 3) - 1] = 18;
                #endregion Notas_Parametros

                table = new Table(UnitValue.CreatePercentArray(withColumns));
                table.SetWidth(UnitValue.CreatePercentValue(100));
                table.SetHorizontalAlignment(HorizontalAlignment.CENTER);

                #region Notas_Header
                table.AddCell(PDFMinedu.getCell("Año lectivo", sizeFont, false, 1, 2, TextAlignment.LEFT));

                for (int i = 1; i <= totalGrados; i++)
                {
                    string anio = gradosList[i - 1].idAnio.ToString();
                    table.AddCell(PDFMinedu.getCell(anio.Equals("0") ? "-" : anio, sizeFont, true));
                }

                table.AddCell(PDFMinedu.getCell((estudianteConstancia.solicitud.idNivel.Equals("B0") || estudianteConstancia.solicitud.idNivel.Equals("F0")) ? "Grado" : "Edad", sizeFont, false, 1, 2, TextAlignment.LEFT));

                for (int i = 1; i <= totalGrados; i++)
                {
                    table.AddCell(PDFMinedu.getCell(switchTextoGrado(gradosList[i - 1].dscGrado), sizeFont));
                }

                table.AddCell(PDFMinedu.getCell("Código modular de la IE", sizeFont, false, 1, 2, TextAlignment.LEFT));

                for (int i = 1; i <= totalGrados; i++)
                {
                    string codModAnexo = (gradosList[i - 1].idAnio == 0) ? "-" : gradosList[i - 1].codMod + "-" + gradosList[i - 1].anexo;
                    table.AddCell(PDFMinedu.getCell(codModAnexo, sizeFont));
                }
                #endregion Notas_Header

                #region Notas_Cursos
                int rows = cursos;

                table.AddCell(PDFMinedu.getCell("Área curricular", sizeFont, false, rows));

                for (int x = 1; x <= rows; x++)
                {
                    table.AddCell(PDFMinedu.getCell(cursoList[x - 1].DscArea, sizeFont, false, 1, 1, TextAlignment.LEFT));

                    for (int i = 1; i <= totalGrados; i++)
                    {
                        //int randomNota = new Random().Next(1, 20);
                        //string nota = (randomNota < 10) ? ("0" + randomNota) : randomNota.ToString();
                        string nota = cursoList[x - 1].GradoNotas[i - 1].NotaFinalArea;

                        table.AddCell(PDFMinedu.getCell(nota == null ? "-" : nota, sizeFont));
                    }

                    //Adición de observaciones
                    //if (x == 1) table.AddCell(PDFMinedu.getCell(obs, sizeFont, false, cursos + competencias + talleres, 1, TextAlignment.JUSTIFIED));
                }
                #endregion Notas_Cursos

                #region Notas_Competencias
                if (competencias > 0)
                {
                    rows = competencias;

                    table.AddCell(PDFMinedu.getCell("Competencias transversales", sizeFont, false, rows));

                    for (int x = 1; x <= rows; x++)
                    {
                        table.AddCell(PDFMinedu.getCell(competenciaList[x - 1].DscArea, sizeFont, false, 1, 1, TextAlignment.LEFT));

                        for (int i = 1; i <= totalGrados; i++)
                        {
                            //int randomNota = new Random().Next(1, 20);
                            //string nota = (randomNota < 10) ? ("0" + randomNota) : randomNota.ToString();
                            string nota = competenciaList[x - 1].GradoNotas[i - 1].NotaFinalArea;
                            table.AddCell(PDFMinedu.getCell(nota == null ? "-" : nota, sizeFont, false, 1, 1));
                        }
                    }
                }
                #endregion Notas_Competencias

                #region Notas_Talleres
                if (talleres > 0)
                {
                    rows = talleres;

                    table.AddCell(PDFMinedu.getCell("Talleres", sizeFont, false, rows));

                    for (int x = 1; x <= rows; x++)
                    {
                        table.AddCell(PDFMinedu.getCell(tallerList[x - 1].DscArea, sizeFont, false, 1, 1, TextAlignment.LEFT));

                        for (int i = 1; i <= totalGrados; i++)
                        {
                            //int randomNota = new Random().Next(1, 20);
                            //string nota = (randomNota < 10) ? ("0" + randomNota) : randomNota.ToString();
                            string nota = tallerList[x - 1].GradoNotas[i - 1].NotaFinalArea;
                            table.AddCell(PDFMinedu.getCell(nota == null ? "-" : nota, sizeFont, false, 1, 1));
                        }
                    }
                }
                #endregion Notas_Talleres

                #region Notas_Final
                table.AddCell(PDFMinedu.getCell("Situación final", sizeFont, true, 1, 2, TextAlignment.LEFT));

                for (int i = 1; i <= totalGrados; i++)
                {
                    string situacionFinal = gradosList[i - 1].situacionFinal != null ? gradosList[i - 1].situacionFinal : "-";
                    table.AddCell(PDFMinedu.getCell(situacionFinal, 6, true));
                }
                #endregion Notas_Final

                document.Add(table);
                #endregion Body

                pdf.AddNewPage(PageSize.A4);
                document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                #region Header2
                table = new Table(UnitValue.CreatePercentArray(new float[] { 25, 50, 25 }));
                table.SetWidth(UnitValue.CreatePercentValue(100));
                table.SetHorizontalAlignment(HorizontalAlignment.CENTER);

                table.AddCell(new Cell(4, 1).Add(image).SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                table.AddCell(PDFMinedu.getCell("MINISTERIO DE EDUCACIÓN", 9, false, 1, 1, TextAlignment.CENTER, null, false));
                table.AddCell(PDFMinedu.getCell(" ", 9, false, 1, 1, TextAlignment.CENTER, null, false));
                table.AddCell(PDFMinedu.getCell("CERTIFICADO OFICIAL DE ESTUDIOS", 10, true, 1, 1, TextAlignment.CENTER, null, false));
                table.AddCell(PDFMinedu.getCell(vistaPrevia ? " " : "CÓDIGO VIRTUAL", 9, false, 1, 1, TextAlignment.CENTER, null, false));
                table.AddCell(PDFMinedu.getCell(estudianteConstancia.solicitud.dscModalidad, 9, false, 1, 1, TextAlignment.CENTER, null, false));
                table.AddCell(PDFMinedu.getCell(vistaPrevia ? " " : estudianteConstancia.solicitud.codigoVirtual, 11, true, 1, 1, TextAlignment.CENTER, null, false));
                table.AddCell(PDFMinedu.getCell("NIVEL " + estudianteConstancia.solicitud.dscNivel, 9, false, 1, 1, TextAlignment.CENTER, null, false));
                table.AddCell(PDFMinedu.getCell(" ", 9, false, 1, 1, TextAlignment.CENTER, null, false));
                document.Add(table);
                #endregion Header2

                PDFMinedu.addEspacios(document, sizeFont);

                #region observaciones
                table = new Table(UnitValue.CreatePercentArray(new float[] { 25, 75 }));
                table.SetWidth(UnitValue.CreatePercentValue(100));
                table.SetHorizontalAlignment(HorizontalAlignment.CENTER);

                table.AddCell(PDFMinedu.getCell("Observaciones: ", 9, true, 1, 2, TextAlignment.LEFT, null, false));
                table.AddCell(PDFMinedu.getCell("", 9, true, 1, 2, TextAlignment.LEFT, null, false).SetHeight(5f));

                table.AddCell(PDFMinedu.getCell("Retiro(s): ", 8, false, 1, 1, TextAlignment.LEFT, null, false, VerticalAlignment.TOP));
                var obs = obsList.Where(x => x.codTipo == 1).Select(y => y.obs).FirstOrDefault();
                table.AddCell(PDFMinedu.getCell(obs == null ? "-" : obs.ToString(), 8, false, 1, 1, TextAlignment.LEFT, null, false));
                table.AddCell(PDFMinedu.getCell("", 9, true, 1, 2, TextAlignment.LEFT, null, false).SetHeight(4f));

                table.AddCell(PDFMinedu.getCell("Traslado(s): ", 8, false, 1, 1, TextAlignment.LEFT, null, false, VerticalAlignment.TOP));
                obs = obsList.Where(x => x.codTipo == 2).Select(y => y.obs).FirstOrDefault();
                table.AddCell(PDFMinedu.getCell(obs == null ? "-" : obs.ToString(), 8, false, 1, 1, TextAlignment.LEFT, null, false));
                table.AddCell(PDFMinedu.getCell("", 9, true, 1, 2, TextAlignment.LEFT, null, false).SetHeight(4f));

                table.AddCell(PDFMinedu.getCell("Prueba(s) de ubicación: ", 8, false, 1, 1, TextAlignment.LEFT, null, false, VerticalAlignment.TOP));
                obs = obsList.Where(x => x.codTipo == 3).Select(y => y.obs).FirstOrDefault();
                table.AddCell(PDFMinedu.getCell(obs == null ? "-" : obs.ToString(), 8, false, 1, 1, TextAlignment.LEFT, null, false));
                table.AddCell(PDFMinedu.getCell("", 9, true, 1, 2, TextAlignment.LEFT, null, false).SetHeight(4f));

                table.AddCell(PDFMinedu.getCell("Convalidación / Revalidación: ", 8, false, 1, 1, TextAlignment.LEFT, null, false, VerticalAlignment.TOP));
                obs = obsList.Where(x => x.codTipo == 4).Select(y => y.obs).FirstOrDefault();
                table.AddCell(PDFMinedu.getCell(obs == null ? "-" : obs.ToString(), 8, false, 1, 1, TextAlignment.LEFT, null, false));
                table.AddCell(PDFMinedu.getCell("", 9, true, 1, 2, TextAlignment.LEFT, null, false).SetHeight(4f));

                Table tableObs = new Table(UnitValue.CreatePercentArray(new float[] { 100 }));
                tableObs.SetWidth(UnitValue.CreatePercentValue(100));
                tableObs.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                tableObs.AddCell(table);

                document.Add(tableObs);
                #endregion observaciones

                #region Footer
                Table tableFooter;
                string urlQRCode = _configuration.GetSection("PDF:QRCodeUrlCertificado").Value + "/validate";
                float fontSize = sizeFont;
                float detailFontSize = fontSize - 1f;

                if (!vistaPrevia)
                {
                    withColumns = new float[] { 15, 50, 35 };
                    tableFooter = new Table(UnitValue.CreatePercentArray(withColumns));
                    tableFooter.SetWidth(UnitValue.CreatePercentValue(100));
                    tableFooter.SetHorizontalAlignment(HorizontalAlignment.CENTER);

                    #region QRImage
                    IDictionary<EncodeHintType, Object> hints = new Dictionary<EncodeHintType, object>();
                    hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.L);

                    BarcodeQRCode barcodeQRCode = new BarcodeQRCode(hashQRCode, hints);
                    PdfFormXObject xObject = barcodeQRCode.CreateFormXObject(PDFMinedu.getColor("BLACK"), pdf);
                    Image qrImage = new Image(xObject);
                    qrImage.ScaleToFit(60, 60);
                    qrImage.SetHorizontalAlignment(HorizontalAlignment.CENTER);

                    Cell qrCell = new Cell(4, 1);
                    qrCell.Add(qrImage);
                    qrCell.SetVerticalAlignment(VerticalAlignment.MIDDLE).SetPadding(0).SetBorder(Border.NO_BORDER);
                    tableFooter.AddCell(qrCell);
                    #endregion QRImage

                    tableFooter.AddCell(PDFMinedu.getCell("Fecha de emisión:", fontSize, false, 1, 1, TextAlignment.RIGHT, null, false).SetPadding(0));
                    tableFooter.AddCell(PDFMinedu.getCell(fechaSolicitud, fontSize, false, 1, 1, TextAlignment.LEFT, null, false).SetPadding(0).SetPaddingLeft(5));

                    tableFooter.AddCell(PDFMinedu.getCell("Hora de emisión:", fontSize, false, 1, 1, TextAlignment.RIGHT, null, false).SetPadding(0));
                    tableFooter.AddCell(PDFMinedu.getCell(horaSolicitud, fontSize, false, 1, 1, TextAlignment.LEFT, null, false).SetPadding(0).SetPaddingLeft(5));

                    tableFooter.AddCell(PDFMinedu.getCell("Firma del director:", fontSize, false, 1, 1, TextAlignment.RIGHT, null, false, VerticalAlignment.TOP).SetPadding(0));

                    Table tblFirma = new Table(UnitValue.CreatePercentArray(new float[] { 100 }));
                    tblFirma.SetWidth(UnitValue.CreatePercentValue(100));
                    tblFirma.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                    tblFirma.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                    tblFirma.AddCell(PDFMinedu.getCell(" ", fontSize, false, 1, 1, TextAlignment.CENTER, null, false).SetPadding(-1).SetHeight(22f));
                    tblFirma.AddCell(
                        PDFMinedu.getCell(director, fontSize, false, 1, 1, TextAlignment.CENTER, null, false)
                            .SetHeight(10f)
                            .SetPadding(0)
                            .SetBorderTop(new DottedBorder(ColorConstants.BLACK, 1))
                    );
                    tableFooter.AddCell(new Cell(1, 1).Add(tblFirma).SetPaddingLeft(5).SetBorder(Border.NO_BORDER));

                    tableFooter.AddCell(PDFMinedu.getCell(
                        string.Format(
                            "* Este certificado puede ser verificado en el sitio web del Ministerio de Educación ({0}), utilizando lectora " +
                            "de códigos QR o teléfono celular enfocando al código QR: el celular debe poseer un software gratuito descargado de internet.\n" +
                            "* EXO: exoneración del área de educación religiosa a solicitud del padre o madre de familia, tutor legal o apoderado.",
                            urlQRCode
                        ),
                        detailFontSize - 1, false, 2, 2, TextAlignment.LEFT, null, false, VerticalAlignment.BOTTOM
                    ).SetPadding(0).SetPaddingTop(4f).SetBorderTop(new SolidBorder(0.25f)));
                    tableFooter.AddCell(PDFMinedu.getCell("N.° " + correlativo, fontSize, true, 1, 1, TextAlignment.CENTER, null, false).SetPadding(0));
                }
                else
                {
                    withColumns = new float[] { 100 };
                    tableFooter = new Table(UnitValue.CreatePercentArray(withColumns));
                    tableFooter.SetWidth(UnitValue.CreatePercentValue(100));
                    tableFooter.SetHorizontalAlignment(HorizontalAlignment.CENTER);

                    tableFooter.AddCell(PDFMinedu.getCell(
                        "* EXO: exoneración del área de educación religiosa a solicitud del padre o madre de familia, tutor legal o apoderado.",
                        detailFontSize - 1, false, 1, 1, TextAlignment.LEFT, null, false
                    ));
                }

                pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new EndPageEventHandler(tableFooter, document));
                #endregion Footer

                #region WatherMark
                if (estudianteConstancia.solicitud.estadoSolicitud.Equals("4"))
                {
                    WaterMarkPDF(pdf);

                }
                #endregion WatherMark

                return memoryStream;
            }
        }

        private string ConcatenarGrados(string[] gradosCursados)
        {
            StringBuilder text = new StringBuilder();

            for (int i = 0; i < gradosCursados.Length; i++)
            {
                switch (gradosCursados[i])
                {
                    case "PRIMERO":
                        text.Append("PRIMER");
                        break;
                    case "TERCERO":
                        text.Append("TERCER");
                        break;
                    default:
                        text.Append(gradosCursados[i]);
                        break;
                }

                if (gradosCursados.Length != 1)
                {
                    if (i == gradosCursados.Length - 2)
                    {
                        text.Append(" y ");
                    }
                    else if (i != gradosCursados.Length - 1)
                    {
                        text.Append(", ");
                    }
                }

            }

            return text.ToString();
        }

        private class EndPageEventHandler : IEventHandler
        {
            private Document doc;
            private Table table;

            public EndPageEventHandler(Table table, Document doc)
            {
                this.table = table;
                this.doc = doc;
            }

            public void HandleEvent(Event @event)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                PdfDocument pdfDoc = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                PdfCanvas canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);

                float height = doc.GetBottomMargin() - doc.GetLeftMargin();
                Rectangle rect = new Rectangle(doc.GetLeftMargin(), doc.GetLeftMargin() - 8, page.GetPageSize().GetWidth() - doc.GetLeftMargin() - doc.GetRightMargin(), height);
                new Canvas(canvas, pdfDoc, rect).Add(table).Close();
            }
        }

        private string switchTextoTipoObs(int tipoObs)
        {
            switch (tipoObs)
            {
                case 1:
                    return "Retiro(s)";
                case 2:
                    return "Traslado(s)";
                case 3:
                    return "Prueba(s) de Ubicación";
                case 4:
                    return "Revalidación / Convalidación";
                default:
                    return tipoObs.ToString();
            }
        }

        private string switchTextoGrado(string grado)
        {
            switch (grado)
            {
                case "PRIMERO":
                    return "1.°";
                case "SEGUNDO":
                    return "2.°";
                case "TERCERO":
                    return "3.°";
                case "CUARTO":
                    return "4.°";
                case "QUINTO":
                    return "5.°";
                case "SEXTO":
                    return "6.°";
                default:
                    return grado;
            }
        }

        void WaterMarkPDF(PdfDocument pdfDoc)
        {
            string waterMarkText = _configuration.GetSection("PDF:WaterMarkTextCertificado").Value;
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            float fontSize = Convert.ToInt32(_configuration.GetSection("PDF:FontSize").Value);

            Rectangle pageSize;
            PdfCanvas canvas;
            int n = pdfDoc.GetNumberOfPages();
            for (int i = 1; i <= n; i++)
            {
                PdfPage page = pdfDoc.GetPage(i);
                pageSize = page.GetPageSize();
                canvas = new PdfCanvas(page);

                //canvas.SetColor(ColorConstants.GRAY, true);
                Paragraph p = new Paragraph(waterMarkText).SetFont(font).SetFontSize(fontSize);
                canvas.SaveState();
                PdfExtGState gs1 = new PdfExtGState();
                gs1.SetFillOpacity(0.6f);
                canvas.SetExtGState(gs1);
                canvas.Fill();
                new Canvas(canvas, pdfDoc, pdfDoc.GetDefaultPageSize()).SetFontColor(WebColors.GetRGBColor("GRAY"))
                    .ShowTextAligned(p, pageSize.GetWidth() / 2, pageSize.GetHeight() / 2, 1, TextAlignment.CENTER, VerticalAlignment.MIDDLE, 45);
                canvas.RestoreState();
                canvas.Release();

            }
        }

        private bool validarReCaptcha(string captcha)
        {
            int validate = int.Parse(_configuration.GetSection("ReCaptcha:CertificadoValidator").Value);

            if (validate == 1)
            {
                try
                {
                    string secretKey = _configuration.GetSection("ReCaptcha:CertificadoKey").Value;
                    HttpClient httpClient = new HttpClient();
                    var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={captcha}").Result;
                    if (res.StatusCode != HttpStatusCode.OK)
                    {
                        return false;
                    }

                    string JSONres = res.Content.ReadAsStringAsync().Result;
                    dynamic JSONdata = JObject.Parse(JSONres);
                    if (JSONdata.success != "true")
                    {
                        return false;
                    }

                    return true;

                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            }
            else
            {
                return true;
            }
        }
    }
}