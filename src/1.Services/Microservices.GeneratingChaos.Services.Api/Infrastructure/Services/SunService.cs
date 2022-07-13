using Microservices.GeneratingChaos.Services.Api.Domain.Models;
using Microservices.GeneratingChaos.Services.Api.Infrastructure.Services.Interfaces;
using Newtonsoft.Json;
using Polly;
using Polly.Contrib.WaitAndRetry;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microservices.GeneratingChaos.Services.Api.Infrastructure.Services
{
    /// <summary>
    /// Class SunService.
    /// Implements the <see cref="Microservices.GeneratingChaos.Services.Api.Infrastructure.Services.Interfaces.ISunService" />
    /// </summary>
    /// <seealso cref="Microservices.GeneratingChaos.Services.Api.Infrastructure.Services.Interfaces.ISunService" />
    public class SunService : ISunService
    {
        /// <summary>
        /// The HTTP client
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="SunService" /> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <exception cref="ArgumentNullException">httpClient</exception>
        public SunService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// Gets the by city asynchronous.
        /// </summary>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>Task&lt;Sun&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Sun> GetByCityAsync(Guid cityId)
        {
            var delay = Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(2), retryCount: 5);
            var policy = Policy.Handle<Exception>().WaitAndRetryAsync(delay);

            var response = await policy.ExecuteAsync<string>(async () =>
            {
                return await _httpClient.GetStringAsync($"v1/sun/{cityId}").ConfigureAwait(false);
            }).ConfigureAwait(false);

            var sunResponse = JsonConvert.DeserializeObject<Sun>(response);
            return sunResponse;
        }
    }
}
