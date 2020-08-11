using System;
using System.Collections.Generic;
using System.Text;

namespace Minedu.MiCertificado.Api.Application.Contracts.Security
{
    public interface IEncryptionServerSecurity
    {
        string Encrypt(string input);
        T Decrypt<T>(string input, T porDefecto);
    }
}
