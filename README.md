# Vehicle Maintenance Service Booking System

A final project implementing a database-driven web application for a small
vehicle service center: customer registration, vehicle registration, service
booking, and service history/reports.

**Student:** Amer Alawadhi — **ID:** 3087

This repository contains two related projects:

## [`Student_ID_3087/`](Student_ID_3087/) — the real, working application (submission folder)

ASP.NET Core MVC (C#) + SQL Server, with full Create/Read/Update/Delete
functionality backed by parameterized stored procedures, client- and
server-side validation, and four reports. Runs cross-platform (macOS,
Windows, Linux) via the .NET CLI and Docker.

Organized to mirror the assignment's suggested submission layout:
`Student_ID_3087/Website/` (the app), `Database/database_script.sql`,
`Screenshots/`, `Report/`, `Presentation/`, and `README.txt`.

**Run it:** see [`Student_ID_3087/README.txt`](Student_ID_3087/README.txt)
— or from `Student_ID_3087/Website/`, run `./start.sh` for a one-command
setup + launch.

Full write-up: [`Student_ID_3087/Report/Project_Report.md`](Student_ID_3087/Report/Project_Report.md)

## [`VehicleServiceBooking/`](VehicleServiceBooking/) — original ASP.NET Web Forms version

The initial implementation built with classic ASP.NET Web Forms, matching the
assignment's literally-named technology stack (Visual Studio, Web Forms,
SQL Server via SSMS). Requires Windows + Visual Studio + SQL Server to run.
