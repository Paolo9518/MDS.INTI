using Minedu.Comun.Helper;
using MDS.Inventario.Api.Application.Contracts.Security;
using MDS.Inventario.Api.Application.Contracts.Services;
using Mappers = MDS.Inventario.Api.Application.Mappers;
using MDS.Inventario.Api.DataAccess.Contracts.UnitOfWork;
using Models = MDS.Inventario.Api.Application.Entities.Models;
using SeguridadWSService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using MDS.Inventario.Api.Application.Utils;
using Newtonsoft.Json;
using MDS.Inventario.Api.Application.Security;

namespace MDS.Inventario.Api.Application.Services
{

    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEncryptionServerSecurity _encryptionServerSecurity;
        private readonly IConfiguration _configuration;
        public readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IUnitOfWork unitOfWork,
            IEncryptionServerSecurity encryptionServerSecurity,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _unitOfWork = unitOfWork;
            _encryptionServerSecurity = encryptionServerSecurity;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

  

        private Models.Siagie.UsuarioResponse GetToken(string token)
        {
            var jsonUsuario = new Models.Siagie.UsuarioResponse();
            var ok = true;
            byte[] key = Encoding.ASCII.GetBytes(_configuration.GetSection("JwT:KeySiagie").Value);
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var validations = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                var claims = handler.ValidateToken(token, validations, out var tokenSecure);

                jsonUsuario = JsonConvert.DeserializeObject<Models.Siagie.UsuarioResponse>(claims.Claims.FirstOrDefault().Value);

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return null;
            }
            return jsonUsuario;
        }

        public async Task<StatusResponse> Login(Models.Certificado.AuthUserModel request)
        {

            try
            {

                // 1. Validar el usuario
                var usuario = _accountService.ValidateUser(request.Username, request.Password);

                if (usuario == null)
                {
                    // devolver un 401 (no autorizado)
                    return StatusCode(401);
                }


                #region TOKEN_CERTIFICADO
                //Encapsular token de Siagie en token de Certificado
                var secretKey = _configuration.GetSection("JwT:Key").Value;
                var key = Encoding.ASCII.GetBytes(secretKey);

                // encriptando datos de la sesion de usuario
                var claimsConfig = new[]
                {
                    new Claim("certificado", _encryptionServerSecurity.Encrypt(userInfo.ToString())),
                    new Claim("certificado", _encryptionServerSecurity.Encrypt(userInfo.ToString())),
                };

                var issuerConfig = _configuration.GetSection("JwT:Issuer").Value;
                var audienceConfig = _configuration.GetSection("JwT:Audience").Value;
                var expiresConfig = DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration.GetSection("JwT:Time").Value));
                var credsConfig = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

                JwtSecurityToken tokenCertificado = new JwtSecurityToken
                (
                    issuer: issuerConfig,
                    audience: audienceConfig,
                    claims: claimsConfig,
                    expires: expiresConfig,
                    signingCredentials: credsConfig
                );
                #endregion TOKEN_CERTIFICADO

               
                Models.Certificado.MenuNivelRolModel menuNivel = null;

                //MENU NIVEL X ROL
                //var resultMenuNivel = await _unitOfWork.ObtenerMenuNivelPorRol(usuarioRequest.idRol);
                var resultMenuNivel = await _unitOfWork.ObtenerMenuNivelPorRol(idRolTemporal);

                if (resultMenuNivel != null && resultMenuNivel.ToList().Count > 0)
                {
                    menuNivel = Mappers.Certificado.MenuNivelRolMapper.Map(resultMenuNivel.FirstOrDefault());
                }

                response.Data = new Models.Certificado.DatosUsuario()
                {
                    usuarioLogin = ReactEncryptationSecurity.Encrypt(usuarioRequest.loginUsuario),
                    nombresCompleto = usuarioRequest.nombreCompleto == null ? ReactEncryptationSecurity.Encrypt(usuarioRequest.nombre + " " + usuarioRequest.apePaterno + " " + usuarioRequest.apeMaterno) : ReactEncryptationSecurity.Encrypt(usuarioRequest.nombreCompleto),
                    numeroDocumento = ReactEncryptationSecurity.Encrypt(usuarioRequest.numeroDocumento == null ? usuarioRequest.dni : usuarioRequest.numeroDocumento),
                    idRol = ReactEncryptationSecurity.Encrypt(usuarioRequest.idRol),
                    dscRol = menuNivel.dscRol.Trim(),
                    tipoSede = ReactEncryptationSecurity.Encrypt(usuarioRequest.tipoSede == null ? usuarioRequest.idTipoSede : usuarioRequest.tipoSede.Trim()),
                    codigoModular = ReactEncryptationSecurity.Encrypt(usuarioRequest.codModular.Trim()),
                    anexo = ReactEncryptationSecurity.Encrypt(usuarioRequest.anexo.Trim()),
                    nombreIE = usuarioRequest.nombreIE.Trim(),
                    idNivel = ReactEncryptationSecurity.Encrypt(usuarioRequest.idNivel),
                    dscNivel = usuarioRequest.dscNivel == null ? usuarioRequest.descNivel : usuarioRequest.dscNivel.Trim(),
                    idModalidad = ReactEncryptationSecurity.Encrypt(usuarioRequest.idModalidad),
                    idMenuNivel = ReactEncryptationSecurity.Encrypt(menuNivel.idMenuNivel.ToString()),
                    dscMenuNivel = menuNivel.dscMenuNivel,
                    token = new JwtSecurityTokenHandler().WriteToken(tokenCertificado)
                };
                response.Success = true;
                response.Messages.Add("Inicio de Sesión exitoso.");
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.Success = false;
                response.Messages.Add("Error general:" + ex);
            }

            return response;

        }
    }
}
