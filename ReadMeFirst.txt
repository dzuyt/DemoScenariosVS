This Visual Studio Solution has mulitple Projects for use to shows few Demo Scenarios

 ****************************
***** Preparation:       *****
 ****************************
(skip this process, if you have already inistal UNit and UUnit Test Adapter, otherwise)
	a. Check to make sure you this solution have NUnit Framework v3 and NUnit3TestAdapter are installed
	b. If not, then Tools -> Nuggest Package MAnager -> Manage NuGet packagaes for Solutions
		i.  Select Browse
		ii. Search for NUnit Test Adapter (get the v3.x)
		iii. Install NUnit v3.x, and NUnit3TestAdapter v3.x or later	
	c. Be use to import in NUnit.framework and make changes to the test-Attribute.  i.e., [Test...]
	d. Add Newtonsoft.josn library
		- http://stackoverflow.com/questions/4444903/how-to-install-json-net-using-nuget


 ****************************
***** Sharing Components *****
 ****************************
- ActiveShareUtils - This has all the shared Class/Functions for the Test Projects needs.

- Files need parameter changes:
	CQLsettings.json file is under the folder C:\ProjectsVS\DemoScenariosVS\ActiveShareComponents
	TargetDevices.CSV file is under the  folder C:\ProjectsVS\DemoScenariosVS\ActiveShareComponents
	DefaultDevices.json - (TBD for future implementation)
	

 ****************************
***** Project Description *****
****************************
Notes:
	* Application Type - WebApp, NativeApp
	* NUnit Parallelism - meaning this project can be used to demonstrates Parallel execution using one script
		concept runs on two or more different devices.  This uses TestFixture Parallelism.	
	* SelGrid Parallel - meaning it is using Selenium Grid technique to runs parallel execution by loading
		the device based on device grid
	* WindTunnel - The Personas & WindTunnel is turn on.

1. PerfectoVirtualTour - (WebApp + NUnit Parallelism) 
	To demonstrate demonstrates the scenario of login into application

2. SpeedTest - (WebApp + NUnit Parallelism) 
	To demonstrates scenario of running speedtest app, capture the throughput results
	and then send it to a phone using SMS

3. Wikipedia - (WebApp + NUnit Parallelism)
	To demonstrates scenario of loading web page and capture screen image and save to a local drive.