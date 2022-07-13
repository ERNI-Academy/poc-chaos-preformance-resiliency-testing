using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.DataBase
{
    /// <summary>
    /// Contains all the helper methods that interact with MongoDb.
    /// </summary>
    /// <seealso cref="Agrira.Api.Infrastructure.DataBase.IDbFactory" />
    public class DbFactory : IDbFactory
    {
        /// <summary>
        /// The configuration
        /// </summary>
        private readonly DbConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbFactory"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="ArgumentNullException">configuration</exception>
        public DbFactory(DbConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Connects to the MongoDB and creates, if not exists, the MongoDb database object.
        /// </summary>
        /// <returns>Return the MongoDB database object</returns>
        public IMongoDatabase Create()
        {
            var client = new MongoClient(_configuration.DbConnectionString);
            return client.GetDatabase(_configuration.DatabaseName);
        }

        /// <summary>
        /// Creates a MongoDb collection if it does not exists.
        /// </summary>
        /// <typeparam name="T">The type of the collection to create</typeparam>
        /// <param name="collectionName">Name of the collection to create</param>
        /// <returns>Returns the MongoCollection</returns>
        public IMongoCollection<T> CreateCollectionIfNotExist<T>(string collectionName)
        {
            return CreateCollectionIfNotExist<T>(collectionName, false);
        }

        /// <summary>
        /// Creates a MongoDb collection if it does not exists.
        /// </summary>
        /// <typeparam name="T">The type of the collection to create</typeparam>
        /// <param name="collectionName">Name of the collection to create</param>
        /// <param name="registerMapId">Optional. Set to true if the entity needs to map to an ID property</param>
        /// <returns>Returns the MongoCollection</returns>
        public IMongoCollection<T> CreateCollectionIfNotExist<T>(string collectionName, bool registerMapId)
        {
            IMongoCollection<T> result = null;
            try
            {
                var database = Create();
                var filter = new BsonDocument("name", collectionName);
                var collectionExists = database.ListCollectionNames(new ListCollectionNamesOptions { Filter = filter }).Any();
                if (collectionExists)
                {

                    result = database.GetCollection<T>(collectionName);
                }
                else
                {
                    database.CreateCollection(collectionName);


                    if (registerMapId && !BsonClassMap.IsClassMapRegistered(typeof(T)))
                    {
                        BsonClassMap.RegisterClassMap<T>(classMap =>
                        {
                            classMap.AutoMap();
                            classMap.MapIdProperty("Id").SetIdGenerator(StringObjectIdGenerator.Instance);
                        });
                    }

                    result = database.GetCollection<T>(collectionName);
                    Type documentType = typeof(T);
                    foreach (var prop in documentType.GetProperties().Where(item => item.PropertyType == typeof(GeoJson2DGeographicCoordinates)))
                    {
                        var indexModel = new CreateIndexModel<T>(Builders<T>.IndexKeys.Geo2DSphere(prop.Name));
                        Task.Run(async () => { await result.Indexes.CreateOneAsync(indexModel).ConfigureAwait(false); }).GetAwaiter().GetResult();
                    }
                }

            }
            catch
            {
                result = null;
            }
            return result;
        }
    }
}
