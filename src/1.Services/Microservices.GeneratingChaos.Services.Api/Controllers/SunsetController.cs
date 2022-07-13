using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microservices.GeneratingChaos.Services.Api.Domain.Models;
using Microservices.GeneratingChaos.Services.Api.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Microservices.GeneratingChaos.Services.Api.Controllers
{
    /// <summary>
    /// Class SunsetController.
    /// Implements the <see cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SunsetController : ControllerBase
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<SunsetController> _logger;
        /// <summary>
        /// The sun service
        /// </summary>
        private readonly ISunService _sunService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SunsetController" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="sunService">The sun service.</param>
        /// <exception cref="ArgumentNullException">logger</exception>
        /// <exception cref="ArgumentNullException">sunService</exception>
        /// <exception cref="ArgumentNullException">logger</exception>
        public SunsetController(ILogger<SunsetController> logger, 
                                ISunService sunService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sunService = sunService ?? throw new ArgumentNullException(nameof(sunService));
        }

        /// <summary>
        /// Gets the specified city identifier.
        /// </summary>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>IActionResult.</returns>
        [HttpGet("{cityId:guid}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Sun))]
        public async Task<IActionResult> GetAsync(Guid cityId)
        {
            if (cityId == Guid.Empty)
            {
                return BadRequest();
            }
            var result = await _sunService.GetByCityAsync(cityId).ConfigureAwait(false);
            return Ok(result);
        }
    }
}
