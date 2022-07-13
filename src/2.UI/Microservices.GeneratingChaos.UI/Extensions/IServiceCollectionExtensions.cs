using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Microservices.GeneratingChaos.UI.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddHttp(this IServiceCollection services, IConfiguration configuration) =>
             services.AddScoped<HttpClient>(s =>
             {
                 // Creating the URI helper needs to wait until the JS Runtime is initialized, so defer it.
                 return new HttpClient
                 {
                     BaseAddress = new Uri(configuration["ApiUrl"])
                 };
             });
    }
}
