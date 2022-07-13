using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Generators.Interfaces;
using System;

namespace Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Generators
{
    /// <inheritdoc/>
    public class IdGenerator : IIdGenerator
    {
        /// <inheritdoc />
        public Guid GenerateNewGuidId()
        {
            return Guid.NewGuid();
        }

        /// <inheritdoc />
        public string GenerateNewId()
        {
            return GenerateNewGuidId().ToString();
        }
    }
}
