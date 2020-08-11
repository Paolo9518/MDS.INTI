using Minedu.Comun.Helper;
using Minedu.MiCertificado.Api.Application.Contracts.Services;
using Mappers = Minedu.MiCertificado.Api.Application.Mappers;
using Models = Minedu.MiCertificado.Api.BusinessLogic.Models;
using Minedu.MiCertificado.Api.DataAccess.Contracts.UnitOfWork;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minedu.MiCertificado.Api.Application.Contracts.Security;
using Microsoft.AspNetCore.Http;
using Minedu.MiCertificado.Api.Application.Utils;
using Minedu.MiCertificado.Api.Application.Mappers.Certificado;
using Minedu.MiCertificado.Api.Application.Security;

namespace Minedu.MiCertificado.Api.Application.Services
{
    public class CertificadoMaestroService : ICertificadoMaestroService
    {
        //private readonly IHttpClientFactory _clientFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISiagieService _siagieService;
        private readonly IEncryptionServerSecurity _encryptionServerSecurity;
        public readonly IHttpContextAccessor _httpContextAccessor;

        public CertificadoMaestroService(
            IUnitOfWork unitOfWork,
            ISiagieService siagieService,
            IEncryptionServerSecurity encryptionServerSecurity,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _siagieService = siagieService;
            _encryptionServerSecurity = encryptionServerSecurity;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<StatusResponse> ObtenerGradoSeccion(Models.Certificado.ParametroModel objetoEncriptado)
        {
            var result = new StatusResponse();
            var desencriptarObjeto = ReactEncryptationSecurity.Decrypt<string>(objetoEncriptado.parametro, "");
            var request = JsonConvert.DeserializeObject<Models.Certificado.GradoSeccionRequest>(desencriptarObjeto);

            try
            {
                var siagie = _encryptionServerSecurity.Decrypt<string>(
                ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", "")
                , "");

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        Models.Certificado.GradoSeccionRequest>(siagie, "gradoseccion", request);

                //Consulta de niveles en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    result.Success = false;
                    result.Data = null;
                    result.Messages.Add("No se logró obtener los registros solicitados.");
                    return result;
                }

                // Consulta de niveles en Siagie: OK
                var gradoseccion = JsonConvert
                    .DeserializeObject<List<Models.Certificado.GradoSeccionResponse>>(statusResponse.Data.ToString());

                result.Success = true;
                result.Data = ReactEncryptationSecurity.Encrypt(JsonConvert.SerializeObject(gradoseccion));
                result.Messages.Add("Solicitud exitosa.");
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Data = null;
                result.Messages.Add("Se presentó un inconveniente al procesar su solicitud.");
                return result;
            }
        }

        public async Task<StatusResponse> ObtenerInstitucionEducativa(Models.Certificado.InstitucionEducativaRequest request)
        {
            var result = new StatusResponse();

            try
            {
                var siagie = _encryptionServerSecurity.Decrypt<string>(
                ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", "")
                , "");

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        Models.Certificado.InstitucionEducativaRequest>(siagie, "institucioneducativa", request);

                //Consulta de niveles en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    result.Success = false;
                    result.Data = null;
                    result.Messages.Add("No se logró obtener las Instituciones Educativas");
                    return result;
                }

                // Consulta de niveles en Siagie: OK
                var institucionList = JsonConvert
                    .DeserializeObject<List<Models.Certificado.InstitucionEducativaResponse>>(statusResponse.Data.ToString());

                result.Success = true;
                result.Data = institucionList;
                result.Messages.Add("Instituciones Educativas, conforme");
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Data = null;
                result.Messages.Add("Se presentó un inconveniente al procesar su solicitud.");
                return result;
            }
        }

