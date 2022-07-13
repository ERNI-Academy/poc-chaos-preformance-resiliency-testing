using Autofac;
using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Generators;
using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.Generators.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Microservices.GeneratingChaos.Services.Api.Infrastructure.AutofacModules
{
    /// <summary>
    /// Application Modile for Autofac
    /// </summary>
    /// <seealso cref="Autofac.Module" />
    public class ApplicationModule
        : Module
    {
        /// <summary>
        /// The configuration
        /// </summary>
        private IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationModule" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public ApplicationModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be
        /// registered.</param>
        /// <remarks>Note that the ContainerBuilder parameter is unique to this module.</remarks>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<IdGenerator>()
                   .As<IIdGenerator>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<Date>()
                   .As<IDate>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<HttpContextAccessor>()
                   .As<IHttpContextAccessor>()
                   .SingleInstance();
        }
    }
}
