using System;
using Minedu.MiCertificado.Api.Application.Contracts.Security;
using Net_Core_JS_Encryption_Decryption;

namespace Minedu.MiCertificado.Api.Application.Security
{
    public static class ReactEncryptationSecurity
    {
        private const string Key = "484060556f8744ab799c69961bc5a6c190f22097";

        public static string Encrypt(string input)
        {
            if (input == null)
            {
                return input;
            }
            return EncryptionHandler.Encrypt(input, Key);
        }

        public static T Decrypt<T>(string input, T porDefecto)
        {
            try
            {
                if (input == null)
                {
                    return porDefecto;
                }
                else
                {
                    return (T)Convert.ChangeType(EncryptionHandler.Decrypt(input, Key), typeof(T));
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
