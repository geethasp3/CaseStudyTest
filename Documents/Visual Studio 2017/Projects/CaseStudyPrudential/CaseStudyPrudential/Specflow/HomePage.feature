Feature: HomePage
	check the labels and buttons and the pages present on the webpage

@mytag
Scenario: Check pages on the page
	Given the url of the website given and navigate to the url
	When  click on signup button 
	Then signup page opened 
	Given create user account and fill the details
	Then user should be created

