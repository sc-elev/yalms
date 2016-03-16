Secret sauce to clone this MVC_ASP project.

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

