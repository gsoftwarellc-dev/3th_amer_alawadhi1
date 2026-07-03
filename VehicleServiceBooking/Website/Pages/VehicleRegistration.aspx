<%@ Page Title="Vehicle Registration" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VehicleRegistration.aspx.cs" Inherits="VehicleServiceBooking.Pages.VehicleRegistration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Vehicle Registration</h1>
    <p>Register a vehicle under an existing customer profile. You must register
       the customer first on the <a href="~/Pages/CustomerRegistration.aspx" runat="server">Customer Registration</a> page.</p>

    <asp:Panel ID="pnlStatus" runat="server" Visible="false" CssClass="status-message">
        <asp:Literal ID="litStatus" runat="server" />
    </asp:Panel>

    <div class="form-card">
        <form id="vehicleClientForm" novalidate>
            <div class="form-grid">
                <div class="form-group">
                    <label for="<%= ddlCustomer.ClientID %>">Owner (Customer)</label>
                    <asp:DropDownList ID="ddlCustomer" runat="server" DataTextField="FullName" DataValueField="CustomerID" />
                    <asp:RequiredFieldValidator ID="rfvCustomer" runat="server" ControlToValidate="ddlCustomer"
                        InitialValue="" CssClass="field-error" Display="Dynamic" ErrorMessage="Please select a customer." />
                </div>
                <div class="form-group">
                    <label for="<%= txtPlateNumber.ClientID %>">Plate Number</label>
                    <asp:TextBox ID="txtPlateNumber" runat="server" placeholder="e.g. A12345" />
                    <span id="plateError" class="field-error"></span>
                </div>
                <div class="form-group">
                    <label for="<%= txtBrand.ClientID %>">Vehicle Brand</label>
                    <asp:TextBox ID="txtBrand" runat="server" placeholder="e.g. Toyota" />
                    <asp:RequiredFieldValidator ID="rfvBrand" runat="server" ControlToValidate="txtBrand"
                        CssClass="field-error" Display="Dynamic" ErrorMessage="Brand is required." />
                </div>
                <div class="form-group">
                    <label for="<%= txtModel.ClientID %>">Vehicle Model</label>
                    <asp:TextBox ID="txtModel" runat="server" placeholder="e.g. Camry" />
                    <asp:RequiredFieldValidator ID="rfvModel" runat="server" ControlToValidate="txtModel"
                        CssClass="field-error" Display="Dynamic" ErrorMessage="Model is required." />
                </div>
                <div class="form-group">
                    <label for="<%= txtYear.ClientID %>">Year</label>
                    <asp:TextBox ID="txtYear" runat="server" TextMode="Number" placeholder="e.g. 2021" />
                    <span id="yearError" class="field-error"></span>
                </div>
            </div>

            <div class="form-actions">
                <asp:Button ID="btnSave" runat="server" CssClass="btn" Text="Register Vehicle" OnClick="btnSave_Click" />
                <asp:Button ID="btnClear" runat="server" CssClass="btn btn-secondary" Text="Clear" CausesValidation="false" OnClick="btnClear_Click" />
            </div>
        </form>
    </div>

    <h2>All Registered Vehicles</h2>
    <div class="table-wrapper">
        <asp:GridView ID="gvVehicles" runat="server" CssClass="data-table" AutoGenerateColumns="false"
            DataKeyNames="VehicleID" OnRowEditing="gvVehicles_RowEditing" OnRowUpdating="gvVehicles_RowUpdating"
            OnRowCancelingEdit="gvVehicles_RowCancelingEdit" OnRowDeleting="gvVehicles_RowDeleting">
            <Columns>
                <asp:BoundField DataField="VehicleID" HeaderText="ID" ReadOnly="true" />
                <asp:BoundField DataField="OwnerName" HeaderText="Owner" ReadOnly="true" />
                <asp:BoundField DataField="PlateNumber" HeaderText="Plate Number" />
                <asp:BoundField DataField="Brand" HeaderText="Brand" />
                <asp:BoundField DataField="Model" HeaderText="Model" />
                <asp:BoundField DataField="Year" HeaderText="Year" />
                <asp:CommandField ShowEditButton="true" />
                <asp:TemplateField HeaderText="Delete">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-danger"
                            OnClientClick="return confirm('Delete this vehicle and all its service bookings?');">Delete</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>No vehicles registered yet.</EmptyDataTemplate>
        </asp:GridView>
    </div>

</asp:Content>
