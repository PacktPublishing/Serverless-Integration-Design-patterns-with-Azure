using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Gremlin.Net.Driver;
using GremlinGraphAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GremlinGraphAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/GraphVertex")]
    public class GraphVertexController : Controller
    {
        private readonly GremlinClient _client;

        public GraphVertexController(GremlinClient client)
        {
            _client = client;
        }

        // POST api/GraphVertex Create Vertex with JSON string 
        [HttpPost]
        public async Task<ActionResult> PostCosmosGraphVertex(GraphVertex graphcontent)
        {
            try
            {
            var result = (dynamic)null;
            dynamic content = JsonConvert.DeserializeObject<dynamic>(graphcontent.Content);
            string type = graphcontent.VertexType;

            StringBuilder builder = new StringBuilder();
                foreach (var item in content)
                {
                    string query = $".property('{item.Name}','{item.Value}')";
                    builder.Append(query);
                }   
            var queryString = string.Concat($"g.addV('{type}')",builder);
            result = await _client.SubmitAsync<dynamic>(queryString);

                var responsemessage = JsonConvert.SerializeObject(result);
                return this.Content(responsemessage, "application/json");
            }

            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }
        // Get Graph Node with Property

        [HttpGet]
        public async Task<ActionResult> GetVertexByProperty(string Property, string Value )
        {
            try
            {
                var queryString = $"g.V().has('{Property}', '{Value}')";
                dynamic result = await _client.SubmitAsync<dynamic>(queryString);
                var responsemessage = JsonConvert.SerializeObject(result);
                return this.Content(responsemessage,"application/json");
            }
            catch (Exception ex)
            {

               return Content(ex.Message);
            }
        }


        [HttpPatch]
        public async Task<object> PatchGraphVertex(GraphVertex graphcontent)
        {
            try
            {
                var result = (dynamic)null;
                dynamic content = JsonConvert.DeserializeObject<dynamic>(graphcontent.Content);
                string type = graphcontent.VertexType;

                StringBuilder builder = new StringBuilder();
                foreach (var item in content)
                {
                    string query = $".property('{item.Name}','{item.Value}')";
                    builder.Append(query);
                }
                var queryString = string.Concat($"g.addV('{type}')", builder);
                result = await _client.SubmitAsync<dynamic>(queryString);

                var responsemessage = JsonConvert.SerializeObject(result);
                return this.Content(responsemessage, "application/json");
            }

            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }

        [HttpDelete]
        public async Task<object> DeleteGraphVertex(string Property, string Value)
        {
            try
            {
                var queryString = $"g.V().has('{Property}', '{Value}').Drop()";
                dynamic result = await _client.SubmitAsync<dynamic>(queryString);
                var responsemessage = JsonConvert.SerializeObject(result);
                return this.Content(responsemessage, "application/json");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
