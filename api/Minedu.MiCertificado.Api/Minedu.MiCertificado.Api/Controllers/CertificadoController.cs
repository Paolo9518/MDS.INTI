using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minedu.Comun.Helper;
using Minedu.MiCertificado.Api.Application.Contracts.Services;
using Models = Minedu.MiCertificado.Api.BusinessLogic.Models;
using Minedu.MiCertificado.Api.Utils;
using Microsoft.AspNetCore.Http;

namespace Minedu.MiCertificado.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificadoController : ControllerBase
    {
        private readonly ICertificadoService _certificadoService;
        private readonly IReniecService _reniecService;

        public CertificadoController(ICertificadoService certificadoService,
            IReniecService reniecService)
        {
            _certificadoService = certificadoService;
            _reniecService = reniecService;
        }

        [HttpPost("consultarCertificado", Name = "PostConsultarCertificado")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostConsultarCertificado([FromBody] Models.Certificado.CertificadoModel model)
        {
            var result = await _certificadoService.ConsultarCertificado(model);

            return Ok(result);
        }

        [HttpPut("updateCertificado", Name = "PutUpdateCertificado")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutUpdateCertificado([FromBody] Models.Certificado.ParametroModel objetoEncriptado)
        {
            var result = await _certificadoService.UpdateCertificado(objetoEncriptado);

            return Ok(result);
        }

        [HttpPost("consultarDatosAcademicos", Name = "PostConsultarDatosAcademicos")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostConsultarDatosAcademicos([FromBody] Models.Certificado.ParametroModel objetoEncriptado)
        {
            var result = await _certificadoService.ConsultarDatosAcademicos(objetoEncriptado);

            return Ok(result);
        }

        [HttpPost("validarDNI", Name = "PostValidarDNI")]//NN
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostValidarDNI(Models.Certificado.PersonaModel model)
        {
            var result = await _certificadoService.ValidarDatosReniec(model.numDoc, model.idNivel);

            if (result == null)
            {
                throw new InvalidOperationException("Ocurrió un problema en la validación con RENIEC.");
            }

            return Ok(result);
        }

        [HttpPost("usuariosPorIE", Name = "PostUsuariosPorIE")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostUsuariosPorIE(Models.Certificado.ParametroModel objetoEncriptado)
        {
            var result = await _certificadoService.ObtenerUsuariosPorIE(objetoEncriptado);

            return Ok(result);
        }

        [HttpPut("updateUsuariosPorIE", Name = "PutUsuariosPorIE")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutUsuariosPorIE(Models.Certificado.ParametroModel objetoEncriptado)
        {
            var result = await _certificadoService.ActualizarUsuariosPorIE(objetoEncriptado);

            return Ok(result);
        }

        [HttpPost("insertsolicitud", Name = "PostInsertRegistroSolicitud")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostInsertRegistroSolicitud([FromBody] Models.Certificado.ParametroModel objetoEncriptado)
        {
            var response = new StatusResponse();
            var result = await _certificadoService.InsertRegistroSolicitud(objetoEncriptado);

            if (result != null)
            {
                response.Success = true;
                response.Data = result;
            }
            else
            {
                response.Success = false;
                response.Data = null;
                response.Messages.Add("Ocurrió un error al generar la solicitud");
            }

            return Ok(response);
        }

        [HttpPost("estadoInstitucion", Name = "PostInsertUpdateInstitucion")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostInsertUpdateInstitucion([FromBody] Models.Certificado.ParametroModel objetoEncriptado)
        {
            var result = await _certificadoService.InsertUpdateInstitucion(objetoEncriptado);

            return Ok(result);
        }

        [HttpPost("postInstitucion", Name = "PostInstitucion")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostInstitucion([FromBody] Models.Certificado.ParametroModel objetoEncriptado)
        {
            var result = await _certificadoService.PostInstitucion(objetoEncriptado);

            return Ok(result);
        }

        [HttpPost("consultarSolicitudRechazada", Name = "PostConsultarSolicitudRechazada")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostConsultarSolicitudRechazada([FromBody] Models.Certificado.ParametroModel objetoEncriptado)
        {
            var result = await _certificadoService.ConsultarSolicitudRechazada(objetoEncriptado);

            return Ok(result);
        }

        [HttpPost("consultarCertificadoEmitido", Name = "PostConsultarCertificadoEmitido")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostConsultarCertificadoEmitido([FromBody] Models.Certificado.ParametroModel objetoEncriptado)
        {
            var result = await _certificadoService.ConsultarCertificadosEmitidos(objetoEncriptado);

            return Ok(result);
        }

        [HttpPost("consultarSolicitudPendiente", Name = "PostConsultarSolicitudPendiente")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostConsultarSolicitudPendiente([FromBody] Models.Certificado.ParametroModel objetoEncriptado)
        {
            var result = await _certificadoService.ConsultarSolicitudPendiente(objetoEncriptado);

            return Ok(result);
        }

        [HttpPost("insertdatosacademicos", Name = "PostInsertDatosAcademicos")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostInsertDatosAcademicos([FromBody] Models.Certificado.ParametroModel objetoEncriptado)
        {
            var response = new StatusResponse();
            var result = await _certificadoService.InsertDatosAcademicos(objetoEncriptado);

            if (result.Success == true)
            {
                response.Success = true;
                response.Data = result;
            }
            else
            {
                response.Success = false;
                response.Data = null;
                response.Messages.Add("Ocurrió un error al generar los datos academicos");
            }

            return Ok(response);
        }

        [HttpPost("validarnotaspendientes", Name = "PostValdarNotasPendientes")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostValdarNotasPendientes([FromBody] Models.Certificado.ParametroModel objetoEncriptado)
        {
            var result = await _certificadoService.ValdarNotasPendientes(objetoEncriptado);

            return Ok(result);
        }

        [HttpPost("estudiantesporaniogradoseccion", Name = "PostEstudiantesPorAnioGradoSeccion")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostEstudiantesPorAnioGradoSeccion([FromBody] Models.Certificado.ParametroModel objetoEncriptado)
        {
            var resultList = await _certificadoService.EstudiantesPorAnioGradoSeccion(objetoEncriptado);

            return Ok(resultList);
        }

        [HttpPost("vistaprevia", Name = "PostPDFVistaPreviaCertificado2")]
        [Produces("application/json", Type = typeof(byte[]))]
        [RequestLimitDDOS]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostPDFVistaPreviaCertificado2([FromBody] Models.Certificado.ParametroModel objetoEncriptado)
        {
            var result = new StatusResponse();

            try
            {
                var pdf = await _certificadoService.VistaPreviaPDFCertificado(objetoEncriptado);

                if (pdf != null)
                {
                    result.Success = true;
                    result.Data = File(pdf, "application/pdf");
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
                    : "Se presentó un inconveniente al procesar su solicitud.");
            }

            return Ok(result);

        }

        [HttpPost("validarsolicitud", Name = "PostValidarSolicitud")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostValidarSolicitud([FromBody] Models.Certificado.CertificadoRequest model)
        {
            var response = new StatusResponse();
            var result = await _certificadoService.ValidarSolicitud(model);

            if (result != null)
            {
                response.Success = true;
                response.Data = result;
            }
            else
            {
                response.Success = false;
                response.Data = null;
                response.Messages.Add("Ocurrió un error al Validar la Solicitud");
            }

            return Ok(response);
        }

        [HttpPost("guardarnotas", Name = "PostPDFGenerarCertificado2")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [RequestLimitDDOS]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostPDFGenerarCertificado2([FromBody] Models.Certificado.ParametroModel objetoEncriptado)
        {
            var result = await _certificadoService.GenerarPDFCertificado(objetoEncriptado);

            return Ok(result);
        }

        [HttpPost("descargarpdf", Name = "PostValidarPDFCertificado2")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [RequestLimitDDOS]
        public async Task<IActionResult> PostValidarPDFCertificado2([FromBody] Models.Certificado.ParametroModel objetoEncriptado)
        {
            StatusResponse response = new StatusResponse();

            byte[] result;

            try
            {
                result = await _certificadoService.ValidarPDFCertificado(objetoEncriptado);

                if (result != null)
                {
                    response.Success = true;
                    response.Data = new FileContentResult(result, "application/pdf");
                    response.Messages.Add("Solicitud procesada exitosamente");
                }
                else
                {
                    response.Success = false;
                    response.Data = null;
                    response.Messages.Add("La información enviada no es correcta. Por favor asegúrese de haber enviado los datos correctos.");
                }
            }
            catch
            {
                response.Success = false;
                response.Data = null;
                response.Messages.Add("Ocurrió un error al procesar su solicitud");
            }

            return Ok(response);
        }

        [HttpPost("registromasivo", Name = "PostRegistroMasivo")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [RequestLimitDDOS]
        public async Task<IActionResult> PostRegistroMasivo([FromBody] Models.Certificado.ParametroModel objetoEncriptado)
        {
            var result = await _certificadoService.RegistroMasivoEstudiantes(objetoEncriptado);

            return Ok(result);
        }

        [HttpPost("validarDNIApoderado", Name = "PostValidarDNIApoderado")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostValidarDNIApoderado(Models.Certificado.SolicitanteModel model)
        {
            var result = await _certificadoService.ValidarDatosReniecApoderado(model.numeroDocumento);
            if (result == null)
            {
                throw new InvalidOperationException("Ocurrió un problema en la validación con RENIEC.");
            }

            return Ok(result);
        }

        [HttpPost("validararea", Name = "PostValidarArea")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostValidarArea([FromBody] Models.Certificado.AreaRequest model)
        {
            var response = new StatusResponse();
            var result = await _certificadoService.ValidarArea(model);

            if (result != null)
            {
                response.Success = true;
                response.Data = result;
            }
            else
            {
                response.Success = false;
                response.Data = null;
                response.Messages.Add("Ocurrió un error al Validar la Solicitud");
            }

            return Ok(response);
        }

        [HttpPost("obtenerareaspordisenio", Name = "PostObtenerAreasPorDisenio")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostObtenerAreasPorDisenio(Models.Certificado.AreaPorDisenioRequest request)
        {
            var result = await _certificadoService.ObtenerAreasPorDisenio(request);
            if (result == null)
            {
                throw new InvalidOperationException("Ocurrió un problema.");
            }

            return Ok(result);
        }

        [HttpPost("vistapreviacertificado", Name = "PostPDFVistaPrevia")]
        [Produces("application/json", Type = typeof(byte[]))]
        [RequestLimitDDOS]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostPDFVistaPrevia([FromBody] Models.Certificado.ParametroModel objetoEncriptado)
        {
            var result = new StatusResponse();

            try
            {
                var pdf = await _certificadoService.VistaPreviaPDF(objetoEncriptado);

                if (pdf != null)
                {
                    result.Success = true;
                    result.Data = File(pdf, "application/pdf");
                    result.Messages.Add("Constancia generada correctamente.");
                }
                else
                {
                    result.Success = false;
                    result.Data = null;
                    result.Messages.Add("La información enviada no es correcta. Por favor asegúrese de haber enviado los datos correctos.");
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Data = null;
                result.Messages.Add(ex.GetType().IsAssignableFrom(typeof(ArgumentException))
                    ? ex.Message
                    : "Se presentó un inconveniente al procesar su solicitud.");
            }

            return Ok(result);

        }

        [HttpPost("actualizarestadosolicitud", Name = "PostEstadoSolicitud")]
        [Produces("application/json", Type = typeof(byte[]))]
        [RequestLimitDDOS]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostEstadoSolicitud([FromBody] Models.Certificado.ParametroModel objetoEncriptado)
        {
            var result = await _certificadoService.UpdateEstadoCertificado(objetoEncriptado);

            return Ok(result);

        }

        [HttpPost("obtenerdatossolicitud", Name = "PostObtenerDatosSolicitud")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostObtenerDatosSolicitud(Models.Certificado.ParametroModel objetoEncriptado)
        {
            var result = await _certificadoService.ObtenerDatosSolicitud(objetoEncriptado);
            if (result == null)
            {
                throw new InvalidOperationException("Ocurrió un problema.");
            }

            return Ok(result);
        }

        [HttpPost("actualizardatosacademicos", Name = "PostUpdateDatosAcademicos")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostUpdateDatosAcademicos([FromBody] Models.Certificado.ParametroModel objetoEncriptado)
        {
            var response = new StatusResponse();
            var result = await _certificadoService.ActualizarDatosAcademicos(objetoEncriptado);

            if (result.Success == true)
            {
                response.Success = true;
                response.Data = result;
            }
            else
            {
                response.Success = false;
                response.Data = null;
                response.Messages.Add("Ocurrió un error al generar los datos academicos");
            }

            return Ok(response);
        }

        [HttpPost("obtenergradosnivelpersona", Name = "PostObtenerGradosActivos")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostObtenerGradosPorNivelPersona(Models.Certificado.ParametroModel objetoEncriptado)
        {
            var result = await _certificadoService.ObtenerGradosPorNivelPersona(objetoEncriptado);
            if (result == null)
            {
                throw new InvalidOperationException("Ocurrió un problema.");
            }

            return Ok(result);
        }

        [HttpPost("obtenerDatosEstudiante", Name = "PostObtenerDatosEstudiante")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [RequestLimitDDOS(Seconds = 10f)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostObtenerDatosEstudiante(Models.Certificado.ParametroModel objetoEncriptado)
        {//Models.Certificado.EstudiantePersonaRequest2 request
            var result = await _certificadoService.ObtenerDatosEstudiante(objetoEncriptado);
            if (result == null)
            {
                throw new InvalidOperationException("Ocurrió un problema.");
            }

            return Ok(result);
        }

        [HttpPost("registrosolicitudes", Name = "PostRegistroSolicitudes")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostRegistroSolicitudes(Models.Certificado.ParametroModel objetoEncriptado)
        {//Models.Certificado.EstudiantePersonaRequest2 request
            var result = await _certificadoService.RegistrarSolicitudes(objetoEncriptado);
            if (result == null)
            {
                throw new InvalidOperationException("Ocurrió un problema.");
            }

            return Ok(result);
        }

        [HttpPost("consultaPorNumeroDocumento", Name = "PostConsultaPorNumeroDocumento")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostConsultaDocumento(Models.Certificado.ParametroModel objetoEncriptado)
        {//Models.Certificado.EstudiantePersonaRequest2 request
            var result = await _certificadoService.ConsultaPorNumeroDocumento(objetoEncriptado);
            if (result == null)
            {
                throw new InvalidOperationException("Ocurrió un problema.");
            }

            return Ok(result);
        }

        [HttpPost("validarCertificadoEstudios", Name = "PostValidarCertificadoEstudios")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostValidarCertificadoEstudios(Models.Certificado.ParametroModel objetoEncriptado)
        {
            var result = await _certificadoService.ValidarCertificadoEstudios(objetoEncriptado);
            if (result == null)
            {
                throw new InvalidOperationException("Ocurrió un problema.");
            }

            return Ok(result);
        }

    }
}