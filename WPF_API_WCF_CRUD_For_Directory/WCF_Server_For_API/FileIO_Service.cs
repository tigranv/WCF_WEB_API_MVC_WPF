using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCF_Server_For_API
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class FileIO_Service : IServiceIO
    {
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

        public string Get(string dirName, string fileName)
        {
            string p = Path.Combine("D:\\", dirName);

            string path = Path.Combine(p, fileName);
            StreamReader sr = File.OpenText(path);
            string textline = sr.ReadLine();
            sr.Close();
            return textline;
        }

        public IEnumerable<string> GetAllFiles(string dirName)
        {
            string p = Path.Combine("D:\\", dirName);
            DirectoryInfo directory = new DirectoryInfo(p);
            var files = directory.GetFiles().Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden)).Select(f => f.Name).ToArray();
            return files;
        }

        public void Post(string dirName, string fileName, string value)
        {
            string p = Path.Combine("D:\\", dirName);
            string path = Path.Combine(p, fileName);
            StreamWriter sw = new StreamWriter(path);
            sw.Write(value);
            sw.Close();
        }

        public void Put(string dirName, string fileName, string value)
        {
            string p = Path.Combine("D:\\", dirName);
            string path = Path.Combine(p, fileName);
            StreamWriter sw = new StreamWriter(path);
            sw.Write(value);
            sw.Close();
        }
    }
}
