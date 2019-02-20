using NUnit.Framework;
using System;
using TechTalk.SpecFlow;

namespace CaseStudyPrudential
{
    [Binding]
    public class CheckTheMenusOnThePageOfWeatherSteps:BaseClass.BasePage
    {

        public CheckTheMenusOnThePageOfWeatherSteps()
        {
            SetUp();
        }
        [Given(@"on the page , menu links available")]
        public void GivenOnThePageMenuLinksAvailable()
        {
            NavigatetoUrl("https://openweathermap.org/");
         
        }
        
        [When(@"click on the menu link")]
        public void WhenClickOnTheMenuLink()
        {
            HomePage _homepage = new HomePage();
            _homepage.guidemenu();
            string Guidetitle = _homepage.checkthetitle();
            Assert.AreEqual("Guide - OpenWeatherMap", Guidetitle);
        }
        
        [Then(@"the page accordingly will be opened")]
        public void ThenThePageAccordinglyWillBeOpened()
        {
            HomePage _homepage = new HomePage();
            _homepage.APImenu();
            string APITitle = _homepage.checkthetitle();
            Assert.AreEqual("Weather API - OpenWeatherMap", APITitle);
        }
    }
}
