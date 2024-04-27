*** Settings ***
Documentation   Resourcesfil för webbtester
Library     SeleniumLibrary
Library    Collections

*** Variables ***
${URL}      https://almedal-web-development.azurewebsites.net/
${expectedID}       custom-card
${correctAdminUsername}     m@m.m
${INCorrectAdminUsername}       m@m.n
${password}     Hej!123
${correctCustomerUsername}      an@an.an
${correctCustomerPassword}      Hej!123
${INcorrectCustomerUsername}    annnanBusAdress@hejhoppsno.pp

*** Keywords ***
Startsida
    [Documentation]     Öppnar chrome och navigerar till startsidan
    Open Browser    browser=Chrome
    Maximize Browser Window
    Go To           ${URL}


user has gone to product side
    Wait Until Page Contains Element    products-link
    Click link    products-link
    Wait Until Page Contains    The Witcher 3: Wild Hunt

added product to chart
    Wait Until Page Contains Element    (The Witcher 3: Wild Hunt)ProductButton
    Click Button    (The Witcher 3: Wild Hunt)ProductButton
    Sleep    2
    Wait Until Page Contains Element    continueButton
    Click Button    continueButton


user clicks on chart
    Wait Until Element Is Visible    id=cart-link
    Click Element    id=cart-link



user should see details about chart
    Sleep    1
    Wait Until Page Contains    Kundvagn - 1 produkter
    Wait Until Page Contains    The Witcher 3: Wild Hunt
    Wait Until Page Contains Element    //img[@alt='CoverPicture']
    Wait Until Element Contains    (The Witcher 3: Wild Hunt)Price    549
    Close Browser

user should se all products
    Wait Until Page Contains Element    class:card-img-top
    Sleep    2
    ${elements}    Get WebElements    class:card-img-top
    ${number_of_elements}    Get Length    ${elements}
    Should Be True    ${number_of_elements} > 0
    Close Browser

user clicks on bli medlem
    Wait Until Page Contains      Bli medlem nu!
    Click Element    medlemsknapp


user should come to create an account-page
    Wait Until Page Contains    Skapa konto
    Close Browser

user should see games
    Wait Until Page Contains Element    xpath://img[contains(@class, 'card-img-top')]

    Close Browser

user should see carousel
    Wait Until Element Is Visible    myCarousel
    Close Browser

user should see reviews
    Wait Until Page Contains Element    xpath://div[contains(@class, 'card-body')]

    
user has gone to event side
    Wait Until Page Contains Element    events-link
    Click Link    events-link



user should see at least 1 event
    Wait Until Page Contains Element    class:card-body
    Wait Until Page Contains Element    //img[@alt='...']
    Wait Until Page Contains    99 kr
    #Wait Until Page Contains    Datum:
    Close Browser


user clicks add-event-button
    Wait Until Page Contains Element    (Lan i almedal)ProductButton
    Click Button    (Lan i almedal)ProductButton
    
user should see details for event
    Wait Until Page Contains    Lan i almedal
    Wait Until Page Contains    Price: 99 kr
    Wait Until Page Contains    Age requirement:
    Close Browser


user clicks on checkout
    Wait Until Page Contains Element    xpath://a[@class='btn btn btn-primary btn-block'][@href='Checkout']
    Click Element    xpath://a[@class='btn btn btn-primary btn-block'][@href='Checkout']



user clicks on klarna radio-button
    Wait Until Page Contains    Klarna
    Click Element    xpath://label[@for='klarna']

    
clicks on eventInfo-button
    Wait Until Page Contains Element    id=(Lan i almedal)InfoButton
    Click Element    id=(Lan i almedal)InfoButton

user chooses sort low to high
    Wait Until Page Contains Element    sortera
    Click Element    sortera
    Wait Until Element Is Visible   xpath=//a[contains(text(), 'Pris: Lägst till högst')]
    Click Element    xpath=//a[contains(text(), 'Pris: Lägst till högst')]

user should see lowest price first
    Wait Until Element Is Visible    custom-card
    ${first_h5}    Get Text    xpath=(//div[@class='custom-card']//h5)[1]    # Hämta texten från det första h5-elementet inom klassen
    Should Be Equal As Strings    ${first_h5}    Call of Duty: Modern Warfare 3    # Jämför texten med förväntat värde "0 kr"
    Close Browser

    Close Browser

user chooses sort age 3+
    Wait Until Page Contains Element    xpath=//div[@class='dropdown']
    Click Element    högstÅlder
    Wait Until Element Is Visible    xpath=//a[contains(text(), '3+')]
    Click Element    xpath=//a[contains(text(), '3+')]

user should NOT see games with higher age restriction
    Sleep    1
    Element Should Not Be Visible    //h5[@class='card-title'][text()='The Witcher 3: Wild Hunt']
    Element Should Not Be Visible    //h5[@class='card-title'][text()='Among Us']
    Element Should Not Be Visible    //h5[@class='card-title'][text()='Valorant']
    Close Browser

user chooses sort by genre Tactical
    Wait Until Page Contains Element    xpath=//div[@class='dropdown']
    Click Element    genre
    Wait Until Element Is Visible    xpath=//a[contains(text(), 'Tactical')]
    Click Element    xpath=//a[contains(text(), 'Tactical')]

user should NOT see games from other genres
    Sleep    1
    Element Should Not Be Visible    //h5[@class='card-title'][text()='The Witcher 3: Wild Hunt']
    #Element Should Not Be Visible    //h5[@class='card-title'][text()='Assassin's Creed Valhalla']
    Element Should Not Be Visible    //h5[@class='card-title'][text()='Among Us']
    Element Should Not Be Visible    //h5[@class='card-title'][text()='Hitman 3']
    Close Browser

user enters username and password
    [Arguments]     ${correctAdminUsername}     ${password}
    Wait Until Element Is Visible    loginBtn
    Click Element    loginBtn
    Wait Until Element Is Visible    email
    Input Text    email    ${correctAdminUsername}
    Input Text    password    ${password}

clicks login
    Wait Until Element Is Visible    LoginBtn
    Click Element    LoginBtn

user should be logged in
    Wait Until Page Contains    Logga ut
    
user should NOT be logged in
    Page Should Not Contain    Logga ut
