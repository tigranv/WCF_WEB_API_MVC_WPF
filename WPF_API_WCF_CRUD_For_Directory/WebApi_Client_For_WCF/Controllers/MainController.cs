using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi_Client_For_WCF.Controllers
{
    public class MainController : ApiController
    {
        // GET: api/Main
        public IEnumerable<string> GetAllFiles(string dirName)
        {
            string p = Path.Combine("D:\\", dirName);
            DirectoryInfo directory = new DirectoryInfo(p);
            var files = directory.GetFiles().Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden)).Select(f => f.Name).ToArray();
            return files;
        }

        // "D:\TestDirectory" 

        // GET: api/Main/5
        public string Get(string dirName, string fileName)
        {
            string p = Path.Combine("D:\\", dirName);

            string path = Path.Combine(p, fileName);
            StreamReader sr = File.OpenText(path);
            string textline = sr.ReadLine();
            sr.Close();
            return textline;
        }

        // POST: api/Main
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Main/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Main/5
        public void Delete(string dirName, string fileName)
        {
            string p = Path.Combine("D:\\", dirName);

            string path = Path.Combine(p, fileName);

            string[] Files = Directory.GetFiles(@p);

            foreach (string file in Files)
            {
                if (file.ToUpper().Contains(fileName.ToUpper()))
                {
                    File.Delete(file);
                }
            }
        }
    }
}
