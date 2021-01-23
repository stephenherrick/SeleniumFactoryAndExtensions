using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumHelper.Test;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace OpenQA.Selenium.Tests
{
    [TestFixture()]
    public class WebDriverExtensionsTests : TestBase
    {
        private IWebDriver driver;
        [SetUp]
        public void setup()
        {
            driver = Factory.AddDriver("chromeheadless");
        }

        [Test()]
        public void FindElementTest()
        {
            var element = driver.FindElement(VisibleElement, TimeSpan.FromSeconds(1));
            Assert.IsTrue(element.Displayed || element.Enabled);
        }

        [Test()]
        public void FindElementsTest()
        {
            var elements = driver.FindElements(VisibleElement, TimeSpan.FromSeconds(1));
            foreach(var element in elements)
            {
                Assert.IsTrue(element.Displayed || element.Enabled);
            }
        }

        [Test()]
        public void WaitForByLocatorTest()
        {
            driver.WaitFor(VisibleElement, TimeSpan.FromSeconds(1));

            var element = driver.FindElement(VisibleElement);

            Assert.IsTrue(element.Displayed || element.Enabled);
        }

        [Test()]
        public void WaitForByElementTest()
        {
            var element = driver.FindElement(VisibleElement);
            driver.WaitFor(element, TimeSpan.FromSeconds(1));


            Assert.IsTrue(element.Displayed || element.Enabled);
        }


        [Test()]
        public void WaitForElementIsInvisibleByLocatorTest()
        {
            driver.WaitForElementIsInvisible(InvisibleElement, TimeSpan.FromSeconds(1));

            var element = driver.FindElement(InvisibleElement);
            Assert.IsFalse(element.Displayed);
        }

        [Test()]
        public void WaitForElementIsInvisibleByElementTest()
        {
            var element = driver.FindElement(InvisibleElement);
            driver.WaitForElementIsInvisible(element, TimeSpan.FromSeconds(1));
            
            Assert.IsFalse(element.Displayed);
        }

        [Test()]
        public void ExplicitWaitTest()
        {
            var watch = new Stopwatch();
            watch.Start();
            driver.ExplicitWait(TimeSpan.FromSeconds(1));
            watch.Stop();
            Assert.IsTrue(watch.ElapsedMilliseconds >= 1000);
        }

        // [Test()]
        // public void NavigateToTest()
        // {
        //     driver.NavigateTo("/app");

        //     Assert.IsTrue(driver.Url == $"{Factory.BaseUrl}/app");
        // }
    }
}