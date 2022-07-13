using MediatR;
using Microservices.GeneratingChaos.Services.Weather.Domain.CommandResponses;
using System;

namespace Microservices.GeneratingChaos.Services.Weather.Domain.Commands
{
    /// <summary>
    /// Class ReadWeatherCommand.
    /// Implements the <see cref="MediatR.IRequest{Microservices.GeneratingChaos.Services.Weather.Domain.CommandResponses.ReadWeatherCommandResponse}" />
    /// </summary>
    /// <seealso cref="MediatR.IRequest{Microservices.GeneratingChaos.Services.Weather.Domain.CommandResponses.ReadWeatherCommandResponse}" />
    public class ReadWeatherCommand: IRequest<ReadWeatherCommandResponse>
    {
        /// <summary>
        /// Gets or sets the city identifier.
        /// </summary>
        /// <value>The city identifier.</value>
        public Guid? CityId { get; set; }
    }
}
