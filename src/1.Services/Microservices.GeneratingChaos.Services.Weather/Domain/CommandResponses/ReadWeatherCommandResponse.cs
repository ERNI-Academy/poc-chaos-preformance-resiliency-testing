using Microservices.GeneratingChaos.BuildingBlocks.Helpers;
using Microservices.GeneratingChaos.Services.Weather.Domain.Models;
using System.Collections.Generic;

namespace Microservices.GeneratingChaos.Services.Weather.Domain.CommandResponses
{
    /// <summary>
    /// Class ReadWeatherCommandResponse.
    /// Implements the <see cref="Microservices.GeneratingChaos.BuildingBlocks.Helpers.BaseCommandResponse{System.Collections.Generic.List{Microservices.GeneratingChaos.Services.Weather.Domain.Models.WeatherForecast}}" />
    /// </summary>
    /// <seealso cref="Microservices.GeneratingChaos.BuildingBlocks.Helpers.BaseCommandResponse{System.Collections.Generic.List{Microservices.GeneratingChaos.Services.Weather.Domain.Models.WeatherForecast}}" />
    public class ReadWeatherCommandResponse : BaseCommandResponse<List<WeatherForecast>>
    {

    }
}
