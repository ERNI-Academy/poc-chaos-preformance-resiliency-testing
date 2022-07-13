using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Repositories;
using Microservices.GeneratingChaos.Services.Api.Domain.Entities;

namespace Microservices.GeneratingChaos.Services.Api.Infrastructure.Repository.Interfaces
{
    /// <summary>
    /// Interface ICityRepository
    /// Implements the <see cref="Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Repositories.IRepository{Microservices.GeneratingChaos.Services.Api.Domain.Entities.City}" />
    /// </summary>
    /// <seealso cref="Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Repositories.IRepository{Microservices.GeneratingChaos.Services.Api.Domain.Entities.City}" />
    public interface ICityRepository : IRepository<City>
    { }
}