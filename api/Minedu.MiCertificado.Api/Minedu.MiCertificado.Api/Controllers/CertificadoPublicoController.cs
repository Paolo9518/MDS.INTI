using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minedu.Comun.Helper;
using Minedu.MiCertificado.Api.Application.Contracts.Services;
using Minedu.MiCertificado.Api.Utils;
using Models = Minedu.MiCertificado.Api.BusinessLogic.Models;

namespace Minedu.MiCertificado.Api.Controllers
{
    [Route("api/certificado/publico")]
    [ApiController]
    public class CertificadoPublicoController : ControllerBase
    {
        private readonly ICertificadoPublicoService _certificadoPublicoService;

        public CertificadoPublicoController(ICertificadoPublicoService certificadoPublicoService)
        {
            _certificadoPublicoService = certificadoPublicoService;
        }

        // Token
        // GET: api/certificado/publico/token
        [HttpGet("token", Name = "PostToken")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        public async Task<IActionResult> PostToken()
        {
            var result = await _certificadoPublicoService.ObtenerToken();

            return Ok(result);
        }

        // IIEE
        // POST: api/certificado/publico/iiee
        [HttpPost("iiee", Name = "PostSiagieIIEE")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostSiagieIIEE([FromBody] Models.Siagie.ColegioRequest request)
        {
            var resultList = await _certificadoPublicoService.ObtenerIIEE(request);

            return Ok(resultList);
        }

        // Validar DJ
        // POST: api/certificado/publico/declaracion-jurada/validar
        [HttpPost("declaracion-jurada/validar", Name = "PostCertificadoValidarDJ")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        public IActionResult PostCertificadoValidarDJ([FromBody] Models.Constancia.DeclaracionRequest encryptedRequest)
        {
            var result = _certificadoPublicoService.ValidarDJ(encryptedRequest);

            return Ok(result);
        }

        // Validar Apoderado
        // POST: api/certificado/publico/apoderado/validar
        [HttpPost("apoderado/validar", Name = "PostCertificadoApoderadoValidar")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostCertificadoApoderadoValidar([FromBody] Models.Certificado.ApoderadoPersonaModularRequest encryptedRequest)
        {
            var result = await _certificadoPublicoService.ValidarApoderado(encryptedRequest);

            return Ok(result);
        }

        // Validar Estudiante
        // POST: api/certificado/publico/estudiante/validar
        [HttpPost("estudiante/validar", Name = "PostCertificadoEstudianteValidar")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostCertificadoEstudianteValidar([FromBody] Models.Certificado.EstudiantePersonaModularRequest encryptedRequest)
        {
            var result = await _certificadoPublicoService.ValidarEstudiante(encryptedRequest);

            return Ok(result);
        }

        // Obtener info de último colegio que cursó v2
        // POST: api/certificado/publico/estudiante/colegio
        [HttpPost("estudiante/colegio", Name = "PostCertificadoEstudianteUltimoColegio")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostCertificadoEstudianteUltimoColegio([FromBody] Models.Certificado.EstudianteModalidadNivelModularRequest encryptedRequest)
        {
            var result = await _certificadoPublicoService.ObtenerUltimoColegioEstudiante(encryptedRequest);

            return Ok(result);
        }

        // Obtener última solicitud de certificado realizado por el estudiante
        // POST: api/certificado/publico/estudiante/solicitud
        [HttpPost("estudiante/solicitud", Name = "PostSolicitudesPendientesEstudiante")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostSolicitudesPendientesEstudiante([FromBody] Models.Certificado.PersonaModalidadNivelRequest encryptedRequest)
        {
            var result = await _certificadoPublicoService.ObtenerSolicitudesPendientesEstudiante(encryptedRequest);

            return Ok(result);
        }

        // Obtener Vista Previa del PDF
        // POST: api/certificado/publico/pdf/vistaprevia
        [HttpPost("pdf/vistaprevia", Name = "PostPDFVistaPreviaCertificado")]
        [Produces("application/json", Type = typeof(byte[]))]
        [RequestLimitDDOS]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostPDFVistaPreviaCertificado([FromBody] Models.Constancia.EstudianteModalidadNivelPersonaRequest encryptedRequest)
        {
            var result = new StatusResponse();

            try
            {
                var pdf = await _certificadoPublicoService.VistaPreviaPDFCertificado(encryptedRequest);

                if (pdf != null)
                {
                    result.Success = true;
                    result.Data = File(pdf, "application/pdf");
                    result.Messages.Add("Certificado generada correctamente.");
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

        // Validar Notas
        // POST: api/certificado/publico/estudiante/nota
        [HttpPost("estudiante/notas", Name = "PostEstudianteNotas")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostEstudianteNotas([FromBody] Models.Constancia.EstudianteModalidadNivelRequest encryptedRequest)
        {
            var result = await _certificadoPublicoService.ValidarEstudianteNotas(encryptedRequest);

            return Ok(result);
        }

        // Generar PDF de Certificado de Estudios
        // POST: api/certificado/publico/pdf/generar
        [HttpPost("pdf/generar", Name = "PostPDFGenerarCertificado")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [RequestLimitDDOS]
        [TokenAutenticationAtribute]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostPDFGenerarCertificado([FromBody] Models.Certificado.SolicitudCertificadoRequest encryptedRequest)
        {
            var result = await _certificadoPublicoService.GenerarPDFCertificado(encryptedRequest);

            return Ok(result);
        }

        byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        // Descargar PDF
        // POST: api/pdf/descargar
        [HttpPost("pdf/descargar", Name = "PostDescargarPDFCertificado")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [RequestLimitDDOS]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostDescargarPDFCertificado([FromBody] Models.Constancia.DescargaRequest encryptedRequest)
        {
            var result = await _certificadoPublicoService.DescargarPDFCertificado(encryptedRequest);
            if (result.Success)
            {
                result.Data = File(ObjectToByteArray(result.Data), "application/pdf");
            }
            return Ok(result);
        }

        // Validar PDF
        // POST: api/pdf/validar
        [HttpPost("pdf/validar", Name = "PostValidarPDFCertificado")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [RequestLimitDDOS]
        public async Task<IActionResult> PostValidarPDFCertificado([FromBody] Models.Certificado.VerificacionCertificadoRequest encryptedRequest)
        {
            var result = await _certificadoPublicoService.ValidarPDFCertificado(encryptedRequest);
            if (result.Success)
            {
                result.Data = File(ObjectToByteArray(result.Data), "application/pdf");
            }
            return Ok(result);
        }
    }
}