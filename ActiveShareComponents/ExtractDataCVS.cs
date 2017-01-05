using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ActiveShareComponents;
using Newtonsoft.Json.Linq;

namespace ActiveShareComponents
{
    public class ExtractDataCVS : IDisposable
    {
        private string path;
        private string[] currentData;
        private StreamReader reader;

        public ExtractDataCVS(string path)
        {
            if (!File.Exists(path)) throw new InvalidOperationException("Path not existant");
            
            this.path = path;
            Initialize();
        }

        private void Initialize()
        {
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            reader = new StreamReader(stream);
        }

        public bool Next()
        {
            string current = null;
            if ((current = reader.ReadLine()) == null) return false;
            currentData = current.Split(',');
            return true;
        }

        public string this[int index]
        {
            get { return currentData[index]; }
        }

        public void Dispose()
        {
            reader.Close();
        }
    } // End Class 


    public class MyFixtureDataCVS
    {
        private static IEnumerable<String[]> GetTargetDeviceFromCSV()
        {
            // Relative Path 
            string path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, System.AppDomain.CurrentDomain.RelativeSearchPath ?? "");
            string newPath = Path.GetFullPath(Path.Combine(path, @"..\..\..\"));

            string currentpath = newPath + "DefaultDevices.csv";

            // Hardcoded the path...
            ExtractDataCVS reader = new ExtractDataCVS(currentpath);
            while (reader.Next())
            {
                String column1 = reader[0];
                String column2 = reader[1];
                String column3 = reader[2];
                String column4 = reader[3];
                String column5 = reader[4];
                yield return new String[] { column1, column2, column3, column4, column5 };
            }
        }

    } //End class MyFixtureData

} // End namespace
