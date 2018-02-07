# User Stories
Feature: Admin
    As an Admin
    I want to add a flight from point A to B or A to B to C 
    Because I want to sell flights

Scenario: Purchasing flights
    Given I'm a User
    When I visit the booking page
    And I click 'Book Flight'
    Then the flight should take me to a site to input transaction details
