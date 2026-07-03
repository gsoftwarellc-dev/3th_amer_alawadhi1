using System;
using System.Data;
using System.Web.UI.WebControls;
using VehicleServiceBooking.App_Code;

namespace VehicleServiceBooking.Pages
{
    public partial class ServiceHistory : System.Web.UI.Page
    {
        private readonly BookingDAL _bookingDal = new BookingDAL();
        private readonly VehicleDAL _vehicleDal = new VehicleDAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindVehicleDropdown();
                BindReports();
                gvResults.DataSource = _bookingDal.GetAll();
                gvResults.DataBind();
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

            ddlHistoryVehicle.DataSource = vehicles;
            ddlHistoryVehicle.DataBind();
            ddlHistoryVehicle.Items.Insert(0, new ListItem("-- Select Vehicle --", ""));
        }

        private void BindReports()
        {
            gvByDate.DataSource = _bookingDal.ReportBookingsByDate();
            gvByDate.DataBind();

            gvByStatus.DataSource = _bookingDal.ReportBookingsByStatus(null);
            gvByStatus.DataBind();

            gvCustomerVehicles.DataSource = _bookingDal.ReportCustomerVehicleList();
            gvCustomerVehicles.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string status = string.IsNullOrWhiteSpace(ddlFilterStatus.SelectedValue) ? null : ddlFilterStatus.SelectedValue;
            string plate = string.IsNullOrWhiteSpace(txtFilterPlate.Text) ? null : txtFilterPlate.Text.Trim();

            DateTime? bookingDate = null;
            DateTime parsedDate;
            if (!string.IsNullOrWhiteSpace(txtFilterDate.Text) && DateTime.TryParse(txtFilterDate.Text, out parsedDate))
            {
                bookingDate = parsedDate;
            }

            gvResults.DataSource = _bookingDal.SearchFilter(status, bookingDate, plate);
            gvResults.DataBind();
        }

        protected void btnResetFilter_Click(object sender, EventArgs e)
        {
            ddlFilterStatus.SelectedIndex = 0;
            txtFilterDate.Text = string.Empty;
            txtFilterPlate.Text = string.Empty;

            gvResults.DataSource = _bookingDal.GetAll();
            gvResults.DataBind();
        }

        protected void ddlHistoryVehicle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ddlHistoryVehicle.SelectedValue))
            {
                gvVehicleHistory.DataSource = null;
                gvVehicleHistory.DataBind();
                return;
            }

            int vehicleId = Convert.ToInt32(ddlHistoryVehicle.SelectedValue);
            gvVehicleHistory.DataSource = _bookingDal.ReportVehicleServiceHistory(vehicleId);
            gvVehicleHistory.DataBind();
        }
    }
}
