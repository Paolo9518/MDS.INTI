using iText.Barcodes;
using iText.Barcodes.Qrcode;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Minedu.MiCertificado.Api.Application.Contracts.Services;
using Models = Minedu.MiCertificado.Api.BusinessLogic.Models;
using Minedu.MiCertificado.Api.DataAccess.Contracts.UnitOfWork;
using System.Threading.Tasks;
using Minedu.Comun.Helper;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Minedu.MiCertificado.Api.Application.Mappers.Constancia;
using Minedu.MiCertificado.Api.Application.Mappers;
using System.Globalization;
using System.IO;
using Minedu.MiCertificado.Api.Application.Constants;
using Microsoft.Extensions.Configuration;
using System.Text;
using Minedu.MiCertificado.Api.Application.Contracts.Security;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Minedu.MiCertificado.Api.Application.Utils;
using iText.Kernel.Pdf.Extgstate;
using iText.Kernel.Colors;
using System.Text.RegularExpressions;
using Minedu.MiCertificado.Api.Application.Security;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Minedu.MiCertificado.Api.Application.Services
{
    public class ConstanciaService : IConstanciaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReniecService _reniecService;
        private readonly ISiagieService _siagieService;
        private readonly IConfiguration _configuration;
        private readonly IEncryptionServerSecurity _encryptionServerSecurity;
        public readonly IHttpContextAccessor _httpContextAccessor;

        public ConstanciaService(IUnitOfWork unitOfWork,
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
        
        //STEP 1
        public async Task<StatusResponse> ValidarDJ(Models.Constancia.DeclaracionRequest encryptedRequest)
        {
            var result = new StatusResponse();

            //Validación de términos y condiciones
            var check = ReactEncryptationSecurity.Decrypt<string>(encryptedRequest.check, "");

            if (!check.Equals("1"))
            {
                result.Success = false;
                result.Messages.Add("Para poder continuar, debe aceptar los términos y condiciones del servicio, dándole clic al recuadro correspondiente.");
                result.Data = null;
                return result;
            }

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
                result.Data = new Models.Constancia.DeclaracionResponse()
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                };
                result.Messages.Add("Validación de Declaración Jurada: Conforme.");
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

        //STEP 3
        public async Task<StatusResponse> ValidarApoderado(Models.Constancia.ApoderadoPersonaRequest encryptedRequest)
        {
            var result = new StatusResponse();

            var request = new Models.Constancia.ApoderadoPersonaRequest()
            {
                tipoDocApoderado = ReactEncryptationSecurity.Decrypt(encryptedRequest.tipoDocApoderado, ""),
                nroDocApoderado = ReactEncryptationSecurity.Decrypt(encryptedRequest.nroDocApoderado, ""),
                nombrePadreApoderado = ReactEncryptationSecurity.Decrypt(encryptedRequest.nombrePadreApoderado, ""),
                nombreMadreApoderado = ReactEncryptationSecurity.Decrypt(encryptedRequest.nombreMadreApoderado, ""),
                fechaNacimientoApoderado = ReactEncryptationSecurity.Decrypt(encryptedRequest.fechaNacimientoApoderado, ""),
                ubigeoApoderado = ReactEncryptationSecurity.Decrypt(encryptedRequest.ubigeoApoderado, "")
            };

            try
            {
                if (!request.tipoDocApoderado.Equals("2")
                    || request.nroDocApoderado.Length != 8
                    || request.fechaNacimientoApoderado.Equals("")
                    || request.ubigeoApoderado.Length != 6)
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

                //Validación con Reniec: OK
                /*var personaRequest = new Models.PersonaRequest()
                {
                    tipoDocumento = request.tipoDocApoderado,
                    nroDocumento = request.nroDocApoderado
                };

                var statusResponse = await _siagieService
                    .GetServiceByQueryAndToken<StatusResponse, Models.PersonaRequest>(siagie, "apoderado", personaRequest);

                //Validación de apoderado en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    string message = (statusResponse.Messages.Count > 0)
                        ? statusResponse.Messages[0]
                        : "Usted no se encuentra registrado como apoderado en el SIAGIE, " +
                        "deberá comunicarse con la Institución Educativa.";

                    throw new ArgumentException(message);
                }

                //Validación de apoderado en Siagie: OK
                var responseApoderado = JsonConvert
                    .DeserializeObject<List<Models.Siagie.ApoderadoPersonaResponse>>(statusResponse.Data.ToString())
                    .First();
                
                result.Success = true;
                result.Data = new { idPersonaApoderado = _encryptionServerSecurity
                    .Encrypt(responseApoderado.idPersonaApoderado.ToString()) };
                result.Messages.Add("Sus datos fueron validados satisfactoriamente.");
                return result;*/
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

        private async Task<Models.ValidationResult> ValidarApoderadoReniec(Models.Constancia.ApoderadoPersonaRequest model)
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

                if (personaResult.fecFallecimientoSpecified)
                {
                    throw new ArgumentException("El solicitante se encuentra fallecido según los registros del RENIEC. Si fuese un error, deberá comunicarse con el RENIEC.");
                }

                if (personaResult.nombrePadre != null
                    && !personaResult.nombrePadre.Trim().ToLower().Equals(persona.nombrePadre.Trim().ToLower()))
                {
                    throw new ArgumentException("Los datos ingresados no coinciden con los registrados en el RENIEC. Por favor, verifique que hayan sido ingresados correctamente.");
                }

                if (personaResult.nombreMadre != null
                    && !personaResult.nombreMadre.Trim().ToLower().Equals(persona.nombreMadre.Trim().ToLower()))
                {
                    throw new ArgumentException("Los datos ingresados no coinciden con los registrados en el RENIEC. Por favor, verifique que hayan sido ingresados correctamente.");
                }

                if (!personaResult.fecNacimiento.Equals(persona.fecNacimiento)
                    || !personaResult.ubigeoDomicilio.Equals(persona.ubigeoDomicilio))
                {
                    throw new ArgumentException("Los datos ingresados no coinciden con los registrados en el RENIEC. Por favor, verifique que hayan sido ingresados correctamente.");
                }

                DateTime nacimiento = DateTime.Parse(personaResult.fecNacimiento); //Fecha de nacimiento
                int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;

                if (edad < 18)
                {
                    throw new ArgumentException("Los datos ingresados corresponden a una persona menor de edad.");
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
        public async Task<StatusResponse> ValidarEstudiante(Models.Constancia.EstudiantePersonaRequest encryptedRequest)
        {
            var result = new StatusResponse();

            try
            {
                var request = new Models.Constancia.EstudiantePersonaRequest()
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
                    ubigeoEstudiante = ReactEncryptationSecurity.Decrypt(encryptedRequest.ubigeoEstudiante, "")
                };

                if (!request.tipoDocEstudiante.Equals("2")
                    || request.nroDocEstudiante.Length != 8
                    || request.fechaNacimientoEstudiante.Equals("")
                    || request.ubigeoEstudiante.Length != 6)
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
                var personaRequest = new Models.PersonaRequest()
                {
                    tipoDocumento = request.tipoDocEstudiante,
                    nroDocumento = request.nroDocEstudiante
                };

                var statusResponse1 = await _siagieService
                    .GetServiceByQueryAndToken<StatusResponse, Models.PersonaRequest>(siagie, "estudiante", personaRequest);

                //Validación de estudiante en Siagie: FAIL?
                if (!statusResponse1.Success)
                {
                    throw new ArgumentException(statusResponse1.Messages[0]);
                }

                //Validación de estudiante en Siagie: OK
                var responseEstudiante = JsonConvert
                    .DeserializeObject<List<Models.Siagie.EstudiantePersonaResponse>>(statusResponse1.Data.ToString())
                    .Where(x => x.idPersonaEstudiante != 0)
                    .FirstOrDefault();

                if (responseEstudiante == null)
                {
                    throw new ArgumentException("La/el estudiante no cuenta con matrícula(s) registrada(s) a partir del 2013 en el SIAGIE. Si considera que se trata de un error, deberá acercarse a la institución educativa.");
                }

                int idPersonaApoderado = 0;
                //Si es con apoderado
                if (request.tipoSolicitante.Equals("1") && !reniecResult.esPadreMadre)
                {
                    personaRequest = new Models.PersonaRequest()
                    {
                        tipoDocumento = request.tipoDocApoderado,
                        nroDocumento = request.nroDocApoderado
                    };

                    var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse, Models.PersonaRequest>(siagie, "apoderado", personaRequest);

                    //Validación de apoderado en Siagie: FAIL?
                    if (!statusResponse.Success)
                    {
                        string message = (statusResponse.Messages.Count > 0)
                            ? statusResponse.Messages[0]
                            : "El solicitante no se encuentra registrado como padre o madre en el RENIEC ni como apoderado en el SIAGIE. Si considera que el registro de apoderado no se encuentra actualizado, podrá acercarse a la institución educativa para actualizarlo en el SIAGIE.";

                        throw new ArgumentException(message);
                    }

                    //Validación de apoderado en Siagie: OK
                    var responseApoderado = JsonConvert
                        .DeserializeObject<List<Models.Siagie.ApoderadoPersonaResponse>>(statusResponse.Data.ToString())
                        .First();

                    var apoderadoEstudianteRequest = new Models.Constancia.ApoderadoEstudianteRequest()
                    {
                        idPersonaApoderado = responseApoderado.idPersonaApoderado,
                        idPersonaEstudiante = responseEstudiante.idPersonaEstudiante
                    };

                    var statusResponse2 = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse, 
                        Models.Constancia.ApoderadoEstudianteRequest>(siagie, "apoderado/estudiante", apoderadoEstudianteRequest);

                    //Validación de relación apoderado-estudiante en Siagie: FAIL?
                    if (!statusResponse2.Success)
                    {
                        string message = (statusResponse2.Messages.Count > 0)
                            ? statusResponse2.Messages[0]
                            : "El solicitante no se encuentra registrado como padre o madre en el RENIEC ni como apoderado en el SIAGIE.Si considera que el registro de apoderado no se encuentra actualizado, podrá acercarse a la institución educativa para actualizarlo en el SIAGIE";
                        throw new ArgumentException(message);
                    }

                    idPersonaApoderado = responseApoderado.idPersonaApoderado;
                }

                result.Success = true;
                result.Data = new {
                    //idPersonaApoderado = reniecResult.esPadreMadre ? _encryptionServerSecurity.Encrypt("0") : null,
                    idPersonaApoderado = _encryptionServerSecurity.Encrypt(idPersonaApoderado.ToString()),
                    //apoderadoSiagie = _encryptionServerSecurity.Encrypt(reniecResult.esPadreMadre ? "0" : "1"),
                    idPersonaEstudiante = _encryptionServerSecurity.Encrypt(responseEstudiante.idPersonaEstudiante.ToString()),
                    nombreEstudiante = ReactEncryptationSecurity.Encrypt(reniecResult.persona.nombres.ToUpper())
                };
                result.Messages.Add(
                    request.tipoSolicitante.Equals("1")
                    ? "Los datos de estudiante fueron validados satisfactoriamente."
                    : "estudiante, sus datos fueron validados satisfactoriamente"
                );
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

        private async Task<Models.ValidationResult> ValidarEstudianteReniec(Models.Constancia.EstudiantePersonaRequest model)
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
                    throw new ArgumentException("Los datos ingresados no coinciden con los registrados en el RENIEC. Por favor, verifique que hayan sido ingresados correctamente.");
                }

                if (personaResult.nombreMadre != null
                    && !personaResult.nombreMadre.Trim().ToLower().Equals(persona.nombreMadre.Trim().ToLower()))
                {
                    throw new ArgumentException("Los datos ingresados no coinciden con los registrados en el RENIEC. Por favor, verifique que hayan sido ingresados correctamente.");
                }

                if (!personaResult.fecNacimiento.Equals(persona.fecNacimiento)
                    || !personaResult.ubigeoDomicilio.Equals(persona.ubigeoDomicilio))
                {
                    throw new ArgumentException("Los datos ingresados no coinciden con los registrados en el RENIEC. Por favor, verifique que hayan sido ingresados correctamente.");
                }

                DateTime nacimiento = DateTime.Parse(personaResult.fecNacimiento); //Fecha de nacimiento
                int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;

                //Tienes apoderado y eres mayor de edad
                //if (model.idPersonaApoderado > 0 && edad >= 18)

                if (model.tipoSolicitante.Equals("1"))
                {
                    if (edad >= 18)
                    {
                        throw new ArgumentException("Los datos del estudiante corresponden a una persona mayor de edad. Ella o él deberá solicitar la constancia mediante la opción “A título personal”.");
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
                                                " deberá solicitar la constancia mediante la opción “Apoderado”.");
                    } else if (edad >= 18 && personaResult.fecFallecimientoSpecified)
                    {
                        throw new ArgumentException("El solicitante se encuentra fallecido según los registros del RENIEC. Si fuese un error, deberá comunicarse con el RENIEC.");
                    }
                }

                //No tienes apoderado y eres menor de edad
                //if (model.idPersonaApoderado <= 0 && edad < 18)
                /*if (model.tipoSolicitante.Equals("1") && edad < 18)
                {
                    throw new ArgumentException("Los datos ingresados NO corresponden a una persona mayor de edad," +
                        " deberá solicitar la constancia mediante la opción: Apoderado.");
                }*/

                result.result = true;
                result.persona = personaResult;
                return result;
            }
            catch (Exception ex)
            {
                result.result = false;
                result.error = ex.GetType().IsAssignableFrom(typeof(ArgumentException))
                    ? ex.Message
                    : "Se presentó un inconveniente al validar la información solicitada.";
                result.persona = null;
                return result;
            }
        }

        //STEP 5 > NIVELES DE ESTUDIANTE
        public async Task<StatusResponse> ObtenerNivelesEstudiante(Models.Constancia.EstudianteModalidadRequest encryptedRequest)
        {
            var result = new StatusResponse();

            try
            {
                var siagie = _encryptionServerSecurity.Decrypt<string>(ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", ""), "");
                var request = new
                {
                    idPersona = _encryptionServerSecurity.Decrypt<int>(encryptedRequest.idPersonaEstudiante, 0),
                    idModalidad = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.idModalidad, "")
                };

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse, 
                        object>(siagie, "niveles", request);

                //Consulta de niveles en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    throw new ArgumentException("No se logró obtener la relación de Niveles Educativos del estudiante.");
                }

                //Consulta de niveles en Siagie: OK
                var estudianteNiveles = JsonConvert
                    .DeserializeObject<List<Models.Siagie.EstudianteNivelesResponse>>(statusResponse.Data.ToString())
                    .ToList();

                result.Success = true;
                result.Data = estudianteNiveles.Select(x => new
                {
                    idNivel = _encryptionServerSecurity.Encrypt(x.idNivel.ToString()),
                    descripcionNivel = x.descripcionNivel
                }).ToList();
                result.Messages.Add("La relación de Niveles Educativos del estudiante, conforme.");
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

        //STEP 5 > DATOS DE LA ÚLTIMA IE V2
        public async Task<StatusResponse> ObtenerUltimoColegioEstudianteV2(Models.Constancia.EstudianteModalidadNivelRequest request)
        {
            var result = new StatusResponse();

            try
            {
                var siagie = _encryptionServerSecurity.Decrypt<string>(ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", ""), "");
                var decrypted = new
                {
                    idPersona = _encryptionServerSecurity.Decrypt<int>(request.idPersonaEstudiante, 0),
                    idModalidad = _encryptionServerSecurity.Decrypt<string>(request.idModalidad, ""),
                    idNivel = _encryptionServerSecurity.Decrypt<string>(request.idNivel, "")
                };

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        object>(siagie, "colegio/datos", decrypted);

                //Consulta de niveles en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    throw new ArgumentException("No se logró obtener los datos de las IIEE del Estudiante.");
                }

                //Consulta de niveles en Siagie: OK
                var estudiantesColegioNivel = JsonConvert
                    .DeserializeObject<List<Models.Siagie.EstudianteColegioNivelResponse>>(statusResponse.Data.ToString())
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

        //STEP 5 > ¿HAY OTRAS SOLICITUDES PARA DICHO ESTUDIANTE?
        public async Task<StatusResponse> ObtenerUltimaSolicitudEstudiante(Models.Constancia.EstudianteModalidadNivelRequest encryptedRequest)
        {
            var result = new StatusResponse();

            var idPersona = _encryptionServerSecurity.Decrypt<int>(encryptedRequest.idPersonaEstudiante, 0);
            var idModalidad = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.idModalidad, "");
            var idNivel = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.idNivel, "");

            try
            {
                if (idPersona.Equals("0"))
                {
                    throw new ArgumentException("Datos de Estudiante inválidos.");
                }

                if (idModalidad.Equals(""))
                {
                    throw new ArgumentException("Modalidad de Estudios inválida.");
                }

                if (idNivel.Equals(""))
                {
                    throw new ArgumentException("Nivel Educativo inválido.");
                }

                var resultSolicitud = await _unitOfWork.ObtenerSolicitudPorPersona(idPersona, idModalidad, idNivel);
                if (resultSolicitud == null)
                {
                    throw new ArgumentException("Ocurrió un problema al verificar si tiene otras solicitudes.");
                }

                if (resultSolicitud.ToList().Count == 0)
                {
                    throw new ArgumentException("No hay otras solicitudes.");
                }

                Models.Constancia.SolicitudModel solicitud = SolicitudMapper.Map(resultSolicitud.FirstOrDefault());

                var resultEstudiante = await _unitOfWork.ObtenerEstudianteValidado(solicitud.idEstudiante);
                if (resultEstudiante == null)
                {
                    throw new ArgumentException("Ocurrió un problema al verificar si tiene otras solicitudes.");
                }

                if (resultEstudiante.ToList().Count == 0)
                {
                    throw new ArgumentException("No hay otras solicitudes.");
                }

                Models.Constancia.EstudianteModel estudiante = EstudianteMapper.Map(resultEstudiante.FirstOrDefault());

                result.Success = true;
                result.Data = new Models.Constancia.UltimaSolicitudResponse()
                {
                    codigoVirtual = ReactEncryptationSecurity.Encrypt(solicitud.codigoVirtual),
                    //fechaCreacion = ReactEncryptationSecurity.Encrypt(solicitud.fechaCreacion.ToString("dd-MM-yyyy HH:mm", CultureInfo.CreateSpecificCulture("es-PE")))
                    datosEstudiante = ReactEncryptationSecurity.Encrypt((estudiante.nombres 
                    + " " + estudiante.apellidoPaterno 
                    + " " + estudiante.apellidoMaterno).ToUpper())
                };
                result.Messages.Add("Última solicitud del estudiante, conforme");
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
        public async Task<byte[]> VistaPreviaPDFConstancia(Models.Constancia.EstudianteModalidadNivelPersonaRequest encryptedRequest)
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

                var estudianteConstancia = await SiagieEstudiante(siagie, request);
                if (estudianteConstancia == null)
                {
                    return null;
                }

                return PDFConstanciaInit(true, estudianteConstancia);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<Models.Constancia.EstudianteConstancia> SiagieEstudiante(string siagie, Models.Constancia.EstudianteModalidadNivelPersonaRequest request)
        {
            Models.Constancia.EstudianteConstancia estudianteConstancia = new Models.Constancia.EstudianteConstancia();

            //OBTENER DATA DE SIAGIE
            var infoEstudiante = await SiagieInfoEstudiante(siagie, request);
            if (infoEstudiante == null)
            {
                return null;
            }

            estudianteConstancia.solicitud = new Models.Constancia.SolicitudModel()
            {
                idModalidad = infoEstudiante.idModalidad,
                abrModalidad = infoEstudiante.abrModalidad,
                dscModalidad = infoEstudiante.dscModalidad,
                idNivel = infoEstudiante.idNivel,
                dscNivel = infoEstudiante.dscNivel,
                idGrado = infoEstudiante.idGrado,
                dscGrado = infoEstudiante.dscGrado,
                estadoSolicitud = "2"
            };

            estudianteConstancia.estudiante = new Models.Constancia.EstudianteModel()
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

            estudianteConstancia.grados = await SiagieGradosEstudiante(siagie, requestModalidadNivel);
            if (estudianteConstancia.grados == null || estudianteConstancia.grados.Count == 0)
            {
                return null;
            }

            //List<NotaModel> notasList = await SiagieNotasEstudiante(idPersona, idModalidad, idNivel);
            estudianteConstancia.notas = await SiagieNotasEstudiante(siagie, requestModalidadNivel);
            if (estudianteConstancia.notas == null || estudianteConstancia.notas.Count == 0)
            {
                return null;
            }

            estudianteConstancia.observaciones = await SiagieObservacionesEstudiante(siagie, requestModalidadNivel);
            /*if (estudianteConstancia.observaciones == null || estudianteConstancia.observaciones.Count == 0)
            {
                return null;
            }*/

            return estudianteConstancia;
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
                    requestModalidadNivelPersona.numeroDocumento
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

        private async Task<List<Models.Constancia.GradoModel>> SiagieGradosEstudiante(string siagie, Models.Constancia.EstudianteModalidadNivelRequest requestModalidadNivel)
        {
            try
            {
                var request = new
                {
                    idPersona = requestModalidadNivel.idPersonaEstudiante,
                    requestModalidadNivel.idModalidad,
                    requestModalidadNivel.idNivel
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
                    .DeserializeObject<List<Models.Constancia.GradoModel>>(statusResponse.Data.ToString())
                    .ToList();

                return result;
            }
            catch
            {
                return null;
            }
        }

        private async Task<List<Models.Constancia.NotaModel>> SiagieNotasEstudiante(string siagie, Models.Constancia.EstudianteModalidadNivelRequest requestModalidadNivel)
        {
            try
            {
                var request = new
                {
                    idPersona = requestModalidadNivel.idPersonaEstudiante,
                    requestModalidadNivel.idModalidad,
                    requestModalidadNivel.idNivel
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
                    .DeserializeObject<List<Models.Constancia.NotaModel>>(statusResponse.Data.ToString())
                    .ToList();

                return result;
            }
            catch
            {
                return null;
            }
        }

        // ORDENAR NOTAS CON LINQ
        private List<Models.PDFNota> OrdenarNotasEstudiante(List<Models.Constancia.NotaModel> notasList, List<Models.Constancia.GradoModel> gradosList, string idTipoArea)
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

        private async Task<List<Models.Constancia.ObservacionModel>> SiagieObservacionesEstudiante(string siagie, Models.Constancia.EstudianteModalidadNivelRequest requestModalidadNivel)
        {
            try
            {
                var request = new
                {
                    idPersona = requestModalidadNivel.idPersonaEstudiante,
                    requestModalidadNivel.idModalidad,
                    requestModalidadNivel.idNivel
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
                    .DeserializeObject<List<Models.Constancia.ObservacionModel>>(statusResponse.Data.ToString())
                    .Where(x => x.tipoObs > 2)
                    .ToList();

                return result;
            }
            catch
            {
                return null;
            }
        }

        private string ConcatenarObservaciones(List<Models.Constancia.ObservacionModel> observaciones)
        {
            try
            {
                string result = "";

                if (observaciones != null)
                {
                    result = observaciones.Select(x => String.Format("{0} - {1} - {2}", x.idAnio.ToString(), x.resolucion.Trim().ToUpper(), x.motivo.Trim().ToUpper()))
                                       .Aggregate(new StringBuilder(), (current, next) => current.Append(next).Append(Environment.NewLine).Append(Environment.NewLine)).ToString();
                }

                return result;
            }
            catch
            {
                return "";
            }
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

        //STEP 6 > GENERAR SOLICITUD + PDF
        public async Task<StatusResponse> GenerarPDFCostancia(Models.Constancia.SolicitudRequest encryptedRequest)
        {
            var result = new StatusResponse();

            try
            {
                //Decrypt
                var request = new Models.Constancia.SolicitudRequest()
                {
                    tipo = new Models.Constancia.TipoSolicitudRequest()
                    {
                        tipoSolicitante = ReactEncryptationSecurity.Decrypt(encryptedRequest.tipo.tipoSolicitante, ""),
                        //apoderadoSiagie = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.tipo.apoderadoSiagie, "0")
                    },
                    solicitante = new Models.Constancia.SolicitanteRequest()
                    {
                        idPersonaApoderado = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.solicitante.idPersonaApoderado, "0"),
                        //idPersonaApoderado = encryptedRequest.solicitante.idPersonaApoderado,
                        tipoDocApoderado = ReactEncryptationSecurity.Decrypt(encryptedRequest.solicitante.tipoDocApoderado, ""),
                        nroDocApoderado = ReactEncryptationSecurity.Decrypt(encryptedRequest.solicitante.nroDocApoderado, ""),
                        ubigeoApoderado = ReactEncryptationSecurity.Decrypt(encryptedRequest.solicitante.ubigeoApoderado, ""),
                    },
                    estudiante = new Models.Constancia.EstudianteRequest()
                    {
                        idPersonaEstudiante = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.estudiante.idPersonaEstudiante, "0"),
                        //idPersonaEstudiante = encryptedRequest.estudiante.idPersonaEstudiante,
                        tipoDocEstudiante = ReactEncryptationSecurity.Decrypt(encryptedRequest.estudiante.tipoDocEstudiante, ""),
                        nroDocEstudiante = ReactEncryptationSecurity.Decrypt(encryptedRequest.estudiante.nroDocEstudiante, ""),
                        ubigeoEstudiante = ReactEncryptationSecurity.Decrypt(encryptedRequest.estudiante.ubigeoEstudiante, ""),
                    },
                    solicitud = new Models.Constancia.InformacionRequest()
                    {
                        idModalidad = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.solicitud.idModalidad, ""),
                        //modalidad = encryptedRequest.solicitud.modalidad,
                        idNivel = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.solicitud.idNivel, ""),
                        //nivel = encryptedRequest.solicitud.nivel,
                        telefonoContacto = ReactEncryptationSecurity.Decrypt(encryptedRequest.solicitud.telefonoContacto, ""),
                        correoElectronico = ReactEncryptationSecurity.Decrypt(encryptedRequest.solicitud.correoElectronico, ""),
                        idMotivo = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.solicitud.idMotivo, ""),
                        //motivo = encryptedRequest.solicitud.motivo,
                        motivoOtros = ReactEncryptationSecurity.Decrypt(encryptedRequest.solicitud.motivoOtros, "")
                    }
                };

                //Validate
                if (request.tipo.tipoSolicitante.Length != 1)
                {
                    throw new ArgumentException("Tipo de solicitante no válido");
                }


                /*if (request.solicitante.idPersonaApoderado.Equals("0"))
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


                if (request.estudiante.idPersonaEstudiante.Equals("0"))
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


                if (request.solicitud.idModalidad.Equals(""))
                {
                    throw new ArgumentException("Modalidad de Estudios inválida");
                }

                if (request.solicitud.idNivel.Equals(""))
                {
                    throw new ArgumentException("Nivel Educativo inválido");
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

                //Validación de Estudiante
                var personaRequest = new Models.PersonaRequest()
                {
                    tipoDocumento = request.estudiante.tipoDocEstudiante,
                    nroDocumento = request.estudiante.nroDocEstudiante
                };

                var statusResponseE = await _siagieService
                    .GetServiceByQueryAndToken<StatusResponse, Models.PersonaRequest>(siagie, "estudiante", personaRequest);

                //Validación de estudiante en Siagie: FAIL?
                if (!statusResponseE.Success)
                {
                    string message = (statusResponseE.Messages.Count > 0)
                        ? statusResponseE.Messages[0]
                        : "El estudiante no cuenta con matrícula(s) registrada(s) a partir del 2013 en el SIAGIE, " +
                        "deberá de comunicarse con la Institución Educativa.";

                    result.Success = false;
                    result.Data = null;
                    result.Messages.Add(message);
                    return result;
                }

                //Con Apoderado
                if (request.tipo.tipoSolicitante.Equals("1") && !request.solicitante.idPersonaApoderado.Equals("0"))
                {
                    //Validación de Apoderado
                    personaRequest = new Models.PersonaRequest()
                    {
                        tipoDocumento = request.solicitante.tipoDocApoderado,
                        nroDocumento = request.solicitante.nroDocApoderado
                    };

                    var statusResponseA = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse, Models.PersonaRequest>(siagie, "apoderado", personaRequest);

                    //Validación de apoderado en Siagie: FAIL?
                    if (!statusResponseA.Success)
                    {
                        string message = (statusResponseA.Messages.Count > 0)
                            ? statusResponseA.Messages[0]
                            : "Usted no se encuentra registrado como apoderado en el SIAGIE, " +
                            "deberá de comunicarse con la Institución Educativa.";
                        throw new ArgumentException(message);
                    }

                    //Validación de Relación entre Apoderado y Estudiante
                    var apoderadoEstudianteRequest = new Models.Constancia.ApoderadoEstudianteRequest()
                    {
                        //idPersonaApoderado = _encryptionServerSecurity.Decrypt<int>(encryptedRequest.solicitante.idPersonaApoderado, 0),
                        idPersonaApoderado = int.Parse(request.solicitante.idPersonaApoderado),
                        //idPersonaEstudiante = _encryptionServerSecurity.Decrypt<int>(encryptedRequest.estudiante.idPersonaEstudiante, 0),
                        idPersonaEstudiante = int.Parse(request.estudiante.idPersonaEstudiante)
                    };

                    var statusResponseEA = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        Models.Constancia.ApoderadoEstudianteRequest>(siagie, "apoderado/estudiante", apoderadoEstudianteRequest);

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
                    idModalidad = request.solicitud.idModalidad,
                    idNivel = request.solicitud.idNivel,
                    idTipoDocumento = request.estudiante.tipoDocEstudiante,
                    numeroDocumento = request.estudiante.nroDocEstudiante
                };

                Models.Constancia.EstudianteConstancia estudianteConstancia = await SiagieEstudiante(siagie, estudianteModalidadNivelPersona);

                Models.ReniecPersona estudiantePersona = await _reniecService.ReniecConsultarPersona(request.estudiante.nroDocEstudiante);
                Models.ReniecPersona apoderadoPersona = await _reniecService.ReniecConsultarPersona(request.solicitante.nroDocApoderado);
                if (estudiantePersona == null || apoderadoPersona == null)
                {
                    throw new ArgumentException("El servicio de RENIEC no se encuentra disponible vuelva a internar en unos momentos.");
                }

                _unitOfWork.BeginTransaction();

                int resultEstudiante = await _unitOfWork.InsertarEstudiante(EstudianteMapper.Map(new Models.Constancia.EstudianteModel()
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

                int resultSolicitante = await _unitOfWork.InsertarSolicitante(SolicitanteMapper.Map(new Models.Constancia.SolicitanteModel()
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

                int idSolicitud = await _unitOfWork.InsertarSolicitud(SolicitudMapper.Map(new Models.Constancia.SolicitudModel()
                {
                    idSolicitud = 0,
                    idEstudiante = resultEstudiante,
                    idSolicitante = resultSolicitante,
                    idMotivo = int.Parse(request.solicitud.idMotivo),
                    idModalidad = estudianteConstancia.solicitud.idModalidad,
                    abrModalidad = estudianteConstancia.solicitud.abrModalidad,
                    dscModalidad = estudianteConstancia.solicitud.dscModalidad,
                    idNivel = estudianteConstancia.solicitud.idNivel,
                    dscNivel = estudianteConstancia.solicitud.dscNivel,
                    idGrado = estudianteConstancia.solicitud.idGrado,
                    dscGrado = estudianteConstancia.solicitud.dscGrado,
                    motivoOtros = request.solicitud.motivoOtros
                }));

                if (idSolicitud <= 0)
                {
                    throw new ArgumentException("Ocurrió un problema al registrar su solicitud (1).");
                }

                int countGrados = 0;
                foreach (Models.Constancia.GradoModel grado in estudianteConstancia.grados)
                {
                    grado.idSolicitud = idSolicitud;
                    var idConstanciaGrado = await _unitOfWork.InsertarGrado(GradoMapper.Map(grado));

                    if (idConstanciaGrado > 0)
                    {
                        countGrados++;
                    }
                }

                if (countGrados != estudianteConstancia.grados.Count)
                {
                    throw new ArgumentException("Ocurrió un problema al registrar su solicitud (2).");
                }

                int countNotas = 0;
                foreach (Models.Constancia.NotaModel nota in estudianteConstancia.notas)
                {
                    nota.idSolicitud = idSolicitud;
                    var idConstanciaNota = await _unitOfWork.InsertarNota(NotaMapper.Map(nota));

                    if (idConstanciaNota > 0)
                    {
                        countNotas++;
                    }
                }

                if (countNotas != estudianteConstancia.notas.Count)
                {
                    throw new ArgumentException("Ocurrió un problema al registrar su solicitud (3).");
                }

                if (estudianteConstancia.observaciones != null)
                {
                    int countObs = 0;
                    foreach (Models.Constancia.ObservacionModel obs in estudianteConstancia.observaciones)
                    {
                        obs.idSolicitud = idSolicitud;
                        var idConstanciaObservacion = await _unitOfWork.InsertarObservacion(ObservacionMapper.Map(obs));

                        if (idConstanciaObservacion > 0)
                        {
                            countObs++;
                        }
                    }

                    if (countObs != estudianteConstancia.observaciones.Count)
                    {
                        throw new ArgumentException("Ocurrió un problema al registrar su solicitud (4).");
                    }
                }

                _unitOfWork.DesactivarSolicitudes(idSolicitud
                    , int.Parse(request.estudiante.idPersonaEstudiante)
                    , estudianteConstancia.solicitud.idModalidad
                    , estudianteConstancia.solicitud.idNivel);

                _unitOfWork.Commit();

                var resultSolicitud = await _unitOfWork.ObtenerSolicitud(SolicitudMapper.Map(new Models.Constancia.SolicitudModel()
                {
                    idSolicitud = idSolicitud
                }));

                Models.Constancia.SolicitudModel solicitud = SolicitudMapper.Map(resultSolicitud.FirstOrDefault());

                var correo = PrepararCorreo(solicitud, apoderadoPersona.nombres, request.solicitud.correoElectronico);
                var correoResult = await EnviarCorreo(correo);

                result.Success = true;
                result.Data = new Models.Constancia.SolicitudResponse()
                {
                    codigoVirtual = ReactEncryptationSecurity.Encrypt(solicitud.codigoVirtual),
                    nroDocEstudiante = ReactEncryptationSecurity.Encrypt(estudiantePersona.numDoc),
                    //nombresEstudiante = estudiantePersona.nombres,
                    nombresSolicitante = ReactEncryptationSecurity.Encrypt(apoderadoPersona.nombres),
                    fechaCreacion = ReactEncryptationSecurity.Encrypt(solicitud.fechaCreacion.ToString("HH:mm", CultureInfo.CreateSpecificCulture("es-PE")))
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

        private Models.CorreoModel PrepararCorreo(Models.Constancia.SolicitudModel solicitud, string nombresApoderado, string correoElectronico)
        {
            string mensaje = "<html><body>Estimad@ <strong>" + nombresApoderado.ToUpper() + "</strong><br/><br/>" +
                   "Su Constancia de Logros de Aprendizaje ha sido generada correctamente, " +
                   "con <strong>código virtual N.° " + solicitud.codigoVirtual + "</strong>, a las " +
                   "<strong>" + solicitud.fechaCreacion.ToString("HH:mm", CultureInfo.CreateSpecificCulture("es-PE")) + " h.</strong><br/><br/>" +
                   "Puede descargarla nuevamente ingresando al siguiente enlace <strong>" + _configuration.GetSection("PDF:QRCodeUrl").Value + "/validate</strong>, " +
                   "digitando el código virtual y el número de DNI del estudiante o leyendo el código QR de la constancia.<br/><br/>" +
                   "Ministerio de Educación<br/><br/></body></html>";

            mensaje.Replace("'", "''");

            return new Models.CorreoModel()
            {
                para = correoElectronico,
                asunto = "Constancia de Logros de Aprendizaje - MINEDU",
                mensaje = mensaje
            };
        }

        private async Task<bool> EnviarCorreo(Models.CorreoModel model)
        {
            try
            {
                return await _unitOfWork.EnviarCorreo(CorreoMapper.Map(model));
            } 
            catch
            {
                return false;
            }
        }

        //STEP 6 + RESULT > DESCARGAR CONSTANCIA
        public async Task<StatusResponse> DescargarPDFConstancia(Models.Constancia.DescargaRequest encryptedRequest)
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
                var pdf = await PDFConstanciaDescarga(request.codigoVirtual, request.tipoDocumento, request.numeroDocumento);

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

        //VALIDAR CONSTANCIA
        public async Task<StatusResponse> ValidarPDFConstancia(Models.Constancia.VerificacionRequest encryptedRequest)
        {
            var result = new StatusResponse();

            var request = new Models.Constancia.VerificacionRequest()
            {
                codigoVirtual = ReactEncryptationSecurity.Decrypt(encryptedRequest.codigoVirtual, ""),
                tipoDocumento = ReactEncryptationSecurity.Decrypt(encryptedRequest.tipoDocumento, ""),
                numeroDocumento = ReactEncryptationSecurity.Decrypt(encryptedRequest.numeroDocumento, ""),
                captcha = encryptedRequest.captcha
            };

            try {

                if (request.numeroDocumento.Equals("") || !validarReCaptcha(request.captcha))
                {
                    throw new ArgumentException("El Código Captcha generado es inválido o posiblemente haya caducado. Inténtelo nuevamente.");
                }

                if (request.codigoVirtual.Length != 8)
                {
                    throw new ArgumentException("El Código Virtual digitado es inválido.");
                }

                if (!request.tipoDocumento.Equals("2"))
                {
                    throw new ArgumentException("El Tipo de Documento es inválido.");
                }

                if (request.numeroDocumento.Length != 8)
                {
                    throw new ArgumentException("El Documento de Identidad del estudiante es inválido.");
                }

                var pdf = await PDFConstanciaDescarga(request.codigoVirtual, request.tipoDocumento, request.numeroDocumento);

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

        private async Task<byte[]> PDFConstanciaDescarga(string codigoVirtual, string tipoDocumento, string numeroDocumento)
        {
            try
            {
                Models.Constancia.EstudianteConstancia estudianteConstancia = new Models.Constancia.EstudianteConstancia();

                var resultSolicitud = await _unitOfWork.ObtenerSolicitudPorCodigoVirtual(codigoVirtual, tipoDocumento, numeroDocumento);
                if (resultSolicitud == null || resultSolicitud.ToList().Count == 0)
                {
                    throw new ArgumentException("No se encontró una solicitud de Constancia de Logros de Aprendizaje que coincida con los datos ingresados.");
                }
                estudianteConstancia.solicitud = SolicitudMapper.Map(resultSolicitud.FirstOrDefault());

                var resultEstudiante = await _unitOfWork.ObtenerEstudianteValidado(estudianteConstancia.solicitud.idEstudiante);
                if (resultEstudiante == null || resultEstudiante.ToList().Count == 0)
                {
                    throw new ArgumentException("Ocurrió un problema al procesar su solicitud");
                }
                estudianteConstancia.estudiante = EstudianteMapper.Map(resultEstudiante.FirstOrDefault());

                var resultGrados = await _unitOfWork.ObtenerGradosValidados(estudianteConstancia.solicitud.idSolicitud);
                if (resultGrados == null || resultGrados.ToList().Count == 0)
                {
                    throw new ArgumentException("Ocurrió un problema al procesar su solicitud");
                }
                estudianteConstancia.grados = resultGrados.Select(GradoMapper.Map).ToList();

                var resultNotas = await _unitOfWork.ObtenerNotasValidadas(estudianteConstancia.solicitud.idSolicitud);
                if (resultNotas == null || resultNotas.ToList().Count == 0)
                {
                    throw new ArgumentException("Ocurrió un problema al procesar su solicitud");
                }
                estudianteConstancia.notas = resultNotas.Select(NotaMapper.Map).ToList();

                var resultObservaciones = await _unitOfWork.ObtenerObservacionesValidadas(estudianteConstancia.solicitud.idSolicitud);
                if (resultObservaciones != null)
                {
                    estudianteConstancia.observaciones = resultObservaciones.Select(ObservacionMapper.Map).ToList();
                }

                return PDFConstanciaInit(false, estudianteConstancia);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private byte[] PDFConstanciaInit(bool vistaPrevia, Models.Constancia.EstudianteConstancia estudianteConstancia)
        {
            try
            {
                List<Models.PDFNota> cursoList = OrdenarNotasEstudiante(estudianteConstancia.notas, estudianteConstancia.grados, "001");
                List<Models.PDFNota> competenciaList = OrdenarNotasEstudiante(estudianteConstancia.notas, estudianteConstancia.grados, "003");
                List<Models.PDFNota> tallerList = OrdenarNotasEstudiante(estudianteConstancia.notas, estudianteConstancia.grados, "002");
                string obs = ConcatenarObservaciones(estudianteConstancia.observaciones);

                return PDFConstancia(vistaPrevia, estudianteConstancia, cursoList, competenciaList, tallerList, obs).ToArray();
            }
            catch
            {
                return null;
            }
        }

        private MemoryStream PDFConstancia(bool vistaPrevia, Models.Constancia.EstudianteConstancia estudianteConstancia, 
            List<Models.PDFNota> cursoList, 
            List<Models.PDFNota> competenciaList, 
            List<Models.PDFNota> tallerList,
            string obs)
        {
            /* Variables */
            List<Models.Constancia.GradoModel> gradosList = estudianteConstancia.grados;

            string correlativo = String.Format("{0:D8}", estudianteConstancia.solicitud.idSolicitud);
            string hashQRCode = _configuration.GetSection("PDF:QRCodeUrl").Value + "/validate/" + estudianteConstancia.solicitud.codigoVirtual;
            //string hashQRCode = "http://192.168.210.152:8050/validate/" + estudianteConstancia.solicitud.codigoVirtual;

            int totalGrados = gradosList.Count;
            string gradosConcatenados = "";

            string[] gradosCursados = gradosList.Where(w => w.idAnio > 0).Select(x => x.dscGrado).ToArray();
            //string gradosConcatenados = string.Join(", ", gradosCursados);
            gradosConcatenados = ConcatenarGrados(gradosCursados);

            int cursos = cursoList.Count;
            int competencias = competenciaList.Count;
            int talleres = tallerList.Count; //16
            //int hdl = 3;
            //string ubigeo = "Lima";
            string fechaSolicitud = estudianteConstancia.solicitud.fechaCreacion.ToString("d 'de' MMMM 'de' yyyy", CultureInfo.CreateSpecificCulture("es-PE"));
            //string fechaSolicitud = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", CultureInfo.CreateSpecificCulture("es-PE"));
            string horaSolicitud = estudianteConstancia.solicitud.fechaCreacion.ToString("HH:mm 'horas'", CultureInfo.CreateSpecificCulture("es-PE"));
            //string horaSolicitud = DateTime.Now.ToString("HH:mm 'horas'", CultureInfo.CreateSpecificCulture("es-PE"));

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

                //pdf.AddEventHandler(PdfDocumentEvent.START_PAGE, new StartPageEventHandler(document));
                document.SetMargins(18, 18, 89 + 18, 18);
                #region Header
                Table table = new Table(UnitValue.CreatePercentArray(new float[] { 25, 50, 25 }));
                table.SetWidth(UnitValue.CreatePercentValue(100));
                table.SetHorizontalAlignment(HorizontalAlignment.CENTER);

                table.AddCell(new Cell(4, 1).Add(image).SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                table.AddCell(PDFMinedu.getCell("MINISTERIO DE EDUCACIÓN", 9, false, 1, 1, TextAlignment.CENTER, null, false));
                table.AddCell(PDFMinedu.getCell(" ", 9, false, 1, 1, TextAlignment.CENTER, null, false));
                table.AddCell(PDFMinedu.getCell("CONSTANCIA DE LOGROS DE APRENDIZAJE", 10, true, 1, 1, TextAlignment.CENTER, null, false));
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

                table.AddCell(PDFMinedu.getCell("EL MINISTERIO DE EDUCACIÓN DEL PERÚ", 9, true, 1, 1, TextAlignment.CENTER, null, false));

                Text text1 = new Text("Hace constar que, de acuerdo con la información registrada en el Sistema de Información de Apoyo a la Gestión de la " +
                "Institución Educativa (Siagie), el/la estudiante " + estudianteConstancia.estudiante.nombres + " " + estudianteConstancia.estudiante.apellidoPaterno + " " +
                estudianteConstancia.estudiante.apellidoMaterno + ", con DNI N.° " + estudianteConstancia.estudiante.numeroDocumento + ", registra " +
                    "calificativos correspondiente(s) a ").SetFont(fontRegular);
                Text text2 = new Text(gradosConcatenados + " ").SetFont(fontBold); // BOLD
                Text text3 = new Text((estudianteConstancia.solicitud.idNivel.Equals("B0") 
                    || estudianteConstancia.solicitud.idNivel.Equals("F0")) ? "grado" : "").SetFont(fontRegular);
                Text text4 = new Text(" de " + estudianteConstancia.solicitud.abrModalidad + ", nivel ").SetFont(fontRegular);
                Text text5 = new Text((estudianteConstancia.solicitud.idNivel.Equals("B0") || estudianteConstancia.solicitud.idNivel.Equals("F0")) ? "de educación" : "").SetFont(fontRegular);
                Text text6 = new Text(" " + estudianteConstancia.solicitud.dscNivel).SetFont(fontBold); // BOLD
                Text text7 = new Text(", según consta en las actas de evaluación respectivas cuyo detalle figura a continuación:").SetFont(fontRegular);

                Paragraph paragraph = new Paragraph().Add(text1).Add(text2).Add(text3).Add(text4).Add(text5).Add(text6).Add(text7);
                Cell cell = new Cell().Add(paragraph)
                    .SetFontSize(8)
                    .SetTextAlignment(TextAlignment.JUSTIFIED)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetBorder(Border.NO_BORDER);

                table.AddCell(cell);

                /*table.AddCell(PDFMinedu.getCell("Hace constar que, de acuerdo a la información registrada con el " +
                    "Sistema de Información de Apoyo a la Gestión de la Institución Educativa (Siagie), el/la " +
                    "estudiante " + estudianteConstancia.estudiante.nombres + " " + estudianteConstancia.estudiante.apellidoPaterno + " " + estudianteConstancia.estudiante.apellidoMaterno + " " +
                    "con DNI N.° " + estudianteConstancia.estudiante.numeroDocumento + " registra calificativos " +
                    "correspondiente(s) al " + gradosConcatenados + " grado de " + estudianteConstancia.solicitud.abrModalidad + ", nivel " +
                    "de educación " + estudianteConstancia.solicitud.dscNivel + ", según consta en las actas de evaluación respectivas, " +
                    "cuyo detalle figura a continuación:",
                    8, false, 1, 1, TextAlignment.JUSTIFIED, null, false));*/
                //table.AddCell(PDFMinedu.getCell("Hello World!", 10, false, 1, 1).SetFont(font));
                document.Add(table);
                #endregion SubHeader

                PDFMinedu.addEspacios(document, sizeFont);

                #region Body

                #region Notas_Parametros
                float[] withColumns = new float[totalGrados + 3];

                withColumns[0] = 3;
                withColumns[1] = 25;

                int withGrados = 54 / totalGrados;
                for (int i = 2; i < totalGrados + 3; i++)
                {
                    withColumns[i] = withGrados;
                }
                withColumns[(totalGrados + 3) - 1] = 18;
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

                table.AddCell(PDFMinedu.getCell("Observaciones", sizeFont, false, 3));

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
                    if (x == 1) table.AddCell(PDFMinedu.getCell(obs, sizeFont, false, cursos + competencias + talleres, 1,TextAlignment.JUSTIFIED));
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
                /*
                #region Notas_Hdl
                rows = hdl;

                table.AddCell(PDFMinedu.getCell("Horas de Libre Disponibilidad", sizeFont, false, rows));

                for (int x = 1; x <= rows; x++)
                {
                    table.AddCell(PDFMinedu.getCell("Hora de Libre Disponibilidad " + x, sizeFont, false, 1, 1, TextAlignment.LEFT));

                    for (int i = 1; i <= totalGrados; i++)
                    {
                        //int randomNota = new Random().Next(1, 20);
                        //string nota = (randomNota < 10) ? ("0" + randomNota) : randomNota.ToString();
                        table.AddCell(PDFMinedu.getCell(" ", sizeFont));
                    }

                    //if (x == 1) table.AddCell(PDFMinedu.getCell("", 8, false, rows, 1));
                }
                #endregion Notas_Hdl
                */
                #region Notas_Final
                table.AddCell(PDFMinedu.getCell("Situación final", sizeFont, true, 1, 2));

                for (int i = 1; i <= totalGrados; i++)
                {
                    table.AddCell(PDFMinedu.getCell(gradosList[i - 1].situacionFinal, 6, true));
                }
                #endregion Notas_Final

                document.Add(table);
                #endregion Body

                PDFMinedu.addEspacios(document, sizeFont);

                #region DateTime
                #endregion DateTime

                #region Footer
                withColumns = new float[] { 10, 70, 20 };
                Table tableFooter = new Table(UnitValue.CreatePercentArray(withColumns));
                tableFooter.SetWidth(UnitValue.CreatePercentValue(100));
                tableFooter.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                //842 - 36 - 36
                //595 - 36 - 36
                //tableFooter.SetWidth(523);
                #region QRImage
                IDictionary<EncodeHintType, Object> hints = new Dictionary<EncodeHintType, object>();
                hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.L);
                //hints.Add(EncodeHintType.CHARACTER_SET, "");

                BarcodeQRCode barcodeQRCode = new BarcodeQRCode(hashQRCode, hints);
                PdfFormXObject xObject = barcodeQRCode.CreateFormXObject(PDFMinedu.getColor("BLACK"), pdf);
                Image qrImage = new Image(xObject);
                qrImage.ScaleToFit(75, 75);
                qrImage.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                //image.SetFixedPosition(10f, 10f);
                #endregion QRImage

                //cell.SetBackgroundColor(ColorConstants.ORANGE);
                Cell qrCell = new Cell(3, 1);
                if (!vistaPrevia)
                {
                    qrCell.Add(qrImage);
                }
                else
                {
                    qrCell.Add(PDFMinedu.getParagraph(" ", 10, false));
                }
                qrCell.SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(Border.NO_BORDER);
                tableFooter.AddCell(qrCell);


                tableFooter.AddCell(PDFMinedu.getCell(vistaPrevia ? " " : "Fecha de emisión:", sizeFont, false, 1, 1, TextAlignment.RIGHT, null, false));
                tableFooter.AddCell(PDFMinedu.getCell(vistaPrevia ? " " : (fechaSolicitud), sizeFont, false, 1, 1, TextAlignment.CENTER, null, false));

                tableFooter.AddCell(PDFMinedu.getCell(vistaPrevia ? " " : "Hora de emisión:", sizeFont, false, 1, 1, TextAlignment.RIGHT, null, false));
                tableFooter.AddCell(PDFMinedu.getCell(vistaPrevia ? " " : horaSolicitud, sizeFont, false, 1, 1, TextAlignment.CENTER, null, false));

                tableFooter.AddCell(PDFMinedu.getCell("* Esta constancia puede ser verificada en el sitio web del Ministerio de Educación " +
                "(" + _configuration.GetSection("PDF:QRCodeUrl").Value + "/validate), " +
                "utilizando lectora de códigos QR o teléfono celular enfocando al código QR: el celular debe poseer un software gratuito descargado de internet.\n" +
                "* EXO: exoneración del área de educación religiosa a solicitud del padre o madre de familia, tutor legal o apoderado."
                , 6, false, 1, 2, TextAlignment.JUSTIFIED, null, false));

                tableFooter.AddCell(PDFMinedu.getCell(vistaPrevia ? " " : "N.° " + correlativo, 8, true, 1, 1, TextAlignment.CENTER, null, false));

                tableFooter.AddCell(PDFMinedu.getCell("Calle Del Comercio 193, San Borja, Lima, Perú / (511) 615-5800",
                    7, false, 1, 2, TextAlignment.CENTER, null, false));

                pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new EndPageEventHandler(tableFooter, document));

                #endregion Footer

                #region WatherMark
                if (!estudianteConstancia.solicitud.estadoSolicitud.Equals("2"))
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
                new Canvas(canvas, pdfDoc, new Rectangle(doc.GetLeftMargin(),
                    18,
                    page.GetPageSize().GetWidth() - doc.GetLeftMargin() - doc.GetRightMargin(),
                    95)).Add(table);
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
            string waterMarkTextConstancia = _configuration.GetSection("PDF:WaterMarkTextConstancia").Value;
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
                Paragraph p = new Paragraph(waterMarkTextConstancia).SetFont(font).SetFontSize(fontSize);
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
            int bypass = int.Parse(_configuration.GetSection("ReCaptcha:ConstanciaValidator").Value);

            if (bypass == 1)
            {
                try
                {
                    string secretKey = _configuration.GetSection("ReCaptcha:ConstanciaKey").Value;
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
            } else
            {
                return true;
            }
        }
    }
}