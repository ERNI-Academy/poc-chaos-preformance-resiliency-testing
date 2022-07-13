using System;
using System.Net;
using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Generators.Interfaces;
using Microservices.GeneratingChaos.Services.Weather.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Microservices.GeneratingChaos.Services.Weather.Controllers
{
    /// <summary>
    /// Class SunController.
    /// Implements the <see cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("v1/[controller]")]
    public class SunController : ControllerBase
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<WeatherForecastController> _logger;
        /// <summary>
        /// The date
        /// </summary>
        private readonly IDate _date;

        /// <summary>
        /// Initializes a new instance of the <see cref="SunController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="date">The date.</param>
        /// <exception cref="ArgumentNullException">logger</exception>
        /// <exception cref="ArgumentNullException">date</exception>
        public SunController(ILogger<WeatherForecastController> logger, IDate date)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _date = date ?? throw new ArgumentNullException(nameof(date));
        }

        /// <summary>
        /// Gets the specified city identifier.
        /// </summary>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>IActionResult.</returns>
        [HttpGet("{cityId:guid}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Sun))]
        public IActionResult Get(Guid cityId)
        {
            if (cityId == Guid.Empty)
            {
                return BadRequest();
            }
            var rng = new Random();
            var now = _date.Now();
            var result = new Sun
            {
                CityId = cityId,
                CurrentDate = now,
                SunRise = new DateTime(now.Year, now.Month, now.Day, rng.Next(5, 8), 0, 0),
                Sunset = new DateTime(now.Year, now.Month, now.Day, rng.Next(17, 20), 0, 0)
            };
            return Ok(result);
        }
    }
}
