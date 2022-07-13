using AutoMapper;
using Microservices.GeneratingChaos.Services.Weather.Domain.CommandResponses;
using Entities=Microservices.GeneratingChaos.Services.Weather.Domain.Entities;
using System.Collections;
using System.Collections.Generic;
using Models=Microservices.GeneratingChaos.Services.Weather.Domain.Models;

namespace Microservices.GeneratingChaos.Services.Weather.Infrastructure.Mappers
{
    /// <summary>
    /// Class WeatherForecastProfile.
    /// Implements the <see cref="AutoMapper.Profile" />
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class WeatherForecastProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherForecastProfile"/> class.
        /// </summary>
        public WeatherForecastProfile()
        {
            CreateMap<ReadWeatherCommandResponse, List<Models.WeatherForecast>>().ConstructUsing(src=> src.ResponseDto);
            CreateMap<Entities.WeatherForecast, Models.WeatherForecast>();
            CreateMap<List<Entities.WeatherForecast>, ReadWeatherCommandResponse>()
                .ForMember(m=> m.ResponseDto, opt=> opt.MapFrom(src => src));
        }
    }
}
