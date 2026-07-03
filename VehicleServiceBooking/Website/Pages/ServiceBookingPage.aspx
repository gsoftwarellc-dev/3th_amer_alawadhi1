<%@ Page Title="Service Booking" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ServiceBookingPage.aspx.cs" Inherits="VehicleServiceBooking.Pages.ServiceBookingPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Service Booking</h1>
    <p>Select a registered vehicle, choose a service type, and pick a date to book
       a maintenance appointment.</p>

    <asp:Panel ID="pnlStatus" runat="server" Visible="false" CssClass="status-message">
        <asp:Literal ID="litStatus" runat="server" />
    </asp:Panel>

    <div class="form-card">
        <div class="form-grid">
            <div class="form-group">
                <label for="<%= ddlVehicle.ClientID %>">Vehicle</label>
                <asp:DropDownList ID="ddlVehicle" runat="server" DataTextField="VehicleLabel" DataValueField="VehicleID" />
                <asp:RequiredFieldValidator ID="rfvVehicle" runat="server" ControlToValidate="ddlVehicle"
                    InitialValue="" CssClass="field-error" Display="Dynamic" ErrorMessage="Please select a vehicle." />
            </div>
            <div class="form-group">
                <label for="serviceTypeSelect">Service Type</label>
                <asp:DropDownList ID="ddlServiceType" runat="server" ClientIDMode="Static" ClientID="serviceTypeSelect">
                    <asp:ListItem Text="-- Select Service Type --" Value="" />
                    <asp:ListItem Text="Oil Change" Value="Oil Change" />
                    <asp:ListItem Text="Tire Rotation" Value="Tire Rotation" />
                    <asp:ListItem Text="Brake Service" Value="Brake Service" />
                    <asp:ListItem Text="Battery Replacement" Value="Battery Replacement" />
                    <asp:ListItem Text="Full Inspection" Value="Full Inspection" />
                    <asp:ListItem Text="Air Conditioning Service" Value="Air Conditioning Service" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvServiceType" runat="server" ControlToValidate="ddlServiceType"
                    InitialValue="" CssClass="field-error" Display="Dynamic" ErrorMessage="Please select a service type." />
                <div id="serviceDetailsBox" class="service-details-box" style="display:none;" aria-live="polite"></div>
            </div>
            <div class="form-group">
                <label for="<%= txtBookingDate.ClientID %>">Booking Date</label>
                <asp:TextBox ID="txtBookingDate" runat="server" TextMode="Date" />
                <asp:RequiredFieldValidator ID="rfvBookingDate" runat="server" ControlToValidate="txtBookingDate"
                    CssClass="field-error" Display="Dynamic" ErrorMessage="Please choose a booking date." />
            </div>
            <div class="form-group">
                <label for="<%= txtNotes.ClientID %>">Notes</label>
                <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" placeholder="Optional notes for the mechanic" />
            </div>
        </div>

        <div class="form-actions">
            <asp:Button ID="btnBook" runat="server" CssClass="btn" Text="Book Service" OnClick="btnBook_Click" />
            <asp:Button ID="btnClear" runat="server" CssClass="btn btn-secondary" Text="Clear" CausesValidation="false" OnClick="btnClear_Click" />
        </div>
    </div>

    <h2>All Service Bookings</h2>
    <div class="table-wrapper">
        <asp:GridView ID="gvBookings" runat="server" CssClass="data-table" AutoGenerateColumns="false"
            DataKeyNames="BookingID" OnRowEditing="gvBookings_RowEditing" OnRowUpdating="gvBookings_RowUpdating"
            OnRowCancelingEdit="gvBookings_RowCancelingEdit" OnRowDeleting="gvBookings_RowDeleting">
            <Columns>
                <asp:BoundField DataField="BookingID" HeaderText="ID" ReadOnly="true" />
                <asp:BoundField DataField="OwnerName" HeaderText="Owner" ReadOnly="true" />
                <asp:BoundField DataField="PlateNumber" HeaderText="Plate" ReadOnly="true" />
                <asp:BoundField DataField="ServiceType" HeaderText="Service Type" ReadOnly="true" />
                <asp:BoundField DataField="BookingDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" ApplyFormatInEditMode="true" />
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <span class='<%# "badge " + Eval("Status").ToString().Replace(" ", "-") %>'><%# Eval("Status") %></span>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlEditStatus" runat="server">
                            <asp:ListItem Text="Pending" Value="Pending" />
                            <asp:ListItem Text="Confirmed" Value="Confirmed" />
                            <asp:ListItem Text="In Progress" Value="In Progress" />
                            <asp:ListItem Text="Completed" Value="Completed" />
                            <asp:ListItem Text="Cancelled" Value="Cancelled" />
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Notes" HeaderText="Notes" />
                <asp:CommandField ShowEditButton="true" />
                <asp:TemplateField HeaderText="Cancel">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Delete" CssClass="btn btn-danger js-cancel-booking"
                            OnClientClick="return confirm('Are you sure you want to cancel this booking?');">Cancel</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>No service bookings yet.</EmptyDataTemplate>
        </asp:GridView>
    </div>

</asp:Content>
