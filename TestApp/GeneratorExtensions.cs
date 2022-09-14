using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace TestApp
{
    public static class GeneratorExtensions
    {
        public static IServiceCollection AddGeneratorService(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<RequestOptions>(configuration.GetRequiredSection(RequestOptions.Section));

            services.AddHttpClient(RequestGenerator.ClientName, (isp, client) => 
            {            
                IOptions<RequestOptions> requestOptions = isp.GetRequiredService<IOptions<RequestOptions>>();
                client.BaseAddress = requestOptions?.Value.BaseUri ?? throw new ArgumentNullException(nameof(requestOptions));
            });

            services.AddHostedService<RequestGenerator>((isp) =>
            {
                RequestOptions? requestOptions = isp.GetRequiredService<IOptions<RequestOptions>>().Value;

                if (requestOptions is null)
                    throw new ArgumentNullException(nameof(requestOptions));

                HttpClient client = isp.GetRequiredService<IHttpClientFactory>().CreateClient(RequestGenerator.ClientName);

                return new RequestGenerator(requestOptions.Rpm, requestOptions.Request, client, isp.GetRequiredService<ILogger<RequestGenerator>>());
            });

            return services;
        }

    }
}
