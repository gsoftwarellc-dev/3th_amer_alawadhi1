# Vehicle Maintenance Service Booking — Static Demo

This folder is a **static, front-end-only preview** of the Vehicle
Maintenance Service Booking System, built specifically to be deployable on
Vercel.

## Why this exists

The real application ([`VehicleServiceBookingCore/`](../VehicleServiceBookingCore))
is an ASP.NET Core MVC app backed by a SQL Server database with full working
Create/Read/Update/Delete functionality, stored procedures, and validation.
Vercel does not run ASP.NET Core applications or host SQL Server, so that
app cannot be deployed there as-is.

This folder is a plain HTML/CSS/JS recreation of the same pages and design,
using hardcoded sample data instead of a live database. It's a **visual
preview only**:

- Forms on every page can be filled in and submitted, but submitting just
  shows an explanatory popup — nothing is saved anywhere.
- Delete/Cancel buttons are disabled in the same way.
- Tables show fixed sample data instead of live query results.

Every page includes a banner at the top linking back to the real,
fully-functional application.

## Running locally

No build step or server required — just open `index.html` in a browser,
or serve the folder with any static file server, e.g.:

```
npx serve .
```

## Deploying to Vercel

From this folder:

```
vercel
```

or connect the GitHub repository to a new Vercel project and set the
**Root Directory** to `VehicleServiceBooking-StaticDemo` if it's part of a
larger repo. No framework preset or build command is needed — this is a
plain static site.

## The real application

See [`../VehicleServiceBookingCore/README.txt`](../VehicleServiceBookingCore/README.txt)
for how to run the actual working system (ASP.NET Core MVC + SQL Server via
Docker), which supports full CRUD, search/filter, and reporting against a
real database.
