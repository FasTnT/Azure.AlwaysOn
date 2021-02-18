using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FasTnT.Azure.AlwaysOn
{
    public class EpcisRepositoryService
    {
        const string PingPath = "v1_2/Query.svc";
        const string PingPayload = @"<?xml version=""1.0"" encoding=""UTF-8""?><soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:epcglobal:epcis-query:xsd:1""><soapenv:Body><urn:GetStandardVersion /></soapenv:Body></soapenv:Envelope>";

        private readonly HttpClient _client;

        public EpcisRepositoryService(EpcisRepositorySettings repositorySettings)
        {
            var base64AuthString = Encoding.UTF8.GetBytes($"{repositorySettings.Username}:{repositorySettings.Password}");
            var encodedBasicAuth = Convert.ToBase64String(base64AuthString);

            _client = new HttpClient { BaseAddress = new Uri(repositorySettings.Url) };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedBasicAuth);
        }

        public Task PingAsync(CancellationToken cancellationToken)
        {
            var requestContent = new StringContent(PingPayload, Encoding.UTF8, "text/xml");
            var request = _client.PostAsync(PingPath, requestContent, cancellationToken);

            return request.ContinueWith(result =>
            {
                result.Result.EnsureSuccessStatusCode();
            });
        }
    }
}
