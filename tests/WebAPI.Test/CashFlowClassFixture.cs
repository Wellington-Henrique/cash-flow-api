using DocumentFormat.OpenXml.Wordprocessing;
using Irony.Parsing;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebAPI.Test
{
    public class CashFlowClassFixture : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public CashFlowClassFixture(CustomWebApplicationFactory customWebApplicationFactory)
        {
            _httpClient = customWebApplicationFactory.CreateClient();
        }

        protected async Task<HttpResponseMessage> DoPost(
            string requestUri, 
            object request, 
            string token = "",
            string culture = "pt-BR")
        {
            AuthorizeRequest(token);
            ChangeRequestCulture(culture);

            return await _httpClient.PostAsJsonAsync(requestUri, request);
        }

        private void AuthorizeRequest(string token)
        {
            if(string.IsNullOrWhiteSpace(token) == false)
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private void ChangeRequestCulture(string cultureInfo)
        {
            _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
            _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(cultureInfo));
        }
    }
}
