# Vehicle Maintenance Service Booking System
### Final Project Report

**Student:** Amer Alawadhi
**ID:** 3087
**Project Name:** Vehicle Maintenance Service Booking System

---

## 1. Project Overview

This project is a fully functional, database-driven web application built for a
small vehicle service center. It allows the center's staff and customers to
register customer profiles, register vehicles under those profiles, book
maintenance services, and track the status and history of every booking.

The system satisfies the core requirement of the assignment: a complete web
application that lets users enter, manage, search, update, delete, and display
data from a relational database, using semantic HTML, responsive CSS, and
JavaScript on the front end, and a C# back end connected to a SQL Server
database.

### Actors / Users
- **Service center admin / staff** — manage bookings, update statuses, view reports.
- **Mechanic** — updates booking status as work progresses (Pending → Confirmed →
  In Progress → Completed).
- **Customer** — registers a profile, registers vehicles, books services, and
  reviews their service history.

---

## 2. Technology Stack

| Layer | Technology |
|---|---|
| Back end | C# / ASP.NET Core MVC (.NET 8) |
| Front end | Razor views (HTML5), external CSS3, external vanilla JavaScript (ES6) |
| Database | Microsoft SQL Server 2022 (containerized via Docker) |
| Data access | ADO.NET (`Microsoft.Data.SqlClient`) with parameterized stored procedures |
| Development tools | Visual Studio Code, .NET CLI, Docker / Colima |

**Note on stack choice:** the assignment's technical environment names
ASP.NET Web Forms or ASP.NET MVC on Visual Studio/Windows with SQL Server via
SSMS. An initial version of this project was built exactly that way (see the
sibling `VehicleServiceBooking/` Web Forms project). It was then rebuilt as
ASP.NET Core MVC — a framework explicitly permitted by the assignment
("ASP.NET Web Forms, ASP.NET MVC, or ASP.NET Core MVC") — specifically so the
application could be built, run, and verified end-to-end on macOS without
requiring a Windows machine or Visual Studio license. All required
technologies (C#, SQL Server, parameterized queries, stored procedures,
semantic HTML, external CSS/JS) are still used; only the specific ASP.NET
flavor and OS differ.

---

## 3. Database Design

Three related tables form the core schema, matching a standard one-to-many-to-many
relationship chain:

```
Customers (1) ──< Vehicles (many) ──< ServiceBookings (many)
```

### Customers
| Column | Type | Constraint |
|---|---|---|
| CustomerID | INT, IDENTITY | Primary Key |
| FullName | NVARCHAR(100) | NOT NULL |
| Email | NVARCHAR(150) | NOT NULL, UNIQUE |
| Phone | NVARCHAR(20) | NOT NULL |
| CreatedDate | DATETIME | DEFAULT GETDATE() |

### Vehicles
| Column | Type | Constraint |
|---|---|---|
| VehicleID | INT, IDENTITY | Primary Key |
| CustomerID | INT | Foreign Key → Customers, ON DELETE CASCADE |
| PlateNumber | NVARCHAR(20) | NOT NULL |
| Brand | NVARCHAR(50) | NOT NULL |
| Model | NVARCHAR(50) | NOT NULL |
| Year | INT | CHECK (Year BETWEEN 1980 AND 2100) |
| CreatedDate | DATETIME | DEFAULT GETDATE() |

### ServiceBookings
| Column | Type | Constraint |
|---|---|---|
| BookingID | INT, IDENTITY | Primary Key |
| VehicleID | INT | Foreign Key → Vehicles, ON DELETE CASCADE |
| ServiceType | NVARCHAR(50) | NOT NULL |
| BookingDate | DATE | NOT NULL |
| Status | NVARCHAR(20) | CHECK (Pending, Confirmed, In Progress, Completed, Cancelled) |
| Notes | NVARCHAR(500) | NULL |
| CreatedDate | DATETIME | DEFAULT GETDATE() |

All foreign keys cascade on delete, so removing a customer also removes their
vehicles and bookings, and removing a vehicle also removes its bookings —
preventing orphaned records.

### Stored Procedures

All data access goes through stored procedures — 23 in total — covering full
CRUD for each table plus dedicated reporting and search procedures:

- **Customers:** `sp_Customer_Insert`, `sp_Customer_GetAll`, `sp_Customer_GetById`,
  `sp_Customer_Update`, `sp_Customer_Delete`
