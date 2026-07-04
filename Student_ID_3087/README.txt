========================================================================
 Student ID: 3087
 Vehicle Maintenance Service Booking System
 ASP.NET Core MVC (C#) + SQL Server (Docker)
 Cross-platform build - runs on macOS, Windows, and Linux
========================================================================

FOLDER STRUCTURE
------------------------------------------------------------------------
Student_ID_3087/
  Website/            <- the application source code (see below)
  Database/
    database_script.sql        <- full schema + sample data + stored procedures
  Screenshots/                 <- screenshots of the running application
  Report/
    Project_Report.md          <- full written report
  Presentation/                <- presentation slides
  README.txt                   <- this file

This mirrors the assignment's suggested folder structure. The one
difference is what sits inside Website/: this project uses ASP.NET Core
MVC (an option explicitly permitted by the assignment, alongside ASP.NET
Web Forms and ASP.NET MVC), so Website/ uses ASP.NET Core's own required
folder names instead of the Web Forms-specific ones named in the
suggested layout. See "FOLDER STRUCTURE MAPPING" further down for the
exact correspondence.

------------------------------------------------------------------------
QUICK START - ONE COMMAND (macOS, nothing pre-installed required)
------------------------------------------------------------------------
Open a terminal inside the Website/ folder (in VS Code: open Website/ as
the workspace folder, then Terminal > New Terminal; or in Finder:
right-click Website/ > "New Terminal at Folder") and run:

     chmod +x start.sh
     ./start.sh

That's it. This single script:
     - Installs Homebrew if it's missing
     - Installs Docker Desktop if it's missing, and starts it
     - Installs the .NET 8 SDK if it's missing
     - Creates the SQL Server container and the database (first run only)
     - Runs the website

The FIRST time you run it, expect it to take several minutes (it downloads
Docker Desktop, .NET, and the SQL Server image - about 2.5 GB total). It
will ask for your Mac password once, when installing Docker Desktop.

Every time AFTER that, everything is already installed, so ./start.sh just
starts SQL Server (if it isn't already running) and launches the website
in a few seconds - it will not reinstall or redownload anything.

Once it prints something like:
     Now listening on: http://localhost:5188

open that URL in Google Chrome. Press Ctrl+C in the terminal to stop the
website when you're done.

(There's also a setup.sh script in Website/ that does the setup steps
without launching the site, and without installing Docker Desktop
automatically - useful if you already have Docker installed and just
want more control. Most people should just use start.sh.)

In VS Code, you can also install the "C#" extension (by Microsoft) for
IntelliSense and to run/debug with the F5 key instead of the terminal -
see the "RUNNING/DEBUGGING FROM VS CODE" section further down.

------------------------------------------------------------------------
WEBSITE/ FOLDER CONTENTS
------------------------------------------------------------------------
Website/
  start.sh                            <- run this (see Quick Start above)
  setup.sh                            <- alternative, no auto Docker install / no auto-launch
  Controllers/
    HomeController.cs
    ServicesController.cs
    CustomersController.cs
    VehiclesController.cs
    BookingsController.cs
    ServiceHistoryController.cs      <- search/filter + 4 reports
  Models/
    Customer.cs, Vehicle.cs, ServiceBooking.cs
    BookingSearchViewModel.cs, ReportModels.cs, ErrorViewModel.cs
  Views/
    Home/Index.cshtml
    Services/Index.cshtml
    Customers/Index.cshtml
    Vehicles/Index.cshtml
    Bookings/Index.cshtml
    ServiceHistory/Index.cshtml
    Shared/_Layout.cshtml, Error.cshtml, _StepIndicator.cshtml
  Data/
    DbHelper.cs                      <- ADO.NET helper (parameterized only)
    CustomerRepository.cs
    VehicleRepository.cs
    BookingRepository.cs
  wwwroot/
    css/site.css
    js/validation.js
    images/hero-car.svg
  Program.cs, appsettings.json
  docker-compose.yml                 <- optional, if you have Docker Compose
  VehicleServiceBooking.csproj

Note: Website/ reads the database script from ../Database/database_script.sql
(one level up, at the Student_ID_3087/ root) - start.sh and setup.sh are
already configured for this path.

------------------------------------------------------------------------
FOLDER STRUCTURE MAPPING (vs. the assignment's suggested layout)
------------------------------------------------------------------------
The assignment's suggested folder structure (Website/Pages, Website/CSS,
Website/JS, Website/Images, Website/App_Code or Controllers) describes a
classic ASP.NET Web Forms layout. This project uses ASP.NET Core MVC
instead - an option explicitly permitted by the assignment ("ASP.NET Web
Forms, ASP.NET MVC, or ASP.NET Core MVC") - so the framework's own
required folder names are used instead of the Web Forms-specific ones.
ASP.NET Core will not run correctly if these folders are renamed, since
routing, view discovery, and static file serving all depend on them.

    Assignment's suggested name          This project's equivalent
    ------------------------------       ------------------------------
    Website/Pages/                       Website/Views/     (Razor views)
    Website/CSS/                         Website/wwwroot/css/
    Website/JS/                          Website/wwwroot/js/
    Website/Images/                      Website/wwwroot/images/
    Website/App_Code or Controllers/     Website/Controllers/ (explicitly
                                                              named as an
                                                              acceptable
                                                              alternative
                                                              in the brief)
    Database/database_script.sql         Database/database_script.sql (same)
    Screenshots/                         Screenshots/                  (same)
    Report/                              Report/                       (same)
    Presentation/                        Presentation/                 (same)
    README.txt                          README.txt                    (same)

A second, literal ASP.NET Web Forms implementation of this same project
also exists outside this submission folder (VehicleServiceBooking/,
alongside Student_ID_3087/), which follows the assignment's suggested
structure exactly (Website/Pages, Website/CSS, Website/JS, Website/Images,
Website/App_Code). It requires Windows + Visual Studio + SQL Server to
run and was not used as the primary submission because it cannot be
built or tested on this machine.

------------------------------------------------------------------------
WHAT setup.sh ACTUALLY DOES (manual equivalent, if you'd rather run it
step by step or the script fails partway through)
------------------------------------------------------------------------

1) START SQL SERVER IN DOCKER
     docker run -d --name vehicleservice-sqlserver \
       -e "ACCEPT_EULA=Y" \
       -e "MSSQL_SA_PASSWORD=YourStrong!Passw0rd" \
       -p 14330:1433 \
       -v vehicleservice-sqldata:/var/opt/mssql \
       mcr.microsoft.com/mssql/server:2022-latest

   Wait about 20-30 seconds for SQL Server to finish starting, then check:
     docker logs vehicleservice-sqlserver | grep "Recovery is complete"

2) CREATE THE DATABASE (run from the Student_ID_3087/ root, or adjust the path)
     docker cp Database/database_script.sql vehicleservice-sqlserver:/tmp/database_script.sql
     docker exec vehicleservice-sqlserver /opt/mssql-tools18/bin/sqlcmd \
       -S localhost -U sa -P 'YourStrong!Passw0rd' -C -i /tmp/database_script.sql

   You should see "VehicleServiceDB created successfully..." with no errors.

3) INSTALL .NET 8 SDK (if `dotnet --version` doesn't work in your terminal)
     curl -sSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh
     chmod +x dotnet-install.sh
     ./dotnet-install.sh --channel 8.0 --install-dir "$HOME/.dotnet"
     export PATH="$HOME/.dotnet:$PATH"
   Add that export line to ~/.zshrc so it persists across terminal sessions.

4) RUN THE WEBSITE (from inside Website/)
     dotnet run

   The connection string in Website/appsettings.json already points to
   "Server=127.0.0.1,14330;Database=VehicleServiceDB;User Id=sa;Password=YourStrong!Passw0rd;..."
   which matches the docker run command above. If you change the SQL
   Server port or password, update appsettings.json to match.

