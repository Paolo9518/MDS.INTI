using Minedu.MiCertificado.Constants;
using System;
using System.Net.Http;

namespace Minedu.MiCertificado.Api.RequestHandlers
{
    public class HttpClientRequestHandler: IRequestHandler
    {
        public string GetReleases(string url)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add(RequestConstants.UserAgent, RequestConstants.UserAgentValue);

                var response = httpClient.GetStringAsync(new Uri(url)).Result;

                return response;
            }
        }
    }
}
