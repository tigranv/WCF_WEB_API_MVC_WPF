using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebApi_Client_For_WCF.ServiceReference1;

namespace WebApi_Client_For_WCF.Controllers
{
    public class MainController : ApiController
    {
        ServiceIOClient proxy = new ServiceIOClient();

        public IEnumerable<string> GetAllFiles(string dirName)
        {
            return proxy.GetAllFilesAsync(dirName).Result;
        }

        public string Get(string dirName, string fileName)
        {
            return proxy.GetAsync(dirName, fileName).Result;
        }

        public void Put([FromUri]string dirName, [FromUri]string fileName, [FromBody]string value)
        {
            proxy.PutAsync(dirName, fileName, value);
        }

        public void Post([FromUri]string dirName, [FromUri]string fileName, [FromBody]string value)
        {
            proxy.PostAsync(dirName, fileName, value);
        }

        public void Delete(string dirName, string fileName)
        {
            proxy.DeleteAsync(dirName, fileName);
        }
    }
}
