using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BookingWebApplication.ServiceReference1; 

namespace BookingWebApplication
{
    public partial class TrainBooking : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStations();
            }
        }

        private void LoadStations()
        {
            using (var client = new ClientServiceSoapClient())
            {
                var stations = client.GetStations();
                if (stations != null && stations.Any())
                {
                    DepartureStationDropDownList.DataSource = stations;
                    DepartureStationDropDownList.DataTextField = "Name";
                    DepartureStationDropDownList.DataValueField = "StationId";
                    DepartureStationDropDownList.DataBind();

                    ArrivalStationDropDownList.DataSource = stations;
                    ArrivalStationDropDownList.DataTextField = "Name";
                    ArrivalStationDropDownList.DataValueField = "StationId";
                    ArrivalStationDropDownList.DataBind();
                }
            }
        }


        protected void SearchButton_Click(object sender, EventArgs e)
        {
            int departureStationId = int.Parse(DepartureStationDropDownList.SelectedValue);
            int arrivalStationId = int.Parse(ArrivalStationDropDownList.SelectedValue);
            DateTime travelDate = DateTime.Parse(TravelDateTextBox.Text.Trim());

            using (var client = new ClientServiceSoapClient())
            {
                var schedules = client.SearchTrains(departureStationId, arrivalStationId, travelDate);

                ScheduleGridView.DataSource = schedules;
                ScheduleGridView.DataBind();
            }
        }






        protected void ScheduleGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
               
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = ScheduleGridView.Rows[rowIndex];

               
                string scheduleId = ((HiddenField)row.FindControl("ScheduleId")).Value;

                
                ScheduleIdHiddenField.Value = scheduleId;
            }
        }

        protected void AdminButton_Click(object sender, EventArgs e)
        {
            // Redirect to the Admin page
            Response.Redirect("~/Admin.aspx"); 
        }

        protected void BookButton_Click(object sender, EventArgs e)
        {
            if (int.TryParse(ScheduleIdTextBox.Text, out int scheduleId) &&
                int.TryParse(SeatCountDropDownList.Text, out int seatCount))
            {
                string nic = NICTextBox.Text.Trim();
                string passengerName = PassengerNameTextBox.Text.Trim();
                string contactInfo = ContactInfoTextBox.Text.Trim();

                using (var client = new ClientServiceSoapClient())
                {
                    var response = client.AddBooking(scheduleId, nic, seatCount, passengerName, contactInfo);
                    if (response.Success)
                    {
                        SuccessMessage.Text = response.Message;
                        ErrorMessage.Text = string.Empty;
                    }
                    else
                    {
                        ErrorMessage.Text = response.Message;
                        SuccessMessage.Text = string.Empty;
                    }
                }
            }
            else
            {
                ErrorMessage.Text = "Please enter valid Schedule ID and Seat Count.";
                SuccessMessage.Text = string.Empty;
            }
        }
    }
}
