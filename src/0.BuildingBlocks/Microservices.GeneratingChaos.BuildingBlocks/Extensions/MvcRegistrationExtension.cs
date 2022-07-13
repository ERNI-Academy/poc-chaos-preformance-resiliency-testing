using AutoMapper;
using FluentValidation.AspNetCore;
using Microservices.GeneratingChaos.BuildingBlocks.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.GeneratingChaos.BuildingBlocks.Extensions
{
    /// <summary>
    /// Class MvcRegistrationExtension.
    /// </summary>
    public static class MvcRegistrationExtension
    {
        /// <summary>
        /// Adds the custom MVC.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services">The services.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddCustomMvc<T>(this IServiceCollection services)
        {
            services.AddAutoMapper();
            // Add framework services.
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            })
                .AddNewtonsoftJson()
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<T>())
                .AddControllersAsServices();  //Injecting Controllers themselves thru DI
                                              //For further info see: http://docs.autofac.org/en/latest/integration/aspnetcore.html#controllers-as-services

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            return services;
        }
    }
}
