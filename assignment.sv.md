# Ticket system

Uppgiften skall lösas i teams på 3 till 4 studenter.

Uppgiften består av två delar, en programmeringsdel och en dokumentationsdel. 

Allt ni gör skall göras i ert GitHub repo (båda kod och dokumentation), som ligger på ert Team. Ni skall använda en ["commit tidigt och ofta"](https://blog.codinghorror.com/check-in-early-check-in-often/) ([1](https://sethrobertson.github.io/GitBestPractices/)) strategi, såklart bör ni endast commita kod och dokumentation som kan kompileras.

Som i dom andra projekt jobbar vi GitHub, en skillnad denna gång är att ert repo protected, så det enda sätt att få in kod är genom pull request.

Kommunikation i teamen skall göras på Slack i en publik Channel (med samma namn som ert team), och med integration till ert github projekt

## Slut datum

Sista chans for att få med kod är Måndag den 19:e kl 20:00 CET.

# Containers

Systemet består av fyra containers (se [arkitektur](http://localhost:8080/architecture/index.html) i docfx), värje container har en solution fil i *src*-mappen.

Det är endast grundläggande delar som har implementeras, så det är mycket sannolikt att alla containers på något sätt måste refactoreres, ändras eller utökas. Och ni får ändra och fixa precis som ni känner.

## Container: Database

All data skall sparas i en SQL databas, och där finns som en del av projektet en simpel databas modell, se denna i docfx avsnittet om [arkitektur](http://localhost:8080/architecture/index.html#database).

I mappen *src* finns en visual studio solution kallat database.sln, denna innehåller en konsol applikation som via [DbUp](https://dbup.readthedocs.io/en/latest/) initialisera databasen genom [transitions](https://dbup.readthedocs.io/en/latest/philosophy-behind-dbup/#transitions-not-states). Den datamodell som är implementerat är väldig grundläggande och kan behövs att utökas, om detta är fallet kom i håg att göra alla förändringar i ett nytt script.

Solutionen innehåller yttermera två andra projekt, ett kallat DatabaseRepository (används även i REST API) detta projekt kod som läser från och skriver till databasen, det andra projekt kallat DatabaseRepositoryTestclient, är en konsol applikation som är använt som ett enkelt sätt att testa DatabaseRepository, detta är inte automatiska testar.

## Container: REST API

Backoffice och TicketShop kommunicera endast med REST API, det betyder att REST APIet sköter kommunikation med databasen och betalleverantörer.

Ett API är ett kontrakt som är emellan server och klient, det betyder att man måste ha en bra dokumentation av APIet, en första version av kontraktet finns på [SwaggerHub](https://app.swaggerhub.com/apis/Distancify6/TicketSystem/1.0.0), och även i eran docfx-dokumentation i mappen [webapi](http://localhost:8080/webapi/swagger.html).

REST API måste göra dom resurser som Backoffice och TicketShop behöver tillgänglige, och med stöd för dom metoder som behövs (GET, POST, PUT, DELETE). 

Från SwaggerHub går det ladda ner boilerplate kod för båda klient och server, detta kan vara en hjälp även om koden kan vara litet svår att läsa.

## Container: Backoffice 
Backoffice systemet används av administratorn till att skåpa och administrera events, venues.

Administratorn skall kunna lägga upp "eventdates" som är ett event på ett specifikt venue på ett specifikt datum, med ett specifikt antal biljetter till salu.

Administratorn hjälper även kunder som har frågor till sina order, och måsta därför enkelt kunna ta fram alla order på en kund (genom att söka på namn eller epost).

### Inloggning (VG)
Det är ett VG kriterium att kunna logga in i Backoffice applikationen som fler olika använder, med ett använder namn och lösenord.

Detta skall byggs med .NET Core's inbyggda säkerhetsfunktioner: [https://docs.microsoft.com/en-us/aspnet/core/security/](https://docs.microsoft.com/en-us/aspnet/core/security/)

## Container: TicketShop (E-handel, biljett shop)
TicketShop systemet används av kunderna till att köpa biljetter.

Som kund vill man lätt kunna hitta intressanta events äntligen genom en sökning eller någon typ an lista på sidan, eventuellt kategorier eller platser.

När man har hittat ett event, vill man kunna läsa mer om det, och köpa X biljetter till eventet vid ett givet datum.

### Asynkron del (VG)
Det är ett VG kriterium att på första sidan av TicketShop ska där vara en områden som uppdateras live med information om hur många som är inne på ett event just nu, och när senaste biljett köptes.

# Dokumentation

I roten för detta projekt kör kommandon  ```docfx .\docfx.json --serve```, detta kompilera dokumentationen och starter en webbserver, du kan nu i en webbläsare gå till [http://localhost:8080](http://localhost:8080) för att browsa dokumentationen.

Dokumentationen innehåller redan nu olika delar, och ert primära jobb är att underhålla denna så att den stämmer med den kod som är incheckat i Github.

Där är ett avsnitt som heter *User stories* detta måsta fyllas i, och det rekommenderas att ni gör någon sort av usecase diagram, där är redan en basal variant i arkitektur dokumentet.

Det viktigaste dokument är API dokumentationen om finns i mappen *webapi* i filen *swagger.json*. Json filen kan var litet svår att underhålla och det enklaste sätt att skriva [Swagger](http://swagger.io) är genom att använda online verktyget [SwaggerHub](https://swaggerhub.com) och skriva APIet med yaml, och sen exportera det som json och skriva över *swagger.json* i *webapi*-mappen.

# Tips / Hints
Börja med i teamen med att göra en förväntningsavstämning, vad är erat mål i team, och vad är er individuella styrkor.

Försök att undvika att någon i teamen är syssellösa, om där är samarbetsvanskligheter se till att ta i dom så tidigt som möjligt.

Om någon i gruppen har svårt vid delar av koden, försök att köra pair-programming, och se till at den om tycker det är svårt är [Driver](https://gist.github.com/jordanpoulton/607a8854673d9f22c696)

Om ni sitter på distans träffas med video en gång per dag (via [skype](https://www.skype.com), [google hangout](https://hangouts.google.com), [appear](https://appear.in) eller likande), och kör pair-programming via [TeamViewer](https://www.teamviewer.com) eller likande.

Använd Github aktivt: Issues, Pull requets, Projects. Det gör det enklare för alla hänga med på alla förändringar och idéer.

Försöka att följa SOLID principerna så långt det går.

Steg (kan göras paralellt):

* I solutionen *RESTAPI.sln* lägg till en *ASP.NET Core Web Application*, välj WebAPI, implementera detta api så att det matcher dokumentationen (denna kan behövs att utökas)
* I solutionen *Backoffice.sln* lägg till en *ASP.NET Core Web Application*, välj Web Application (Model-View-Controller), implementera denna, om rest apiet inte är klar än, gör en [stub](https://stackoverflow.com/questions/9777822/what-does-to-stub-mean-in-programming)
* I solutionen *TicketShop.sln* lägg till en *ASP.NET Core Web Application*, välj Web Application (Model-View-Controller), implementera denna, om rest apiet inte är klar än, gör en [stub](https://stackoverflow.com/questions/9777822/what-does-to-stub-mean-in-programming)


# Betygskrav
Grundtanken för betygen är att alla i teamen bidra med lika mycket, dom enda parametern som gör det möjligt att försöka att reda ut om någon gör mer eller mindre än andra är incheckningar i GitHub, aktivitet i gruppens Slack Channel (därför måste den bland annat var publik) och kommunikation vid lektionstillfällen. Men grundtanken  är att ni är **ett** team.

## G
* Tre webbsites gjort i ASP.NET Core 2 (i denna prio-ordning)

    1. Ett REST API  
    2. Ett administrativt gränssnitt (Backoffice)
    3. Ett använder gränssnitt (TicketShop)

* Det ska var möjligt att skåpa ett produkt i Backoffice och konsumera (köpa) det i TicketShop
* Input validering på all input, i backend
* Dokumentationen måsta stämma med implementationen
* Loggning med .NET Core logging och SEQ
* Båda Backoffice och TicketShop måste vara tillgängligt på svenska och engelska

## VG (G + minst 3 för VG)
Om man gör VG delar skall det på något sätt vara enkelt att se vilka, ett förslag är att skriva det i *index.md*

* Cake build script (kan med fördel konfigureras som det första)
* AppVoyage (kan med fördel konfigureras efter cake scriptet så att ni kan få in status på github)
* Deployat till Azure och demo körs därifrån
* Regex används till inputvalidering  dom ställen vart det gir mening
* Loggning i Azure
* Enhetstestar
* Köpt biljett sickas till kunderna med email
* Multi använder backoffice med inloggning som bygger på [ASP.NET security](https://docs.microsoft.com/en-us/aspnet/core/security/)
* Asynkron del, implementerad  med React och SignalR (se lektion nr. 12)