- **Vehicles:** `sp_Vehicle_Insert`, `sp_Vehicle_GetAll`, `sp_Vehicle_GetByCustomer`,
  `sp_Vehicle_GetById`, `sp_Vehicle_Update`, `sp_Vehicle_Delete`
- **ServiceBookings:** `sp_Booking_Insert`, `sp_Booking_GetAll`,
  `sp_Booking_GetByVehicle`, `sp_Booking_GetById`, `sp_Booking_UpdateStatus`,
  `sp_Booking_Update`, `sp_Booking_Delete`, `sp_Booking_SearchFilter`
- **Reports:** `sp_Report_BookingsByDate`, `sp_Report_BookingsByStatus`,
  `sp_Report_VehicleServiceHistory`, `sp_Report_CustomerVehicleList`

No SQL string concatenation of user input is used anywhere in the project.
Every call from C# passes parameters via `SqlParameter` objects, which
prevents SQL injection by construction.

---

## 4. Application Structure

The site is organized around one controller per feature area, each backed by
its own Razor view, following the MVC pattern:

| Page | Controller | Purpose |
|---|---|---|
| Home | `HomeController` | Landing page with a step-by-step "How It Works" guide |
| Services | `ServicesController` | Service catalog and price guide |
| 1. Register Customer | `CustomersController` | Create/Read/Update/Delete customers |
| 2. Register Vehicle | `VehiclesController` | Create/Read/Update/Delete vehicles, linked to a customer |
| 3. Book Service | `BookingsController` | Create/Read/Update/Cancel service bookings |
| 4. Service History & Reports | `ServiceHistoryController` | Search/filter bookings + 4 reports |

Data access is isolated into a `Data/` layer (`DbHelper`, `CustomerRepository`,
`VehicleRepository`, `BookingRepository`) so controllers never talk to
`SqlConnection`/`SqlCommand` directly — they call repository methods, which
call stored procedures.

