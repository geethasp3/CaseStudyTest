using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseStudyPrudential.BaseClass
{



    public class BasePage
    {
        public static IWebDriver Driver;
        public static string websiteUrl;

        [SetUp]
        public void SetUp()
        {
            Driver = new ChromeDriver();
            Driver.Manage().Window.Maximize();
          Driver.Manage().Timeouts().ImplicitWait= TimeSpan.FromSeconds(120);

        }

        public void  NavigatetoUrl(string websiteUrl)
        {
            Driver.Navigate().GoToUrl(websiteUrl);
        }


    


        public void handleNotifications()
        {
            ChromeOptions _options = new ChromeOptions();
            _options.AddArguments("--disable-notifications");

          
          //  System.setProperty("webdriver.chrome.driver", "path/to/driver/exe");

           Driver  = new ChromeDriver(_options);
        }
        [TearDown]
        public void Cleanup()
        {
            Driver.Quit();
        }
    }
}
