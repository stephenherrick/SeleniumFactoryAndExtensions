using OpenQA.Selenium.Interactions;
using System;

namespace OpenQA.Selenium
{
    public static class WebElementExtensions
    {
        /// <summary>
        /// Clears text field and then sends keys to a text field.
        /// </summary>
        /// <param name="input">Text to insert into field</param>
        /// <param name="clearField">If true, the field is cleared before sending text, else the existing text remains</param>
        public static void SendKeys(this IWebElement element, string input, bool clearField = false)
        {
            if (clearField)
            {
                element.Clear();
                //This section was added because occasionally the clear() method does not always clear the field.
                if (!String.IsNullOrEmpty(element.Text))
                {
                    element.SendKeys(Keys.Control + "a" + Keys.Control);
                }
            }
            
            element.SendKeys(input);
        }

        public static void Tap(this IWebElement element, IWebDriver driver)
        {
            var builder = new Actions(driver);
            builder.Click(element).Click().Build().Perform();
        }

    }
}