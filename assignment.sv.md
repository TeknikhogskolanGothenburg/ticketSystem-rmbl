# Ticket system


Som i dom andra projekt jobbar vi GitHub, en skilnad denna gång är att ert repo protected, så det enda sätt att få in kod är gennom pull request.

Kommunikation i teamen skall göras på Slack i en publik Channel (med samma namn som ert team), och med integration till ert github projekt

## Slut datum

Sista chans for att få med kod är Måndag den 19:e kl 20:00 CET.

# Containers

Systemet består av fyra containers (se [arkitektur](http://localhost:8080/architecture/index.html) i docfx), varja container har en solution fil i *src*-mappen.

Det är endast grundläggande delar som har implementeras, så det är mycket sannolikt att alla containers på något sätt måste refactoreres, ändras eller utökas. Och ni får ändra och fixa präcis som ni kännar.

## Container: Database

All data skall sparas i en SQL databas, och där finns som en del av projeket en simpel databas model, se denna i docfx avsnittet om [arkitektur](http://localhost:8080/architecture/index.html#database).

I mappen *src* finns en visual studio solution kallat database.sln, denna innehåller en konsol applikation som via [DbUp](https://dbup.readthedocs.io/en/latest/) initalisere databasen gennom [transitions](https://dbup.readthedocs.io/en/latest/philosophy-behind-dbup/#transitions-not-states). Den datamodel som är implementeret är väldig basic och kan behövs att utökas, om detta är fallet kom i håg att göra alla förandringer i ett nytt script.

Solutionen innehåller ytermer två andra projekt, ett kallat DatabaseRepository (använns även i REST API) detta projekt kod som läser från och skriver till databasen, det andra projekt kallat DatabaseRepositoryTestclient, är en konsol applikation som är använt som ett enkelt sätt att testa DatabaseRepository, detta är inte automatiska testar.

## Container: REST API

Backoffice och TicketShop kommunikera endast med REST API, det betyder att REST APIet sköter kommunikation med databasen och betalleventöra.

Ett API är ett kontrakt som är emellan server och klient, det betyder att man måste ha en bra dokumentation av APIet, en första version av kontraktet finns på [SwaggerHub](https://app.swaggerhub.com/apis/Distancify6/TicketSystem/1.0.0), och även i eran docfx-dokumentation i mappen [webapi](http://localhost:8080/webapi/swagger.html).

REST API måste göra dom ressourcer som Backoffice och TicketShop behöver tillgängelige, och med stöd för dom metoder som behövs (GET, POST, PUT, DELETE). 

Från SwaggerHub går det ladda ner boilerplate kod för båda klient och server, detta kan vara en hjälp även om koden kan vara litet svår att läsa.

## Container: Backoffice 
 basseret på swagger api
skåpa biljet som pdf

Inloging (VG):

## Container: TicketShop (E-handel, biljett shop)

Asyknon del (VG): Visar hur många som är inne på ett event just nu, och när senasta biljett köptes, och vilka events som är populära just nu

# Dokumentatonsdel

I roten för detta projekt kör komandoen ```docfx .\docfx.json --serve```, detta kompilera dokumentationen och starter en webserver, du kan nu i en webbläser gå till [http://localhost:8080](http://localhost:8080) för att browsa dokumentationen.

Dokumentationen innehåller redan nu olika delar, och ert primära jobb är att underhålla denna så att den stämmer med den kod som är incheckkat i Github.

Där är ett avsnitt som hetter *User stories* detta måsta fyllas i, och det rekomenderas att ni gör någon sort av usecase diagram, där är redan en basal variant i arkitektur dokumentet.

Det viktigista dokument är API dokumentationen om finns i mappen *webapi* i filen *swagger.json*. Json filen kan var litet svår att underhålla och det enkalsta sätt att skriva [Swagger](http://swagger.io) är gennom att använda online verktyget [SwaggerHub](https://swaggerhub.com) och skriva APIet med yaml, och sen exportera det som json och skriva över *swagger.json* i *webapi*-mappen.

# Tips / Hints
Börja med i teamen med att göra en förventnings avstämning, vad är erant mål i team, och vad är eran indivuella styrkor.

Försök att unvika att någon i teamen är syssellösa, om där är samarbejds vansklighetter se till att ta i dom så tidigt som möjligt.

Om någon i gruppen har svårt vid delar av koden, försök att köra pair-programming, och se till at den om tycker det är svårt är [Driver](https://gist.github.com/jordanpoulton/607a8854673d9f22c696)

Om ni sittar på distans taffas med video en gång per dag (via [skype](https://www.skype.com), [google hangout](https://hangouts.google.com), [appear](https://appear.in) eller likande), och kör pair-programming via [TeamViewer](https://www.teamviewer.com) eller likande.

Använn Github aktivt: Issues, Pull requets, Projects. Det gör det enklare för alla hänga med på alla förandringer och ideer.

Forsög att att följa SOLID så långt det går.

Steg (kan göras paralellt):

* I solutionen *RESTAPI.sln* lägg till en *ASP.NET Core Web Application*, välj WebAPI, implementera detta api så att det matcher dokumentationen (denna kan behövs att utökas)
* I solutionen *Backoffice.sln* lägg till en *ASP.NET Core Web Application*, välj Web Application (Model-View-Controller), implementera denna, om rest apiet inte är klar än, gör en [stub](https://stackoverflow.com/questions/9777822/what-does-to-stub-mean-in-programming)
* I solutionen *TicketShop.sln* lägg till en *ASP.NET Core Web Application*, välj Web Application (Model-View-Controller), implementera denna, om rest apiet inte är klar än, gör en [stub](https://stackoverflow.com/questions/9777822/what-does-to-stub-mean-in-programming)


# Betygskrav
## G
* Tre websites gjort i ASP.NET Core 2 (i denna prioorning)

    1. Ett REST API  
    2. Ett administrativtgräsnitt (Backoffice)
    3. Ett användergränsitt (TicketShop)

* Det ska var möjligt att skåpa ett produkt i det adminstrativa gränsnitt och konsumera (köpa) det i användergränsnittet
* Input validering på alla input, i backend
* Dokumentationen måsta stämma med implementationen
* Logging med .NET Core logging och SEQ
* Båda Backoffice och TicketShop måste vara tillgängeligt på svenska och engelska

## VG
* Cake build script (kan med fördel konfigureras som det första)
* AppVoyage (kan med fördel konfigureras efter cake scriptet så att ni kan få in status på github)
* Deployat till Azure och demo körs därifrån
* Multi använder backoffice med inlogning som bygger på 
* Regex använns till input validering dom ställen vart det gir mening
* Logging i Azure
* Asynkron del, implemntard med React och SignalR
* Enhetstestar