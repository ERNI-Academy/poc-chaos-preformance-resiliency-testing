using System;

namespace Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Generators.Interfaces
{
    /// <summary>
    /// Interface to control the Id generation.
    /// </summary>
    public interface IIdGenerator
    {
        /// <summary>
        /// Generates the new identifier.
        /// </summary>
        /// <returns>New string representing the unique id.</returns>
        string GenerateNewId();

        /// <summary>
        /// Generates the new identifier.
        /// </summary>
        /// <returns>New Guid representing the unique id.</returns>
        Guid GenerateNewGuidId();
    }
}
