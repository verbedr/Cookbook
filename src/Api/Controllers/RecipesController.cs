using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Halcyon.HAL;
using Common.ValueObjects;
using Cookbook.Contracts.Services;
using Cookbook.Contracts.Requests.RecipeRequests;
using Cookbook.Api.Models.RecipeModels;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route(RootUri)]
    public class RecipesController : Controller
    {
        public const string RootUri = "/api/recipes";
        IRecipeService _service;

        public RecipesController(IRecipeService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // GET api/values
        [HttpGet]
        [ProducesResponseType(typeof(OverviewModel), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public async Task<IActionResult> Overview(OverviewRequest request)
        {
            var result = await _service.Overview(request);
            return Ok(new HALResponse(new { })
                .AddLinks(new Link("self", $"{Request.Scheme}://{Request.Host}{RootUri}"))
                .AddLinks(new Link("home", $"{Request.Scheme}://{Request.Host}/"))
                .AddEmbeddedCollection("item", result.Items.Select(x => new HALResponse(x)
                    .AddLinks(new Link("self", $"{Request.Scheme}://{Request.Host}{RootUri}/{{Id}}")))));
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
