<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="VehicleServiceBooking.Pages.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <section class="hero" aria-label="Welcome banner">
        <div class="hero-text">
            <h1>Reliable Vehicle Maintenance, Made Simple</h1>
            <p>AutoCare Service Center helps you register your vehicle, book maintenance
               appointments, and track service history &mdash; all in one place.</p>
            <a class="btn" href="~/Pages/ServiceBookingPage.aspx" runat="server">Book a Service</a>
            <a class="btn btn-secondary" href="~/Pages/Services.aspx" runat="server">View Services</a>
        </div>
        <div class="hero-image">
            <img src="~/Images/hero-car.svg" runat="server" alt="Mechanic inspecting a car engine at a service center" width="360" height="240" />
        </div>
    </section>

    <section aria-label="Why choose us">
        <h2>Why Choose Us</h2>
        <div class="card-grid">
            <div class="card">
                <h3>Certified Mechanics</h3>
                <p>Our team of certified mechanics inspects and services every vehicle
                   with care and precision.</p>
            </div>
            <div class="card">
                <h3>Easy Online Booking</h3>
                <p>Register your vehicle and book a maintenance slot in minutes through
                   our simple booking form.</p>
            </div>
            <div class="card">
                <h3>Full Service History</h3>
                <p>Track every booking and status update for each of your vehicles from
                   your customer profile.</p>
            </div>
        </div>
    </section>

    <section aria-label="Getting started">
        <h2>Getting Started</h2>
        <ol>
            <li>Register as a customer using the <a href="~/Pages/CustomerRegistration.aspx" runat="server">Customer Registration</a> page.</li>
            <li>Add your vehicle details on the <a href="~/Pages/VehicleRegistration.aspx" runat="server">Vehicle Registration</a> page.</li>
            <li>Book a maintenance service on the <a href="~/Pages/ServiceBookingPage.aspx" runat="server">Service Booking</a> page.</li>
            <li>Track progress on the <a href="~/Pages/ServiceHistory.aspx" runat="server">Service History &amp; Reports</a> page.</li>
        </ol>
    </section>

</asp:Content>