5) VERIFY THE SITE
   - Home page loads with the hero section.
   - Services page shows the pricing table.
   - Register a customer on the "1. Register Customer" page.
   - Register a vehicle for that customer on "2. Register Vehicle"
     (try an invalid plate/year first to see validation messages).
   - Book a service on "3. Book Service" (select a service type to see
     the JavaScript-driven details box appear).
   - Edit a booking's status or cancel a booking (a confirm() dialog
     appears before cancellation).
   - Go to "4. Service History & Reports" and try the search/filter form.

6) STOPPING / RESTARTING LATER
   - Stop the website: Ctrl+C in the terminal running `dotnet run`.
   - Stop SQL Server:   docker stop vehicleservice-sqlserver
   - Next time, just restart SQL Server (data persists in the Docker
     volume, no need to re-run the database script):
                        docker start vehicleservice-sqlserver
                        dotnet run

------------------------------------------------------------------------
RUNNING/DEBUGGING FROM VS CODE INSTEAD OF THE TERMINAL
------------------------------------------------------------------------
   - Install the "C#" extension (ms-dotnettools.csharp) from the Extensions
     panel if VS Code doesn't prompt you automatically.
   - Open the Website/ folder in VS Code (not Student_ID_3087/ itself).
   - Press F5, or use the Run and Debug panel, and choose ".NET Core Launch".
     VS Code will build and run the project and can attach a debugger with
     breakpoints, which `dotnet run` in a plain terminal can't do.
   - Docker/SQL Server still need to be running first (steps 1-2 above, or
     just run ./setup.sh once from inside Website/) - VS Code doesn't
     manage that automatically.

