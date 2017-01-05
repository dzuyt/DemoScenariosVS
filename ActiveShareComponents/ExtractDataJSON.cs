using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ActiveShareComponents;
using Newtonsoft.Json.Linq;

namespace ActiveShareComponents
{
    public class ExtractDataJSON : IDisposable
    {
        private string path;
        private string[] currentData;
        private StreamReader reader;

        public ExtractDataJSON(string path)
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


    public class MyFixtureDataJSON
    {

        private static IEnumerable<String[]> GetTargetDeviceFromJSON()
        {
            String currentpath = "C:\\ProjectsVS\\DemoScenariosVS\\DefaultDevices.json";
            ExtractDataJSON reader = new ExtractDataJSON(currentpath);
 
            // Hardcoded the path...
            JObject targetDevice = JObject.Parse(File.ReadAllText(@currentpath));
           int i = 0;

            // Keep count up the element, unitl the element value is null....
            while (i <= targetDevice.Count)
            {
                String deviceId = (String) targetDevice.SelectToken((String)("devices[" + i + "].device.id"));
                String deviceOS = (String) targetDevice.SelectToken((String)("devices[" + i + "].device.os"));
                String deviceModel =  (String) targetDevice.SelectToken((String)("devices[" + i + "].device.version"));
                String deviceVersion =  (String) targetDevice.SelectToken((String)("devices[" + i + "].device.model"));
                String deviceDescription =  (String) targetDevice.SelectToken((String)("devices[" + i + "].device.description"));
                yield return new String[] { deviceOS, deviceVersion, deviceModel, deviceId, deviceDescription };
                i++;
            }
        }

    } //End class MyFixtureData

} // End namespace
