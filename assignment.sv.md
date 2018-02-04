# Ticket system

Som i dom andra projekt jobbar vi GitHub, en skilnad denna gång är att ert repo protected, så det enda sätt att få in kod är gennom pull request.

## Slut datum

Sista chans for att få med kod är Måndag den 19:e kl 20:00 CET.

# Containers

## Container: Database

## Container: REST API

## Container: Backoffice 
använ mvc till skåpa ett rest api, basseret på swagger api
skåpa biljet som pdf


## Container: E-handel (biljett shop)



# Dokumentatonsdel
user stories
usecase diagram
diagram som viser hur olika dela hänger i hop

# Tips
Försök att unvika att någon är syssellösa

Om någon i gruppen har svårt vid delar av koden, försök att köras pair-programming, och se till at den om tycker det är svårt är [Driver](https://gist.github.com/jordanpoulton/607a8854673d9f22c696)

Om ni sittar på distans taffas med video en gång per dag (via [skype](https://www.skype.com), [google hangout](https://hangouts.google.com), [appear](https://appear.in) eller likande), och kör pair-programming via [TeamViewer](https://www.teamviewer.com) eller likande.

Använn Github aktivt: Issues, Pull requets, Projects. Det gör det enklare för alla hänga med på alla förandringer och ideer.

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
* Cake (kan med fördel konfigureras som det första)
* AppVoyage
* Deployat till Azure och demo körs därifrån
* Multi använder backoffice
* Regex använns till input validering dom ställen vart det gir mening
* SOLID
* Logging i Azure
