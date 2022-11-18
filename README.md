# EndToEndBlazorWasmPlaywrightTesting
## An ttempt to get Blazor WASM, WebAPI, and Playwright to all play nicely together in memory for testing.

This initial commit demonstrates testing in memory using Playwright, against a Blazor Server front end, a separate WebAPI and NUnit test project.
The WebAPI is created using WebApplicationFactory, this allows you to inject mocked databases, storage and other dependencies. 
The HttpClient provided by the WebApplicationFramework is then passed to the Blazor Server front end to make use of the Mocked services. 

The goal of this project is to reproduce the same in-memory testing for a Blazor WASM front end instead of the server. The WASM version will be tracked on branch. 

#Project Structure 

![image](https://user-images.githubusercontent.com/18427214/202762027-58c572fe-2563-49da-8c97-c31798e8458a.png)

- BlazorApp - Blazor Server Application (Based on Max Schmitt's example https://github.com/mxschmitt/razor-playwright-dotnet-example)
- BlazorWasmApp - (Not currently used on this branch) Stand along Blazor Wasm application. - Target to test in memory.
- NUnitTests - Adapted version of Max Schmitt's example https://github.com/mxschmitt/razor-playwright-dotnet-example 
  - The key here is that the UnitTest1.cs inherits the BlazorTests.cs and starts the server in OneTimeSetUp. The Utilimiate goal would be starting both hosts (WebAPI & FrontEnd) in an un-namespaced OneTimeSetUp to cover all tests and reuse the hosts between testing. 
- WebAPI 
  - The intention will be to mock services like Databases, storage, vaults etc. so that the in-memory tests execute very fast. 
 
   
 # Test Execution. 
 If you run both tests "CountTest" & "FetchDataTest" on this main branch they should pass.
 
 ![image](https://user-images.githubusercontent.com/18427214/202765414-c1b627e5-e496-468a-88c8-f44930e5a02a.png)
 
There is a runsettings file contained in the root of the directory, if you select this file in Test Explorer you will run the tests in headless mode, allowing you to see the tests executing in the browser.

![image](https://user-images.githubusercontent.com/18427214/202765861-0e1e0217-8cc0-4947-8327-6085dbda1f19.png)

If you put a breakpoint on WebAPI > Controllers > WeatherForecastController at link #24 when the "FetchDataTest" executes, if you inspect the Http Request you will notice the host is localhost without any port, this shows that the HttpClient from the WebApplicationFramework is working as expected.

![image](https://user-images.githubusercontent.com/18427214/202766748-795764f1-7fda-476b-8dc7-f57e247ad36d.png)


#Now, The goal is reproduce this for a WASM application.
