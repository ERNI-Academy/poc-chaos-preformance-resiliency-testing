using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microservices.GeneratingChaos.Services.Api.Domain.Models;
using Microservices.GeneratingChaos.Services.Api.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Microservices.GeneratingChaos.Services.Api.Controllers
{
    /// <summary>
    /// Class WeatherForecastController.
    /// Implements the <see cref="MicroserviceControllerBase" />
    /// </summary>
    /// <seealso cref="MicroserviceControllerBase" />
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WeatherForecastController : MicroserviceControllerBase
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<WeatherForecastController> _logger;
        /// <summary>
        /// The weather service
        /// </summary>
        private readonly IWeatherService _weatherService;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherForecastController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="weatherService">The weather service.</param>
        /// <param name="cache">The cache.</param>
        /// <exception cref="ArgumentNullException">logger</exception>
        /// <exception cref="ArgumentNullException">weatherService</exception>
        /// <exception cref="ArgumentNullException">cache</exception>
        public WeatherForecastController(ILogger<WeatherForecastController> logger, 
                                         IWeatherService weatherService, 
                                         IDistributedCache cache):base(cache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _weatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
        }


        /// <summary>
        /// get all weather forecasts as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<WeatherForecast>))]
        public async Task<IActionResult> GetAllAsync()
        {
            var encodedCacheWeather = await _cache.GetAsync("Weather").ConfigureAwait(false);
            if (encodedCacheWeather != null)
            {
                return Ok(Encoding.UTF8.GetString(encodedCacheWeather));
            }

            var result = await _weatherService.GetAllAsync().ConfigureAwait(false);
            if (result == null)
            {
                return BadRequest();
            }
            
            var serializedWeather = JsonConvert.SerializeObject(result);
            var weatherEncoded = Encoding.UTF8.GetBytes(serializedWeather);
            _cache.Set("Weather", weatherEncoded, CreateDistributedCacheEntryOptions(10*60));

            return Ok(result);
        }

        /// <summary>
        /// get all weather forcast by city as an asynchronous operation.
        /// </summary>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpGet("{cityId:guid}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<WeatherForecast>))]
        public async Task<IActionResult> GetByCityAsync(Guid cityId)
        {
            var encodedCacheWeather = await _cache.GetAsync($"Weather_{cityId}").ConfigureAwait(false);
            if (encodedCacheWeather != null)
            {
                return Ok(Encoding.UTF8.GetString(encodedCacheWeather));
            }

            var result = await _weatherService.GetByCityAsync(cityId).ConfigureAwait(false);
            if (result == null)
            {
                return BadRequest();
            }

            var serializedWeather = JsonConvert.SerializeObject(result);
            var weatherEncoded = Encoding.UTF8.GetBytes(serializedWeather);
            _cache.Set($"Weather_{cityId}", weatherEncoded, CreateDistributedCacheEntryOptions(30));

            return Ok(result);
        }

        private DistributedCacheEntryOptions CreateDistributedCacheEntryOptions(int seconds)
        {
            return new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(seconds) };
        }

    }
}
