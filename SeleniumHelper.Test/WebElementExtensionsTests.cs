using NuGet.Frameworks;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumHelper.Test;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenQA.Selenium.Tests
{
    [TestFixture()]
    public class WebElementExtensionsTests : TestBase
    {
        [Test]
        public void SendKeysTest()
        {
            var driver = Factory.AddDriver("chromeheadless");

            var element = driver.FindElement(VisibleElement);
            element.SendKeys("delete me");
            element.SendKeys("updated", true);
            var fieldValue = element.GetAttribute("value");

            Assert.AreEqual("updated", fieldValue);
        }

        //[Test]
        //public void TapTest()
        //{
        //    var driver = Factory.AddDriver("chrome");
        //    driver.FindElement(Button).Tap(driver);

        //    Assert.AreNotEqual(Factory.BaseUrl, driver.Url);
        //}
    }
}