A shared `_Layout.cshtml` provides the site header, numbered navigation
(1. Register Customer → 2. Register Vehicle → 3. Book Service → 4. Service
History), and footer across every page. A reusable `_StepIndicator.cshtml`
partial view shows a visual progress indicator (a row of numbered pills) on
each workflow page, highlighting which step of the process the user is on —
this was added specifically to make the intended order of operations obvious
to a first-time visitor, since the four pages must be used in sequence
(a vehicle can't be registered before its owning customer exists, and a
booking can't be created before its vehicle exists).

---

## 5. CRUD Operations

| Operation | Where Implemented |
|---|---|
| Create | Add customers (`CustomersController.Create`), vehicles (`VehiclesController.Create`), and bookings (`BookingsController.Create`) |
| Read | Display all customers, all vehicles (with owner name via SQL join), all bookings (with owner/vehicle info via SQL join) |
| Update | Inline-editable table rows for customers, vehicles, and bookings (edit name/email/phone, plate/brand/model/year, or booking date/status/notes) |
| Delete | Delete customers/vehicles (cascades to dependent rows) and cancel/delete bookings, each guarded by a JavaScript confirmation dialog |

Every Create/Update action is guarded by both client-side validation
(instant feedback) and server-side validation (authoritative — the same
checks are re-run in the controller regardless of what the browser sent).

---

## 6. Validation Rules

| Field | Rule | Enforced |
|---|---|---|
| Customer name | Required, non-empty | Client (JS) + Server (C#) |
| Email | Must match a standard email regular expression | Client (JS) + Server (C#) |
| Phone | 7–20 characters, digits/spaces/+/- only | Server (C#) |
| Plate number | Regex `^[A-Za-z]{1,3}[\s-]?\d{1,6}$` (covers UAE-style and general formats, e.g. "A12345", "DXB 12345") | Client (JS) + Server (C#) |
| Vehicle year | Must be between 1980 and (current year + 1) | Client (JS) + Server (C#) + database CHECK constraint |
| Service type | Must be selected from a fixed list of valid types | Server (C#) |
| Booking date | Required, valid date | Client (HTML5 date input) + Server (C#) |
| Booking status | Must be one of: Pending, Confirmed, In Progress, Completed, Cancelled | Server (C#) + database CHECK constraint |

Server-side validation is the authoritative layer per the assignment's
requirement; client-side validation exists purely to give the user immediate
feedback before a round trip to the server.

---

## 7. JavaScript Features

Implemented in a single external file, `wwwroot/js/validation.js`:

- **DOM access** via `getElementById()` and `querySelectorAll()`.
- **Event handling** via `addEventListener()` for `input`, `blur`, and `click`
  events.
- **Regular expressions** for email, plate number, and year validation.
- **Selection statements** (`if`/`else`) to decide whether a field is valid.
- **Loop statements** (`for` loops) to iterate over all email inputs and all
  confirm/cancel buttons on a page.
- **DOM manipulation** via `innerHTML` and `textContent` to show/hide the
  service-type detail box and inline field error messages.
- **`confirm()`** dialogs before any destructive action (deleting a customer,
  vehicle, or cancelling a booking), so a user cannot accidentally remove data.

---

## 8. CSS / Responsive Design

Implemented in a single external stylesheet, `wwwroot/css/site.css`:

- **Typography** — a consistent font stack, heading scale, and color palette
  defined via CSS custom properties (`:root` variables).
- **Box model styling** — consistent padding/border/radius/shadow across
  cards, buttons, and form fields.
- **Flexbox** — used for the site header/navigation, the hero section, the
  step-by-step guide, and the search filter bar.
- **CSS Grid** — used for the card grids (service cards, "Why Choose Us"
  cards) and the multi-column form layouts.
- **Float and position** — a `float-note` utility and a `position-banner`
  component (used on the Service History page) demonstrate both properties
  as required.
- **Media queries** — two breakpoints (768px and 480px) collapse the
  multi-column layouts into a single-column, touch-friendly layout on mobile
  screens, and reduce the hero section's height so it doesn't dominate a
  small screen.

---

## 9. Reports / Data Display

Four distinct reports are available on the Service History & Reports page,
each backed by its own stored procedure:

1. **Service bookings by date** — all bookings ordered chronologically.
2. **Service bookings by status** — all bookings grouped/filterable by status.
3. **Vehicle service history** — full booking history for a single selected
   vehicle.
4. **Customer vehicle list** — every customer alongside all vehicles they own.

A live search/filter form on the same page lets a user filter bookings by
status, date, and/or plate number, executed against the database via
`sp_Booking_SearchFilter` (a parameterized procedure with optional filters).

---

## 10. Testing / Verification

Because this version of the project targets macOS rather than Windows, it
was built, compiled, and run directly in a real environment rather than only
reviewed as source code:

- `dotnet build` completes with **0 warnings, 0 errors**.
- SQL Server 2022 was run in a Docker container; `database_script.sql` was
  executed against it directly via `sqlcmd`, confirming it creates the
  database, all tables, sample data, and all 23 stored procedures without error.
- The running application was tested end-to-end via HTTP requests: all six
  pages return HTTP 200, static assets (CSS/JS/images) load correctly, and a
  live Create → verify-in-database → Delete round trip was performed against
  the Customers table to confirm the full data path (browser → controller →
  stored procedure → SQL Server → back to browser) works correctly.
- The search/filter feature was verified to return correctly scoped results
  when filtering by status and by plate number.

---

## 11. Project Structure (Deliverable Layout)

```
Student_ID_3087/
  Website/
    start.sh                     One-command setup + launch script
    setup.sh                     Manual setup script (no auto Docker install)
    Controllers/                 6 MVC controllers
    Models/                      Customer, Vehicle, ServiceBooking, view models
    Views/                       Razor views (.cshtml), one folder per controller
    Data/                        ADO.NET data access layer (parameterized only)
    wwwroot/                     css/site.css, js/validation.js, images/
    Program.cs, appsettings.json Application entry point and configuration
  Database/database_script.sql   Full schema, sample data, stored procedures
  Screenshots/                   Screenshots of the running application
  Report/Project_Report.md       This report
  Presentation/                  Presentation slides
  README.txt                     Full setup and run instructions
```

---

## 12. Conclusion

This project delivers a complete, working Vehicle Maintenance Service Booking
System that satisfies every functional requirement of the assignment: three
related database tables with proper keys and constraints, full CRUD
operations backed exclusively by parameterized stored procedures, a
multi-page responsive front end built with semantic HTML/CSS/JavaScript, and
both client- and server-side validation. The application was additionally
verified to run correctly end-to-end rather than simply reviewed as static
code, giving confidence that the submitted solution works as described.
