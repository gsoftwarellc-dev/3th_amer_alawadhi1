<%@ Page Title="Services" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Services.aspx.cs" Inherits="VehicleServiceBooking.Pages.Services" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Our Services</h1>
    <p>AutoCare Service Center offers a full range of maintenance and repair
       services for all vehicle makes and models.</p>

    <div class="card-grid">
        <article class="card">
            <h3>Oil Change</h3>
            <p>Full synthetic or conventional oil change with filter replacement.</p>
            <p class="price">From AED 120</p>
        </article>
        <article class="card">
            <h3>Tire Rotation</h3>
            <p>Rotation, balancing, and pressure check for all four tires.</p>
            <p class="price">From AED 80</p>
        </article>
        <article class="card">
            <h3>Brake Service</h3>
            <p>Brake pad and rotor inspection, replacement, and fluid top-up.</p>
            <p class="price">From AED 250</p>
        </article>
        <article class="card">
            <h3>Battery Replacement</h3>
            <p>Battery testing, replacement, and charging system check.</p>
            <p class="price">From AED 300</p>
        </article>
        <article class="card">
            <h3>Full Inspection</h3>
            <p>Comprehensive multi-point inspection covering engine, brakes, suspension,
               and electrical systems.</p>
            <p class="price">From AED 150</p>
        </article>
        <article class="card">
            <h3>Air Conditioning Service</h3>
            <p>AC performance check, refrigerant recharge, and filter cleaning.</p>
            <p class="price">From AED 180</p>
        </article>
    </div>

    <h2>Service Price Guide</h2>
    <div class="table-wrapper">
        <table class="data-table">
            <caption>Estimated pricing per service type</caption>
            <thead>
                <tr>
                    <th scope="col">Service Type</th>
                    <th scope="col">Estimated Duration</th>
                    <th scope="col">Starting Price (AED)</th>
                </tr>
            </thead>
            <tbody>
                <tr><td>Oil Change</td><td>30 minutes</td><td>120</td></tr>
                <tr><td>Tire Rotation</td><td>30 minutes</td><td>80</td></tr>
                <tr><td>Brake Service</td><td>1&ndash;2 hours</td><td>250</td></tr>
                <tr><td>Battery Replacement</td><td>20 minutes</td><td>300</td></tr>
                <tr><td>Full Inspection</td><td>1 hour</td><td>150</td></tr>
                <tr><td>Air Conditioning Service</td><td>45 minutes</td><td>180</td></tr>
            </tbody>
        </table>
    </div>

    <p><a class="btn" href="~/Pages/ServiceBookingPage.aspx" runat="server">Book One of These Services</a></p>

</asp:Content>
