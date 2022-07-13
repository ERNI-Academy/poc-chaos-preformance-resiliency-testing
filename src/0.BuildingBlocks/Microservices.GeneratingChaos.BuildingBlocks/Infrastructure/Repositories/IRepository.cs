using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.SeedWork;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Repositories
{
    /// <summary>
    /// Repository Interface
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="System.IDisposable" />
    public interface IRepository<TEntity> where TEntity : Entity
    {
        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The added entity</returns>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// Adds the many.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns>Task.</returns>
        Task AddManyAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;TEntity&gt;.</returns>
        Task<TEntity> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>Task&lt;IEnumerable&lt;TEntity&gt;&gt;.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        /// <value>The query.</value>
        IMongoQueryable<TEntity> Query { get; }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task.</returns>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// Removes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        Task RemoveAsync(Guid id);
    }
}
