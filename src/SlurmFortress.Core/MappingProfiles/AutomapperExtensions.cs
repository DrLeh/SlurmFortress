using AutoMapper;
using System.Linq.Expressions;

namespace SlurmFortress.Core.MappingProfiles;

public static class AutomapperExtensions
{
    /// <summary>
    /// Ignore the field in the destination from mapping.
    /// </summary>
    public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(
        this IMappingExpression<TSource, TDestination> map, Expression<Func<TDestination, object>> selector)
    {
        map.ForMember(selector, config => config.Ignore());
        return map;
    }

    /// <summary>
    /// Ignore the validating that the given source member is not configured for mapping
    /// </summary>
    public static IMappingExpression<TSource, TDestination> IgnoreSourceValidation<TSource, TDestination>(
        this IMappingExpression<TSource, TDestination> map, Expression<Func<TSource, object>> selector)
    {
        map.ForSourceMember(selector, config => config.DoNotValidate());
        return map;
    }
}
