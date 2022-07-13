using Autofac;
using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.AutofacModules;
using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.DataBase;
using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Generators.Interfaces;
using Microservices.GeneratingChaos.Services.Api.Domain.Entities;
using Microservices.GeneratingChaos.Services.Api.Infrastructure.Repository;
using Microservices.GeneratingChaos.Services.Api.Infrastructure.Repository.Interfaces;

namespace Microservices.GeneratingChaos.Services.Api.Infrastructure.AutofacModules
{
    /// <summary>
    /// Repository registration module
    /// </summary>
    /// <seealso cref="Agrira.Api.Infrastructure.Autofac.RepositoryModule" />
    public class RepositoryModules : RepositoryModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryModules" /> class.
        /// </summary>
        /// <param name="dbFactory">The database factory.</param>
        public RepositoryModules(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        /// <summary>
        /// Register all repositories using <see cref="ContainerBuilder" />
        /// </summary>
        /// <param name="builder">Builder to register all repositories</param>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.Register(ctx => { return new CityRepository(GetCollection<City>("City"), 
                                                                ctx.Resolve<IIdGenerator>(), 
                                                                ctx.Resolve<IDate>()); })
                   .As<ICityRepository>()
                   .InstancePerLifetimeScope();
        }
    }
}
