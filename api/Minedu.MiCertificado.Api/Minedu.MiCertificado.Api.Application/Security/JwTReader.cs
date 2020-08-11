using Microsoft.AspNetCore.Http;
using Minedu.MiCertificado.Api.Application.Contracts.Security;

namespace Minedu.MiCertificado.Api.Application.Utils
{
    public class JwTReader
    {
        public static T Leerkey<T>(
           IEncryptionServerSecurity encryptionServerSecurity,
           IHttpContextAccessor httpContextAccessor,
           string key,
           T porDefecto)
        {
            return encryptionServerSecurity.Decrypt<T>(
                ReadRequest.getKeyValue<string>(httpContextAccessor, key, ""),
                porDefecto);
        }
    }
}
