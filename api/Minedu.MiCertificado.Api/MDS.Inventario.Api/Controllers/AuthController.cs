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
        public async Task<IActionResult> GetLogin([FromBody] Models.Helpers.ParametroHelper encryptedRequest)
        {
            var response = await _authService.Login(encryptedRequest);

            return Ok(response);
        }
        
    }
}