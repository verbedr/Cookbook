using Common.Services;
using Cookbook.Contracts.Requests.RecipeRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Repository;
using Cookbook.Contracts.Responses.RecipeResponses;

namespace Cookbook.Services.Handlers.RecipeHandlers
{
    public class OverviewHandler : QueryHandler<OverviewRequest, OverviewResponse>
    {
        public OverviewHandler(IUnitOfWork context) : base(context)
        {
        }

        protected override Task<OverviewResponse> QueryAsync(OverviewRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
