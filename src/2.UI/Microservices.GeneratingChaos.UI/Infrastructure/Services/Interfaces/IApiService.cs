using Microservices.GeneratingChaos.UI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservices.GeneratingChaos.UI.Infrastructure.Services.Interfaces
{
    /// <summary>
    /// Interface IApiService
    /// </summary>
    public interface IApiService
    {
        /// <summary>
        /// Gets the weather by city identifier asynchronous.
        /// </summary>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>Task&lt;IEnumerable&lt;WheatherForecastDto&gt;&gt;.</returns>
        Task<IEnumerable<WheatherForecastDto>> GetWeatherByCityIdAsync(Guid cityId);

        /// <summary>
        /// Gets the cities asynchronous.
        /// </summary>
        /// <returns>Task&lt;IEnumerable&lt;CityDto&gt;&gt;.</returns>
        Task<IEnumerable<CityDto>> GetCitiesAsync();
    }
}
