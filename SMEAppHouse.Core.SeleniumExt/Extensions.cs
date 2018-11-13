using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace SMEAppHouse.Core.SeleniumExt
{
    public static class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <param name="timeoutInSeconds"></param>
        /// <returns></returns>
        public static bool WaitUntilElementIsPresent(this IWebDriver driver, By by, int timeoutInSeconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d =>
            {
                try
                {
                    var result = d.FindElement(by);

                    if (result != null) return true;
                }
                catch (Exception)
                {
                    return false;
                }
                return false;
            });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <param name="timeoutInSeconds"></param>
        /// <returns></returns>
        public static bool WaitUntilElementIsNotPresent(this IWebDriver driver, By by, int timeoutInSeconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d =>
            {
                try
                {
                    var result = d.FindElement(by);

                    if (result == null) return true;
                }
                catch (Exception)
                {
                    return true;
                }
                return false;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <param name="valuePartial"></param>
        /// <param name="timeoutInSeconds"></param>
        /// <returns></returns>
        public static bool WaitUntilElementInnerTextContains(this IWebDriver driver, By by, string valuePartial, int timeoutInSeconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d =>
            {
                try
                {
                    var element = d.FindElement(by);
                    if (!string.IsNullOrEmpty(element?.Text))
                    {
                        return element.Text.ToLower().Contains(valuePartial.ToLower().Trim());
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
                return false;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <param name="valuePartial"></param>
        /// <param name="timeoutInSeconds"></param>
        /// <returns></returns>
        public static bool WaitUntilElementWithInnerValueExist(this IWebDriver driver, By by, string valuePartial, int timeoutInSeconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d =>
            {
                try
                {
                    var elements = d.FindElements(by);

                    if (elements.Select(element => element.GetAttribute("innerHTML"))
                                .Any(attribVal => !string.IsNullOrEmpty(attribVal) && attribVal.Contains(valuePartial)))
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
                return false;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <param name="attribute"></param>
        /// <param name="valuePartial"></param>
        /// <param name="timeoutInSeconds"></param>
        /// <returns></returns>
        public static bool WaitUntilElementWithAttributeValueExist(this IWebDriver driver, By by, string attribute, string valuePartial, int timeoutInSeconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d =>
            {
                try
                {
                    var elements = d.FindElements(by);

                    if (elements.Select(element => GetElementAttributeValue(driver, element, attribute))
                                .Any(attribVal => !string.IsNullOrEmpty(attribVal) && attribVal.Contains(valuePartial)))
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
                return false;
            });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="element"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static string GetElementAttributeValue(this IWebDriver driver, IWebElement element, string attribute)
        {
            return (string)((IJavaScriptExecutor)driver).ExecuteScript(
                $"return arguments[0].getAttribute('{attribute}');", element);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static Image GetEntireScreenshot(this IWebDriver driver)
        {
            // Get the total size of the page
            var totalWidth = (int)(long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.offsetWidth"); //documentElement.scrollWidth");
            var totalHeight = (int)(long)((IJavaScriptExecutor)driver).ExecuteScript("return  document.body.parentNode.scrollHeight");
            // Get the size of the viewport
            var viewportWidth = (int)(long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.clientWidth"); //documentElement.scrollWidth");
            var viewportHeight = (int)(long)((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight"); //documentElement.scrollWidth");

            // We only care about taking multiple images together if it doesn't already fit
            if (totalWidth <= viewportWidth && totalHeight <= viewportHeight)
            {
                var screenshot = driver.TakeScreenshot();
                return screenshot.ToImage();
            }
            // Split the screen in multiple Rectangles
            var rectangles = new List<Rectangle>();
            // Loop until the totalHeight is reached
            for (var y = 0; y < totalHeight; y += viewportHeight)
            {
                var newHeight = viewportHeight;
                // Fix if the height of the element is too big
                if (y + viewportHeight > totalHeight)
                {
                    newHeight = totalHeight - y;
                }
                // Loop until the totalWidth is reached
                for (var x = 0; x < totalWidth; x += viewportWidth)
                {
                    var newWidth = viewportWidth;
                    // Fix if the Width of the Element is too big
                    if (x + viewportWidth > totalWidth)
                    {
                        newWidth = totalWidth - x;
                    }
                    // Create and add the Rectangle
                    var currRect = new Rectangle(x, y, newWidth, newHeight);
                    rectangles.Add(currRect);
                }
            }
            // Build the Image
            var stitchedImage = new Bitmap(totalWidth, totalHeight);
            // Get all Screenshots and stitch them together
            var previous = Rectangle.Empty;
            foreach (var rectangle in rectangles)
            {
                // Calculate the scrolling (if needed)
                if (previous != Rectangle.Empty)
                {
                    var xDiff = rectangle.Right - previous.Right;
                    var yDiff = rectangle.Bottom - previous.Bottom;
                    // Scroll
                    ((IJavaScriptExecutor)driver).ExecuteScript($"window.scrollBy({xDiff}, {yDiff})");
                }
                // Take Screenshot
                var screenshot = driver.TakeScreenshot();
                // Build an Image out of the Screenshot
                var screenshotImage = screenshot.ToImage();
                // Calculate the source Rectangle
                var sourceRectangle = new Rectangle(viewportWidth - rectangle.Width, viewportHeight - rectangle.Height, rectangle.Width, rectangle.Height);
                // Copy the Image
                using (var graphics = Graphics.FromImage(stitchedImage))
                {
                    graphics.DrawImage(screenshotImage, rectangle, sourceRectangle, GraphicsUnit.Pixel);
                }
                // Set the Previous Rectangle
                previous = rectangle;
            }
            return stitchedImage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="screenshot"></param>
        /// <returns></returns>
        public static Image ToImage(this Screenshot screenshot)
        {
            Image screenshotImage;
            using (var memStream = new MemoryStream(screenshot.AsByteArray))
            {
                screenshotImage = Image.FromStream(memStream);
            }
            return screenshotImage;
        }
    }
}
