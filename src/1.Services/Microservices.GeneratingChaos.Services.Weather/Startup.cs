using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microservices.GeneratingChaos.BuildingBlocks.Extensions;
using Microservices.GeneratingChaos.BuildingBlocks.Infrastructure.DataBase;
using Microservices.GeneratingChaos.Services.Weather.Domain.Entities;
using Microservices.GeneratingChaos.Services.Weather.Infrastructure.AutofacModules;
using Microservices.GeneratingChaos.Services.Weather.Infrastructure.Mappers;
using Microservices.GeneratingChaos.Services.Weather.Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Microservices.GeneratingChaos.Services.Weather
{
    /// <summary>
    /// Class Startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>IServiceProvider.</returns>
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCustomMvc<Startup>()
                    .AddSwagger("v1", "Microservices Generating Chaos Weather Service", "Sample weather service for netconf 2019")
                    .AddOptions();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration["RedisCacheUrl"];
                options.InstanceName = Configuration["RedisCacheInstance"];
            });

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            var factory = new DbFactory(Configuration.Get<DbConfiguration>());
            var assembly = typeof(Startup).Assembly;
            containerBuilder.RegisterModule(new MediatorModule());
            containerBuilder.RegisterModule(new ApplicationModule(Configuration));
            containerBuilder.RegisterRepositoryModule(assembly, factory);
            containerBuilder.RegisterProfiles<WeatherForecastProfile>(assembly);
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
            app.UseSwagger("Microservices Generating Chaos Weather Service V1", "v1");
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
            var weatherSeedFile = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().FullName), "Seed", "Weather.json");
            if (File.Exists(weatherSeedFile))
            {
                var weatherRepository = app.ApplicationServices.GetRequiredService<IWeatherRepository>();
                var weathers = JsonConvert.DeserializeObject<List<WeatherForecast>>(File.ReadAllText(weatherSeedFile, System.Text.Encoding.UTF7));
                weatherRepository.AddManyAsync(weathers).Wait();
                                
                lifetime.ApplicationStarted.Register(() =>
                {
                    var weatherEncoded = Encoding.UTF8.GetBytes(File.ReadAllText(weatherSeedFile, System.Text.Encoding.UTF8));
                    var options = new DistributedCacheEntryOptions()
                                            .SetSlidingExpiration(TimeSpan.FromSeconds(30));
                    cache.Set("Weather", weatherEncoded, options);
                });

            }
        }
    }
}
