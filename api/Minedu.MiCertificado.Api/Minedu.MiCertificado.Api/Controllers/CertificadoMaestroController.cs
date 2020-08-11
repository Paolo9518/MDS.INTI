using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minedu.Comun.Helper;
using Minedu.MiCertificado.Api.Application.Contracts.Services;
using Models = Minedu.MiCertificado.Api.BusinessLogic.Models;

namespace Minedu.MiCertificado.Api.Controllers
{
    [Route("api/certificado/maestro")]
    [ApiController]
    public class CertificadoMaestroController : ControllerBase
    {
        private readonly ICertificadoMaestroService _certificadoMaestroService;

        public CertificadoMaestroController(ICertificadoMaestroService certificadoMaestroService)
        {
            _certificadoMaestroService = certificadoMaestroService;
        }
        //OK-S
        // GET: api/certificado/maestro/gradoseccion
        [HttpPost("gradoseccion", Name = "GetGradoSeccion")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetGradoSeccion([FromBody] Models.Certificado.ParametroModel objetoEncriptado)
        {
            var resultList = await _certificadoMaestroService.ObtenerGradoSeccion(objetoEncriptado);

            return Ok(resultList);
        }

        //OK-S
        // POST: api/certificado/maestro/institucioneducativa
        [HttpPost("institucioneducativa", Name = "PostInstitucionEducativa")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostInstitucionEducativa([FromBody] Models.Certificado.InstitucionEducativaRequest modelRequest)
        {
            var resultList = await _certificadoMaestroService.ObtenerInstitucionEducativa(modelRequest);

            return Ok(resultList);
        }

        //OK-S
        // GET: api/certificado/maestro/tipoarea
        [HttpGet("tipoarea", Name = "GetTipoDeArea")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetTipoDeArea()
        {
            var resultList = await _certificadoMaestroService.ObtenerTipoDeArea();

            return Ok(resultList);
        }

        //OK-S
        // POST: api/certificado/maestro/anioporie
        [HttpPost("anioporie", Name = "GetAnio")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAnios([FromBody] Models.Certificado.ParametroModel objetoEncriptado)
        {
            var resultList = await _certificadoMaestroService.ObtenerAnios(objetoEncriptado);

            return Ok(resultList);
        }

        //OK-C
        // GET: api/certificado/maestro/areaportipoarea
        [HttpPost("areaportipoarea", Name = "GetArea")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetArea([FromBody] Models.Certificado.ParametroModel objetoEncriptado)
        {
            var resultList = await _certificadoMaestroService.ObtenerArea(objetoEncriptado);

            return Ok(resultList);
        }

        //OK-S
        // POST: api/certificado/maestro/institucioneducativa/datos
        [HttpPost("institucioneducativa/datos", Name = "GetInstitucionEducativa")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetInstitucionEducativa([FromBody] Models.Certificado.ParametroModel objetoEncriptado)
        {
            var resultList = await _certificadoMaestroService.ObtenerDatosInstitucionEducativaxCodigoModular(objetoEncriptado);

            return Ok(resultList);
        }

        //OK-C
        // POST: api/certificado/maestro/insertararea
        [HttpPost("insertararea", Name = "postArea")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostArea([FromBody] Models.Certificado.AreaRequest modelRequest)
        {
            var resultList = await _certificadoMaestroService.InsertArea(modelRequest);

            return Ok(resultList);
        }
        //OK-S (RE-UTILIZADO)?
        // POST: api/certificado/maestro/gradospornivel
        [HttpPost("gradospornivel", Name = "GetGradosPorNivel")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetGradosPorNivel([FromBody] Models.Certificado.GradoSeccionRequest modelRequest)
        {
            var resultList = await _certificadoMaestroService.ObtenerGradosPorNivel(modelRequest);

            return Ok(resultList);
        }
        //OK-C
        // POST: api/certificado/maestro/aniosporsolicitud
        [HttpPost("aniosporsolicitud", Name = "GetAnioSolicitud")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAnioSolicitud([FromBody] Models.Certificado.ParametroModel objetoEncriptado)
        {
            var resultList = await _certificadoMaestroService.ObtenerAniosSolicitud(objetoEncriptado);

            return Ok(resultList);
        }
    }
}