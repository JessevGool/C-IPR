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

            JArray rfile = JArray.Parse(Encrypter.DecryptString(file, "kip"));
            rfile.Add(jObject);
            System.IO.File.WriteAllText(getFile(), Encrypter.EncryptString(rfile.ToString(), "kip"));
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
            System.IO.File.WriteAllText(getFile(), Encrypter.EncryptString("[]", "kip"));
        }


        public static String getFile()
        {
            string fileName = @"\Data\Data.txt";
            return Environment.CurrentDirectory.Substring(0, 69) + fileName;
        }


    }
}
