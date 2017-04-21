using GraphPlottinAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
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
        //public IHttpActionResult Get([FromUri]string function)
        //{
            
        //    CreateGraph = new GraphsCreator();
        //    List<Models.Point> XYList = new List<Models.Point>();
        //    XYList = CreateGraph.GetXY();
        //    return Ok(XYList);
        //}

        public IHttpActionResult Post([FromUri]string function, [FromBody] string param)
        {
            //JavaScriptSerializer jss = new JavaScriptSerializer();
            //RequestParameters parameter = jss.Deserialize<RequestParameters>(param);

            CreateGraph = new GraphsCreator();
            List<Models.Point> XYList = new List<Models.Point>();
            XYList = CreateGraph.GetXY();
            return Ok(XYList);
        }

    }
}
