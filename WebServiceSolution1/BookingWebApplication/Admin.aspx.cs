using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BookingWebApplication.ServiceReference2;

namespace BookingWebApplication
{
    public partial class Admin : Page
    {
        private AdminServiceSoapClient adminServiceClient;

        protected void Page_Load(object sender, EventArgs e)
        {
            adminServiceClient = new AdminServiceSoapClient();
            if (!IsPostBack)
            {
                if (Session["IsAuthenticated"] == null || !(bool)Session["IsAuthenticated"])
                {
                    // Redirect to login page with return URL
                    Response.Redirect($"~/Login.aspx?ReturnUrl={HttpUtility.UrlEncode(Request.RawUrl)}");
                }
            }
        }
        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            
            Session.Clear(); 
            Session.Abandon(); 

            
            Response.Redirect("Login.aspx");
        }



        protected void AddTrainButton_Click(object sender, EventArgs e)
        {
            string name = TrainNameTextBox.Text;
            int seatCount;

            if (int.TryParse(TrainSeatCountTextBox.Text, out seatCount))
            {
                TrainSuccessMessage.Text = adminServiceClient.AddTrain(name, seatCount);
                TrainErrorMessage.Text = string.Empty;
            }
            else
            {
                TrainErrorMessage.Text = "Invalid seat count.";
                TrainSuccessMessage.Text = string.Empty;
            }
        }

        protected void UpdateTrainButton_Click(object sender, EventArgs e)
        {
            int trainId;
            string name = UpdateTrainNameTextBox.Text;
            int seatCount;

            if (int.TryParse(UpdateTrainIdTextBox.Text, out trainId) && int.TryParse(UpdateTrainSeatCountTextBox.Text, out seatCount))
            {
                adminServiceClient.UpdateTrain(trainId, name, seatCount);
                UpdateTrainSuccessMessage.Text = "Train updated successfully.";
                UpdateTrainErrorMessage.Text = string.Empty;
            }
            else
            {
                UpdateTrainErrorMessage.Text = "Invalid train ID or seat count.";
                UpdateTrainSuccessMessage.Text = string.Empty;
            }
        }

        protected void DeleteTrainButton_Click(object sender, EventArgs e)
        {
            int trainId;

            if (int.TryParse(DeleteTrainIdTextBox.Text, out trainId))
            {
                adminServiceClient.DeleteTrain(trainId);
                DeleteTrainSuccessMessage.Text = "Train deleted successfully.";
                DeleteTrainErrorMessage.Text = string.Empty;
            }
            else
            {
                DeleteTrainErrorMessage.Text = "Invalid train ID.";
                DeleteTrainSuccessMessage.Text = string.Empty;
            }
        }

        protected void AddStationButton_Click(object sender, EventArgs e)
        {
            string name = StationNameTextBox.Text;
            StationSuccessMessage.Text = adminServiceClient.AddStation(name);
            StationErrorMessage.Text = string.Empty;
        }

        protected void UpdateStationButton_Click(object sender, EventArgs e)
        {
            int stationId;
            string name = UpdateStationNameTextBox.Text;

            if (int.TryParse(UpdateStationIdTextBox.Text, out stationId))
            {
                adminServiceClient.UpdateStation(stationId, name);
                UpdateStationSuccessMessage.Text = "Station updated successfully.";
                UpdateStationErrorMessage.Text = string.Empty;
            }
            else
            {
                UpdateStationErrorMessage.Text = "Invalid station ID.";
                UpdateStationSuccessMessage.Text = string.Empty;
            }
        }

        protected void DeleteStationButton_Click(object sender, EventArgs e)
        {
            int stationId;

            if (int.TryParse(DeleteStationIdTextBox.Text, out stationId))
            {
                adminServiceClient.DeleteStation(stationId);
                DeleteStationSuccessMessage.Text = "Station deleted successfully.";
                DeleteStationErrorMessage.Text = string.Empty;
            }
            else
            {
                DeleteStationErrorMessage.Text = "Invalid station ID.";
                DeleteStationSuccessMessage.Text = string.Empty;
            }
        }

        protected void AddScheduleButton_Click(object sender, EventArgs e)
        {
            int trainId, departureStationId, arrivalStationId;
            DateTime departureTime, arrivalTime;

            if (int.TryParse(AddScheduleTrainIdTextBox.Text, out trainId) &&
                int.TryParse(DepartureStationIdTextBox.Text, out departureStationId) &&
                int.TryParse(ArrivalStationIdTextBox.Text, out arrivalStationId) &&
                DateTime.TryParse(DepartureTimeTextBox.Text, out departureTime) &&
                DateTime.TryParse(ArrivalTimeTextBox.Text, out arrivalTime))
            {
                ScheduleSuccessMessage.Text = adminServiceClient.AddSchedule(trainId, departureStationId, arrivalStationId, departureTime, arrivalTime);
                ScheduleErrorMessage.Text = string.Empty;
            }
            else
            {
                ScheduleErrorMessage.Text = "Invalid input.";
                ScheduleSuccessMessage.Text = string.Empty;
            }
        }

        protected void GetBookingsButton_Click(object sender, EventArgs e)
        {
            List<Booking> bookings = new List<Booking>(adminServiceClient.GetBookings());
            BookingsGridView.DataSource = bookings;
            BookingsGridView.DataBind();
        }

        protected void BookingsGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteBooking")
            {
                int bookingId = Convert.ToInt32(e.CommandArgument);
                adminServiceClient.DeleteBooking(bookingId);
                GetBookingsButton_Click(sender, e); 
            }
        }

    }
}
