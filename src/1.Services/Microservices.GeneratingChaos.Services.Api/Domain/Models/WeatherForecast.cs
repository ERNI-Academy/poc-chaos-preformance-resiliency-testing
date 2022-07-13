using System;

namespace Microservices.GeneratingChaos.Services.Api.Domain.Models
{
    /// <summary>
    /// Class WeatherForecast.
    /// </summary>
    public class WeatherForecast
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
