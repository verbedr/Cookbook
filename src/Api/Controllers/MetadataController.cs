using Common.ValueObjects;
using Cookbook.Api.Models.Metadata;
using Cookbook.Contracts.Requests.MetadataRequests;
using Cookbook.Contracts.Services;
using Halcyon.HAL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cookbook.Api.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class MetadataController : Controller
    {
        IMetadataService _service;

        public MetadataController(IMetadataService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet("products")]
        [ProducesResponseType(typeof(ProductsModel), 200)]
        public async Task<IActionResult> Products(ProductsRequest request)
        {
            var result = await _service.Products(request);
            return Ok(new HALResponse(new { })
                .AddLinks(new Link("self", $"{Request.Scheme}://{Request.Host}/api/recipes"))
                .AddLinks(new Link("home", $"{Request.Scheme}://{Request.Host}/"))
                .AddEmbeddedCollection("item", result.Items.Select(x => new HALResponse(x)
                    .AddLinks(new Link("self", $"{Request.Scheme}://{Request.Host}/api/products/{{Id}}")))));
        }
    }
}
