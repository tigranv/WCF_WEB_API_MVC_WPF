using GraphPlottinAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Shapes;

namespace GraphPlottinAPI.Controllers
{
    public class PlotGraphController : ApiController
    {
        private GraphsCreator CreateGraph;

        public IHttpActionResult Post([FromUri]string function, [FromBody] RequestParameters param)
        {
            Thread.Sleep(5000);
            CreateGraph = new GraphsCreator();
            List<Models.Point> XYList = new List<Models.Point>();
            XYList = CreateGraph.GetXY(function, param);
            return Ok(XYList);
        }

    }
}
