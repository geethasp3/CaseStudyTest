using System;
using TechTalk.SpecFlow;

namespace CaseStudyPrudential
{
    [Binding]
    public class HomePageSteps:BaseClass.BasePage
    {

        public HomePageSteps()
        {
            SetUp();
        }
        [Given(@"the url of the website given and navigate to the url")]
        public void GivenTheUrlOfTheWebsiteGivenAndNavigateToTheUrl()
        {

            NavigatetoUrl("https://openweathermap.org/");
            HomePage _homepage = new HomePage();
            _homepage.current_location();


        }
        
        [Given(@"create user account and fill the details")]
        public void GivenCreateUserAccountAndFillTheDetails()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"click on signup button")]
        public void WhenClickOnSignupButton()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"signup page opened")]
        public void ThenSignupPageOpened()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"user should be created")]
        public void ThenUserShouldBeCreated()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
