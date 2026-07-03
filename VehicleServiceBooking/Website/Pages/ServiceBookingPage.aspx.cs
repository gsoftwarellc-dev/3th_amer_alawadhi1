using System;
using System.Data;
using System.Web.UI.WebControls;
using VehicleServiceBooking.App_Code;

namespace VehicleServiceBooking.Pages
{
    public partial class ServiceBookingPage : System.Web.UI.Page
    {
        private readonly VehicleDAL _vehicleDal = new VehicleDAL();
        private readonly BookingDAL _bookingDal = new BookingDAL();

        private static readonly string[] ValidServiceTypes =
        {
            "Oil Change", "Tire Rotation", "Brake Service",
            "Battery Replacement", "Full Inspection", "Air Conditioning Service"
        };

        private static readonly string[] ValidStatuses =
        {
            "Pending", "Confirmed", "In Progress", "Completed", "Cancelled"
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindVehicleDropdown();
                BindBookings();
            }
        }

        private void BindVehicleDropdown()
        {
            DataTable vehicles = _vehicleDal.GetAll();
            vehicles.Columns.Add("VehicleLabel", typeof(string));
            foreach (DataRow row in vehicles.Rows)
            {
                row["VehicleLabel"] = string.Format("{0} - {1} {2} ({3})",
                    row["PlateNumber"], row["Brand"], row["Model"], row["OwnerName"]);
            }

            ddlVehicle.DataSource = vehicles;
            ddlVehicle.DataBind();
            ddlVehicle.Items.Insert(0, new ListItem("-- Select Vehicle --", ""));
        }

        private void BindBookings()
        {
            gvBookings.DataSource = _bookingDal.GetAll();
            gvBookings.DataBind();
        }

        private void ShowStatus(string message, bool isSuccess)
        {
            pnlStatus.Visible = true;
            pnlStatus.CssClass = "status-message " + (isSuccess ? "success" : "error");
            litStatus.Text = message;
        }

        protected void btnBook_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            DateTime bookingDate;
            bool dateIsValid = DateTime.TryParse(txtBookingDate.Text, out bookingDate);

            if (string.IsNullOrWhiteSpace(ddlVehicle.SelectedValue))
            {
                ShowStatus("Please select a vehicle.", false);
                return;
            }
            if (string.IsNullOrWhiteSpace(ddlServiceType.SelectedValue) || Array.IndexOf(ValidServiceTypes, ddlServiceType.SelectedValue) < 0)
            {
                ShowStatus("Please select a valid service type.", false);
                return;
            }
            if (!dateIsValid)
            {
                ShowStatus("Please choose a valid booking date.", false);
                return;
            }

            var booking = new ServiceBooking
            {
                VehicleID = Convert.ToInt32(ddlVehicle.SelectedValue),
                ServiceType = ddlServiceType.SelectedValue,
                BookingDate = bookingDate,
                Status = "Pending",
                Notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim()
            };

            _bookingDal.Insert(booking);
            ShowStatus("Service booking created for " + ddlServiceType.SelectedValue + " on " + bookingDate.ToString("yyyy-MM-dd") + ".", true);
            ClearForm();
            BindBookings();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            ddlVehicle.SelectedIndex = 0;
            ddlServiceType.SelectedIndex = 0;
            txtBookingDate.Text = string.Empty;
            txtNotes.Text = string.Empty;
        }

        protected void gvBookings_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvBookings.EditIndex = e.NewEditIndex;
            BindBookings();
        }

        protected void gvBookings_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvBookings.EditIndex = -1;
            BindBookings();
        }

        protected void gvBookings_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int bookingId = Convert.ToInt32(gvBookings.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvBookings.Rows[e.RowIndex];

            string dateText = ((TextBox)row.Cells[4].Controls[0]).Text.Trim();
            DropDownList ddlEditStatus = (DropDownList)row.Cells[5].FindControl("ddlEditStatus");
            string notes = ((TextBox)row.Cells[6].Controls[0]).Text.Trim();

            DateTime bookingDate;
            bool dateIsValid = DateTime.TryParse(dateText, out bookingDate);
            string status = ddlEditStatus.SelectedValue;

            if (!dateIsValid || Array.IndexOf(ValidStatuses, status) < 0)
            {
                ShowStatus("Please provide a valid date and status when editing.", false);
                e.Cancel = true;
                return;
            }

            DataTable existing = _bookingDal.GetById(bookingId);
            string serviceType = existing.Rows.Count > 0 ? existing.Rows[0]["ServiceType"].ToString() : "";

            var booking = new ServiceBooking
            {
                BookingID = bookingId,
                ServiceType = serviceType,
                BookingDate = bookingDate,
                Status = status,
                Notes = string.IsNullOrWhiteSpace(notes) ? null : notes
            };

            _bookingDal.Update(booking);
            gvBookings.EditIndex = -1;
            ShowStatus("Booking updated successfully.", true);
            BindBookings();
        }

        protected void gvBookings_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int bookingId = Convert.ToInt32(gvBookings.DataKeys[e.RowIndex].Value);
            _bookingDal.Delete(bookingId);
            ShowStatus("Booking cancelled.", true);
            BindBookings();
        }
    }
}
