using OpenQA.Selenium;
using System;
using TechTalk.SpecFlow;

namespace CaseStudyPrudential
{

  

    [Binding]
    public class EnterCitySteps:BaseClass.BasePage
    {



      

        public EnterCitySteps()
        {

            SetUp();
        }

        [Given(@"navigate to the page")]
        public void GivenNavigateToThePage()
        {
            NavigatetoUrl("https://openweathermap.org/");
        }
        
        [When(@"enter the valid city name")]
        public void WhenEnterTheValidCityName(string cityname)
        {

            
        }


        [When(@"enter the valid city name vadodara")]
        public void WhenEnterTheValidCityNameVadodara()
        {
            HomePage _homepage = new HomePage();
          _homepage.EnterCityname  ("vadodara");

        }


        [When(@"Click on the search button")]
        public void WhenClickOnTheSearchButton()
        {
            HomePage _homepage = new HomePage();
            _homepage. SearchButton();
        }
        
        [Then(@"weather details for the city should be shown")]
        public void ThenWeatherDetailsForTheCityShouldBeShown()
        {
           
           
        }
    }
}
