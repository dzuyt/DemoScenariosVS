using System;
using System.Collections.Generic;
using System.Diagnostics;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.IO;
using ActiveShareComponents;

namespace PerfectoVirtualTour
{
    /// <summary>
    /// Summary description for RemoteWebDriverTest
    /// 
    /// For programming samples and updated templates refer to the Perfecto GitHub at: https://github.com/PerfectoCode
    /// </summary>
    ///
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
            Console.WriteLine(this.deviceOS + " " + this.deviceVersion + " " + this.deviceModel + " " + this.deviceId +" " + this.deviceDescription);

            capabilities.SetPerfectoLabExecutionId(host);

            // Add a persona to your script (see https://community.perfectomobile.com/posts/1048047-available-personas)
            if (windTunnelIsOn == "true")
            {
                capabilities.SetCapability(WindTunnelUtils.WIND_TUNNEL_PERSONA_CAPABILITY, WindTunnelUtils.SARA);
            }


            // Name your script
            capabilities.SetCapability("scriptName", "VS_PerfectoVirtualTour_" + deviceModel + "_v" + deviceVersion);

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

            Console.WriteLine("REPORT URL:  " + reportUrl);
            driver.Close();

            // In case you want to download the report or the report attachments, do it here.
            //try
            //{

            //    driver.DownloadReport(DownloadReportTypes.pdf, Utils.GetReportFolder() + "\\report_" + deviceOS);
            //    // driver.DownloadAttachment(DownloadAttachmentTypes.video, "C:\\test\\report\\video", "flv");
            //    // driver.DownloadAttachment(DownloadAttachmentTypes.image, "C:\\test\\report\\images", "jpg");
            //}
            //catch (Exception ex)
            //{
            //    Trace.WriteLine(string.Format("Error getting test logs: {0}", ex.Message));
            //}

            driver.Quit();
        }

        [TestCase]
        public void PerfectoVirtualTour_LoginTest()
        {
            //Write your test here
            //Write your test here
            try
            {
                // reset the device and browse to the website on the default browser
                driver.Navigate().GoToUrl("http://nxc.co.il/demoaut/index.php");
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));

                try
                {
                    // Verify that arrived at the correct page, look for the Header Text
                    Dictionary<String, Object> params1 = new Dictionary<String, Object>();
                    params1.Add("content", "Perfecto Virtual Tours");
                    params1.Add("timeout", "10");
                    Object result1 = driver.ExecuteScript("mobile:checkpoint:text", params1);
                }
                catch (NoSuchElementException n)
                {
                    Trace.WriteLine("Not displaying the opening web page");
                    throw n;
                }

                // search for the username field
                driver.Context = "WEBVIEW";
                driver.FindElementByXPath("//*[@name=\"username\"]").SendKeys("John");

                // search for password field and enter the pw
                driver.FindElementByXPath("//*[@name=\"password\"]").SendKeys("Perfecto1");

                // find the Sign in button and click on it
                driver.FindElementByXPath("//button[text()='Sign in']").Click();

                // Verifying using VISUAL context
                try
                {
                    Dictionary<String, Object> params5 = new Dictionary<String, Object>();
                    params5.Add("content", "Welcome back John");
                    params5.Add("timeout", "10");
                    Object result5 = driver.ExecuteScript("mobile:checkpoint:text", params5);
                }
                catch (Exception w)
                {
                    Trace.WriteLine("'Welcome back John' text not found");
                    throw w;
                }


            }
            catch (Exception e)
            {
                Trace.WriteLine("Script failed with the following exception:" + e.Message);
            }

            
        }
    }
}
