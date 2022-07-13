using System;

namespace Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Generators.Interfaces
{
    /// <summary>
    /// Interface to control the Date Creation.
    /// </summary>
    public interface IDate
    {
        /// <summary>
        /// Gets the now.
        /// </summary>
        /// <returns>New Datetime representing Now</returns>
        DateTime Now();

        /// <summary>
        /// Parses the specified date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>DateTime.</returns>
        DateTime Parse(string date);

        /// <summary>
        /// Parses the specified date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="alternative">The alternative.</param>
        /// <returns>DateTime.</returns>
        DateTime Parse(string date, DateTime alternative);

        /// <summary>
        /// Parses from unix.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>DateTime.</returns>
        DateTime ParseFromUnix(string date);

        /// <summary>
        /// UTCs the now.
        /// </summary>
        /// <returns>DateTime.</returns>
        DateTime UtcNow();
    }
}
