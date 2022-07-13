using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.SeedWork;
using System;

namespace Microservices.GeneratingChaos.Services.Api.Domain.Entities
{
    /// <summary>
    /// Class Sun.
    /// Implements the <see cref="Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.SeedWork.Entity" />
    /// </summary>
    /// <seealso cref="Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.SeedWork.Entity" />
    public class Sun : Entity
    {
        /// <summary>
        /// Gets or sets the city identifier.
        /// </summary>
        /// <value>The city identifier.</value>
        public Guid CityId { get; set; }
        /// <summary>
        /// Gets or sets the current date.
        /// </summary>
        /// <value>The current date.</value>
        public DateTime CurrentDate { get; set; }
        /// <summary>
        /// Gets or sets the sun rise.
        /// </summary>
        /// <value>The sun rise.</value>
        public DateTime SunRise { get; set; }
        /// <summary>
        /// Gets or sets the sunset.
        /// </summary>
        /// <value>The sunset.</value>
        public DateTime Sunset { get; set; }

    }
}
