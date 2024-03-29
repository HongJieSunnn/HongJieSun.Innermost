﻿using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEnumerableExtensions
{
    public static class MongoDBIEnumerableExtensions
    {
        public static FilterDefinition<T> CombineFilterDefinitions<T>(this IEnumerable<FilterDefinition<T>> filterDefinitions)
        {
            return filterDefinitions.Aggregate((filterBefore, filterNext) => filterBefore & filterNext);
        }

        public static FilterDefinition<T> CombineFilterDefinitions<T>(this IEnumerable<FilterDefinition<T>> filterDefinitions,FilterDefinition<T> firstDefinition)
        {
            if (!filterDefinitions.Any())
                return firstDefinition;

            var filter = firstDefinition;

            return filter & filterDefinitions.Aggregate((filterBefore, filterNext) => filterBefore & filterNext);
        }
    }
}
