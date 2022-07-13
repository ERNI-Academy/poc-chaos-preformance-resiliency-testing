using System;
using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.SeedWork;

namespace Microservices.GeneratingChaos.Services.Weather.Domain.Entities
{
    /// <summary>
    /// Class WeatherForecast.
    /// Implements the <see cref="Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.SeedWork.Entity" />
    /// </summary>
    /// <seealso cref="Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.SeedWork.Entity" />
    public class WeatherForecast : Entity
    {
        /// <summary>
        /// Gets or sets the city identifier.
        /// </summary>
        /// <value>The city identifier.</value>
        public Guid CityId { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the temperature.
        /// </summary>
        /// <value>The temperature.</value>
        public double Temperature { get; set; }

        /// <summary>
        /// Gets or sets the humidity.
        /// </summary>
        /// <value>The humidity.</value>
        public double Humidity { get; set; }

        /// <summary>
        /// Gets or sets the pressure.
        /// </summary>
        /// <value>The pressure.</value>
        public double Pressure { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public string Icon { get; set; }
    }
}
