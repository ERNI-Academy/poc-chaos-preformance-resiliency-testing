using Microservices.GeneratingChaos.UI.Domain.Models;
using Microservices.GeneratingChaos.UI.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Microservices.GeneratingChaos.UI.Infrastructure.Services
{
    /// <summary>
    /// Class ApiService.
    /// Implements the <see cref="Microservices.GeneratingChaos.UI.Infrastructure.Services.Interfaces.IApiService" />
    /// </summary>
    /// <seealso cref="Microservices.GeneratingChaos.UI.Infrastructure.Services.Interfaces.IApiService" />
    public class ApiService : IApiService
    {
        private readonly HttpClient Http;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiService"/> class.
        /// </summary>
        /// <param name="http">The HTTP.</param>
        /// <exception cref="ArgumentNullException">http</exception>
        public ApiService(HttpClient http)
        {
            Http = http ?? throw new ArgumentNullException(nameof(http));
        }

        /// <summary>
        /// get weather by city identifier as an asynchronous operation.
        /// </summary>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>Task&lt;IEnumerable&lt;WheatherForecastDto&gt;&gt;.</returns>
        public async Task<IEnumerable<WheatherForecastDto>> GetWeatherByCityIdAsync(Guid cityId)
        {
            var policy = CreatePolicy();

            var response = await policy.ExecuteAsync<WheatherForecastDto[]>(async () =>
            {
                return await Http.GetFromJsonAsync<WheatherForecastDto[]>($"api/v1/WeatherForecast/{cityId}").ConfigureAwait(false);
            }).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// get cities as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;IEnumerable&lt;CityDto&gt;&gt;.</returns>
        public async Task<IEnumerable<CityDto>> GetCitiesAsync()
        {
            var policy = CreatePolicy();
            var response = await policy.ExecuteAsync<CityDto[]>(async () =>
            {
                return await Http.GetFromJsonAsync<CityDto[]>("api/v1/City").ConfigureAwait(false);
            }).ConfigureAwait(false);

            return response;            
        }

        private static AsyncRetryPolicy CreatePolicy()
        {
            var delay = Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(2), retryCount: 5);
            var policy = Policy.Handle<Exception>().WaitAndRetryAsync(delay);
            return policy;
        }
    }
}
