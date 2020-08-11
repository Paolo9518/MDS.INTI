using Minedu.Comun.Helper;
using MDS.Inventario.Api.Application.Contracts.Services;
using MDS.Inventario.Api.Application.Mappers.Constancia;
using Models = MDS.Inventario.Api.Application.Entities.Models;
using MDS.Inventario.Api.DataAccess.Contracts.UnitOfWork;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MDS.Inventario.Api.Application.Contracts.Security;
using Microsoft.AspNetCore.Http;
using MDS.Inventario.Api.Application.Utils;

namespace MDS.Inventario.Api.Application.Services
{
    public class MaestroService : IMaestroService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISiagieService _siagieService;
        private readonly IEncryptionServerSecurity _encryptionServerSecurity;
        public readonly IHttpContextAccessor _httpContextAccessor;

        public MaestroService(
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

        #region CONSTANCIA
        public async Task<StatusResponse> ObtenerConstanciaMenu()
        {
            var response = new StatusResponse();

            var result = await _unitOfWork.ObtenerMenu(MenuMapper.Map(new Models.Constancia.MenuModel(), 
                _encryptionServerSecurity));

            if (result == null || result.ToList().Count == 0)
            {
                response.Success = false;
                response.Data = null;
                response.Messages.Add("Ocurrió un problema al obtener los Menus");
                return response;
            }

            response.Success = true;
            response.Data = result.Select(x => MenuMapper.Map(x, _encryptionServerSecurity));
            response.Messages.Add("Se cargaron los Menus exitosamente");

            return response;
        }

        public async Task<StatusResponse> ObtenerConstanciaDeclaracionJurada()
        {
            var response = new StatusResponse();

            var result = await _unitOfWork.ObtenerDeclaracionJurada();

            if (result == null || result.ToList().Count == 0)
            {
                response.Success = false;
                response.Data = null;
                response.Messages.Add("Ocurrió un problema al obtener la DJ");
                return response;
            }

            response.Success = true;
            response.Data = result.Select(x => DeclaracionJuradaMapper.Map(x, _encryptionServerSecurity)).FirstOrDefault();
            response.Messages.Add("Se cargó la DJ exitosamente");

            return response;
        }

        public async Task<StatusResponse> ObtenerConstanciaMotivos()
        {
            var response = new StatusResponse();

            var result = await _unitOfWork.ObtenerMotivo(MotivoMapper.Map(new Models.Constancia.MotivoModel(), _encryptionServerSecurity));

            if (result == null || result.ToList().Count == 0)
            {
                response.Success = false;
                response.Data = null;
                response.Messages.Add("Ocurrió un problema al obtener los Motivos");
                return response;
            }

            response.Success = true;
            response.Data = result.Select(x => MotivoMapper.Map(x, _encryptionServerSecurity));
            response.Messages.Add("Se cargaron los Motivos exitosamente");

            return response;
        }

        public async Task<StatusResponse> ObtenerConstanciaModalidades()
        {
            var result = new StatusResponse();

            try
            {
                var siagie = _encryptionServerSecurity.Decrypt<string>(
                ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", "")
                , "");

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        object>(siagie, "modalidades", null);

                //Consulta de niveles en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    result.Success = false;
                    result.Data = null;
                    result.Messages.Add("No se logró obtener los datos de las modalidades");
                    return result;
                }

                // Consulta de niveles en Siagie: OK
                var estudianteModalidades = JsonConvert
                    .DeserializeObject<List<Models.Siagie.EstudianteModalidadesResponse>>(statusResponse.Data.ToString());

                result.Success = true;
                result.Data = estudianteModalidades.Select(x => new
                {
                    idModalidad = _encryptionServerSecurity.Encrypt(x.idModalidad.ToString()),
                    descripcionModalidad = x.descripcionModalidad
                }).ToList();
                result.Messages.Add("Modalidades, conforme");
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
        #endregion CONSTANCIA

        public async Task<StatusResponse> ObtenerReniecDepartamentos()
        {
            var result = new StatusResponse();

            try
            {
                var siagie = _encryptionServerSecurity.Decrypt<string>(
                ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", "")
                , "");

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        object>(siagie, "departamentos", null);

                //Consulta de niveles en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    result.Success = false;
                    result.Data = null;
                    result.Messages.Add("No se logró obtener los departamentos");
                    return result;
                }

                // Consulta de niveles en Siagie: OK
                var estudianteModalidades = JsonConvert
                    .DeserializeObject<List<Models.UbigeoResponse>>(statusResponse.Data.ToString());

                result.Success = true;
                result.Data = estudianteModalidades;
                result.Messages.Add("Modalidades, conforme");
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

        public async Task<StatusResponse> ObtenerReniecProvincias(Models.DepartamentoRequest request)
        {
            var result = new StatusResponse();

            try
            {
                var siagie = _encryptionServerSecurity.Decrypt<string>(
                ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", "")
                , "");

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        Models.DepartamentoRequest>(siagie, "provincias", request);

                //Consulta de niveles en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    result.Success = false;
                    result.Data = null;
                    result.Messages.Add("No se logró obtener las provincias");
                    return result;
                }

                // Consulta de niveles en Siagie: OK
                var ubigeoList = JsonConvert
                    .DeserializeObject<List<Models.UbigeoResponse>>(statusResponse.Data.ToString());

                result.Success = true;
                result.Data = ubigeoList;
                result.Messages.Add("Provincias, conforme");
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

        public async Task<StatusResponse> ObtenerReniecDistritos(Models.ProvinciaRequest request)
        {
            var result = new StatusResponse();

            try
            {
                var siagie = _encryptionServerSecurity.Decrypt<string>(
                ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", "")
                , "");

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        Models.ProvinciaRequest>(siagie, "distritos", request);

                //Consulta de niveles en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    result.Success = false;
                    result.Data = null;
                    result.Messages.Add("No se logró obtener las provincias");
                    return result;
                }

                // Consulta de niveles en Siagie: OK
                var ubigeoList = JsonConvert
                    .DeserializeObject<List<Models.UbigeoResponse>>(statusResponse.Data.ToString());

                result.Success = true;
                result.Data = ubigeoList;
                result.Messages.Add("Distritos, conforme");
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


        public async Task<StatusResponse> ObtenerSiagieDepartamentos()
        {
            var result = new StatusResponse();

            try
            {
                var siagie = _encryptionServerSecurity.Decrypt<string>(
                ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", "")
                , "");

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        object>(siagie, "departamentos/siagie", null);

                //Consulta de niveles en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    result.Success = false;
                    result.Data = null;
                    result.Messages.Add("No se logró obtener los departamentos");
                    return result;
                }

                // Consulta de niveles en Siagie: OK
                var estudianteModalidades = JsonConvert
                    .DeserializeObject<List<Models.UbigeoResponse>>(statusResponse.Data.ToString());

                result.Success = true;
                result.Data = estudianteModalidades;
                result.Messages.Add("Modalidades, conforme");
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

        public async Task<StatusResponse> ObtenerSiagieProvincias(Models.DepartamentoRequest request)
        {
            var result = new StatusResponse();

            try
            {
                var siagie = _encryptionServerSecurity.Decrypt<string>(
                ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", "")
                , "");

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        Models.DepartamentoRequest>(siagie, "provincias/siagie", request);

                //Consulta de niveles en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    result.Success = false;
                    result.Data = null;
                    result.Messages.Add("No se logró obtener las provincias");
                    return result;
                }

                // Consulta de niveles en Siagie: OK
                var ubigeoList = JsonConvert
                    .DeserializeObject<List<Models.UbigeoResponse>>(statusResponse.Data.ToString());

                result.Success = true;
                result.Data = ubigeoList;
                result.Messages.Add("Provincias, conforme");
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

        public async Task<StatusResponse> ObtenerSiagieDistritos(Models.ProvinciaRequest request)
        {
            var result = new StatusResponse();

            try
            {
                var siagie = _encryptionServerSecurity.Decrypt<string>(
                ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", "")
                , "");

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        Models.ProvinciaRequest>(siagie, "distritos/siagie", request);

                //Consulta de niveles en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    result.Success = false;
                    result.Data = null;
                    result.Messages.Add("No se logró obtener las provincias");
                    return result;
                }

                // Consulta de niveles en Siagie: OK
                var ubigeoList = JsonConvert
                    .DeserializeObject<List<Models.UbigeoResponse>>(statusResponse.Data.ToString());

                result.Success = true;
                result.Data = ubigeoList;
                result.Messages.Add("Distritos, conforme");
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

        #region CERTIFICADO_PÚBLICO
        public async Task<StatusResponse> ObtenerCertificadoMenus()
        {
            var response = new StatusResponse();

            var result = await _unitOfWork.ObtenerCertificadoMenu(Mappers.Certificado.MenuMapper.Map(new Models.Certificado.MenuCertificadoModel(),
                _encryptionServerSecurity));

            if (result == null || result.ToList().Count == 0)
            {
                response.Success = false;
                response.Data = null;
                response.Messages.Add("Ocurrió un problema al obtener los Menus");
                return response;
            }

            response.Success = true;
            response.Data = result.Select(x => Mappers.Certificado.MenuMapper.Map(x, _encryptionServerSecurity));
            response.Messages.Add("Se cargaron los Menus exitosamente");

            return response;
        }

        public async Task<StatusResponse> ObtenerCertificadoDeclaracionJurada()
        {
            var response = new StatusResponse();

            var result = await _unitOfWork.ObtenerCertificadoDeclaracionJurada();

            if (result == null || result.ToList().Count == 0)
            {
                response.Success = false;
                response.Data = null;
                response.Messages.Add("Ocurrió un problema al obtener la DJ");
                return response;
            }

            response.Success = true;
            response.Data = result.Select(x => Mappers.Certificado.DeclaracionJuradaMapper.Map(x, _encryptionServerSecurity)).FirstOrDefault();
            response.Messages.Add("Se cargó la DJ exitosamente");

            return response;
        }

        //FALTA VERIFICAR QUE JALE TODAS LAS MODALIDADES
        public async Task<StatusResponse> ObtenerCertificadoModalidades()
        {
            var result = new StatusResponse();

            try
            {
                var siagie = _encryptionServerSecurity.Decrypt<string>(
                ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", "")
                , "");

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        object>(siagie, "modalidades", null);

                //Consulta de niveles en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    result.Success = false;
                    result.Data = null;
                    result.Messages.Add("No se logró obtener los datos de las modalidades");
                    return result;
                }

                // Consulta de niveles en Siagie: OK
                var estudianteModalidades = JsonConvert
                    .DeserializeObject<List<Models.Siagie.EstudianteModalidadesResponse>>(statusResponse.Data.ToString());

                result.Success = true;
                result.Data = estudianteModalidades.Select(x => new
                {
                    idModalidad = _encryptionServerSecurity.Encrypt(x.idModalidad.ToString()),
                    descripcionModalidad = x.descripcionModalidad
                }).ToList();
                result.Messages.Add("Modalidades, conforme");
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

        public async Task<StatusResponse> ObtenerCertificadoMotivos()
        {
            var response = new StatusResponse();

            var result = await _unitOfWork.ObtenerCertificadoMotivo(Mappers.Certificado.MotivoMapper.Map(new Models.Certificado.MotivoCertificadoModel(), _encryptionServerSecurity));

            if (result == null || result.ToList().Count == 0)
            {
                response.Success = false;
                response.Data = null;
                response.Messages.Add("Ocurrió un problema al obtener los Motivos");
                return response;
            }

            response.Success = true;
            response.Data = result.Select(x => Mappers.Certificado.MotivoMapper.Map(x, _encryptionServerSecurity));
            response.Messages.Add("Se cargaron los Motivos exitosamente");

            return response;
        }
        
        //TODOS LOS GRADOS X NIVEL
        public async Task<StatusResponse> ObtenerCertificadoGrados(Models.ModalidadNivelRequest encryptedRequest)
        {
            var result = new StatusResponse();

            try
            {
                var request = new Models.ModalidadNivelRequest()
                {
                    idModalidad = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.idModalidad, ""),
                    idNivel = _encryptionServerSecurity.Decrypt<string>(encryptedRequest.idNivel, "")
                };

                if (request.idModalidad.Length != 2
                    || request.idNivel.Length != 2)
                {
                    throw new ArgumentException("Se presentó un inconveniente obtener la relación de grados.");
                }

                var siagie = _encryptionServerSecurity.Decrypt<string>(
                ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", "")
                , "");

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                         Models.ModalidadNivelRequest>(siagie, "grado", request);

                //Consulta de grados en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    result.Success = false;
                    result.Data = null;
                    result.Messages.Add("No se logró obtener los datos de los grados");
                    return result;
                }

                // Consulta de niveles en Siagie: OK
                var grados = JsonConvert
                    .DeserializeObject<List<Models.Siagie.GradosResponse>>(statusResponse.Data.ToString());

                result.Success = true;
                result.Data = grados.Select(x => new
                {
                    idGrado = _encryptionServerSecurity.Encrypt(x.idGrado.ToString()),
                    x.descripcionGrado
                }).ToList();
                result.Messages.Add("Modalidades, conforme");
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
        #endregion CERTIFICADO_PÚBLICO

        public async Task<StatusResponse> ObtenerDRE()
        {
            var result = new StatusResponse();

            try
            {
                var siagie = _encryptionServerSecurity.Decrypt<string>(
                ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", "")
                , "");

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        object>(siagie, "dre", null);

                //Consulta de niveles en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    result.Success = false;
                    result.Data = null;
                    result.Messages.Add("No se logró obtener las DRE");
                    return result;
                }

                // Consulta de niveles en Siagie: OK
                var ubigeoList = JsonConvert
                    .DeserializeObject<List<Models.Certificado.DreDatosGenerales>>(statusResponse.Data.ToString());

                result.Success = true;
                result.Data = ubigeoList;
                result.Messages.Add("Dres, conforme");
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

        public async Task<StatusResponse> ObtenerUGEL(Models.Certificado.UgelRequest request)
        {
            var result = new StatusResponse();

            try
            {
                var siagie = _encryptionServerSecurity.Decrypt<string>(
                ReadRequest.getKeyValue<string>(_httpContextAccessor, "siagie", "")
                , "");

                var statusResponse = await _siagieService
                        .GetServiceByQueryAndToken<StatusResponse,
                        Models.Certificado.UgelRequest>(siagie, "ugel", request);

                //Consulta de niveles en Siagie: FAIL?
                if (!statusResponse.Success)
                {
                    result.Success = false;
                    result.Data = null;
                    result.Messages.Add("No se logró obtener las UGELes");
                    return result;
                }

                // Consulta de niveles en Siagie: OK
                var ubigeoList = JsonConvert
                    .DeserializeObject<List<Models.Certificado.UgelDatosGenerales>>(statusResponse.Data.ToString());

                result.Success = true;
                result.Data = ubigeoList;
                result.Messages.Add("Ugeles, conforme");
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
