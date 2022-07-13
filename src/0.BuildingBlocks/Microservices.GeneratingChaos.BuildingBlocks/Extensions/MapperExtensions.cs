using System;
using AutoMapper;

namespace Microservices.GeneratingChaos.BuildingBlocks.Extensions
{
    /// <summary>
    /// AutoMapper extension
    /// </summary>
    public static class MapperExtensions
    {
        /// <summary>
        /// Ignores the unmapped.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <exception cref="ArgumentNullException">profile</exception>
        public static void IgnoreUnmapped(this IProfileExpression profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException(nameof(profile));
            }
            profile.ForAllMaps(IgnoreUnmappedProperties);
        }

        /// <summary>
        /// Ignores the unmapped.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="filter">The filter.</param>
        /// <exception cref="ArgumentNullException">profile</exception>
        public static void IgnoreUnmapped(this IProfileExpression profile, Func<TypeMap, bool> filter)
        {
            if (profile == null)
            {
                throw new ArgumentNullException(nameof(profile));
            }
            profile.ForAllMaps((map, expr) =>
            {
                if (filter(map))
                {
                    IgnoreUnmappedProperties(map, expr);
                }
            });
        }

        /// <summary>
        /// Ignores the unmapped.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="src">The source.</param>
        /// <param name="dest">The dest.</param>
        public static void IgnoreUnmapped(this IProfileExpression profile, Type src, Type dest)
        {
            profile.IgnoreUnmapped((TypeMap map) => map.SourceType == src && map.DestinationType == dest);
        }

        /// <summary>
        /// Ignores the unmapped.
        /// </summary>
        /// <typeparam name="TSrc">The type of the source.</typeparam>
        /// <typeparam name="TDest">The type of the dest.</typeparam>
        /// <param name="profile">The profile.</param>
        public static void IgnoreUnmapped<TSrc, TDest>(this IProfileExpression profile)
        {
            profile.IgnoreUnmapped(typeof(TSrc), typeof(TDest));
        }

        /// <summary>
        /// Ignores the unmapped properties.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="expr">The expr.</param>
        private static void IgnoreUnmappedProperties(TypeMap map, IMappingExpression expr)
        {
            foreach (string propName in map.GetUnmappedPropertyNames())
            {
                if (map.SourceType.GetProperty(propName) != null)
                {
                    expr.ForSourceMember(propName, opt => opt.DoNotValidate());
                }
                if (map.DestinationType.GetProperty(propName) != null)
                {
                    expr.ForMember(propName, opt => opt.Ignore());
                }
            }
        }
    }
}
