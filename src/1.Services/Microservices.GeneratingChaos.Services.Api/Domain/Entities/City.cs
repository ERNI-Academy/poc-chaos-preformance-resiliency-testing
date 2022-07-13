using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.SeedWork;
using System;

namespace Microservices.GeneratingChaos.Services.Api.Domain.Entities
{
    /// <summary>
    /// Class City.
    /// Implements the <see cref="Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.SeedWork.Entity" />
    /// </summary>
    /// <seealso cref="Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.SeedWork.Entity" />
    public class City : Entity
    {
        /// <summary>
        /// Gets or sets the city identifier.
        /// </summary>
        /// <value>The city identifier.</value>
        public Guid CityId { get; set; }
        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        /// <value>The name of the city.</value>
        public string CityName { get; set; }
        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>The country.</value>
        public string Country { get; set; }
    }
}
