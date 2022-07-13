using Autofac;
using System;
using System.Reflection;
using System.Linq;
using AutoMapper;
using System.Collections.Generic;
using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.AutofacModules;
using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.DataBase;

namespace Microservices.GeneratingChaos.BuildingBlocks.Extensions
{
    /// <summary>
    /// The class for extending service collection to support swagger.
    /// </summary>
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// The method for creating a configuration.
        /// </summary>
        /// <typeparam name="Type">The type of the type.</typeparam>
        /// <param name="lifetimeScope">The lifetimescope.</param>
        /// <returns>The mapper configuration.</returns>
        public static MapperConfiguration CreateProfilesConfiguration<Type>(ILifetimeScope lifetimeScope)
        {
            var profiles = GetProfiles<Type>(lifetimeScope);
            return new MapperConfiguration(cfg =>
            {
                cfg.ConstructServicesUsing(lifetimeScope.Resolve);
                cfg.IgnoreUnmapped();
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            });
        }

        /// <summary>
        /// The profile collection retrieval method
        /// </summary>
        /// <typeparam name="T">The type of class where profiles coexist in same assembly.</typeparam>
        /// <param name="scope">The scope.</param>
        /// <returns>The profile enumerable.</returns>
        /// <exception cref="ArgumentNullException">scope</exception>
        public static IEnumerable<Profile> GetProfiles<T>(ILifetimeScope scope)
        {
            if (scope == null)
            {
                throw new ArgumentNullException(nameof(scope));
            }

            return from t in typeof(T).Assembly.GetTypes()
                   where typeof(Profile).IsAssignableFrom(t) && scope.IsRegistered(t)
                   select scope.Resolve(t) as Profile;
        }

        /// <summary>
        /// The profile collection retrieval method
        /// </summary>
        /// <typeparam name="T">The type of class where profiles coexist in same assembly.</typeparam>
        /// <returns>The profile enumerable.</returns>
        public static IEnumerable<Type> GetProfilesTypes<T>()
        {
            return from t in typeof(T).Assembly.GetTypes()
                   where typeof(Profile).IsAssignableFrom(t)
                   select t;
        }

        /// <summary>
        /// Extension method for adding infrastructure modules to container builder
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="factory">The factory.</param>
        /// <exception cref="ArgumentNullException">assembly</exception>
        public static void RegisterRepositoryModule(this ContainerBuilder builder, Assembly assembly, DbFactory factory)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }
            var parameters = new object[] { factory };
            var modules = assembly.GetTypes()
                              .Where(p => typeof(RepositoryModule).IsAssignableFrom(p) && !p.IsAbstract)
                              .Select(p => (RepositoryModule)Activator.CreateInstance(p, parameters));

            foreach (var module in modules)
            {
                builder.RegisterModule(module);
            }
        }

        /// <summary>
        /// The register profile method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="assembly">The assembly.</param>
        public static void RegisterProfiles<T>(this ContainerBuilder builder, Assembly assembly)
        {
            var profiles = GetProfilesTypes<T>();
            builder.RegisterTypes(profiles.ToArray()).InstancePerLifetimeScope();

            builder.Register(ctx => { return CreateProfilesConfiguration<T>(ctx.Resolve<ILifetimeScope>()); }).As<IConfigurationProvider>().InstancePerLifetimeScope();
            builder.Register(
              ctx =>
              {
                  var scope = ctx.Resolve<ILifetimeScope>();
                  return new Mapper(
                    ctx.Resolve<IConfigurationProvider>(),
                    scope.Resolve);
              })
              .As<IMapper>()
              .InstancePerLifetimeScope();
        }
    }
}
