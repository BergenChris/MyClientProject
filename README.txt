STAP 1 

Database Configuratie
    Connection string instellen
        Open het bestand appsettings.json of App.config / Web.config.
        Pas de connection string aan met jouw lokale gegevens:
        "ConnectionStrings": {
              "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=MijnDatabase;Trusted_Connection=True;"
                    }
Database aanmaken via migratie
    Open Je project in Visual Studio.
    Ga naar Tools > NuGet Package Manager > Package Manager Console.
    Voer het volgende commando uit:
        update-database -context shopdbcontext

STAP 2 

Run het project en druk op de gele knop "Reset DB en voer dummiedata opnieuw toe"
Nu kan je mijn project verkennen.


Opbouw en logica
-----------------

Projectbeschrijving – Online Winkel in .NET
Voor dit project heb ik een online winkel ontwikkeld met behulp van het .NET-framework. Het platform stelt gebruikers in staat om producten te bekijken, bestellingen te plaatsen en hun aankopen te beheren. De applicatie ondersteunt meerdere soorten gebruikersaccounts, elk met hun eigen rechten en functionaliteiten:

Gebruikerstypes (elk type heeft een andere korting)
Gast
Gasten kunnen het aanbod van de winkel bekijken, maar hebben beperkte toegang. Ze kunnen bestellingen plaatsen, maar hebben geen profiel om deze erna terug te zien. ze geven wel naam en email-adres in voor opslag bestelling(DB) en persoonlijke begroeting (website)

Klant
Geregistreerde klanten genieten van volledige functionaliteit. Ze kunnen producten aan hun winkelmandje toevoegen, bestellingen plaatsen en hun bestelgeschiedenis bekijken.

    BVB:     
    "Name": "Anna Baker",
    "UserEmail": "Annabaker@app.be",

    "Name": "Thomas Lambert",
    "UserEmail": "ThomasLambert@app.be", (heeft reeds item in zijn bestelmandje)

Werknemer
Werknemers hebben toegang tot een beheerdersgedeelte waar ze producten kunnen toevoegen, bewerken of verwijderen. Ze kunnen ook zelf bestellingen doen.

    BVB:  "Name": "Laura Spencer",
          "UserEmail": "LauraSpencer@shop.be",    

Functionaliteiten
Authenticatie en autorisatie:
Inloggen is mogelijk op meerdere manieren. Afhankelijk van het type gebruiker worden bepaalde functies zichtbaar of afgeschermd.

Database-integratie:
Alle gegevens worden opgeslagen in een relationele database, waaronder gebruikersprofielen, producten, winkelmandjes en bestellingen.

Bestelbeheer:
Na het afrekenen wordt elke bestelling opgeslagen in de database met bijbehorende details zoals datum, producten, totaalbedrag en klantinformatie. Klanten kunnen deze bestellingen later opnieuw bekijken in hun profiel.

Technologieën
ASP.NET Core voor de backend-logica

Entity Framework Core voor databasebeheer en ORM

SQL Server als relationele database

Razor Pages of MVC voor de gebruikersinterface

Deze structuur zorgt voor een schaalbare, veilige en gebruiksvriendelijke online winkelervaring waarbij elke gebruiker de functionaliteit krijgt die past bij zijn of haar rol.
