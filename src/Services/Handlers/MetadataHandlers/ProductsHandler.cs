using Common.Services;
using Cookbook.Contracts.Requests.MetadataRequests;
using Cookbook.Contracts.Responses.MetadataResponses;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Common.Repository;
using Cookbook.Domain.Query.Filters;
using Common.Domain;
using Cookbook.Domain.Query.Projections;
using AutoMapper;
using Cookbook.Contracts.PropertyBags;

namespace Cookbook.Services.Handlers.MetadataHandlers
{
    internal class ProductsHandler : QueryRequestHandler<ProductsRequest, ProductsResponse>
    {
        private readonly IMapper _mapper;
        private readonly IQuery<SearchProductsFilter, SearchProductsProjection> _query;

        protected ProductsHandler(IUnitOfWork context, IMapper mapper, IQuery<SearchProductsFilter, SearchProductsProjection> query) : base(context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _query = query ?? throw new ArgumentNullException(nameof(query));
        }

        protected override async Task<ProductsResponse> QueryAsync(ProductsRequest request)
        {
            var query = await _query.ExecuteAsync(new SearchProductsFilter());
            var result = await query.ToListAsync();
            return new ProductsResponse { Items = _mapper.Map<ProductSummary[]>(result) };
        }
    }
}
