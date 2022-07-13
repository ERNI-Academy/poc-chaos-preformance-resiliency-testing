
using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Generators.Interfaces;
using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Repositories;
using Microservices.GeneratingChaos.Services.Api.Domain.Entities;
using Microservices.GeneratingChaos.Services.Api.Infrastructure.Repository.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.GeneratingChaos.Services.Api.Infrastructure.Repository
{
    /// <summary>
    /// Class CityRepository.
    /// Implements the <see cref="Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Repositories.RepositoryBase{Microservices.GeneratingChaos.Services.Api.Domain.Entities.City}" />
    /// Implements the <see cref="Microservices.GeneratingChaos.Services.Api.Infrastructure.Repository.Interfaces.ICityRepository" />
    /// </summary>
    /// <seealso cref="Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Repositories.RepositoryBase{Microservices.GeneratingChaos.Services.Api.Domain.Entities.City}" />
    /// <seealso cref="Microservices.GeneratingChaos.Services.Api.Infrastructure.Repository.Interfaces.ICityRepository" />
    /// <inheridoc />
    public class CityRepository : RepositoryBase<City>, ICityRepository
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
        /// Initializes a new instance of the <see cref="CityRepository"/> class.
        /// </summary>
        /// <param name="mongoCollection">The mongo collection.</param>
        /// <param name="idGenerator">The identifier generator.</param>
        /// <param name="date">The date.</param>
        /// <exception cref="ArgumentNullException">idGenerator</exception>
        /// <exception cref="ArgumentNullException">date</exception>
        public CityRepository(IMongoCollection<City> mongoCollection, 
                              IIdGenerator idGenerator, 
                              IDate date):base(mongoCollection)
        {
            _idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
            _date = date ?? throw new ArgumentNullException(nameof(date));
        }

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <value>The query.</value>
        /// <inheridoc />
        public IMongoQueryable<City> Query { get => _collection.AsQueryable<City>(new AggregateOptions { AllowDiskUse = true }); }

        /// <summary>
        /// add as an asynchronous operation.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task&lt;City&gt;.</returns>
        /// <inheridoc />
        public async Task<City> AddAsync(City entity)
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
        /// <inheridoc />
        public async Task AddManyAsync(IEnumerable<City> entities)
        {
            foreach (var entity in entities)
            {
                var find = await _collection.FindAsync(item => item.CityId == entity.CityId).ConfigureAwait(false);
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
        /// <returns>Task&lt;IEnumerable&lt;City&gt;&gt;.</returns>
        /// <inheridoc />
        public async Task<IEnumerable<City>> GetAllAsync()
        {
            return await ExecuteAsync<IEnumerable<City>>(async () =>
            {
                var cursor = await _collection.FindAsync(c => true).ConfigureAwait(false);
                return await cursor.ToListAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// get by identifier as an asynchronous operation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;City&gt;.</returns>
        /// <inheridoc />
        public async Task<City> GetByIdAsync(Guid id)
        {
            return await ExecuteAsync<City>(async () =>
            {
                var cursor = await _collection.FindAsync(c => c.CityId == id).ConfigureAwait(false);
                return await cursor.FirstOrDefaultAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// remove as an asynchronous operation.
        /// </summary>
        /// <param name="id">The identifier.</param>
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
        public async Task UpdateAsync(City entity)
        {
            entity.Modified = _date.Now();
            await ExecuteAsync(() => _collection.FindOneAndReplaceAsync(f => f.Id == entity.Id, entity)).ConfigureAwait(false);
        }
    }
}
