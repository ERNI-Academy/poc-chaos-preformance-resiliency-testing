using Microservices.GeneratingChaos.Services.Api.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Microservices.GeneratingChaos.Services.Api.Infrastructure.Services.Interfaces
{
    /// <summary>
    /// Interface ISunService
    /// </summary>
    public interface ISunService
    {
        /// <summary>
        /// Gets the by city asynchronous.
        /// </summary>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>Task&lt;Sun&gt;.</returns>
        Task<Sun> GetByCityAsync(Guid cityId);
    }
}
