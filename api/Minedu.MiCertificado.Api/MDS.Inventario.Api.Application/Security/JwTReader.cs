using Microsoft.AspNetCore.Http;
using MDS.Inventario.Api.Application.Contracts.Security;

namespace MDS.Inventario.Api.Application.Utils
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
