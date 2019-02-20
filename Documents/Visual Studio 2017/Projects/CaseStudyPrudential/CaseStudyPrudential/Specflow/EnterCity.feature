Feature: EnterCity Check the weather of the city which is searched 
Scenario Outline:Open the url 
Given  navigate to the page
When  enter the valid city name <cityname>
And  Click on the search button 
Then weather details for the city should be shown 

Examples: 
|cityname|
|vadodara|
