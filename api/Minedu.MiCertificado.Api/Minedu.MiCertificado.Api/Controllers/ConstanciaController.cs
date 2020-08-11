using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minedu.Comun.Helper;
using Minedu.MiCertificado.Api.Application.Contracts.Services;
using Minedu.MiCertificado.Api.CrossCutting;
using Minedu.MiCertificado.Api.Utils;
using System;
using System.Threading.Tasks;
using Models = Minedu.MiCertificado.Api.BusinessLogic.Models;

namespace Minedu.MiCertificado.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConstanciaController : ControllerBase
    {
        private readonly IConstanciaService _constanciaService;

        public ConstanciaController(IConstanciaService constanciaService)
        {
            _constanciaService = constanciaService;
        }

        // Validar DJ
        // POST: api/constancia/declaracion-jurada/validar
        [HttpPost("declaracion-jurada/validar", Name = "PostValidarDeclaracion")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        public async Task<IActionResult> PostValidarDeclaracion([FromBody] Models.Constancia.DeclaracionRequest encryptedRequest)
        {
            var result = await _constanciaService.ValidarDJ(encryptedRequest);
            return Ok(result);
        }

        // Validar Apoderado
        // POST: api/constancia/apoderado/validar
        [HttpPost("apoderado/validar", Name = "PostApoderadoValidar")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostApoderadoValidar([FromBody] Models.Constancia.ApoderadoPersonaRequest encryptedRequest)
        {
            var result = await _constanciaService.ValidarApoderado(encryptedRequest);
            return Ok(result);
        }

        // Validar Estudiante
        // POST: api/constancia/estudiante/validar
        [HttpPost("estudiante/validar", Name = "PostEstudianteValidar")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostEstudianteValidar([FromBody] Models.Constancia.EstudiantePersonaRequest encryptedRequest)
        {
            var result = await _constanciaService.ValidarEstudiante(encryptedRequest);
            return Ok(result);
        }

        // Obtener Niveles de Estudiante
        // POST: api/constancia/estudiante/nivel
        [HttpPost("estudiante/nivel", Name = "PostEstudianteNiveles")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostEstudianteNiveles([FromBody] Models.Constancia.EstudianteModalidadRequest encryptedRequest)
        {
            var result = await _constanciaService.ObtenerNivelesEstudiante(encryptedRequest);
            return Ok(result);
        }

        // Obtener info de último colegio que cursó v2
        // POST: api/constancia/estudiante/colegio
        [HttpPost("estudiante/colegio", Name = "PostEstudianteUltimoColegioV2")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostEstudianteUltimoColegioV2([FromBody] Models.Constancia.EstudianteModalidadNivelRequest encryptedRequest)
        {
            var result = await _constanciaService.ObtenerUltimoColegioEstudianteV2(encryptedRequest);
            return Ok(result);
        }

        // Obtener última solicitud de constancia realizada por el estudiante
        // POST: api/constancia/estudiante/solicitud
        [HttpPost("estudiante/solicitud", Name = "PostEstudianteUltimaSolicitud")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostEstudianteUltimaSolicitud([FromBody] Models.Constancia.EstudianteModalidadNivelRequest encryptedRequest)
        {
            var result = await _constanciaService.ObtenerUltimaSolicitudEstudiante(encryptedRequest);
            return Ok(result);
        }

        // Obtener Vista Previa del PDF
        // POST: api/constancia/pdf/vistaprevia
        [HttpPost("pdf/vistaprevia", Name = "PostPDFVistaPreviaConstancia")]
        [Produces("application/json", Type = typeof(byte[]))]
        [RequestLimitDDOS]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostPDFVistaPreviaConstancia([FromBody] Models.Constancia.EstudianteModalidadNivelPersonaRequest encryptedRequest)
        {
            var result = new StatusResponse();

            try
            {
                var pdf = await _constanciaService.VistaPreviaPDFConstancia(encryptedRequest);

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

        // Generar PDF de Constancia de Logros de Aprendizaje
        // POST: api/constancia/pdf/generar
        [HttpPost("pdf/generar", Name = "PostPDFGenerarConstancia")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [RequestLimitDDOS]
        [TokenAutenticationAtribute]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostPDFGenerarConstancia([FromBody] Models.Constancia.SolicitudRequest encryptedRequest)
        {
            var result = await _constanciaService.GenerarPDFCostancia(encryptedRequest);
            return Ok(result);
        }
        
        // Validar PDF
        // POST: api/pdf/descargar
        [HttpPost("pdf/descargar", Name = "PostDescargarPDFConstancia")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [RequestLimitDDOS]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostDescargarPDFConstancia([FromBody] Models.Constancia.DescargaRequest encryptedRequest)
        {
            var result = await _constanciaService.DescargarPDFConstancia(encryptedRequest);
            if (result.Success)
            {
                result.Data = File(Tools.ConvertToBytes(result.Data), "application/pdf");
            }
            return Ok(result);
        }

        // Validar PDF
        // POST: api/pdf/validar
        [HttpPost("pdf/validar", Name = "PostValidarPDFConstancia")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [RequestLimitDDOS]
        public async Task<IActionResult> PostValidarPDFConstancia([FromBody] Models.Constancia.VerificacionRequest encryptedRequest)
        {
            var result = await _constanciaService.ValidarPDFConstancia(encryptedRequest);
            if (result.Success)
            {
                result.Data = File(Tools.ConvertToBytes(result.Data), "application/pdf");
            }
            return Ok(result);
        }
    }
}