
using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Generators.Interfaces;
using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Repositories;
using Microservices.GeneratingChaos.Services.Weather.Domain.Entities;
using Microservices.GeneratingChaos.Services.Weather.Infrastructure.Repository.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.GeneratingChaos.Services.Weather.Infrastructure.Repository
{
    /// <summary>
    /// Class WeatherForecastRepository.
    /// Implements the <see cref="Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Repositories.RepositoryBase{Microservices.GeneratingChaos.Services.Weather.Domain.Entities.WeatherForecast}" />
    /// Implements the <see cref="Microservices.GeneratingChaos.Services.Weather.Infrastructure.Repository.Interfaces.IWeatherRepository" />
    /// </summary>
    /// <seealso cref="Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Repositories.RepositoryBase{Microservices.GeneratingChaos.Services.Weather.Domain.Entities.WeatherForecast}" />
    /// <seealso cref="Microservices.GeneratingChaos.Services.Weather.Infrastructure.Repository.Interfaces.IWeatherRepository" />
    /// <inheridoc />
    public class WeatherForecastRepository : RepositoryBase<WeatherForecast>, IWeatherRepository
    {
        /// <summary>
        /// The identifier generator
        /// </summary>
        private readonly IIdGenerator _idGenerator;
        /// <summary>
        /// The date
        /// </summary>
        private readonly IDate _date;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherForecastRepository"/> class.
        /// </summary>
        /// <param name="mongoCollection">The mongo collection.</param>
        /// <param name="idGenerator">The identifier generator.</param>
        /// <param name="date">The date.</param>
        /// <exception cref="ArgumentNullException">idGenerator</exception>
        /// <exception cref="ArgumentNullException">date</exception>
        public WeatherForecastRepository(IMongoCollection<WeatherForecast> mongoCollection,
                              IIdGenerator idGenerator,
                              IDate date) : base(mongoCollection)
        {
            _idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
            _date = date ?? throw new ArgumentNullException(nameof(date));
        }

        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        /// <value>The query.</value>
        /// <inheridoc />
        public IMongoQueryable<WeatherForecast> Query { get => _collection.AsQueryable<WeatherForecast>(new AggregateOptions { AllowDiskUse = true }); }

        /// <summary>
        /// add as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The added entity</returns>
        /// <inheridoc />
        public async Task<WeatherForecast> AddAsync(WeatherForecast entity)
        {
            entity.Id = _idGenerator.GenerateNewGuidId();
            entity.Created = _date.Now();
            entity.Modified = entity.Created;
            await ExecuteAsync(() => _collection.InsertOneAsync(entity)).ConfigureAwait(false);
            return entity;
        }

        /// <summary>
        /// add many as an asynchronous operation.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns>Task.</returns>
        /// <inheridoc />
        public async Task AddManyAsync(IEnumerable<WeatherForecast> entities)
        {
            foreach (var entity in entities)
            {
                var find = await _collection.FindAsync(item => item.CityId == entity.CityId &&
                                                               item.Date == entity.Date).ConfigureAwait(false);
                var findEntities = await find.ToListAsync();

                if (findEntities == null || !findEntities.Any())
                {
                    await AddAsync(entity).ConfigureAwait(false);
                }
                else
                {
                    await UpdateAsync(entity).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// get all as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;IEnumerable&lt;TEntity&gt;&gt;.</returns>
        /// <inheridoc />
        public async Task<IEnumerable<WeatherForecast>> GetAllAsync()
        {
            return await ExecuteAsync<IEnumerable<WeatherForecast>>(async () =>
            {
                var cursor = await _collection.FindAsync(c => true).ConfigureAwait(false);
                return await cursor.ToListAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the by city identifier.
        /// </summary>
        /// <param name="cityId">The city identifier.</param>
        /// <returns>Task&lt;IEnumerable&lt;WeatherForecast&gt;&gt;.</returns>
        public async Task<IEnumerable<WeatherForecast>> GetByCityId(Guid cityId)
        {
            return await ExecuteAsync<IEnumerable<WeatherForecast>>(async () =>
            {
                var cursor = await _collection.FindAsync(c => c.CityId == cityId).ConfigureAwait(false);
                return await cursor.ToListAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// get by identifier as an asynchronous operation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;TEntity&gt;.</returns>
        /// <inheridoc />
        public async Task<WeatherForecast> GetByIdAsync(Guid id)
        {
            return await ExecuteAsync<WeatherForecast>(async () =>
            {
                var cursor = await _collection.FindAsync(c => c.Id == id).ConfigureAwait(false);
                return await cursor.FirstOrDefaultAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// remove as an asynchronous operation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        /// <inheridoc />
        public async Task RemoveAsync(Guid id)
        {
            await ExecuteAsync(() => _collection.FindOneAndDeleteAsync(f => f.CityId == id)).ConfigureAwait(false);
        }

        /// <summary>
        /// update as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <inheridoc />
        public async Task UpdateAsync(WeatherForecast entity)
        {
            entity.Modified = _date.Now();
            await ExecuteAsync(() => _collection.FindOneAndReplaceAsync(f => f.Id == entity.Id, entity)).ConfigureAwait(false);
        }
    }
}
