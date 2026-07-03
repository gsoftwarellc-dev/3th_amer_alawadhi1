# Vehicle Maintenance Service Booking System

A final project implementing a database-driven web application for a small
vehicle service center: customer registration, vehicle registration, service
booking, and service history/reports.

**Student:** Amer Alawadhi — **ID:** 3087

This repository contains three related projects:

## [`VehicleServiceBookingCore/`](VehicleServiceBookingCore/) — the real, working application

ASP.NET Core MVC (C#) + SQL Server, with full Create/Read/Update/Delete
functionality backed by parameterized stored procedures, client- and
server-side validation, and four reports. Runs cross-platform (macOS,
Windows, Linux) via the .NET CLI and Docker.

**Run it:** see [`VehicleServiceBookingCore/README.txt`](VehicleServiceBookingCore/README.txt)
— or from that folder, run `./start.sh` for a one-command setup + launch.

Full write-up: [`VehicleServiceBookingCore/Report/Project_Report.md`](VehicleServiceBookingCore/Report/Project_Report.md)

## [`VehicleServiceBooking-StaticDemo/`](VehicleServiceBooking-StaticDemo/) — static preview for Vercel

A plain HTML/CSS/JS recreation of the same pages using hardcoded sample data,
with no backend or database. Built specifically because Vercel does not run
ASP.NET Core applications or host SQL Server — this is a visual-only preview
deployable as a static site. See its own
[README](VehicleServiceBooking-StaticDemo/README.md) for details and
deployment instructions.

## [`VehicleServiceBooking/`](VehicleServiceBooking/) — original ASP.NET Web Forms version

The initial implementation built with classic ASP.NET Web Forms, matching the
assignment's literally-named technology stack (Visual Studio, Web Forms,
SQL Server via SSMS). Requires Windows + Visual Studio + SQL Server to run.
