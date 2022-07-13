using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Microservices.GeneratingChaos.BuildingBlocks.Extensions
{
    /// <summary>
    /// Class SwaggerRegistrationExtension.
    /// </summary>
    public static class SwaggerRegistrationExtension
    {
        /// <summary>
        /// Adds the swagger.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="version">The version.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services, string version, string title, string description)
        {
            services.AddSwaggerGen(options =>
            {
                options.SchemaFilter<EnumNamesSchemaFilter> ();
                options.SwaggerDoc(version,
                    new OpenApiInfo
                    {
                        Title = title,
                        Version = version,
                        Description = description
                    });

                // XML Documentation
                var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
            return services;
        }

        /// <summary>
        /// Uses the swagger.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="name">The name.</param>
        /// <param name="version">The version.</param>
        /// <returns>IApplicationBuilder.</returns>
        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, string name, string version)
        {
            app.UseSwagger()
               .UseSwaggerUI(c =>
               {
                   c.SwaggerEndpoint($"/swagger/{version}/swagger.json", name);
               });
            return app;
        }

        private class EnumNamesSchemaFilter : ISchemaFilter
        {
            private const string NAME = "x-enumNames";

            public void Apply(OpenApiSchema model, SchemaFilterContext context)
            {
                var typeInfo = context.Type;
                // Chances are something in the pipeline might generate this automatically at some point in the future
                // therefore it's best to check if it exists.
                if (typeInfo.IsEnum && !model.Extensions.ContainsKey(NAME))
                {
                    var names = Enum.GetNames(context.Type);
                    var arr = new OpenApiArray();
                    arr.AddRange(names.Select(name => new OpenApiString(name)));
                    model.Extensions.Add(NAME, arr);
                }
            }
        }
    }
}
