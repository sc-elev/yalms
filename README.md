Secret sauce to clone this MVC_ASP project.

-   $ git clone whatever-the-url
    $ cd yalms
  
Start Visual studio, open the yalms.sln file. Start the Nuget package 
mgmt console, let it update the new padkages. In the console:

    PM> Update-package --reinstall EntityFramework

Remove any Migrations/\*.cs file existing.  Using *View | Server Explorer |
Data Connections | defaultdb* remove all database tables.

Remove all migrations files under Migrations (just leave Configuration.cd in
place).

Then reset the dataase:

    PM> sqllocaldb info
    PM> sqllocaldb stop v11.0
    PM> sqllocaldb delete v11.0
    PM> update-database

