using Common.Services;
using Cookbook.Contracts.Requests.RecipeRequests;
using Cookbook.Contracts.Requests.RecipeResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Repository;

namespace Cookbook.Services.Handlers.RecipeHandlers
{
    public class OverviewHandler : QueryRequestHandler<OverviewRequest, OverviewResponse>
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
