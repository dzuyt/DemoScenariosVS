using System;
using System.Diagnostics;
// using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.IO;
using ActiveShareComponents;

namespace Wikipedia
{
    /// <summary>
    /// Summary description for RemoteWebDriverTest
    /// 
    /// For programming samples and updated templates refer to the Perfecto GitHub at: https://github.com/PerfectoCode
    /// </summary>
    [TestFixture]
    [TestFixtureSource(typeof(MyFixtureDataCVS), "GetTargetDeviceFromCSV")]
    [Parallelizable(ParallelScope.Fixtures)]
    public class RemoteWebDriverTest
    {
        private RemoteWebDriverExtended driver;
        String deviceOS, deviceVersion, deviceModel, deviceId, deviceDescription;
        String executionID;

        public RemoteWebDriverTest(String deviceOS, String deviceVersion, String deviceModel, String deviceId, String deviceDescription)
        {
            this.deviceOS = deviceOS;
            this.deviceVersion = deviceVersion;
            this.deviceModel = deviceModel;
            this.deviceId = deviceId;
            this.deviceDescription = deviceDescription;
        }

        [SetUp]
        public void PerfectoOpenConnection()
        {
            var browserName = "mobileOS";
            String host, username, password, windTunnelIsOn;

            Utils.GetHostAndCredentials(out host, out username, out password, out windTunnelIsOn);

            DesiredCapabilities capabilities = new DesiredCapabilities(browserName, string.Empty, new Platform(PlatformType.Any));
            capabilities.SetCapability("user", username);
            capabilities.SetCapability("password", password);

            capabilities.SetCapability("platformName", deviceOS);
            capabilities.SetCapability("platformVersion", deviceVersion);
            capabilities.SetCapability("model", deviceModel);
            capabilities.SetCapability("deviceId", deviceId);
            capabilities.SetCapability("description", deviceDescription);
            Console.WriteLine(this.deviceOS + " " + this.deviceVersion + " " + this.deviceModel + " " + this.deviceId + " " + this.deviceDescription);

            capabilities.SetPerfectoLabExecutionId(host);

            // Add a persona to your script (see https://community.perfectomobile.com/posts/1048047-available-personas)
            if (windTunnelIsOn == "true")
            {
                capabilities.SetCapability(WindTunnelUtils.WIND_TUNNEL_PERSONA_CAPABILITY, WindTunnelUtils.SARA);
            }


            // Name your script
            capabilities.SetCapability("scriptName", "VS_Wikipedia_" + deviceModel + "_v" + deviceVersion);

            var url = new Uri(string.Format("https://{0}/nexperience/perfectomobile/wd/hub", host));
            System.Threading.Thread.Sleep(2000);
            driver = new RemoteWebDriverExtended(new HttpAuthenticatedCommandExecutor(url), capabilities);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(40));

            ICapabilities cap = driver.Capabilities;
            executionID = (String)cap.GetCapability("executionId");
            Console.WriteLine(executionID);
        }


        [TearDown]
        public void PerfectoCloseConnection()
        {
            // Retrieve the URL of the Single Test Report, can be saved to your execution summary and used to download the report at a later point
            string reportUrl = (string)(driver.Capabilities.GetCapability(WindTunnelUtils.SINGLE_TEST_REPORT_URL_CAPABILITY));

            driver.Close();

            // In case you want to download the report or the report attachments, do it here.
            try
            {
                driver.DownloadReport(DownloadReportTypes.pdf, Utils.GetReportFolder() + "\\report_"+ deviceOS);
                // driver.DownloadAttachment(DownloadAttachmentTypes.video, "C:\\test\\report\\video", "flv");
                // driver.DownloadAttachment(DownloadAttachmentTypes.image, "C:\\test\\report\\images", "jpg");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Error getting test logs: {0}", ex.Message));
            }

            driver.Quit();
        }

        [TestCase]
        public void Wikipedia_CaptureImageTest()
        {
            // browse to the target page 
            driver.Navigate().GoToUrl("wikipedia.org");
            System.Diagnostics.Debug.WriteLine("What is going on");

            // RemoteWebDriver creates the screenshot as a byte stream
            Screenshot screen = driver.GetScreenshot();
            // copy the screenshot to the disk file after verifying that folder exists
            try
            {
                String reportDir = Utils.GetReportFolder();
                DirectoryInfo dir = new DirectoryInfo(reportDir);
                //dir.CreateSubdirectory("images");
                screen.SaveAsFile(dir + "\\wikipedia.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception e)
            {
                Trace.WriteLine(string.Format("Unable to create the screenshot file. Reason: {0}", e.Message));
            }


        }

    }
}
