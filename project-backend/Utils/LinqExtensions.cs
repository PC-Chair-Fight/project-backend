using System;
using System.Collections.Generic;
using System.Linq;

namespace project_backend.Utils
{
    public static class LinqExtensions
    {
        // allows using conditions within a linq query
        public static IQueryable<TSource> If<TSource>(
            this IQueryable<TSource> source,
            bool condition,
            Func<IQueryable<TSource>, IQueryable<TSource>> @then,
            Func<IQueryable<TSource>, IQueryable<TSource>> @else = null
        ) => condition ? 
            @then(source) : 
            @else != null ? 
                @else(source) : 
                source;

        // allows using conditions within a linq query
        public static IEnumerable<TSource> If<TSource>(
            this IEnumerable<TSource> source,
            bool condition,
            Func<IEnumerable<TSource>, IEnumerable<TSource>> @then,
            Func<IEnumerable<TSource>, IEnumerable<TSource>> @else = null
        ) => condition ?
            @then(source) :
            @else != null ?
                @else(source) :
                source;
    }
}
