using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minedu.Comun.Helper;
using MDS.Inventario.Api.Application.Contracts.Services;
using MDS.Inventario.Api.Utils;
using Models = MDS.Inventario.Api.Application.Entities.Models;

namespace MDS.Inventario.Api.Controllers
{
    //[EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        //OK-S
        // POST: api/auth/auth
        [HttpPost("auth", Name = "GetLogin")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        public async Task<IActionResult> GetLogin([FromBody] Models.Certificado.AuthModel encryptedRequest)
        {
            var response = await _authService.ObtenerUsuario(encryptedRequest);

            return Ok(response);
        }

        //OK-S
        // POST: api/auth/rol/centralizado
        [HttpPost("rol/centralizado", Name = "PostRolCentralizado")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostRolCentralizado([FromBody] Models.Certificado.RolCentralizadoRequest encryptedRequest)
        {
            var response = await _authService.ObtenerRolCentralizado(encryptedRequest);

            return Ok(response);
        }

        //OK
        // POST: api/auth/rol/descentralizado
        [HttpPost("rol/descentralizado", Name = "PostRolesDescentralizados")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostRolesDescentralizados([FromBody] Models.Certificado.RolDescentralizadoRequest encryptedRequest)
        {
            var response = await _authService.ObtenerRolesDescentralizados(encryptedRequest);

            return Ok(response);
        }

        //OK-C
        // POST: api/auth/rol/usuario
        /*[HttpPost("rol/usuario", Name = "PostRolPorModular")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostRolPorModular([FromBody] Models.Certificado.UsuarioModularRequest encryptedRequest)
        {
            var response = await _authService.ObtenerRolPorModular(encryptedRequest);

            return Ok(response);
        }*/

        //OK-C
        // POST: api/auth/menu/nivel
        [HttpPost("menu/nivel", Name = "PostMenuNivelPorRol")]
        [Produces("application/json", Type = typeof(StatusResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostMenuNivelPorRol([FromBody] Models.Certificado.RolRequest model)
        {
            var response = await _authService.ObtenerMenuNivelPorRol(model);

            return Ok(response);
        }

        //OK
        [HttpPost]
        [Route("token")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [RequestLimitDDOS]
        [TokenAutenticationAtribute(CreateToken = true)]
        public async Task<IActionResult> GenerarToken()
        {
            return Ok(true);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginSistema([FromBody] Models.Certificado.AuthUserModel modelo)
        {
            if (string.IsNullOrEmpty(modelo.token))
            {
                return BadRequest(new
                {
                    ErrorMessage = "Datos incompletos",
                    CodeError = 501
                });
            }

            var respuesta = await _authService.Login(modelo);

            //ViewBag.Login = true;
            return Ok(respuesta);
        }
    }
}