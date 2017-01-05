using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Reflection;

namespace ActiveShareComponents
{
    public class Utils
    {
        public static object JsonConvert { get; private set; }

        public static string GetReportFolder()
        {
            // Relative Path 
            string path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, System.AppDomain.CurrentDomain.RelativeSearchPath ?? "");
            string newPath = Path.GetFullPath(Path.Combine(path, @"..\..\"));

            string reportFolder = "Reports";
            string targetFolder = newPath + "\\" + reportFolder;


            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }
            return targetFolder;
        }

        public static void GetHostAndCredentials(out string host, out string user, out string password, out string windTunnelIsOn)
        {
            // Relative Path 
            string path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, System.AppDomain.CurrentDomain.RelativeSearchPath ?? "");
            string newPath = Path.GetFullPath(Path.Combine(path, @"..\..\..\"));

            JObject jO = JObject.Parse(File.ReadAllText(@newPath + "CQLsettings.json"));

            if ((host = Environment.GetEnvironmentVariable("PMHOST")) == null)
                host = (String)jO.SelectToken("cqlSettings[0].cqlSetting.perfectoCloud");
      
            if ((user = Environment.GetEnvironmentVariable("PMUSER")) == null)
                user = (String)jO.SelectToken("cqlSettings[0].cqlSetting.perfectoUsername");

            if ((password = Environment.GetEnvironmentVariable("PMPW")) == null)
                password = (String)jO.SelectToken("cqlSettings[0].cqlSetting.perfectoPassword");

            if ((windTunnelIsOn = Environment.GetEnvironmentVariable("PMWT")) == null)
                windTunnelIsOn = (String)jO.SelectToken("cqlSettings[0].cqlSetting.windTunnelIsOn");
        }

    }
}
