using GraphPlottinAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Windows;
using System.Windows.Shapes;

namespace GraphPlottinAPI.Controllers
{
    public class PlotGraphController : ApiController
    {
        private GraphsCreator CreateGraph;

        // GET: api/PlotGraph
        public IHttpActionResult Get()
        {
            string result = "Please send parameters to get your plot";
            return Ok(result);
        }

        // GET: api/PlotGraph/5
        public IHttpActionResult Get(int id)
        {
            CreateGraph = new GraphsCreator();
            List<Models.Point> XYList = new List<Models.Point>();
            XYList = CreateGraph.GetXY();
            return Ok(XYList);
        }

        
    }
}
