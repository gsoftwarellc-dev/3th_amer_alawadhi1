<%@ Page Title="Customer Registration" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerRegistration.aspx.cs" Inherits="VehicleServiceBooking.Pages.CustomerRegistration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Customer Registration</h1>
    <p>Register a new customer profile. Registered customers can then add
       vehicles and book maintenance services.</p>

    <asp:Panel ID="pnlStatus" runat="server" Visible="false" CssClass="status-message">
        <asp:Literal ID="litStatus" runat="server" />
    </asp:Panel>

    <div class="form-card">
        <div class="form-grid">
            <div class="form-group">
                <label for="<%= txtFullName.ClientID %>">Full Name</label>
                <asp:TextBox ID="txtFullName" runat="server" CssClass="js-required" placeholder="e.g. Ahmed Al Mazrouei" />
                <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName"
                    CssClass="field-error" Display="Dynamic" ErrorMessage="Customer name is required." />
            </div>
            <div class="form-group">
                <label for="<%= txtEmail.ClientID %>">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" placeholder="name@example.com" />
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                    CssClass="field-error" Display="Dynamic" ErrorMessage="Email is required." />
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                    CssClass="field-error" Display="Dynamic" ErrorMessage="Enter a valid email address."
                    ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" />
            </div>
            <div class="form-group">
                <label for="<%= txtPhone.ClientID %>">Phone</label>
                <asp:TextBox ID="txtPhone" runat="server" placeholder="e.g. 0501234567" />
                <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtPhone"
                    CssClass="field-error" Display="Dynamic" ErrorMessage="Phone number is required." />
            </div>
        </div>

        <div class="form-actions">
            <asp:Button ID="btnSave" runat="server" CssClass="btn" Text="Register Customer" OnClick="btnSave_Click" />
            <asp:Button ID="btnClear" runat="server" CssClass="btn btn-secondary" Text="Clear" CausesValidation="false" OnClick="btnClear_Click" />
        </div>
    </div>

    <h2>Registered Customers</h2>
    <div class="table-wrapper">
        <asp:GridView ID="gvCustomers" runat="server" CssClass="data-table" AutoGenerateColumns="false"
            DataKeyNames="CustomerID" OnRowCommand="gvCustomers_RowCommand" OnRowEditing="gvCustomers_RowEditing"
            OnRowUpdating="gvCustomers_RowUpdating" OnRowCancelingEdit="gvCustomers_RowCancelingEdit"
            OnRowDeleting="gvCustomers_RowDeleting">
            <Columns>
                <asp:BoundField DataField="CustomerID" HeaderText="ID" ReadOnly="true" />
                <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="Phone" HeaderText="Phone" />
                <asp:CommandField ShowEditButton="true" />
                <asp:TemplateField HeaderText="Delete">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-danger"
                            OnClientClick="return confirm('Delete this customer and all related vehicles/bookings?');">Delete</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>No customers registered yet.</EmptyDataTemplate>
        </asp:GridView>
    </div>

</asp:Content>
