using Microservices.GeneratingChaos.Services.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservices.GeneratingChaos.Services.Api.Infrastructure.Services.Interfaces
{
    /// <summary>
    /// Interface IWeatherService
    /// </summary>
    public interface IWeatherService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Task&lt;IEnumerable&lt;WeatherForecast&gt;&gt;.</returns>
        Task<IEnumerable<WeatherForecast>> GetAllAsync();

        /// <summary>
        /// Gets the by city asynchronous.
        /// </summary>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>Task&lt;IEnumerable&lt;WeatherForecast&gt;&gt;.</returns>
        Task<IEnumerable<WeatherForecast>> GetByCityAsync(Guid cityId);
    }
}
