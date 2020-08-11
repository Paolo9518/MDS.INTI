using Minedu.MiCertificado.Api.Application.Contracts.Configuration;
using Minedu.MiCertificado.Api.BusinessLogic.Models.Siagie;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Web;
using Minedu.MiCertificado.Api.BusinessLogic.Models;
using Minedu.MiCertificado.Api.Application.Contracts.Services;

namespace Minedu.MiCertificado.Api.Application.Services
{
    public class SiagieService : ISiagieService
    {
        private readonly HttpClient _httpClient;

        public SiagieService(ISiagieConfig siagieConfig)
        {

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(siagieConfig.ServiceUrl)
            };

            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<T> PostServiceToken<T>(TokenRequest request)
        {
            var content = JsonConvert.SerializeObject(request);
            var buffer = Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _httpClient.PostAsync("api/auth", byteContent);

            if (!response.IsSuccessStatusCode)
                return default(T);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task<T> GetServiceByQueryAndToken<T, Y>(string token, string method, Y filter)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var filterResult = filter != null ? ToUrl(filter) : "";
            var response = await _httpClient.GetAsync(string.Format("api/certificado/{0}?{1}", method, filterResult));

            if (!response.IsSuccessStatusCode)
            {
                return default(T);
            }

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task<T> GetServiceResponseById<T>(string controller, int id)
        {

            var response = await _httpClient.GetAsync(string.Format("{0}/{1}", controller, id));

            if (!response.IsSuccessStatusCode)
                return default(T);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }

        public string ToUrl(object obj)
        {
            var urlBuilder = new StringBuilder();
            var properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            for (int i = 0; i < properties.Length; i++)
            {
                urlBuilder.AppendFormat("{0}={1}&", properties[i].Name, properties[i].GetValue(obj, null));
            }

            if (urlBuilder.Length > 1)
            {
                urlBuilder.Remove(urlBuilder.Length - 1, 1);
            }

            return urlBuilder.ToString();
        }

        public string GetQueryString(object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return String.Join("&", properties.ToArray());
        }
    }
}
