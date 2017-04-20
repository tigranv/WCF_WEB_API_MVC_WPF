using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GraphPlottinAPI.Controllers
{
    public class PlotGraphController : ApiController
    {
        // GET: api/PlotGraph
        public IHttpActionResult Get()
        {
            string result = "Please send parameters to get your plot";
            return Ok(result);
        }

        // GET: api/PlotGraph/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/PlotGraph
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/PlotGraph/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/PlotGraph/5
        public void Delete(int id)
        {
        }
    }
}
