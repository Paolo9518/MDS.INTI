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
using MDS.Inventario.Api.Application.Mappers;

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

        public async Task<StatusResponse> Login(Models.UsuarioExtends request)
        {
            var response = new StatusResponse();
            try
            {
                // 1. Validar el usuario
                var responseUsuario = await _unitOfWork.ValidarUsuario(request.NombreUsuario, request.Contrasenia);

                if (responseUsuario == null)
                {
                    response.Data = null;
                    response.Success = false;
                    response.Messages.Add("Usuario no Autorizado");
                    return response;
                }
                var usuario = responseUsuario.Select(x => PersonalMapper.Map(x)).FirstOrDefault();

                #region TOKEN_CERTIFICADO
                //Encapsular token de Siagie en token de Certificado
                var secretKey = _configuration.GetSection("JwT:Key").Value;
                var key = Encoding.ASCII.GetBytes(secretKey);

                // encriptando datos de la sesion de usuario
                var claimsConfig = new[]
                {
                    new Claim("certificado", _encryptionServerSecurity.Encrypt(usuario.NombreCompleto)),
                    new Claim("certificado", _encryptionServerSecurity.Encrypt(usuario.NumeroDocumento)),
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

                response.Data = new Models.Helpers.DatosUsuarioHelper()
                {
                    //usuario = ReactEncryptationSecurity.Encrypt(),
                    nombresCompleto = usuario.NombreCompleto,
                    numeroDocumento = ReactEncryptationSecurity.Encrypt(usuario.NumeroDocumento),
                    idRol = ReactEncryptationSecurity.Encrypt(usuario.IdRol.ToString()),
                    dscRol = usuario.DscRol,
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