        public async Task<StatusResponse> ObtenerTipoDeArea()
        {
            var result = new StatusResponse();

            try
            {
                var siagie = _encryptionServerSecurity.Decrypt<string>(
                ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", "")
                , "");

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        object>(siagie, "tipoarea", null);

                //Consulta de niveles en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    result.Success = false;
                    result.Data = null;
                    result.Messages.Add("No se logró obtener los tipos de area");
                    return result;
                }

                // Consulta de niveles en Siagie: OK
                var tiposdearea = JsonConvert
                    .DeserializeObject<List<Models.Certificado.TipoAreaResponse>>(statusResponse.Data.ToString());

                var response = tiposdearea.Where(x => x.idTipoArea != "003");

                result.Success = true;
                result.Data = ReactEncryptationSecurity.Encrypt(JsonConvert.SerializeObject(response));
                result.Messages.Add("Consulta exitosa");
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Data = null;
                result.Messages.Add("Se presentó un inconveniente al procesar su solicitud.");
                return result;
            }
        }

        public async Task<StatusResponse> ObtenerAnios(Models.Certificado.ParametroModel objetoEncriptado)
        {
            var result = new StatusResponse();
            var desencriptarObjeto = ReactEncryptationSecurity.Decrypt<string>(objetoEncriptado.parametro, "");
            var request = JsonConvert.DeserializeObject<Models.Certificado.GradoSeccionRequest>(desencriptarObjeto);

            try
            {
                var siagie = _encryptionServerSecurity.Decrypt<string>(
                ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", "")
                , "");

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        object>(siagie, "aniosie", request);

                var aniosIe = JsonConvert
                  .DeserializeObject<List<Models.Certificado.AnioPorIeResponse>>(statusResponse.Data.ToString());

                if (!statusResponse.Success)
                {
                    result.Success = false;
                    result.Data = null;
                    result.Messages.Add("No se logró obtener los Años por IE");
                    return result;
                }

                result.Success = true;
                result.Data = ReactEncryptationSecurity.Encrypt(JsonConvert.SerializeObject(aniosIe));
                result.Messages.Add("Años por IE, conforme");
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Data = null;
                result.Messages.Add("Se presentó un inconveniente al procesar su solicitud.");
                return result;
            }
        }

        public async Task<StatusResponse> ObtenerArea(Models.Certificado.ParametroModel objetoEncriptado)
        {
            var response = new StatusResponse();
            var desencriptarObjeto = ReactEncryptationSecurity.Decrypt<string>(objetoEncriptado.parametro, "");
            var request = JsonConvert.DeserializeObject<Models.Certificado.AreaRequest>(desencriptarObjeto);
            var nivel = ReactEncryptationSecurity.Decrypt<string>(request.nivel, "00");

            var result = await _unitOfWork.ObtenerArea(request.CodigoTipoArea, nivel);

            if (result == null || result.ToList().Count == 0)
            {
                response.Success = false;
                response.Data = null;
                response.Messages.Add("Ocurrió un problema al obtener las Areas");
                return response;
            }
            var areas = result.Select(x => AreaMapper.Map(x));
            response.Success = true;
            response.Data = ReactEncryptationSecurity.Encrypt(JsonConvert.SerializeObject(areas.ToList()));
            response.Messages.Add("Se cargaron las Areas exitosamente");

            return response;
        }

        /*public async Task<StatusResponse> ObtenerDatosInstitucionEducativa(Models.Certificado.InstitucionEducativaPorDreUgelRequest request)
        {
            var result = new StatusResponse();

            try
            {
                var siagie = _encryptionServerSecurity.Decrypt<string>(
                ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", "")
                , "");


                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        Models.Certificado.InstitucionEducativaPorDreUgelRequest>(siagie, "datosinstitucioneducativa", request);

                //Consulta de niveles en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    result.Success = false;
                    result.Data = null;
                    result.Messages.Add("No se logró obtener las Instituciones Educativas");
                    return result;
                }

                // Consulta de niveles en Siagie: OK
                var institucionList = JsonConvert
                    .DeserializeObject<List<Models.Certificado.InstitucionEducativaPorDreUgelResponse>>(statusResponse.Data.ToString());

                result.Success = true;
                result.Data = institucionList;
                result.Messages.Add("Instituciones Educativas, conforme");
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Data = null;
                result.Messages.Add("Se presentó un inconveniente al procesar su solicitud.");
                return result;
            }
        }*/

        public async Task<StatusResponse> ObtenerDatosInstitucionEducativaxCodigoModular(Models.Certificado.ParametroModel objetoEncriptado)
        {
            var result = new StatusResponse();
            var desencriptarObjeto = ReactEncryptationSecurity.Decrypt<string>(objetoEncriptado.parametro, "");
            var request = JsonConvert.DeserializeObject<Models.Certificado.InstitucionEducativaPorDreUgelRequest>(desencriptarObjeto);

            try
            {
                var siagie = _encryptionServerSecurity.Decrypt<string>(
                ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", "")
                , "");

                request.IdNivel = ReactEncryptationSecurity.Decrypt<string>(request.IdNivel, "00");
                request.NombreIE = (request.NombreIE=="" ? request.NombreIE : request.NombreIE.ToUpper());

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        Models.Certificado.InstitucionEducativaPorDreUgelRequest>(siagie, "institucioneducativa/datos", request);

                //Consulta de niveles en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    result.Success = false;
                    result.Data = null;
                    result.Messages.Add("No se logró obtener las Instituciones Educativas");
                    return result;
                }

                // Consulta de niveles en Siagie: OK
                var institucionList = JsonConvert
                    .DeserializeObject<List<Models.Certificado.InstitucionEducativaPorDreUgelResponse>>(statusResponse.Data.ToString());

                result.Success = true;
                result.Data = ReactEncryptationSecurity.Encrypt(JsonConvert.SerializeObject(institucionList));
                result.Messages.Add("Instituciones Educativas, conforme");
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Data = null;
                result.Messages.Add("Se presentó un inconveniente al procesar su solicitud.");
                return result;
            }
        }

        public async Task<StatusResponse> InsertArea(Models.Certificado.AreaRequest request)
        {
            var response = new StatusResponse();
            var rows = default(string);
            try
            {
                var nivel = ReactEncryptationSecurity.Decrypt<string>(request.nivel, "00");
                var usuario = ReactEncryptationSecurity.Decrypt<string>(request.usuario, "00");
                var modalidad = ReactEncryptationSecurity.Decrypt<string>(request.idModalidad, "00");
                _unitOfWork.BeginTransaction();

                rows = await _unitOfWork.InsertArea(request.CodigoTipoArea, nivel, request.DescripcionArea,usuario, modalidad);

                if (rows.Length > 0)
                {
                    _unitOfWork.Commit();
                    response.Success = true;
                    response.Data = rows;
                    response.Messages.Add("Se registró el área exitosamente.");
                    return response;
                }
                else
                {
                    _unitOfWork.Rollback();
                    response.Success = false;
                    response.Data = null;
                    response.Messages.Add("Ocurrió un problema");
                    return response;
                }
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                response.Success = false;
                response.Data = null;
                response.Messages.Add("Ocurrió un problema");
                return response;
            }


        }

        public async Task<StatusResponse> ObtenerGradosPorNivel(Models.Certificado.GradoSeccionRequest request)
        {
            var result = new StatusResponse();

            try
            {
                var siagie = _encryptionServerSecurity.Decrypt<string>(
                ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", "")
                , "");

                var requestDecrypted = new
                {
                    idModalidad = "01",
                    idNivel = ReactEncryptationSecurity.Decrypt<string>(request.IdNivel, "00"),
                    idPersona = 1110//Cualquier numero de ID
                };

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        object>(siagie, "pdf/grados", requestDecrypted);

                var aniosIe = JsonConvert
                  .DeserializeObject<List<Models.Certificado.GradoSeccionResponse>>(statusResponse.Data.ToString());

                if (!statusResponse.Success)
                {
                    result.Success = false;
                    result.Data = null;
                    result.Messages.Add("No se logró obtener los Años por IE");
                    return result;
                }

                result.Success = true;
                result.Data = aniosIe;
                result.Messages.Add("Años por IE, conforme");
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Data = null;
                result.Messages.Add("Se presentó un inconveniente al procesar su solicitud.");
                return result;
            }
        }

        public async Task<StatusResponse> ObtenerAniosSolicitud(Models.Certificado.ParametroModel objetoEncriptado)
        {
            var result = new StatusResponse();
            var desencriptarObjeto = ReactEncryptationSecurity.Decrypt<string>(objetoEncriptado.parametro, "");
            var request = JsonConvert.DeserializeObject<Models.Certificado.GradoSeccionRequest>(desencriptarObjeto);

            var anio = new Models.Certificado.AnioPorSolicitudResponse();

            try
            {
                string IdNivel = ReactEncryptationSecurity.Decrypt<string>(request.IdNivel, "00");
                string CodigoModular = request.CodigoModular;
                string Anexo = request.Anexo;
                string EstadoSolicitud = request.EstadoSolicitud;

                var resultado = await _unitOfWork.ObtenerAniosSolicitud(IdNivel, CodigoModular, Anexo, EstadoSolicitud);
                var response = resultado.Select(x=>new Models.Certificado.AnioPorSolicitudResponse {IdAnio=x.ANIO_CULMINACION }).ToList();

                //Consulta de niveles en Siagie: FAIL?
                if (response.Count() == 0)
                {
                    result.Success = false;
                    result.Data = null;
                    result.Messages.Add("No se logró obtener los registros solicitados.");
                    return result;
                }
                
                result.Success = true;
                result.Data = ReactEncryptationSecurity.Encrypt(JsonConvert.SerializeObject(response));
                result.Messages.Add("Solicitud exitosa.");
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Data = null;
                result.Messages.Add("Se presentó un inconveniente al procesar su solicitud.");
                return result;
            }
        }
    }
}