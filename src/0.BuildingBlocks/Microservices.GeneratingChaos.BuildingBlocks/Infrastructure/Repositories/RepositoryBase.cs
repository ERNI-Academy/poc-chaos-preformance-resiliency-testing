using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.SeedWork;
using MongoDB.Driver;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;
using System;
using System.Threading.Tasks;

namespace Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Repositories
{
    /// <summary>
    /// Class RepositoryBase.
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    public class RepositoryBase<TEntity> where TEntity : Entity
    {
        /// <summary>
        /// The collection
        /// </summary>
        protected readonly IMongoCollection<TEntity> _collection;

        /// <summary>
        /// Initializes a new instance of the repository base class.
        /// </summary>
        /// <param name="mongoCollection">The mongo collection.</param>
        protected RepositoryBase(IMongoCollection<TEntity> mongoCollection)
        {
            _collection = mongoCollection;
        }

        /// <summary>Creates the asynchronous retry policy.</summary>
        /// <returns>The <seealso cref="AsyncRetryPolicy"/> object</returns>
        public AsyncRetryPolicy CreateAsyncRetryPolicy()
        {
            var delay = Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(2), retryCount: 5);
            return Policy.Handle<Exception>().WaitAndRetryAsync(delay);
        }

        /// <summary>Executes the parameter function asynchronously.</summary>
        /// <param name="func">The function.</param>
        /// <returns>The Task</returns>
        protected Task ExecuteAsync(Func<Task> func)
        {
            var policy = CreateAsyncRetryPolicy();
            return policy.ExecuteAsync(func);
        }

        /// <summary>Executes the parameter function asynchronously.</summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function.</param>
        /// <returns>The Task</returns>
        protected Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> func)
        {
            var policy = CreateAsyncRetryPolicy();
            return policy.ExecuteAsync<TResult>(func);
        }
    }
}
