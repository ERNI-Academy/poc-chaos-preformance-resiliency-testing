using AutoMapper;
using MediatR;
using Microservices.GeneratingChaos.Services.Weather.Domain.CommandResponses;
using Microservices.GeneratingChaos.Services.Weather.Domain.Commands;
using Microservices.GeneratingChaos.Services.Weather.Domain.Entities;
using Microservices.GeneratingChaos.Services.Weather.Infrastructure.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.GeneratingChaos.Services.Weather.Infrastructure.CommandHandler
{
    /// <summary>
    /// Class WeatherCommandHandler.
    /// Implements the <see cref="MediatR.IRequestHandler{Domain.Commands.ReadWeatherCommand, Domain.CommandResponses.ReadWeatherCommandResponse}" />
    /// </summary>
    /// <seealso cref="MediatR.IRequestHandler{Domain.Commands.ReadWeatherCommand, Domain.CommandResponses.ReadWeatherCommandResponse}" />
    public class WeatherCommandHandler : IRequestHandler<ReadWeatherCommand, ReadWeatherCommandResponse>
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<WeatherCommandHandler> _logger;
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;
        /// <summary>
        /// The weather repository
        /// </summary>
        private readonly IWeatherRepository _weatherRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherCommandHandler" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="weatherRepository">The weather repository.</param>
        /// <exception cref="ArgumentNullException">logger</exception>
        /// <exception cref="ArgumentNullException">mapper</exception>
        /// <exception cref="ArgumentNullException">weatherRepository</exception>
        /// <exception cref="ArgumentNullException">logger</exception>
        /// <exception cref="ArgumentNullException">mapper</exception>
        public WeatherCommandHandler(ILogger<WeatherCommandHandler> logger, IMapper mapper, IWeatherRepository weatherRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _weatherRepository = weatherRepository ?? throw new ArgumentNullException(nameof(weatherRepository));
        }

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;ReadWeatherCommandResponse&gt;.</returns>
        public async Task<ReadWeatherCommandResponse> Handle(ReadWeatherCommand request, CancellationToken cancellationToken)
        {            
            List<WeatherForecast> weatherForecast;

            if (request != null && request.CityId.HasValue)
            {
                _logger.LogInformation($"Getting weather forecast for city:{request.CityId.Value}");
                var weatherForecastsFromRepository = await _weatherRepository.GetByCityId(request.CityId.Value).ConfigureAwait(false);
                weatherForecast = weatherForecastsFromRepository.ToList();
            }
            else
            {
                _logger.LogInformation("Getting weather forecast for all cities");
                var weatherForecastsFromRepository = await _weatherRepository.GetAllAsync().ConfigureAwait(false);
                weatherForecast = weatherForecastsFromRepository.ToList();
            }

            var response = _mapper.Map<ReadWeatherCommandResponse>(weatherForecast);
            return response;
        }
    }
}
