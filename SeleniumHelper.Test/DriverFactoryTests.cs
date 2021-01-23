using NUnit.Framework;
using SeleniumHelper;
using SeleniumHelper.Test;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeleniumHelper.Tests
{
    [TestFixture]
    public class DriverFactoryTests : TestBase
    {
        [Test, Order(1)]
        public void DriverFactoryTest()
        {
            Factory.AddDriver("chromeheadless");
            Assert.IsNotNull(Factory.WebDrivers[0]);
        }

        [Test, Order(2)]
        public void AddDriverTest()
        {
            var driverCount = Factory.WebDrivers.Count;

            Factory.AddDriver("chromeheadless");
            Assert.AreEqual(driverCount+1, Factory.WebDrivers.Count);
            
        }
    }
}