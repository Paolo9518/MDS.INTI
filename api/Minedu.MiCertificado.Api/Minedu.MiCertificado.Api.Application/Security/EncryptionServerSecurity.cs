using Microsoft.AspNetCore.DataProtection;
using Minedu.MiCertificado.Api.Application.Contracts.Security;
using System;

namespace Minedu.MiCertificado.Api.Application.Security
{
    public class EncryptionServerSecurity : IEncryptionServerSecurity
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private const string Key = "99401632e98ba0f3dbcfdafc5c5d3320f242394d";

        public EncryptionServerSecurity(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
        }

        public string Encrypt(string input)
        {
            if (input == null)
            {
                return input;
            }
            var protector = _dataProtectionProvider.CreateProtector(Key);
            return protector.Protect(input);
        }

        public T Decrypt<T>(string input, T porDefecto)
        {
            if (input == null)
            {
                return porDefecto;
            }
            else
            {
                var protector = _dataProtectionProvider.CreateProtector(Key);
                return (T)Convert.ChangeType(protector.Unprotect(input), typeof(T));
            }
        }
    }
}
