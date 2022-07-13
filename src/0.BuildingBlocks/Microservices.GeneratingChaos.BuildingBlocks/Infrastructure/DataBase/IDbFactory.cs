using MongoDB.Driver;

namespace Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.DataBase
{
    /// <summary>
    /// The interface for database factory.
    /// </summary>
    public interface IDbFactory
    {
        /// <summary>
        /// The method for creating the mongoDB instance.
        /// </summary>
        /// <returns>The mongo db object.</returns>
        IMongoDatabase Create();

        /// <summary>
        /// The method for creating collections in the mongo database.
        /// </summary>
        /// <typeparam name="T">The type of mongo db entity.</typeparam>
        /// <param name="collectionName">The collection name for the database.</param>
        /// <returns>The mongo collection created.</returns>
        IMongoCollection<T> CreateCollectionIfNotExist<T>(string collectionName);

        /// <summary>
        /// The method for creating collections in the mongo database.
        /// </summary>
        /// <typeparam name="T">The type of mongo db entity.</typeparam>
        /// <param name="collectionName">The collection name for the database.</param>
        /// <param name="registerMapId">The register map identifier flag.</param>
        /// <returns>The mongo collection created.</returns>
        IMongoCollection<T> CreateCollectionIfNotExist<T>(string collectionName, bool registerMapId);
    }
}
