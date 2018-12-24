using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POStest
{
  
    public abstract class POSBaseClass
    {
        public string urlstring="http://localhost:8088";

        public static IWebDriver driver;


     public IWebDriver  getdriver()
        {

        return    driver = new ChromeDriver();
        }


        public static By Element (string ele)
        {
            return By.CssSelector(ele);

        }




    }

  
}
