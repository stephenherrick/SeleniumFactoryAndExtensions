using Microsoft.Edge.SeleniumTools;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SeleniumHelper
{
    public class DriverFactory
    {
        /// <summary>
        /// Instances of a WebDriver.
        /// <see cref="IWebDriver"/>
        /// </summary>
        public IList<IWebDriver> WebDrivers { get; private set; }

        /// <summary>
        /// This is the Base URL of the application, typically set in appsettings.json
        /// </summary>
        public string BaseUrl { get; private set; }

        /// <summary>
        /// The address of the selenium server, typically set in appsettings.json
        /// </summary>
        public Uri RemoteHubUri { get; private set; }

        /// <summary>
        /// Set to true to run on a remote selenium server, typically set in appsettings.json
        /// </summary>
        public bool IsRemote { get; private set; }

        private string BaseApplicationPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        /// <summary>
        /// General time span used for Implicit and Fluent waits
        /// </summary>
        public TimeSpan GlobalWaitTime { get; private set; }

        /// <summary>
        /// Manage collection of WebDrivers
        /// </summary>
        /// <param name="baseUrl"><see cref="BaseUrl"/></param>
        public DriverFactory(string baseUrl)
        {
            WebDrivers = new List<IWebDriver>();
            GlobalWaitTime = TimeSpan.FromSeconds(10);
            IsRemote = false;
            //RemoteHubUri = new Uri(string.Empty);
            BaseUrl = baseUrl;
        }

        /// <summary>
        /// Manage collection of WebDrivers when using Selenium Grid
        /// </summary>
        /// <param name="baseUrl"><see cref="BaseUrl"/></param>
        /// <param name="hubUri">Selenium Grid hub url<see cref="RemoteHubUri"/></param>
        public DriverFactory(string baseUrl, string hubUri = "http://localhost:4444/")
        {
            WebDrivers = new List<IWebDriver>();
            GlobalWaitTime = TimeSpan.FromSeconds(10);
            IsRemote = true;
            RemoteHubUri = new Uri(hubUri);
            BaseUrl = baseUrl;
        }

        /// <summary>
        /// Add a WebDriver to the WebDrivers list
        /// </summary>
        /// <param name="browserName"></param>
        /// <param name="waitTIme">Optional, default is set to 30 seconds</param>
        /// <returns></returns>
        public IWebDriver AddDriver(string browserName, int waitTIme = 10)
        {
            GlobalWaitTime = TimeSpan.FromSeconds(waitTIme);
            var driver = SetBrowser(browserName);

            driver.Manage().Timeouts().ImplicitWait = GlobalWaitTime;
            try
            {
                driver.Manage().Window.Maximize();
            }
            catch (Exception) { }

            driver.Navigate().GoToUrl(BaseUrl);
            WebDrivers.Add(driver);
            return driver;
        }

        /// <summary>
        /// Closes all browsers and driver instances.
        /// </summary>
        public void ForceCloseDrivers()
        {
            var processes = new List<string> { "chromedriver", "geckodriver", "IEDriverServer", "msedgedriver" };
            foreach (string app in processes)
            {
                foreach (var process in Process.GetProcessesByName(app))
                {
                    process.Kill();
                }
            }
        }

        /// <summary>
        /// Set the WebDriver browser by browser name.
        /// </summary>
        /// <param name="browserName">Options not case sensitive and are: "chrome", "firefox", "chromeheadless", "firefoxheadless", "ie"</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>Returns a the appropriate WebDriver by browser name</returns>
        private IWebDriver SetBrowser(string browserName)
        {
            if (IsRemote)
            {
                switch (browserName.ToLower())
                {
                    case "firefox":
                        var rfireoptions = AcceptUknownIssuer();
                        return new RemoteWebDriver(RemoteHubUri, rfireoptions.ToCapabilities(), GlobalWaitTime);

                    case "chrome":
                        var rcoptions = new ChromeOptions();
                        return new RemoteWebDriver(RemoteHubUri, rcoptions.ToCapabilities(), GlobalWaitTime);


                    case "edge":
                        var edgeHeadlessOptions = new EdgeOptions();
                        edgeHeadlessOptions.UseChromium = true;
                        return new RemoteWebDriver(RemoteHubUri, edgeHeadlessOptions.ToCapabilities(), GlobalWaitTime);


                    default:
                        throw new ArgumentException("Provide a valid browser name.");
                }
            }
            else
            {
                switch (browserName.ToLower())
                {
                    case "firefox":
                        FirefoxDriverService fireservice = FirefoxDriverService.CreateDefaultService(BaseApplicationPath);
                        var fireoptions = AcceptUknownIssuer();
                        return new FirefoxDriver(fireservice, fireoptions, GlobalWaitTime);

                    case "chrome":
                        var coptions = new ChromeOptions();
                        return new ChromeDriver(BaseApplicationPath, coptions, GlobalWaitTime);

                    case "firefoxheadless":
                        FirefoxDriverService firehservice = FirefoxDriverService.CreateDefaultService(BaseApplicationPath);
                        var firehoptions = AcceptUknownIssuer();
                        firehoptions.AddArgument("--headless");
                        return new FirefoxDriver(firehservice, firehoptions, GlobalWaitTime);

                    case "chromeheadless":
                        var choptions = new ChromeOptions();
                        choptions.AddArgument("--headless --disable-gpu");
                        return new ChromeDriver(BaseApplicationPath, choptions, GlobalWaitTime);

                    case "edge":
                        var edgeOptions = new EdgeOptions();
                        edgeOptions.UseChromium = true;
                        return new EdgeDriver(BaseApplicationPath, edgeOptions, GlobalWaitTime);

                    case "edgeheadless":
                        var edgeHeadlessOptions = new EdgeOptions();
                        edgeHeadlessOptions.UseChromium = true;
                        edgeHeadlessOptions.AddArgument("--headless --disable-gpu");
                        return new EdgeDriver(BaseApplicationPath, edgeHeadlessOptions, GlobalWaitTime);

                    default:
                        throw new ArgumentException("Provide a valid browser name.");
                }
            }
        }
        private FirefoxOptions AcceptUknownIssuer()
        {
            FirefoxOptions options = new FirefoxOptions();
            options.AcceptInsecureCertificates = true;

            return options;
        }
    }
}