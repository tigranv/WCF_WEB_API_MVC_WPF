using GraphPlottinAPI.Models;
using System.Collections.Generic;
using System.Threading;
using System.Web.Http;

namespace GraphPlottinAPI.Controllers
{
    public class PlotGraphController : ApiController
    {
        private GraphsCreator CreateGraph;

        public IHttpActionResult Post([FromUri]string function, [FromBody] RequestParameters param)
        {
            //Thread.Sleep(5000);
            CreateGraph = new GraphsCreator();
            List<Point> XYList = new List<Point>();
            XYList = CreateGraph.GetXY(function, param);
            return Ok(XYList);
        }

    }
}
