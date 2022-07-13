using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Repositories;
using Microservices.GeneratingChaos.Services.Weather.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservices.GeneratingChaos.Services.Weather.Infrastructure.Repository.Interfaces
{
    /// <summary>
    /// Interface IWeatherRepository
    /// Implements the <see cref="Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Repositories.IRepository{Microservices.GeneratingChaos.Services.Weather.Domain.Entities.WeatherForecast}" />
    /// </summary>
    /// <seealso cref="Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Repositories.IRepository{Microservices.GeneratingChaos.Services.Weather.Domain.Entities.WeatherForecast}" />
    public interface IWeatherRepository : IRepository<WeatherForecast>
    {
        /// <summary>
        /// Gets the by city identifier.
        /// </summary>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>Task&lt;IEnumerable&lt;WeatherForecast&gt;&gt;.</returns>
        Task<IEnumerable<WeatherForecast>> GetByCityId(Guid cityId);
    }
}
