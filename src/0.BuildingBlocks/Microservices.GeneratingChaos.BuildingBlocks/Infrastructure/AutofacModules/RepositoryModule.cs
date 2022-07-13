using Autofac;
using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.DataBase;
using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.SeedWork;
using MongoDB.Driver;

namespace Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.AutofacModules
{
    /// <summary>
    /// Creates a Module containing all the Repository.
    /// </summary>
    public class RepositoryModule : Module
    {
        /// <summary>
        /// The factory for the database.
        /// </summary>
        private readonly IDbFactory _dbFactory;

        /// <summary>
        /// Initializes a new instance of <see cref="InfrastructureModule" /> class.
        /// </summary>
        /// <param name="dbFactory">The database factory.</param>
        protected RepositoryModule(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <typeparam name="TEntity">Document Type</typeparam>
        /// <returns><seealso cref="IMongoCollection{TDocument}" /></returns>
        protected IMongoCollection<TEntity> GetCollection<TEntity>() where TEntity : Entity
        {
            return _dbFactory.Create().GetCollection<TEntity>(nameof(TEntity));
        }

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <typeparam name="TEntity">Document Type</typeparam>
        /// <param name="collectionName">Name of the collection.</param>
        /// <returns><seealso cref="IMongoCollection{TDocument}" /></returns>
        protected IMongoCollection<TEntity> GetCollection<TEntity>(string collectionName)
        {
            return _dbFactory.CreateCollectionIfNotExist<TEntity>(collectionName);

        }
    }
}
