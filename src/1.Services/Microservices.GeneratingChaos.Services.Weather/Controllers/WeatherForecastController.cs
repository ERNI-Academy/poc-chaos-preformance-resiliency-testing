using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using MediatR;
using Microservices.GeneratingChaos.Services.Weather.Domain.Commands;
using Microservices.GeneratingChaos.Services.Weather.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Microservices.GeneratingChaos.Services.Weather.Controllers
{
    /// <summary>
    /// Class WeatherForecastController.
    /// Implements the <see cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("v1/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<WeatherForecastController> _logger;
        /// <summary>
        /// The mediator
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// The cache
        /// </summary>
        private readonly IDistributedCache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherForecastController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="cache">The cache.</param>
        /// <exception cref="ArgumentNullException">logger</exception>
        /// <exception cref="ArgumentNullException">mediator</exception>
        /// <exception cref="ArgumentNullException">cache</exception>
        public WeatherForecastController(ILogger<WeatherForecastController> logger,
                                         IMediator mediator,
                                         IDistributedCache cache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
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

            var command = new ReadWeatherCommand();
            var commandResponse = await _mediator.Send(command).ConfigureAwait(false);
            if (commandResponse == null)
            {
                return BadRequest();
            }

            if (!commandResponse.Succeed)
            {
                return BadRequest(commandResponse.Error);
            }

            var serializedWeather = JsonConvert.SerializeObject(commandResponse.ResponseDto);
            var weatherEncoded = Encoding.UTF8.GetBytes(serializedWeather);
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(30));
            _cache.Set("Weather", weatherEncoded, options);

            return Ok(commandResponse.ResponseDto);
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

            var command = new ReadWeatherCommand
            {
                CityId = cityId
            };

            var commandResponse = await _mediator.Send(command).ConfigureAwait(false);

            if (!commandResponse.Succeed)
            {
                return BadRequest(commandResponse.Error);
            }

            var serializedWeather = JsonConvert.SerializeObject(commandResponse.ResponseDto);
            var weatherEncoded = Encoding.UTF8.GetBytes(serializedWeather);
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(30));
            _cache.Set($"Weather_{cityId}", weatherEncoded, options);

            return Ok(commandResponse.ResponseDto);
        }
    }
}
