using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace REST_For_FileSystem_API.Controllers
{
    public class DirectoryController : ApiController
    {
        // GET: api/Directory
        public IEnumerable<string> GetAllFiles()
        {
            DirectoryInfo directory = new DirectoryInfo(@"D:\TestDirectory");
            var files = directory.GetFiles().Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden)).Select(f => f.Name).ToArray();
            return files;
        }

        // GET: api/Directory/5
        public string Get(string name)
        {
            DirectoryInfo directory = new DirectoryInfo(@"D:\TestDirectory");
            string path = Path.Combine(@"D:\TestDirectory", name); //.Substring(1, name.Length - 2));
            StreamReader sr = File.OpenText(path);
            string textline = sr.ReadLine();
            sr.Close();
            return textline;
        }

        // POST: api/Directory
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Directory/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Directory/5
        public void Delete(int id)
        {
        }
    }
}
