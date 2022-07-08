using DocumentsExternal.Models;
using DocumentsExternal.Models.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DocumentsExternal.Services
{
    public class AvaxApiClient : IAvaxApiClient
    {
        private readonly ILogger<AvaxApiClient> logger;
        private readonly DocumentsConfiguration configuration;
        private readonly HttpClient client;

        public AvaxApiClient(ILogger<AvaxApiClient> logger, IOptions<DocumentsConfiguration> options, HttpClient client)
        {
            this.logger = logger;
            this.configuration = options.Value;
            this.client = client;
            this.client.BaseAddress = new Uri(configuration.Hosts.AvaxApi);
        }

        public async Task<FileDocumentView> GetNotificationFile(Guid id)
        {
            var response = await client.GetAsync($"api/external/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsAsync<FileDocumentView>();
                return content;
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                logger.LogWarning("Получен ответ с ошибкой от сервиса Avalanche API. Сообщение: {Message}", message);
                return null;
            }
        }
    }
}
