## Minnesanteckningar sprint avslutningsmöte 18/3.

#### Problem (osorterade & oprioriterade)

  - Alexander har varit sjuk (om än på plats).
  - Struligt innan Pekka vare helt på plats, i en situation där
    han var central för att komma vidare.
  - I backspegeln hade det vrait bättre att bygga direkt utifrån
    MS identitetsramverk istället för att bygga ihop databasen
    med det senare.

#### Prioriteringar:

   - 1: Scheman och schemaläggning.
   - 2: Inlämningsuppgifter.
   - 3: Delade filer.
   - 4: Veckoschema (vy).
   - 5: Administration (kan hårdkodas/"mockupas" vi dema.).







## Secret sauce to clone this MVC_ASP project.

-   $ git clone whatever-the-url
    $ cd yalms

Restart Visual studio, open the yalms.sln file. Rebuild the project -
this will also pull missing nuget packages.

Possibly, if running into troubles with the entity framework when rebuilding:

    PM> Update-package --reinstall EntityFramework

Using *View | Server Explorer | Data Connections | defaultdb* remove all
database tables.

Remove all migrations files under Migrations (just leave Configuration.cs in
place).

Then reset the dataase:

    PM> sqllocaldb info
    PM> sqllocaldb stop v11.0
    PM> sqllocaldb delete v11.0
    PM> update-database

