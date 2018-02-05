# Ticket system

Som i dom andra projekt jobbar vi GitHub, en skilnad denna gång är att ert repo protected, så det enda sätt att få in kod är gennom pull request.

## Slut datum

Sista chans for att få med kod är Måndag den 19:e kl 20:00 CET.

# Containers

Systemet består av fyra containers (se [arkitektur dokumentet](http://localhost:8080/architecture/index.html) i docfx), varja container har en solution fil i *src*-mappen.

Det är endast grundläggande delar som har implementeras, så det är mycket sannolikt att alla containers på något sätt måste refactoreres, ändras eller utökas. Och ni får ändra och fixa pärcis som ni kännar.

## Container: Database

All data skall spara i SQL databas, och där finns som en del av projeket en simpel databas model, se denna i docfx avsnittet om [arkitektur](http://localhost:8080/architecture/index.html#database).

I mappen *src* finns en visual studio solution kallat database.sln, denna innehåller en konsol applikation som via [DbUp](https://dbup.readthedocs.io/en/latest/) and initalisere databasen gennom [transitions](https://dbup.readthedocs.io/en/latest/philosophy-behind-dbup/#transitions-not-states). Den datamodel som är implementeret är väldig basic och kan behövs att utökas, om detta är fallet kom i håg att göra alla förandringer i ett nytt script.

## Container: REST API

Backoffice och e-handel kommunikera endast med REST API, det betyder att REST APIet sköter kommunikation med databasen och betalleventöra.

Ett API är ett kontrakt som är emellan server och klient, det betyder att man måste ha en bra dokumentation av APIet, kontraktet finns på [SwaggerHub](https://app.swaggerhub.com/apis/Distancify6/TicketSystem/1.0.0), och även i eran docfx-dokumentation i mappen [webapi](http://localhost:8080/webapi/swagger.html).

Från SwaggerHub går det ladda ner boilerplate kod för båda klient och server, detta kan vara en hjälp även om koden kan vara litet svår att läsa.

## Container: Backoffice 
använ mvc till skåpa ett rest api, basseret på swagger api
skåpa biljet som pdf


## Container: E-handel (biljett shop)

Asyknon del (VG): Visar hur många som är inne på ett event just nu, och när senasta biljett köptes, och vilka events som är populära just nu

# Dokumentatonsdel
user stories
usecase diagram
diagram som viser hur olika dela hänger i hop
opdaterat system diagrammer


# Tips / Hints
Försök att unvika att någon är syssellösa

Om någon i gruppen har svårt vid delar av koden, försök att köras pair-programming, och se till at den om tycker det är svårt är [Driver](https://gist.github.com/jordanpoulton/607a8854673d9f22c696)

Om ni sittar på distans taffas med video en gång per dag (via [skype](https://www.skype.com), [google hangout](https://hangouts.google.com), [appear](https://appear.in) eller likande), och kör pair-programming via [TeamViewer](https://www.teamviewer.com) eller likande.

Använn Github aktivt: Issues, Pull requets, Projects. Det gör det enklare för alla hänga med på alla förandringer och ideer.

Forsög att att följa SOLID så långt det går.

# Betygskrav
## G
* To websites gjort i ASP.NET Core 2

    * Ett administrativtgräsnitt (backoffice)
    * Ett användergränsitt (e-handel)

* Det ska var möjligt att skåpa ett produkt i det adminstrativa gränsnitt och konsumera (köpa) det i användergränsnittet
* Input validering på alla input, i backend
* Dokumentation
* Logging med .NET Core logging och SEQ
* Båda det administrativtagräsnitt och användergränsitt måste vara tillgängeligt på svenska och engelska

## VG
* Cake build script (kan med fördel konfigureras som det första)
* AppVoyage (kan med fördel konfigureras efter cake scriptet så att ni kan få in status på github)
* Deployat till Azure och demo körs därifrån
* Multi använder backoffice med inlogning som bygger på 
* Regex använns till input validering dom ställen vart det gir mening
* Logging i Azure
* Asynkron del, implemntard med React och SignalR
