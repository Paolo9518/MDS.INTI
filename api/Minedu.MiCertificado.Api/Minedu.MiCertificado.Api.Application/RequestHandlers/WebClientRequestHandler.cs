using Minedu.MiCertificado.Constants;
using System.Net;

namespace Minedu.MiCertificado.Api.RequestHandlers
{
    public class WebClientRequestHandler: IRequestHandler
    {
        public string GetReleases(string url)
        {
            var client = new WebClient();
            client.Headers.Add(RequestConstants.UserAgent, RequestConstants.UserAgentValue);

            var response = client.DownloadString(url);

            return response;
        }
    }
}
