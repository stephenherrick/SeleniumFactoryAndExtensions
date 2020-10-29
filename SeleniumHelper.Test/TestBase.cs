using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;

namespace SeleniumHelper.Test
{
    [SetUpFixture]
    public class TestBase
    {
        public DriverFactory Factory { get; set; }
        public string BaseUrl { get; private set; }
        public static By VisibleElement = By.Id("visible");
        public static By InvisibleElement = By.Id("invisible");
        public static By Button = By.Name("button");

        [OneTimeSetUp]
        public void InitialSetUp()
        {
            BaseUrl = $"{AppDomain.CurrentDomain.BaseDirectory}/TestPage.html";
            Factory = new DriverFactory(BaseUrl);
        }

        [OneTimeTearDown]
        public void FinalTearDown()
        {  
            foreach(var driver in Factory.WebDrivers)
            {
                driver.Close();
                driver.Quit();
            }
            Factory.ForceCloseDrivers();
        }
    }
}
