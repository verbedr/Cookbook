using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Halcyon.HAL;
using Common.ValueObjects;
using Cookbook.Api.Infrastructure;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/recipes")]
    public class RecipesController : Controller
    {
        // GET api/values
        [HttpGet]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        public IActionResult Get(bool includeDetails)
        {
            var items = new string[] { "value1", "value2" }.Select(x => new HALResponse(x)
                                .AddLinks(new Link("self", $"{Request.Scheme}://{Request.Host}/api/recipe/{{Id}}"))).ToList();
            return Ok(new HALResponse(new { })
                            .AddLinks(new Link("self", $"{Request.Scheme}://{Request.Host}/api/recipes"))
                            .AddLinks(new Link("home", $"{Request.Scheme}://{Request.Host}/"))
                            .AddEmbeddedCollection("item", items));
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
