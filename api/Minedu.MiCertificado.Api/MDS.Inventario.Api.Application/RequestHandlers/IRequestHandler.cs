using Newtonsoft.Json.Linq;

namespace MDS.Inventario.Api.RequestHandlers
{
    public interface IRequestHandler
    {
        //Method to get the releases of the repo provided by the url
        string GetReleases(string url);
    }
}
