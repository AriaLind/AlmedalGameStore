*** Settings ***
Documentation   Almedal Game Store
Library         SeleniumLibrary



*** Variables ***
${BASE_URL}     https://almedal-web.azurewebsites.net/products
${BASE_URL1}    https://almedal-web.azurewebsites.net/cart
${BASE_URL2}    https://almedal-web.azurewebsites.net/Checkout










*** Test Cases ***

Sortera produkter


    [Documentation]     Customer opens the product page
    [Tags]              product-page
          Open Browser        ${BASE_URL}

    Wait Until Element Is Visible    xpath=//button[contains(text(), 'Sortera')]

    # Sortera dropdown-menyn
    Execute JavaScript    document.evaluate("//button[contains(text(), 'Sortera')]", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.click()

    # Välj alternativet "Price: Low to High" från dropdown-menyn
    Execute JavaScript    document.evaluate("//a[contains(text(), 'Price: Low to High')]", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.click()

    # Välj alternativet "Högsta ålder: 3+" från dropdown-menyn
    Execute JavaScript    document.evaluate("//button[contains(text(), 'Högsta ålder')]", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.click()
    Execute JavaScript    document.evaluate("//a[contains(text(), '3+')]", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.click()

    # Välj alternativet "Genre: RPG" från dropdown-menyn
    Execute JavaScript    document.evaluate("//button[contains(text(), 'Genre')]", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.click()
    Execute JavaScript    document.evaluate("//a[contains(text(), 'RPG')]", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.click()


    Close Browser

Lägg till produkt i varukorgen
    [Documentation]     När jag besöker webbplatsen vill jag snabbt och enkelt kunna hitta en tydlig knapp som låter mig lägga till en vara i varukorgen.
    [Tags]              add-to-cart
              Open Browser        ${BASE_URL}   chrome

    Wait Until Element Is Visible    id=custom-card
        #klicka info knappen
    Execute JavaScript    document.getElementById('(Hitman 3)InfoButton').click()
        #klicka add knappen för att lägga produkten i varukorgen
    Execute JavaScript    document.getElementById('(Hitman 3)ProductButton').click()
    Set Selenium Implicit Wait    2s

    Close Browser

Shopping Cart
    [Documentation]     Customer goes to the shopping cart and selects payment options
    [Tags]              shopping-cart
    Open Browser    ${BASE_URL1}    chrome
    Wait Until Element Is Visible    xpath=//h5[text()='Cart - 2 items']
    Wait Until Page Contains Element    xpath=//h6[text()='Hitman 3']
    Wait Until Page Contains Element    xpath=//h6[text()='Valorant']
    Click Button    css=.btn.btn-primary.btn-sm.me-1.mb-2
    Click Button    css=.btn.btn-danger.btn-sm.mb-2
    Click Button    css=.btn.btn-primary.px-3.me-2
    Click Button    css=.btn.btn-primary.px-3.ms-2

    Close Browser




Gå till utcheckningssidan
    [Documentation]     Kunden går till utcheckningssidan från kundvagnen
    [Tags]
         Open Browser        ${BASE_URL1}   chrome
        Wait Until Page Contains Element    xpath=//h5[text()='Sammanfattning']
       Click Element    xpath=//a[text()='Gå till utcheckning']
    Close Browser



Interagera med utcheckningssidan
    [Documentation]     Kunden interagerar med utcheckningssidan genom att ange en kampanjkod och klicka på en knapp
    [Tags]
     Open Browser    ${BASE_URL2}   chrome
    # Vänta tills sidan innehåller önskade element
    Execute JavaScript    return document.querySelector('h4 span').innerText === 'Your cart'
    Execute JavaScript    return document.evaluate("//li/div/h6[text()='The Witcher 3: Wild Hunt']", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue !== null
    Execute JavaScript    return document.evaluate("//li/div/h6[text()='Hitman 3']", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue !== null
    Execute JavaScript    return document.evaluate("//li/div/h6[text()='Valorant']", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue !== null

    Execute JavaScript    document.querySelector("input[name='_promoCodeModel.PromoCode']").value = 'YOUR_PROMO_CODE'

    Execute JavaScript    document.querySelector(".btn.btn-secondary").click()
    Close Browser

Välj Klarna som betalningsalternativ
    [Documentation]     Kunden väljer Klarna som betalningsalternativ på utcheckningssidan.
    [Tags]              payment-options
     Open Browser        ${BASE_URL2}   chrome

  Click Element        //label[@for='klarna']
  Execute JavaScript    document.getElementById('someElementId')
  Execute JavaScript    window.scrollTo(0, 500)
  Element Should Be Visible    xpath=//form[contains(@class, 'card')]
  Close Browser

Välj kreditkort som betalningsalternativ
    [Documentation]      Kunden väljer kreditkort som betalningsalternativ på utcheckningssidan och fyller i betalningsuppgifte
    [Tags]              kreditkort
    Open Browser        ${BASE_URL2}   chrome
    Click Element        //label[@for='credit']

     Wait Until Element Is Visible    id=firstName
     # Fill in the billing address fields
    Input Text    id=firstName    Kalle
    Input Text    id=lastName    Anka
    Input Text    id=username    Hassan
    Input Text    id=email    xasanmire2016@gmail.com
    Input Text    id=address    Skolgatan 1
    Input Text    id=city    Boras
    Input Text    id=zip    50422
    Input Text    id=Phonenumber    033-333 45 77

    Click Element    xpath=//label[@for='same-address']

    # Fill in the payment information fields
    Input Text    id=cc-name    Johan Andersson
    Input Text    id=cc-number    1234567812345678
    Input Text    id=cc-expiration    12/24
    Input Text    id=cc-cvv    345
    Click Button    xpath=//button[@type='submit']
    Element Should Be Visible    xpath=//input[contains(@name, '_promoCodeModel.PromoCode')]

    Close Browser

Välj betalkort som betalningsalternativ
    [Documentation]     Kunden väljer betalkort som betalningsalternativ på utcheckningssidan och fyller i betalningsuppgifter.
    [Tags]              betalkort
    Open Browser        ${BASE_URL2}   chrome
     Set Selenium Implicit Wait    5s
    Wait Until Page Contains Element    //label[@for='debit']
    Execute JavaScript   document.getElementById('debit').click();

     Wait Until Element Is Visible    id=firstName
     # Fill in the billing address fields
    Input Text    id=firstName    Kalle
    Input Text    id=lastName    Anka
    Input Text    id=username    Hassan
    Input Text    id=email    xasanmire2016@gmail.com
    Input Text    id=address    Skolgatan 1
    Input Text    id=city    Boras
    Input Text    id=zip    45324
    Input Text    id=Phonenumber    033-333 45 77

    Click Element    xpath=//label[@for='same-address']

    # Fill in the payment information fields
    Input Text    id=cc-name    Johan Andersson
    Input Text    id=cc-number    4545567842345678
    Input Text    id=cc-expiration    12/25
    Input Text    id=cc-cvv    345
    Click Button    xpath=//button[@type='submit']
    Element Should Be Visible    xpath=//button[contains(text(), 'Aktivera')]
    Close Browser


