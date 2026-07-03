<%@ Page Title="Service History & Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ServiceHistory.aspx.cs" Inherits="VehicleServiceBooking.Pages.ServiceHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Service History &amp; Reports</h1>
    <p>Search and filter service bookings, or browse the reports below.</p>

    <div class="position-banner">
        <span class="badge-corner">Live Search</span>
        Use the filters below to search bookings by status, date, or plate number.
        Results update from the database when you click "Search".
    </div>

    <div class="filter-bar">
        <div class="form-group">
            <label for="<%= ddlFilterStatus.ClientID %>">Status</label>
            <asp:DropDownList ID="ddlFilterStatus" runat="server">
                <asp:ListItem Text="All Statuses" Value="" />
                <asp:ListItem Text="Pending" Value="Pending" />
                <asp:ListItem Text="Confirmed" Value="Confirmed" />
                <asp:ListItem Text="In Progress" Value="In Progress" />
                <asp:ListItem Text="Completed" Value="Completed" />
                <asp:ListItem Text="Cancelled" Value="Cancelled" />
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <label for="<%= txtFilterDate.ClientID %>">Booking Date</label>
            <asp:TextBox ID="txtFilterDate" runat="server" TextMode="Date" />
        </div>
        <div class="form-group">
            <label for="<%= txtFilterPlate.ClientID %>">Plate Number</label>
            <asp:TextBox ID="txtFilterPlate" runat="server" placeholder="e.g. A12345" />
        </div>
        <div class="form-group">
            <asp:Button ID="btnSearch" runat="server" CssClass="btn" Text="Search" OnClick="btnSearch_Click" />
        </div>
        <div class="form-group">
            <asp:Button ID="btnResetFilter" runat="server" CssClass="btn btn-secondary" Text="Reset" CausesValidation="false" OnClick="btnResetFilter_Click" />
        </div>
    </div>

    <h2>Search Results</h2>
    <div class="table-wrapper">
        <asp:GridView ID="gvResults" runat="server" CssClass="data-table" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="BookingID" HeaderText="ID" />
                <asp:BoundField DataField="OwnerName" HeaderText="Owner" />
                <asp:BoundField DataField="PlateNumber" HeaderText="Plate" />
                <asp:BoundField DataField="ServiceType" HeaderText="Service Type" />
                <asp:BoundField DataField="BookingDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="Status" HeaderText="Status" />
            </Columns>
            <EmptyDataTemplate>No bookings match your search criteria.</EmptyDataTemplate>
        </asp:GridView>
    </div>

    <hr />

    <h2>Reports</h2>

    <h3>1. Service Bookings by Date</h3>
    <div class="table-wrapper">
        <asp:GridView ID="gvByDate" runat="server" CssClass="data-table" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="PlateNumber" HeaderText="Plate" />
                <asp:BoundField DataField="OwnerName" HeaderText="Owner" />
                <asp:BoundField DataField="ServiceType" HeaderText="Service Type" />
                <asp:BoundField DataField="BookingDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="Status" HeaderText="Status" />
            </Columns>
            <EmptyDataTemplate>No bookings recorded.</EmptyDataTemplate>
        </asp:GridView>
    </div>

    <h3>2. Service Bookings by Status</h3>
    <div class="table-wrapper">
        <asp:GridView ID="gvByStatus" runat="server" CssClass="data-table" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="PlateNumber" HeaderText="Plate" />
                <asp:BoundField DataField="OwnerName" HeaderText="Owner" />
                <asp:BoundField DataField="ServiceType" HeaderText="Service Type" />
                <asp:BoundField DataField="BookingDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="Status" HeaderText="Status" />
            </Columns>
            <EmptyDataTemplate>No bookings recorded.</EmptyDataTemplate>
        </asp:GridView>
    </div>

    <h3>3. Vehicle Service History</h3>
    <div class="form-group" style="max-width:320px;">
        <label for="<%= ddlHistoryVehicle.ClientID %>">Select Vehicle</label>
        <asp:DropDownList ID="ddlHistoryVehicle" runat="server" AutoPostBack="true"
            OnSelectedIndexChanged="ddlHistoryVehicle_SelectedIndexChanged"
            DataTextField="VehicleLabel" DataValueField="VehicleID" />
    </div>
    <div class="table-wrapper">
        <asp:GridView ID="gvVehicleHistory" runat="server" CssClass="data-table" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="ServiceType" HeaderText="Service Type" />
                <asp:BoundField DataField="BookingDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="Status" HeaderText="Status" />
                <asp:BoundField DataField="Notes" HeaderText="Notes" />
            </Columns>
            <EmptyDataTemplate>Select a vehicle to view its service history.</EmptyDataTemplate>
        </asp:GridView>
    </div>

    <h3>4. Customer Vehicle List</h3>
    <div class="table-wrapper">
        <asp:GridView ID="gvCustomerVehicles" runat="server" CssClass="data-table" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="FullName" HeaderText="Customer" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="Phone" HeaderText="Phone" />
                <asp:BoundField DataField="PlateNumber" HeaderText="Plate" />
                <asp:BoundField DataField="Brand" HeaderText="Brand" />
                <asp:BoundField DataField="Model" HeaderText="Model" />
                <asp:BoundField DataField="Year" HeaderText="Year" />
            </Columns>
            <EmptyDataTemplate>No customers or vehicles recorded.</EmptyDataTemplate>
        </asp:GridView>
    </div>

</asp:Content>
