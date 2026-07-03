using System;
using System.Web.UI.WebControls;
using VehicleServiceBooking.App_Code;

namespace VehicleServiceBooking.Pages
{
    public partial class CustomerRegistration : System.Web.UI.Page
    {
        private readonly CustomerDAL _customerDal = new CustomerDAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCustomers();
            }
        }

        private void BindCustomers()
        {
            gvCustomers.DataSource = _customerDal.GetAll();
            gvCustomers.DataBind();
        }

        private void ShowStatus(string message, bool isSuccess)
        {
            pnlStatus.Visible = true;
            pnlStatus.CssClass = "status-message " + (isSuccess ? "success" : "error");
            litStatus.Text = message;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();

            // Server-side validation (authoritative), mirrors client-side JS checks.
            if (string.IsNullOrWhiteSpace(fullName))
            {
                ShowStatus("Customer name is required.", false);
                return;
            }
            if (!ValidationHelper.IsValidEmail(email))
            {
                ShowStatus("Please enter a valid email address.", false);
                return;
            }
            if (!ValidationHelper.IsValidPhone(phone))
            {
                ShowStatus("Please enter a valid phone number.", false);
                return;
            }

            var customer = new Customer
            {
                FullName = fullName,
                Email = email,
                Phone = phone
            };

            try
            {
                _customerDal.Insert(customer);
                ShowStatus("Customer \"" + fullName + "\" registered successfully.", true);
                ClearForm();
                BindCustomers();
            }
            catch (Exception)
            {
                ShowStatus("A customer with that email may already exist, or a database error occurred.", false);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtFullName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
        }

        protected void gvCustomers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        protected void gvCustomers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCustomers.EditIndex = e.NewEditIndex;
            BindCustomers();
        }

        protected void gvCustomers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvCustomers.EditIndex = -1;
            BindCustomers();
        }

        protected void gvCustomers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int customerId = Convert.ToInt32(gvCustomers.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvCustomers.Rows[e.RowIndex];

            string fullName = ((TextBox)row.Cells[1].Controls[0]).Text.Trim();
            string email = ((TextBox)row.Cells[2].Controls[0]).Text.Trim();
            string phone = ((TextBox)row.Cells[3].Controls[0]).Text.Trim();

            if (string.IsNullOrWhiteSpace(fullName) || !ValidationHelper.IsValidEmail(email) || !ValidationHelper.IsValidPhone(phone))
            {
                ShowStatus("Please provide a valid name, email, and phone number when editing.", false);
                e.Cancel = true;
                return;
            }

            var customer = new Customer
            {
                CustomerID = customerId,
                FullName = fullName,
                Email = email,
                Phone = phone
            };

            _customerDal.Update(customer);
            gvCustomers.EditIndex = -1;
            ShowStatus("Customer updated successfully.", true);
            BindCustomers();
        }

        protected void gvCustomers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int customerId = Convert.ToInt32(gvCustomers.DataKeys[e.RowIndex].Value);
            _customerDal.Delete(customerId);
            ShowStatus("Customer deleted.", true);
            BindCustomers();
        }
    }
}
