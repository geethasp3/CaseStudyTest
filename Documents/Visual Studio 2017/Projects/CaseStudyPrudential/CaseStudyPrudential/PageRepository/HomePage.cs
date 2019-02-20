using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseStudyPrudential
{
    public class HomePage:BaseClass.BasePage
    {




      


        public void EnterCityname(string city_name)
        {
            Driver.FindElement(By.XPath("//form[@id='searchform']/div/input")).SendKeys(city_name);
        }

        public void SearchButton()
        {
            Driver.FindElement(By.CssSelector("button[class='btn btn-orange']")).Click();


        }


        public void getcityweatherdetails()
        {

            string gettext = Driver.FindElement(By.CssSelector("span[class='badge badge-info']")).Text;


        }

        public void current_location()
        {
            Driver.FindElement(By.CssSelector("button[class='btn search-cities__lnk']")).Click();


        }

        public void guidemenu()
        {
            Driver.FindElement(By.CssSelector("a[href='/guide']")).Click();

        }

        public void APImenu()
        {
            Driver.FindElement(By.CssSelector("a[href='/api']")).Click();

        }

        public string checkthetitle()
        {
        string titletext= Driver.Title;
            return titletext;

        }

        public void signup_page()
        {

            Driver.FindElement(By.CssSelector("a[href='//home.openweathermap.org/users/sign_up']")).Click();

            Driver.FindElement(By.CssSelector("input[id='user_username']")).SendKeys("abc");

            Driver.FindElement(By.CssSelector("input[id='user_email']")).SendKeys("abc@gmail.com");
            Driver.FindElement(By.CssSelector("input[id='user_password']")).SendKeys("abc");
            Driver.FindElement(By.CssSelector("input[id='user_password_confirmation']")).SendKeys("abc");

            Driver.FindElement(By.CssSelector("input[id='agreement_is_age_confirmed']")).Click();
            Driver.FindElement(By.CssSelector("input[id='agreement_is_accepted']")).SendKeys("abc");
        }
      








    }
}
