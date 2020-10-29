using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace OpenQA.Selenium
{
    public static class WebDriverExtensions
    {
        /// <summary>
        /// A fluent wait wrapper for FindElement. Waits for a given amount of time for an element to display before locating.
        /// </summary>
        /// <param name="by"><see cref="By"/></param>
        /// <param name="waitTime">Amount of time wait for element to not be visible<see cref="TimeSpan"/></param>
        /// <returns>Single element<see cref="IWebElement"/></returns>
        public static IWebElement FindElement(this IWebDriver driver, By by, TimeSpan waitTime)
        {
            try
            {
                Console.WriteLine(String.Format("Finding element at: {0}", by.ToString()));
                driver.WaitFor(by, waitTime);
            }
            catch (Exception e)
            {
                if (e is NoSuchElementException || e is StaleElementReferenceException)
                {
                    Console.WriteLine("{0}, Element not found at: {1}", e.Message, by.ToString());
                }
                throw e;
            }
            return driver.FindElement(by);
        }

        /// <summary>
        ///  A fluent wait wrapper for FindElements. Waits for a given amount of time for an element to display before locating.
        /// </summary>
        /// <param name="by"><see cref="By"/></param>
        /// <param name="waitTime">Amount of time wait for element to not be visible<see cref="TimeSpan"/></param>
        /// <returns>List of Elements<seealso cref="IWebElement"/></returns>
        public static IList<IWebElement> FindElements(this IWebDriver driver, By by, TimeSpan waitTime)
        {
            try
            {
                Console.WriteLine(String.Format("Finding elements at: {0}", by.ToString()));
                driver.WaitFor(by, waitTime);
            }
            catch (Exception e)
            {
                if (e is NoSuchElementException || e is StaleElementReferenceException || e is WebDriverTimeoutException)
                {
                    Console.WriteLine("{0}, Elements not found at: {1}", e.Message, by.ToString());
                }
                throw e;
            }
            return driver.FindElements(by).Where(e => e.Displayed == true).ToList();
        }

        /// <summary>
        /// Wait until a specified element is visible.
        /// </summary>
        /// <param name="locator">The locator of the element<see cref="By"/></param>
        /// <param name="waitTime">Amount of time wait for element to not be visible<see cref="TimeSpan"/></param>

        public static void WaitFor(this IWebDriver driver, By locator, TimeSpan waitTime)
        {
            WebDriverWait wait = new WebDriverWait(driver, waitTime);
            IWebElement element;
            wait.Until((d) =>
            {
                element = d.FindElement(locator);
                if (element.Displayed || element.Enabled)
                {
                    return element;
                }
                return null;
            });
        }

        /// <summary>
        /// Wait for a specific element. This waits until a specified element is visible.
        /// </summary>
        /// <param name="element"><see cref="IWebElement"/></param>
        /// <param name="waitTime">Amount of time wait for element to not be visible<see cref="TimeSpan"/></param>
        public static void WaitFor(this IWebDriver driver, IWebElement element, TimeSpan waitTime)
        {
            WebDriverWait wait = new WebDriverWait(driver, waitTime);
            wait.Until((d) =>
            {
                if (element.Displayed &&
                    element.Enabled)
                {
                    return element;
                }
                return null;
            });
        }

        /// <summary>
        /// Wait for a specific element is not visible.
        /// </summary>
        /// <param name="locator"><see cref="By"/></param>
        /// <param name="waitTime">Amount of time wait for element to not be visible<see cref="TimeSpan"/></param>
        public static void WaitForElementIsInvisible(this IWebDriver driver, By locator, TimeSpan waitTime)
        {
            WebDriverWait wait = new WebDriverWait(driver, waitTime);
            wait.Until((d) =>
            {
                IWebElement element = d.FindElement(locator);
                if (!element.Displayed ||
                    !element.Enabled)
                {
                    return element;
                }
                return null;
            });
        }

        /// <summary>
        /// Wait for an element to not be visisble or disabled.
        /// </summary>
        /// <param name="element"><see cref="IWebElement"/> </param>
        /// <param name="waitTime">time span to wait</param>
        public static void WaitForElementIsInvisible(this IWebDriver driver, IWebElement element, TimeSpan waitTime)
        {
            DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(driver);
            fluentWait.Timeout = waitTime;
            fluentWait.PollingInterval = TimeSpan.FromMilliseconds(250);
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            IWebElement searchResult = fluentWait.Until((d) =>
            {
                if (!element.Displayed || !element.Enabled)
                {
                    return element;
                }
                return null;
            });
        }

        /// <summary>
        /// Call to 'halt execution' and wait a specific amount of time.
        /// <seealso cref="Thread.Sleep(int)"/>
        /// </summary>
        /// <param name="waitTime">time span to wait</param>
        public static void ExplicitWait(this IWebDriver driver, TimeSpan waitTime)
        {
            Thread.Sleep(waitTime);
        }

        /// <summary>
        /// Navigate to a page within the existing base url
        /// e.g. "/contact" for "https://baseUrl.com/contact"
        /// </summary>
        /// <param name="path">Path of page under the existing base url</param>
        public static void NavigateTo(this IWebDriver driver, string path)
        {
            var baseUrl = new Uri(driver.Url).Authority;
            driver.Navigate().GoToUrl($"https://{baseUrl}{path}");
        }
    }
}