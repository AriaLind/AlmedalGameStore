# README

De flesta connectionstringsen och addresserna bör vara ändrade till lokala, men det kan vara så att jag missat något. Om det inte skulle gå att köra programmet eller om något strular så är det bara att höra av sig så kikar jag på det.

Jag är osäker på hur våra logic apps fungerar på cloud-sidan, men jag tror inte att deras connection strings behöver ändras. Om det är så att du behöver miljövariabler eller liknande så är det bara att höra av sig så skickar jag connection stringen.

## Lite snabb setup

1. Efter att du gjort en update-database, gå in i databasen och lägg till två nycklar i tabellen ApiAuthKeys som har följande namn, och dom måste vara exakta. Deras värden kan vara vad du vill.
    - MongoDbApi
    - SqlApi
2. Ändra MongoDbUnitOfWorks connection string till din lokala MongoDb connection string. (I konstruktorn för unit of worken)
