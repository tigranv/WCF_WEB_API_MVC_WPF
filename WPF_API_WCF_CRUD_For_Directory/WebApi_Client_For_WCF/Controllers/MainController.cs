using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;


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

        public void Put([FromUri]string dirName, [FromUri]string fileName, [FromBody]string value)
        {
            string p = Path.Combine("D:\\", dirName);
            string path = Path.Combine(p, fileName);
            StreamWriter sw = new StreamWriter(path);
            sw.Write(value);
            sw.Close();
        }

        public void Post([FromUri]string dirName, [FromUri]string fileName, [FromBody]string value)
        {
            string p = Path.Combine("D:\\", dirName);
            string path = Path.Combine(p, fileName);
            StreamWriter sw = new StreamWriter(path);
            sw.Write(value);
            sw.Close();
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
