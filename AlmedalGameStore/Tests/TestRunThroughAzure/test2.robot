*** Settings ***
Documentation   Almedal Game Store
Library         SeleniumLibrary

*** Variables ***
${BASE_URL}     https://almedal-web.azurewebsites.net/products
${CART_URL}     https://almedal-web.azurewebsites.net/cart
${CHECKOUT_URL}  https://almedal-web.azurewebsites.net/Checkout
${BROWSER}       Headlesschrome

*** Test Cases ***

Sort Products
    [Documentation]     The customer sorts products by price, age, and genre.
    [Tags]              sort
    Open Browser        ${BASE_URL}    ${BROWSER}
    Wait Until Element Is Visible    xpath=//button[contains(text(), 'Sortera')]
    # Sort dropdown menu
    Execute JavaScript    document.evaluate("//button[contains(text(), 'Sortera')]", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.click()
    # Select "Price: Low to High" from dropdown menu
    Execute JavaScript    document.evaluate("//a[contains(text(), 'Price: Low to High')]", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.click()
    # Select "Highest age: 3+" from dropdown menu
    Execute JavaScript    document.evaluate("//button[contains(text(), 'Högsta ålder')]", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.click()
    Execute JavaScript    document.evaluate("//a[contains(text(), '3+')]", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.click()
    # Select "Genre: Sports" from dropdown menu
    Execute JavaScript    document.evaluate("//button[contains(text(), 'Genre')]", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.click()
    Execute JavaScript    document.evaluate("//a[contains(text(), 'Sports')]", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.click()
     Wait Until Page Contains Element    xpath=//img[@src='https://almedalstorage.blob.core.windows.net/almedalblob/fifa22.jpg']
    Close Browser


Add Product To Cart
    [Documentation]     When I visit the website, I want to quickly and easily find a clear button that lets me add an item to the cart.
    [Tags]              add-to-cart
    Open Browser        ${BASE_URL}   ${BROWSER}
    Wait Until Element Is Visible    id=custom-card
    # Click info button
    Execute JavaScript    document.getElementById('(Hitman 3)InfoButton').click()
    # Click add button to add the product to the cart
    Execute JavaScript    document.getElementById('(Hitman 3)ProductButton').click()
    Set Selenium Implicit Wait    2s
    Close Browser

Shopping Cart
    [Documentation]     The customer goes to the shopping cart and selects payment options.
    [Tags]              shopping-cart
    Open Browser    ${CART_URL}    ${BROWSER}

 Wait Until Element Is Visible    xpath=//h5[text()='Cart - 2 items']
    Wait Until Page Contains Element    xpath=//h6[text()='Hitman 3']
    Wait Until Page Contains Element    xpath=//h6[text()='Valorant']
    Click Button    css=.btn.btn-primary.btn-sm.me-1.mb-2   # Interact With Remove Item Button
    Click Button    css=.btn.btn-danger.btn-sm.mb-2   # Interact With Move To Wishlist Button
    Click Button    css=.btn.btn-primary.px-3.me-2  # Interact With Product Quantity (Minus button)
    Click Button    css=.btn.btn-primary.px-3.ms-2  # Interact With Product Quantity (Plus button)
    Close Browser

Go To Checkout Page
    [Documentation]     The customer goes to the checkout page from the cart.
    [Tags]              checkout
    Open Browser        ${CART_URL}   ${BROWSER}
    Wait Until Page Contains Element    xpath=//h5[text()='Sammanfattning']
    Click Element    xpath=//a[text()='Gå till utcheckning']
    Close Browser

Interact With Checkout Page
    [Documentation]     The customer interacts with the checkout page by entering a promo code and clicking a button.
    [Tags]              interact
    Open Browser    ${CHECKOUT_URL}   ${BROWSER}
    Execute JavaScript    return document.querySelector('h4 span').innerText === 'Your cart'
    Execute JavaScript    return document.evaluate("//li/div/h6[text()='The Witcher 3: Wild Hunt']", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue !== null
    Execute JavaScript    return document.evaluate("//li/div/h6[text()='Hitman 3']", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue !== null
    Execute JavaScript    return document.evaluate("//li/div/h6[text()='Valorant']", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue !== null
    Execute JavaScript    document.querySelector("input[name='_promoCodeModel.PromoCode']").value = 'YOUR_PROMO_CODE'
    Element Should Be Visible    xpath=//button[contains(text(), 'Aktivera')]
    Close Browser

Select Klarna As Payment Option
    [Documentation]     The customer selects Klarna as the payment option on the checkout page.
    [Tags]              payment-options
    Open Browser        ${CHECKOUT_URL}   ${BROWSER}
    Click Element        //label[@for='klarna']
    Execute JavaScript    document.getElementById('someElementId')
    Execute JavaScript    window.scrollTo(0, 500)
    Element Should Be Visible    xpath=//form[contains(@class, 'card')]
    Close Browser

Select Credit Card As Payment Option
    [Documentation]      The customer selects a credit card as the payment option on the checkout page and fills in the payment details.
    [Tags]              credit-card
    Open Browser        ${CHECKOUT_URL}   ${BROWSER}
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

Select Debit Card As Payment Option
    [Documentation]     The customer selects a debit card as the payment option on the checkout page and fills in the payment details.
    [Tags]              debit-card
    Open Browser        ${CHECKOUT_URL}   ${BROWSER}
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
