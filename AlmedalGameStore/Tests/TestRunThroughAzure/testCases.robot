*** Settings ***
Documentation   Emelies webbtester för att testa Almedal gamestore
Library     SeleniumLibrary
Library    Collections
Resource    resources.robot
Test Setup     Startsida

*** Test Cases ***
Översikt av kundkorg ska visa överskådlig information
    [Documentation]     I överblicken ska det framgå: Antal produkter (för varje enskild produkt), Miniatyrbild av produkten, Produktens namn och Pris
    [Tags]      Kundkorgsöversikt
    Given user has gone to product side
    And added product to chart
    When user clicks on chart
    Then user should see details about chart

Se alla produkter för att hitta en produkt att köpa
    [Documentation]     När en kund/användare navigerar till produktsidan ska alla produkter visas. Man ska även kunna se pris. Det ska även finnas en knapp för att kunna lägga till produkten i varukorgen.
    [Tags]      Produktlista
    Given user has gone to product side
    Then user should se all products

Erbjuda meddlemskap
    [Documentation]     Startsidan ska erbjuda kunden att bli medlem
    [Tags]      Starsida
    When user clicks on bli medlem
    Then user should come to create an account-page

Se spel och relevant information
    [Documentation]     Startsidan ska visa några spel
    [Tags]      Starsida
    Then user should see games

Karusellen skall finnas synlig
    [Documentation]     Startsidan ska visa karusellen
    [Tags]      Starsida
    Then user should see carousel

#Se recensioner
#    [Documentation]     Startsidan ska visa recensioner
#    [Tags]      Starsida
#    Then user should see reviews

Se kommande event
    [Documentation]     Eventsidan ska visa kommande event
    [Tags]      Event
    When user has gone to event side
    Then user should see at least 1 event

Platsinfo om event
    [Documentation]     Detaljvyn för event ska visa detaljer om event
    [Tags]      Event
    When user has gone to event side
    And clicks on eventInfo-button
    Then user should see details for event

#Sortera bland spel - lägst pris till högst
    #[Documentation]     Sorterar från lägst pris till högst
    #[Tags]      Sort
    #Given user has gone to product side
    #When user chooses sort low to high
    #Then user should see lowest price first

Sortera bland spel - efter ålder
    [Documentation]     Sorterar utifrån ålder
    [Tags]      Sort
    Given user has gone to product side
    When user chooses sort age 3+
    Then user should NOT see games with higher age restriction

Sortera bland spel - efter genre
    [Documentation]     Sorterar efter genre tactical
    [Tags]      Sort
    Given user has gone to product side
    When user chooses sort by genre Tactical
    Then user should NOT see games from other genres

Logga in admin
    [Documentation]     Loggar in admin
    [Tags]      Admin
    When user enters username and password     ${correctAdminUsername}     ${password}
    And clicks login
    Then user should be logged in

Ogiltig användare ska inte kunna logga in som admin
    [Documentation]     Ogiltiga inloggningsuppgifter ska inte kunna logga in admin
    [Tags]      Admin
    When user enters username and password     ${INCorrectAdminUsername}     ${password}
    And clicks login
    Then user should NOT be logged in

Logga in redan registrerad kund
    [Documentation]     Giltiga inloggningsuppgifter ska logga in kund
    [Tags]      Kundinloggning
    When user enters username and password      ${correctCustomerUsername}     ${correctCustomerPassword}
    And clicks login
    Then user should be logged in

Ogiltig användare ska inte kunna logga in som kund
    [Documentation]     Ogiltiga inloggningsuppgifter ska inte kunna logga in kund
    [Tags]      Kundinloggning
    When user enters username and password     ${INcorrectCustomerUsername}     ${correctCustomerPassword}
    And clicks login
    Then user should NOT be logged in
