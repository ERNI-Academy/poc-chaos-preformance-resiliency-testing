using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microservices.GeneratingChaos.Services.Api.Domain.Models;
using Microservices.GeneratingChaos.Services.Api.Infrastructure.Repository;
using Microservices.GeneratingChaos.Services.Api.Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Microservices.GeneratingChaos.Services.Api.Controllers
{
    /// <summary>
    /// Class CityController.
    /// Implements the <see cref="MicroserviceControllerBase" />
    /// </summary>
    /// <seealso cref="MicroserviceControllerBase" />
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CityController : MicroserviceControllerBase
    {
        /// <summary>
        /// The city repository
        /// </summary>
        private readonly ICityRepository _cityRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CityController" /> class.
        /// </summary>
        /// <param name="cityRepository">The city repository.</param>
        /// <param name="cache">The cache.</param>
        /// <exception cref="ArgumentNullException">cityRepository</exception>
        /// <exception cref="ArgumentNullException">cache</exception>
        public CityController(ICityRepository cityRepository,
                              IDistributedCache cache):base(cache)
        {
            _cityRepository = cityRepository ?? throw new ArgumentNullException(nameof(cityRepository));
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<City>))]
        public async Task<IActionResult> Get()
        {
            var encodedCacheCities = await _cache.GetAsync("Cities").ConfigureAwait(false);
            if (encodedCacheCities != null)
            {
                return Ok(Encoding.UTF8.GetString(encodedCacheCities));
            }

            var cities = await _cityRepository.GetAllAsync().ConfigureAwait(false);
            if (cities != null && cities.Any())
            {
                var serializedCities = JsonConvert.SerializeObject(cities);
                var citiesEncoded = Encoding.UTF8.GetBytes(serializedCities);
                var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(20));
                _cache.Set("Cities", citiesEncoded, options);
            }

            return Ok(cities);
        }
    }

    /// <summary>
    /// Class MicroserviceControllerBase.
    /// Implements the <see cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    public abstract class MicroserviceControllerBase : ControllerBase
    {
        /// <summary>
        /// The cache
        /// </summary>
        internal readonly IDistributedCache _cache;

        /// <summary>
        /// Initialize new instance of the MicroserviceControllerBase class.
        /// </summary>
        /// <param name="cache">The cache.</param>
        /// <exception cref="ArgumentNullException">cache</exception>
        public MicroserviceControllerBase(IDistributedCache cache)
{            
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }
    }
}
