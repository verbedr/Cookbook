using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Common
{
    public static class Extensions
    {
        public static async Task<IEnumerable<T1>> SelectManyAsync<T, T1>(this IEnumerable<T> enumeration, Func<T, Task<IEnumerable<T1>>> func)
        {
            return (await Task.WhenAll(enumeration.Select(func))).SelectMany(s => s);
        }

        public static T[] CastToArray<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));

            return enumerable as T[] ?? enumerable.ToArray();
        }

        public async static Task<List<T>> MaterializeQueryAsync<T>(this IQueryable query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            return await query.ToListAsync();
        }
    }
}
