using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microservices.GeneratingChaos.BuildingBlocks.Extensions;
using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.DataBase;
using Microservices.GeneratingChaos.Services.Api.Domain.Entities;
using Microservices.GeneratingChaos.Services.Api.Infrastructure.AutofacModules;
using Microservices.GeneratingChaos.Services.Api.Infrastructure.Repository.Interfaces;
using Microservices.GeneratingChaos.Services.Api.Infrastructure.Services;
using Microservices.GeneratingChaos.Services.Api.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Microservices.GeneratingChaos.Services.Api
{
    /// <summary>
    /// Class Startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>IServiceProvider.</returns>
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCustomMvc<Startup>()
                    .AddSwagger("v1", "Microservices Generating Chaos Api", "Sample API for Chaos example")
                    .AddOptions();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration["RedisCacheUrl"];
                options.InstanceName = Configuration["RedisCacheInstance"];
            });

            services.AddHttpClient<IWeatherService, WeatherService>(client =>
            {
                client.BaseAddress = new Uri(Configuration["WeatherServiceUrl"]);
            }).SetHandlerLifetime(TimeSpan.FromMinutes(5));

            services.AddHttpClient<ISunService, SunService>(client =>
            {
                client.BaseAddress = new Uri(Configuration["WeatherServiceUrl"]);
            }).SetHandlerLifetime(TimeSpan.FromMinutes(5));

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            var factory = new DbFactory(Configuration.Get<DbConfiguration>());
            var assembly = typeof(Startup).Assembly;
            containerBuilder.RegisterModule(new MediatorModule());
            containerBuilder.RegisterModule(new ApplicationModule(Configuration));
            containerBuilder.RegisterRepositoryModule(assembly, factory);
            return new AutofacServiceProvider(containerBuilder.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="cache">The cache.</param>
        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env,
                              ILoggerFactory loggerFactory,
                              IHostApplicationLifetime lifetime,
                              IDistributedCache cache)
        {
            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                loggerFactory.CreateLogger<Startup>()
                             .LogDebug("Using PATH BASE '{pathBase}'", pathBase);
                app.UsePathBase(pathBase);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });
            app.UseSwagger("Microservices Generating Chaos Api V1", "v1");
            SeedData(app, lifetime, cache);
        }

        /// <summary>
        /// Seeds the data.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="cache">The cache.</param>
        private void SeedData(IApplicationBuilder app,
                              IHostApplicationLifetime lifetime,
                              IDistributedCache cache)
        {
            var citySeedFile = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().FullName), "Seed", "CitySeeder.json");
            if (File.Exists(citySeedFile))
            {
                var cityRepository = app.ApplicationServices.GetRequiredService<ICityRepository>();
                var cities = JsonConvert.DeserializeObject<List<City>>(File.ReadAllText(citySeedFile, Encoding.UTF7));
                cityRepository.AddManyAsync(cities).Wait();

                lifetime.ApplicationStarted.Register(() =>
                {
                    var weatherEncoded = Encoding.UTF8.GetBytes(File.ReadAllText(citySeedFile, Encoding.UTF8));
                    var options = new DistributedCacheEntryOptions()
                                            .SetSlidingExpiration(TimeSpan.FromSeconds(20));
                    cache.Set("Cities", weatherEncoded, options);
                });
            }
        }
    }
}
