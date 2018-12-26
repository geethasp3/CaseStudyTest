using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using POStest.TestFramework;
using Protractor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POStest.Selenium
{
   public static class Driver
    {

        public static NgWebDriver Instance { get; set; }
        public static bool Initialised { get; set; }
        private static TimeSpan _waitTimeoutTimeSpan;
        private static TimeSpan _waitTimeoutWrappedTimeSpan = TimeSpan.FromSeconds(5);
        private static IWebDriver driver;
        private static By lastBy { get; set; }


        public static void Initialize()
        {
            WaitTimeOutInSeconds = 60;

            ChromeOptions chromeOptions = new ChromeOptions();

            switch (ProjSettings.Browser)
            {
                case Browsers.Chrome:
                    driver = new ChromeDriver(ProjSettings.ChromeDriverFolderPath, chromeOptions);
                    driver.Manage().Window.Size = new Size(ProjSettings.WIDTH_WINDOW, ProjSettings.HEIGHT_WINDOW);
                    break;
                case Browsers.MobileWrapper:
                //    ProjSettings.DeleteLocalStorage();
                    chromeOptions.BinaryLocation = ProjSettings.MobilePosWrapperPath;
                    driver = new ChromeDriver(ProjSettings.ChromeDriverFolderPath, chromeOptions);
                    break;
                case Browsers.iPod:
                 //   ProjSettings.DeleteLocalStorage();
                    chromeOptions.BinaryLocation = ProjSettings.MobilePosWrapperPath;
                    chromeOptions.EnableMobileEmulation("Apple iPhone 5");
                    driver = new ChromeDriver(ProjSettings.ChromeDriverFolderPath, chromeOptions);
                    break;
                //case Browsers.IE:
                //    InternetExplorerOptions ieOptions = new InternetExplorerOptions();
                //    ieOptions.EnsureCleanSession = true;
                //    ieOptions.EnableNativeEvents = false;
                //    driver = new InternetExplorerDriver(ProjSettings.IEDriverFolderPath, ieOptions);
                //    break;
                case Browsers.Backend:
                    // Do not initialize Driver for Browserless Tests
                    return;
            }

            // Timeouts for Protractor.NET library
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(WaitTimeOutInSeconds);

            Instance = new NgWebDriver(driver);
            Initialised = true;
        }

        public static void Resize(int width, int height)
        {
            int innerWidth = DriverJQueryHelper.GetInnerWidth();
            int innerHeight = DriverJQueryHelper.GetInnerHeight();

            if (innerWidth != ProjSettings.WIDTH_WINDOW || innerHeight != ProjSettings.HEIGHT_WINDOW)
            {
                Instance.Manage().Window.Size = new Size(ProjSettings.WIDTH_WINDOW, ProjSettings.HEIGHT_WINDOW);

                int w = width + (width - DriverJQueryHelper.GetInnerWidth());
                int h = height + (height - DriverJQueryHelper.GetInnerHeight());

                Instance.Manage().Window.Size = new Size(w, h);
            }
        }

        public static int WaitTimeOutInSeconds
        {
            set { _waitTimeoutTimeSpan = new TimeSpan(value * 10000000); }
            get { return (int)_waitTimeoutTimeSpan.TotalSeconds; }
        }

        public static string BaseAddress
        {
            get { return "http://localhost:8080/"; }
        }

        public static object ScreenshotCapture { get; private set; }

        public static void TakeScreenshot(string name, bool fullscreen = false)
        {
            string finalName = DateTime.Now.ToString("yyyyMMdd-HHmmss") + "_" + name + ".png";
            string fullPath = Path.Combine(ProjSettings.ScreenshotsPath, finalName);
            if (ProjSettings.Browser.Equals(Browsers.Chrome) || ProjSettings.Browser.Equals(Browsers.IE))
            {
                ITakesScreenshot screenshotDriver = Instance.WrappedDriver as ITakesScreenshot;
                Screenshot screenshot = screenshotDriver.GetScreenshot();
                screenshot.SaveAsFile(fullPath, ScreenshotImageFormat.Png);
            }
            else
            {
               // ScreenshotCapture.TakeScreenshot(fullPath, fullscreen);
            }
        }

        public static void ClickSpecial(this NgWebElement element)
        {
            try
            {
                element.Click();
            }
            catch
            {
                // Temporary workaround for StaleElementReferenceException
                // Necessary to search once more time and click again to adjust new reference
                Console.WriteLine("Click Special failed! Again try is performed!");
                Thread.Sleep(2000);
                WaitFor(lastBy);
                Find(lastBy).Click();
            }
        }

        public static void ScrollToAndClick(this NgWebElement element)
        {
            try
            {
                element.ScrollTo().Click();
            }
            catch (ElementNotVisibleException)
            {
                // Debug Logging Purpose
                Console.WriteLine("ScrollToAndClick Failed Once! Another try will be performed!");
                Console.WriteLine($"Element is null? --> {element == null}");
                Console.WriteLine($"Element.GetTextContent --> {element.GetTextContent()}");
                Console.WriteLine($"Element.Enabled --> {element.Enabled}");
                Console.WriteLine($"Element.Displayed --> {element.Displayed}");
                Console.WriteLine($"Element.Selected --> {element.Selected}");
                Console.WriteLine($"Element.GetAttribute --> {element.GetAttribute("class")}");

                // Workaround - try second time
                element.ScrollTo().Click();
            }
        }

        public static NgWebElement ScrollTo(this NgWebElement element)
        {
            Instance.WaitForAngular();

            Actions actions = new Actions(driver);
            actions.MoveToElement(element).Release().Build().Perform();

            return element;
        }

        public static void VirtualReceiptScrollToAndClick(this NgWebElement element)
        {
            Instance.WaitForAngular();

            Actions actions = new Actions(driver);
            actions.SendKeys(Keys.PageUp).Release().Build();
            int counter = 20;

            while (!element.Displayed && counter-- > 0)
            {
                actions.Perform();
               // element = FindElement(lastBy);
            }

            // Hack in order to be sure that element is really displayed properly
            // It's due that sometime part of element is visible but still not clickable for Selenium
            try
            {
                element.Click();
            }
            catch (InvalidOperationException)
            {
                actions.Perform();
                element.Click();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void SendKeys(string keys)
        {
            Instance.WaitForAngular();
            Actions actions = new Actions(driver);
            actions.SendKeys(keys);
            actions.Perform();
        }

        //public static NgWebElement Find(NgBy by)
        //{
        //    try

        //    {
        //        return FindElement(by);
        //        //return FindElement(by);
        //    }
        //    catch (NoSuchElementException)
        //    {
        //        WaitFor(by);
        //    }

        //    return FindElement(by);
        //}

        //private static NgWebElement FindElement(NgBy by)
        //{
        //    throw new NotImplementedException();
        //}

        public static NgWebElement Find(By by, int index)
        {
            try
            {
                return FindElements(by)[index];
            }
            catch (ArgumentOutOfRangeException)
            {
                WaitFor(by, index);
            }
            catch (NoSuchElementException)
            {
                WaitFor(by);
            }

            return FindElements(by)[index];
        }

        public static ReadOnlyCollection<NgWebElement> FindAll(By by)
        {
            return FindElements(by);
        }

        public static void ClearAndSendKeys(this NgWebElement element, string text)
        {
            try
            {
                if (ProjSettings.IsVirtualKeyboardEnabled)
                {//
                  //  Modal.VirtualKeyboardModal.ChangeFocus();
                }

                element.Click();
                element.Clear();
                element.SendKeys(text);
            }
            catch
            {
                Console.WriteLine("ClearAndSendKeys failed! Again try is performed!");
                Thread.Sleep(2000);
                element.SendKeys(text);
            }
        }

        public static void SwipeLeft(this NgWebElement element)
        {
            Actions actions = new Actions(Instance);
            actions.ClickAndHold(element).MoveByOffset(-70, 0).Release().Build().Perform();
            // Temporary workaround for Swipe Method
            Thread.Sleep(800);
        }

        public static void SwipeRight(this NgWebElement element)
        {
            Actions actions = new Actions(Instance);
            actions.ClickAndHold(element).MoveByOffset(50, 0).Release().Build().Perform();
        }

        public static string GetValue(this NgWebElement element)
        {
            return element.GetAttribute("value");
        }

        public static bool IsExistElement(By by, bool waitIfNotExists = false)
        {
            try
            {
                Instance.WaitForAngular();

                if (waitIfNotExists)
                {
                    WaitFor(by);
                }

                return FindElement(by) != null;
            }
            catch (InvalidOperationException e)
            {
                // Temporary workaround for Protractor problem with MobileWrapper CefSharp
                // Check if InvalidOperatior was caused by getTestability issue and if yes, try once again, else return false
                if (e.Message.Contains("no injector found for element argument to getTestability") ||
                    e.Message.Contains("Cannot read property '$$testability'"))
                {
                    WaitFor(by);
                    return FindElement(by) != null;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsExistElement(By by, int index, bool waitIfNotExists = false)
        {
            try
            {
                Instance.WaitForAngular();
                if (waitIfNotExists) WaitFor(by, index);
                return FindElements(by)[index] != null;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsVisibleIfExist(By by, bool waitIfNotExists = false)
        {
            try
            {
                Instance.WaitForAngular();

                if (waitIfNotExists)
                {
                    WaitFor(by);
                }

                // NgWebElement element = FindElement(by);

                // return element != null ? element.Displayed : false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void ClickElement(By by)
        {
           // DriverJQueryHelper.JQueryAction(by, "click()");
        }

        public static void ClickElement(By by, int index)
        {
           // DriverJQueryHelper.JQueryAction(by, "eq(" + index + ").click()");
        }

        public static void ExecuteScript(string script)
        {
            Instance.WaitForAngular();
            IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)Instance.WrappedDriver;
            javaScriptExecutor.ExecuteScript(script);
        }

        public static string GetElementText(By by)
        {
            IWebElement element = Find(by);
            return element.Text;
        }

        private static IWebElement Find(By by)
        {
            throw new NotImplementedException();
        }

        public static string GetAttributeValue(By by, string attributeName)
        {
            IWebElement element = Find(by);
            return element.GetAttribute(attributeName);
        }

        public static string GetTPTextID(this NgWebElement element)
        {
            string tpTextID = element.GetAttribute("tp-text-id");

            return tpTextID;
        }

        public static string GetDataTPTextID(this NgWebElement element)
        {
            string dataTPTextID = element.GetAttribute("data-tp-text-id");

            return dataTPTextID;
        }

        public static string GetTextContent(this NgWebElement element)
        {
            string textContent = element.GetAttribute("textContent");

            return textContent;

        }

        public static string GetBackgroundColor(this NgWebElement element)
        {
            string backgroundColor = element.GetCssValue("background-color");

            return backgroundColor;
        }

        public static string GetBorderColor(this NgWebElement element)
        {
            string borderColor = element.GetCssValue("border-color");

            return borderColor;
        }

        public static int GetIndexOfSearchedElement(this IList<NgWebElement> list, string searchText)
        {
            var element = list.Where(x => x.GetTextContent().Equals(searchText)).FirstOrDefault();

            return list.IndexOf(element);
        }

        public static bool IsNgInvalid(this NgWebElement element)
        {
            string isNgInvalid = element.GetAttribute("class");

            return string.IsNullOrEmpty(isNgInvalid) ? false : isNgInvalid.Contains("ng-invalid");
        }

        public static bool IsNgHide(this NgWebElement element)
        {
            string isNgHide = element.GetAttribute("class");

            return string.IsNullOrEmpty(isNgHide) ? false : isNgHide.Contains("ng-hide");
        }

        public static bool IsReadonly(this NgWebElement element)
        {
            string isReadonly = element.GetAttribute("readonly");

            return string.IsNullOrEmpty(isReadonly) ? false : isReadonly.Equals("true");
        }

        public static bool ClassContains(this NgWebElement element, string className)
        {
            string classNames = element.GetAttribute("class");

            return string.IsNullOrEmpty(className) ? false : classNames.Contains(className);
        }

        public static void Url(string url)
        {
            Instance.Url = url;
        }

        public static void RefreshApp()
        {
            Instance.Navigate().Refresh();
        }

        public static void Quit()
        {
            try
            {
                Instance.Quit();
            }
            catch
            {
                Console.WriteLine("There is no instance Driver to be Quit!");
            }

            Initialised = false;
        }

        public static void Reboot()
        {
            Quit();
            Initialize();
        }

        public static string GetUrl()
        {
            // To Avoid $$getTestability Protractor issue it's necessary to wait for it's loading
            // It's caused with the MobileWrapper of Sephora

            Stopwatch sw = Stopwatch.StartNew();
            string url = null;

            while (url == null && sw.ElapsedMilliseconds < 5000)
            {
                try
                {
                    url = Instance.Url;
                }
                catch
                {
                    url = null;
                }
            }

            return url;
        }

        #region Predicates
        public static Func<NgWebElement, bool, bool> EnabilityCondition = (element, value) => { return element.Enabled == value; };
        #endregion

        #region Wait Methods

        public static void WaitFor(By by)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Instance, _waitTimeoutWrappedTimeSpan);
                wait.Timeout = _waitTimeoutWrappedTimeSpan;
                wait.Until(x =>
                {
                   IWebElement ieElement = FindElement(by);
                    if (ieElement != null) return ieElement;
                    return null;
                });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + Environment.NewLine + by + " not found");
            }
        }

        public static void WaitFor(By by, int index)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Instance, _waitTimeoutWrappedTimeSpan);
                wait.Timeout = _waitTimeoutWrappedTimeSpan;
                wait.Until(x =>
                {
                    IList<NgWebElement> ieElements = FindElements(by);
                    if (ieElements.Count > 0 && index < ieElements.Count)
                    {
                        return ieElements[index];
                    }
                    return null;
                });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + Environment.NewLine + by + " not found");
            }
        }

        public static NgWebElement WaitAndFind(By by, Func<NgWebElement, bool, bool> predicate, bool value)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Instance, _waitTimeoutWrappedTimeSpan);
                wait.Timeout = _waitTimeoutWrappedTimeSpan;
                NgWebElement webElement = wait.Until(x =>
                {
                    NgWebElement ieElement = Instance.FindElement(by);
                    bool check = predicate(ieElement, value);
                    if (ieElement != null && check) return ieElement;
                    return null;
                });

                return webElement;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + Environment.NewLine + @by + " not found with given condition");
            }
        }

        public static bool WaitAndFind<T>(Func<T> predicateAction, T predicateValue = default(T))
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Instance, _waitTimeoutWrappedTimeSpan);
                wait.Timeout = _waitTimeoutWrappedTimeSpan;

                return wait.Until(x =>
                {
                    return new Func<T>(predicateAction)().Equals(predicateValue);
                });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + Environment.NewLine + "Element was not found or condition was not meet on '" + predicateAction.Target + "'");
            }
        }

        #endregion

        #region Private Methods

        private static IWebElement FindElement(By by)
        {
            lastBy = by;
            return Instance.FindElement(by);
        }

        private static ReadOnlyCollection<NgWebElement> FindElements(By by)
        {
            lastBy = by;
            return Instance.FindElements(by);
        }

        #endregion
    }
}
