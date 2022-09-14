using Microsoft.Extensions.Options;

namespace TestApp
{
    public class RequestGenerator : BackgroundService
    {
        public const string ClientName = "GeneratorClient";

        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        private readonly int delay;

        private readonly string _request;

        public RequestGenerator(int rpm, string request, HttpClient client, ILogger<RequestGenerator> logger)
        {
            _logger = logger;
            _httpClient = client;
            delay = (int)(60000 / rpm);
            _request = request;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(delay);
                HttpResponseMessage responseMessage = await _httpClient.PostAsync(_request, null, stoppingToken);

                if (!responseMessage.IsSuccessStatusCode)
                {
                    _logger.LogError("Not successed request");
                }
                else
                {
                    _logger.LogInformation(await responseMessage.Content.ReadAsStringAsync(stoppingToken));
                }
            }
        }

    }
}