------------------------------------------------------------------------
HOW TO RUN ON WINDOWS (equivalent steps)
------------------------------------------------------------------------
   - Install the .NET 8 SDK from https://dotnet.microsoft.com if not already present.
   - Install Docker Desktop, or install SQL Server Express/Developer edition
     directly and skip the container entirely.
   - If using a local SQL Server instance instead of Docker, update the
     connection string in Website/appsettings.json accordingly, e.g.:
       "Server=.\\SQLEXPRESS;Database=VehicleServiceDB;Integrated Security=True;TrustServerCertificate=True;"
   - Run Database/database_script.sql in SSMS.
   - From the Website/ folder: `dotnet run`, or open the folder in Visual
     Studio / VS Code and run from there.

------------------------------------------------------------------------
KEY TECHNICAL NOTES
------------------------------------------------------------------------
- All database access uses stored procedures called through parameterized
  SqlCommand objects (see Website/Data/DbHelper.cs and *Repository.cs).
  No SQL string concatenation of user input is used anywhere.
- Server-side validation is implemented in each controller (email regex,
  plate number regex, year range, required fields) in addition to
  DataAnnotations on the models.
- Client-side validation and interactivity is implemented in
  Website/wwwroot/js/validation.js: regular expressions for email/plate/year,
  DOM manipulation (innerHTML, textContent), addEventListener-based event
  handling, selection statements, and loops.
- CSS (Website/wwwroot/css/site.css) implements an external stylesheet with
  typography, box model styling, float/position usage, Flexbox
  (header/nav, hero, filter bar), CSS Grid (card grids, form grids), and
  media queries for responsive desktop/mobile layouts.
- Relationships: Customers (1) -> Vehicles (many) -> ServiceBookings (many),
  enforced with foreign keys and ON DELETE CASCADE.
- This is a from-scratch rebuild of an earlier ASP.NET Web Forms version
  of this same project, switched to ASP.NET Core MVC specifically so it
  can be built, run, and tested directly on macOS without Visual Studio
  or Windows.

------------------------------------------------------------------------
TROUBLESHOOTING
------------------------------------------------------------------------
- "Cannot connect to the Docker daemon": open Docker Desktop and wait for
  the whale icon in the menu bar to stop animating, then try again.
- "docker: command not found": Docker Desktop isn't installed yet - see
  step 1 in Quick Start above.
- SQL connection errors from the app: confirm the container is running
  (`docker ps`) and that appsettings.json's port/password match the
  `docker run` command used to start it.
- Re-running database_script.sql is safe - it drops and recreates
  VehicleServiceDB from scratch each time, so sample data resets.
- Port 14330 or 5188 already in use: something else is using that port.
  Either stop that process, or change the port in appsettings.json /
  the docker run command (and use `dotnet run --urls http://localhost:PORT`
  to pick a different app port).
