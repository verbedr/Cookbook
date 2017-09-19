using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cookbook.Domain.Query.Filters;
using Cookbook.Domain.Query.Projections;

namespace Cookbook.Domain.Query
{
    internal class SearchProductsQuery : IQuery<SearchProductsFilter, SearchProductsProjection>
    {
        public Task<SearchProductsProjection> ExecuteAsync(SearchProductsFilter request)
        {
            throw new NotImplementedException();
        }
    }
}
