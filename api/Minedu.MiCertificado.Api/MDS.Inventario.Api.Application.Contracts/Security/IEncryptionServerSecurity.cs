using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.Application.Contracts.Security
{
    public interface IEncryptionServerSecurity
    {
        string Encrypt(string input);
        T Decrypt<T>(string input, T porDefecto);
    }
}
