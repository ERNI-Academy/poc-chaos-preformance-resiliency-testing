using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Generators.Interfaces;
using System;

namespace Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Generators
{
    /// <inheritdoc/>
    public class Date : IDate
    {
        /// <inheritdoc/>
        public DateTime Now()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// Parses the specified date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>DateTime.</returns>
        /// <exception cref="ArgumentNullException">date</exception>
        public DateTime Parse(string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                throw new ArgumentNullException(nameof(date));
            }
            return DateTime.Parse(date);
        }
        /// <summary>
        /// Parses the specified date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="alternative">The alternative.</param>
        /// <returns>DateTime.</returns>
        public DateTime Parse(string date, DateTime alternative)
        {
            if (string.IsNullOrEmpty(date))
            {
                return alternative;
            }
            return DateTime.Parse(date);
        }

        /// <summary>
        /// Parses from unix.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>DateTime.</returns>
        public DateTime ParseFromUnix(string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                return Now();
            }

            return DateTimeOffset.FromUnixTimeSeconds(long.Parse(date)).DateTime;
        }

        /// <summary>
        /// UTCs the now.
        /// </summary>
        /// <returns>DateTime.</returns>
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
