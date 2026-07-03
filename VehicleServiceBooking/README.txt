========================================================================
 Vehicle Maintenance Service Booking System
 ASP.NET Web Forms (C#) + SQL Server
========================================================================

PROJECT STRUCTURE
------------------------------------------------------------------------
VehicleServiceBooking/
  Website/                     <- Open this folder's .sln in Visual Studio
    VehicleServiceBooking.sln
    VehicleServiceBooking.csproj
    Web.config                 <- Connection string is here
    Site.Master / Site.Master.cs
    Pages/
      Home.aspx
      Services.aspx
      CustomerRegistration.aspx
      VehicleRegistration.aspx
      ServiceBookingPage.aspx
      ServiceHistory.aspx      <- Search/filter + 4 reports
    CSS/site.css
    JS/validation.js
    Images/hero-car.svg
    App_Code/
      DbHelper.cs               <- Central ADO.NET helper (parameterized only)
      Customer.cs / CustomerDAL.cs
      Vehicle.cs / VehicleDAL.cs
      ServiceBooking.cs / BookingDAL.cs
      ValidationHelper.cs       <- Server-side validation (regex, year range)
  Database/
    database_script.sql         <- Full schema + sample data + stored procedures
  Screenshots/                  <- Add screenshots of the running site here
  Report/                       <- Add your written report here
  Presentation/                 <- Add your presentation slides here

------------------------------------------------------------------------
HOW TO RUN (WINDOWS + VISUAL STUDIO + SQL SERVER)
------------------------------------------------------------------------

1) DATABASE SETUP
   a. Open SQL Server Management Studio (SSMS) and connect to your local
      SQL Server / SQL Server Express instance.
   b. Open Database/database_script.sql.
   c. Click "Execute" (or press F5). This will:
        - Drop and recreate the "VehicleServiceDB" database
        - Create the Customers, Vehicles, and ServiceBookings tables
        - Insert sample customers, vehicles, and bookings
        - Create all stored procedures used by the website
   d. Verify no errors appear in the Messages pane.

2) CONFIGURE THE CONNECTION STRING
   Open Website/Web.config and confirm the <connectionStrings> section
   matches your SQL Server instance name, for example:

     <connectionStrings>
       <add name="VehicleServiceDB"
            connectionString="Server=.\SQLEXPRESS;Database=VehicleServiceDB;Integrated Security=True;TrustServerCertificate=True;"
            providerName="System.Data.SqlClient" />
     </connectionStrings>

   If your SQL Server instance is named differently (e.g. just "." for a
   default instance, or "localhost\MSSQLSERVER01"), update the Server=
   value accordingly.

3) OPEN AND RUN IN VISUAL STUDIO
   a. Open Visual Studio (2019 or later recommended).
   b. File > Open > Project/Solution > select
      Website/VehicleServiceBooking.sln
   c. Let Visual Studio restore NuGet packages (Microsoft.CodeDom.Providers.DotNetCompilerPlatform).
   d. Set Pages/Home.aspx as the start page if it does not open automatically
      (right-click Home.aspx > "Set As Start Page").
   e. Press F5 (or Ctrl+F5) to run using IIS Express.
   f. The site will open in your default browser at a URL like
      https://localhost:xxxxx/Pages/Home.aspx
   g. Use Google Chrome to verify layout and interactivity.

4) VERIFY THE SITE
   - Home page loads with hero section and service cards.
   - Services page shows the service list and pricing table.
   - Register a customer on the Customer Registration page.
   - Register a vehicle for that customer on the Vehicle Registration page
     (try an invalid plate/year first to see validation messages).
   - Book a service on the Service Booking page (select a service type to
     see the JavaScript-driven details box appear).
   - Edit a booking's status or delete/cancel a booking (a confirm() dialog
     will appear before cancellation).
   - Go to Service History & Reports and try the search/filter form, then
     review the four report tables (by date, by status, vehicle history,
     customer vehicle list).

------------------------------------------------------------------------
KEY TECHNICAL NOTES
------------------------------------------------------------------------
- All database access uses stored procedures called through parameterized
  SqlCommand objects (see App_Code/DbHelper.cs and *DAL.cs). No SQL string
  concatenation of user input is used anywhere.
- Server-side validation is implemented in App_Code/ValidationHelper.cs
  and in each page's code-behind (email regex, plate number regex, year
  range, required fields), in addition to ASP.NET validator controls.
- Client-side validation and interactivity is implemented in
  JS/validation.js: regular expressions for email/plate/year, DOM
  manipulation (innerHTML, textContent), addEventListener-based event
  handling, selection statements, and loops.
- CSS (CSS/site.css) implements an external stylesheet with typography,
  box model styling, float/position usage, Flexbox (header/nav, hero,
  filter bar), CSS Grid (card grids, form grids), and media queries for
  responsive desktop/mobile layouts.
- Relationships: Customers (1) -> Vehicles (many) -> ServiceBookings (many),
  enforced with foreign keys and ON DELETE CASCADE.

------------------------------------------------------------------------
TROUBLESHOOTING
------------------------------------------------------------------------
- "Cannot open database VehicleServiceDB": re-run database_script.sql in
  SSMS and confirm it completed without errors.
- "Login failed" or connection errors: check the Server= value in
  Web.config matches your actual SQL Server instance name.
- NuGet restore errors on first open: right-click the solution in
  Solution Explorer > "Restore NuGet Packages", then rebuild.
