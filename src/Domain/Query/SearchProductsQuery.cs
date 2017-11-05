using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cookbook.Domain.Query.Filters;
using Cookbook.Domain.Query.Projections;
using Cookbook.Domain.Entities;

namespace Cookbook.Domain.Query
{
    internal class SearchProductsQuery : IQuery<SearchProductsFilter, SearchProductsProjection>
    {
        private readonly CookbookFactory _factory;

        public SearchProductsQuery(CookbookFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public Task<IQueryable<SearchProductsProjection>> ExecuteAsync(SearchProductsFilter request)
        {
            return Task.FromResult(_factory.Query<Product>().Select(x => new SearchProductsProjection { Name = x.Name }));
        }
    }
}
