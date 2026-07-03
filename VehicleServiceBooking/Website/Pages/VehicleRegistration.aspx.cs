using System;
using System.Web.UI.WebControls;
using VehicleServiceBooking.App_Code;

namespace VehicleServiceBooking.Pages
{
    public partial class VehicleRegistration : System.Web.UI.Page
    {
        private readonly CustomerDAL _customerDal = new CustomerDAL();
        private readonly VehicleDAL _vehicleDal = new VehicleDAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCustomerDropdown();
                BindVehicles();
            }
        }

        private void BindCustomerDropdown()
        {
            ddlCustomer.DataSource = _customerDal.GetAll();
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("-- Select Customer --", ""));
        }

        private void BindVehicles()
        {
            gvVehicles.DataSource = _vehicleDal.GetAll();
            gvVehicles.DataBind();
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

            string plateNumber = txtPlateNumber.Text.Trim();
            string brand = txtBrand.Text.Trim();
            string model = txtModel.Text.Trim();
            int year;
            bool yearIsNumeric = int.TryParse(txtYear.Text.Trim(), out year);

            if (string.IsNullOrWhiteSpace(ddlCustomer.SelectedValue))
            {
                ShowStatus("Please select a customer.", false);
                return;
            }
            if (!ValidationHelper.IsValidPlateNumber(plateNumber))
            {
                ShowStatus("Please enter a valid plate number (e.g. A12345).", false);
                return;
            }
            if (string.IsNullOrWhiteSpace(brand) || string.IsNullOrWhiteSpace(model))
            {
                ShowStatus("Vehicle brand and model are required.", false);
                return;
            }
            if (!yearIsNumeric || !ValidationHelper.IsValidYear(year))
            {
                ShowStatus("Please enter a reasonable vehicle year (1980 - " + (DateTime.Now.Year + 1) + ").", false);
                return;
            }

            var vehicle = new Vehicle
            {
                CustomerID = Convert.ToInt32(ddlCustomer.SelectedValue),
                PlateNumber = plateNumber,
                Brand = brand,
                Model = model,
                Year = year
            };

            _vehicleDal.Insert(vehicle);
            ShowStatus("Vehicle \"" + brand + " " + model + "\" registered successfully.", true);
            ClearForm();
            BindVehicles();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtPlateNumber.Text = string.Empty;
            txtBrand.Text = string.Empty;
            txtModel.Text = string.Empty;
            txtYear.Text = string.Empty;
            ddlCustomer.SelectedIndex = 0;
        }

        protected void gvVehicles_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvVehicles.EditIndex = e.NewEditIndex;
            BindVehicles();
        }

        protected void gvVehicles_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvVehicles.EditIndex = -1;
            BindVehicles();
        }

        protected void gvVehicles_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int vehicleId = Convert.ToInt32(gvVehicles.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvVehicles.Rows[e.RowIndex];

            string plateNumber = ((TextBox)row.Cells[2].Controls[0]).Text.Trim();
            string brand = ((TextBox)row.Cells[3].Controls[0]).Text.Trim();
            string model = ((TextBox)row.Cells[4].Controls[0]).Text.Trim();
            string yearText = ((TextBox)row.Cells[5].Controls[0]).Text.Trim();

            int year;
            bool yearIsNumeric = int.TryParse(yearText, out year);

            if (!ValidationHelper.IsValidPlateNumber(plateNumber) || string.IsNullOrWhiteSpace(brand)
                || string.IsNullOrWhiteSpace(model) || !yearIsNumeric || !ValidationHelper.IsValidYear(year))
            {
                ShowStatus("Please provide a valid plate number, brand, model, and year when editing.", false);
                e.Cancel = true;
                return;
            }

            var vehicle = new Vehicle
            {
                VehicleID = vehicleId,
                PlateNumber = plateNumber,
                Brand = brand,
                Model = model,
                Year = year
            };

            _vehicleDal.Update(vehicle);
            gvVehicles.EditIndex = -1;
            ShowStatus("Vehicle updated successfully.", true);
            BindVehicles();
        }

        protected void gvVehicles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int vehicleId = Convert.ToInt32(gvVehicles.DataKeys[e.RowIndex].Value);
            _vehicleDal.Delete(vehicleId);
            ShowStatus("Vehicle deleted.", true);
            BindVehicles();
        }
    }
}
