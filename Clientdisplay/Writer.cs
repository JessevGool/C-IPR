using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace ServerApp
{
    class Writer
    { 

        public void writeData(JObject jObject)
        {
            String file = getFileContent();

            JArray rfile = JArray.Parse(file);
            rfile.Add(jObject);
            System.IO.File.WriteAllText(getFile(), rfile.ToString());
        }

        public static string getFileContent()
        {
            using (StreamReader sr = new StreamReader(getFile()))
            {
                // Read the stream to a string, and write the string to the console.
                return sr.ReadToEnd();
            }
        }

        public void clearFile()
        {
            System.IO.File.WriteAllText(getFile(), "[]");
        }


        public static string getPath()
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            string Startsplit = startupPath.Substring(0, startupPath.LastIndexOf("bin"));
            string split = Startsplit.Replace(@"\", "/");
            return split;
        }

        public static string getFile()
        {
            string fileName = @"\Data\Data.txt";
            return getPath() + fileName;
        }


    }
}
