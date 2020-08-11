using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minedu.Comun.Helper;
using Minedu.MiCertificado.Api.Application.Contracts.Services;
using Models = Minedu.MiCertificado.Api.BusinessLogic.Models;

namespace Minedu.MiCertificado.Api.Controllers
{
    //[EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class MaestroController : Controller
    {
        private readonly IMaestroService _maestroService;

        public MaestroController(IMaestroService maestroService)
        {
            _maestroService = maestroService;
        }

        #region CONSTANCIA
        // GET: api/maestro/constancia/menu
        [HttpGet("constancia/menu", Name = "GetConstanciaMenu")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        public async Task<IActionResult> GetConstanciaMenu()
        {
            var resultList = await _maestroService.ObtenerConstanciaMenu();
            return Ok(resultList);
        }

        // GET: api/maestro/constancia/declaracion-jurada
        [HttpGet("constancia/declaracion-jurada", Name = "GetConstanciaDeclaracionJurada")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        public async Task<IActionResult> GetConstanciaDeclaracionJurada()
        {
            var resultList = await _maestroService.ObtenerConstanciaDeclaracionJurada();
            return Ok(resultList);
        }

        // GET: api/maestro/constancia/motivo
        [HttpGet("constancia/motivo", Name = "GetConstanciaMotivos")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetConstanciaMotivos()
        {
            var resultList = await _maestroService.ObtenerConstanciaMotivos();
            return Ok(resultList);
        }

        // GET: api/maestro/constancia/modalidad
        [HttpGet("constancia/modalidad", Name = "GetConstanciaModalidades")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetConstanciaModalidades()
        {
            var resultList = await _maestroService.ObtenerConstanciaModalidades();
            return Ok(resultList);
        }
        #endregion CONSTANCIA


        // GET: api/maestro/reniec/departamento
        [HttpGet("reniec/departamento", Name = "GetReniecDepartamentos")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetReniecDepartamentos()
        {
            var resultList = await _maestroService.ObtenerReniecDepartamentos();
            return Ok(resultList);
        }

        // POST: api/maestro/reniec/provincia
        [HttpPost("reniec/provincia", Name = "PostReniecProvincias")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostReniecProvincias([FromBody] Models.DepartamentoRequest request)
        {
            var resultList = await _maestroService.ObtenerReniecProvincias(request);
            return Ok(resultList);
        }

        // POST: api/maestro/reniec/distrito
        [HttpPost("reniec/distrito", Name = "PostReniecDistritos")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostReniecDistritos([FromBody] Models.ProvinciaRequest request)
        {
            var resultList = await _maestroService.ObtenerReniecDistritos(request);
            return Ok(resultList);
        }

        // GET: api/maestro/siagie/departamento
        [HttpGet("siagie/departamento", Name = "GetSiagieDepartamentos")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetSiagieDepartamentos()
        {
            var resultList = await _maestroService.ObtenerSiagieDepartamentos();

            return Ok(resultList);
        }

        // POST: api/maestro/siagie/provincia
        [HttpPost("siagie/provincia", Name = "PostSiagieProvincias")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostSiagieProvincias([FromBody] Models.DepartamentoRequest request)
        {
            var resultList = await _maestroService.ObtenerSiagieProvincias(request);

            return Ok(resultList);
        }

        // POST: api/maestro/siagie/distrito
        [HttpPost("siagie/distrito", Name = "PostSiagieDistritos")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostSiagieDistritos([FromBody] Models.ProvinciaRequest request)
        {
            var resultList = await _maestroService.ObtenerSiagieDistritos(request);

            return Ok(resultList);
        }

        #region CERTIFICADO_PÚBLICO
        // GET: api/maestro/certificado/menu
        [HttpGet("certificado/menu", Name = "GetCertificadoMenus")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        public async Task<IActionResult> GetCertificadoMenus()
        {
            var resultList = await _maestroService.ObtenerCertificadoMenus();

            return Ok(resultList);
        }

        // GET: api/maestro/certificado/declaracion-jurada
        [HttpGet("certificado/declaracion-jurada", Name = "GetCertificadoDJ")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        public async Task<IActionResult> GetCertificadoDJ()
        {
            var resultList = await _maestroService.ObtenerCertificadoDeclaracionJurada();

            return Ok(resultList);
        }

        // GET: api/maestro/certificado/modalidad
        [HttpGet("certificado/modalidad", Name = "GetCertificadoModalidades")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCertificadoModalidades()
        {
            var resultList = await _maestroService.ObtenerCertificadoModalidades();

            return Ok(resultList);
        }


        // GET: api/maestro/certificado/motivo
        [HttpGet("certificado/motivo", Name = "GetCertificadoMotivos")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCertificadoMotivos()
        {
            var resultList = await _maestroService.ObtenerCertificadoMotivos();

            return Ok(resultList);
        }

        // POST: api/maestro/certificado/grado
        [HttpPost("certificado/grado", Name = "GetCertificadoGrado")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCertificadoGrado([FromBody] Models.ModalidadNivelRequest encryptedRequest)
        {
            var resultList = await _maestroService.ObtenerCertificadoGrados(encryptedRequest);

            return Ok(resultList);
        }

        #endregion CERTIFICADO_PÚBLICO

        //OK-S
        // GET: api/maestro/certificado/dre
        [HttpGet("dre", Name = "GetDRE")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetDRE()
        {
            var resultList = await _maestroService.ObtenerDRE();

            return Ok(resultList);
        }

        //OK-S
        // POST: api/maestro/certificado/ugel
        [HttpPost("ugel", Name = "PostUGEL")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostUGEL([FromBody] Models.Certificado.UgelRequest modelRequest)
        {
            var resultList = await _maestroService.ObtenerUGEL(modelRequest);

            return Ok(resultList);
        }
    }
}
