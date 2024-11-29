<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="BookingWebApplication.Admin" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Dashboard</title>
    <link rel="stylesheet" type="text/css" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <style>
        body {
            background-color: #f8f9fa;
            font-family: Arial, sans-serif;
        }
        .sidebar {
            background-color: #343a40;
            min-height: 100vh;
            padding: 15px;
            color: white;
        }
        .sidebar h4 {
            text-align: center;
            margin-bottom: 20px;
        }
        .sidebar a {
            color: white;
            text-decoration: none;
            padding: 10px;
            display: block;
            border-radius: 5px;
            transition: background 0.3s;
        }
        .sidebar a:hover {
            background-color: #495057;
        }
        .main-content {
            margin-left: 200px;
            padding: 20px;
        }
        .card {
            margin-bottom: 20px;
        }
        .text-danger {
            text-align: center;
            margin-top: 10px;
        }
        .logout-button {
            position: absolute;
            right: 20px;
            top: 20px;
        }
        .blurred-background {
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-size: cover;
            filter: blur(8px);
            z-index: -1; 
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div class="sidebar">
            <h4>Admin Menu</h4>
            <asp:Button ID="LogoutButton" runat="server" Text="Logout" CssClass="btn btn-danger logout-button" OnClick="LogoutButton_Click" />
            <a href="#addTrain">Add Train</a>
            <a href="#updateTrain">Update Train</a>
            <a href="#deleteTrain">Delete Train</a>
            <a href="#addStation">Add Station</a>
            <a href="#updateStation">Update Station</a>
            <a href="#deleteStation">Delete Station</a>
            <a href="#addSchedule">Add Schedule</a>
            <a href="#getBookings">Get Bookings</a>
        </div>

        <div class="main-content">
            <h2 class="text-center">Admin Dashboard</h2>

            <!-- Add your card containers for each functionality -->
            <div class="card">
                <div class="card-body">
                    <h4 id="addTrain">Add Train</h4>
                    <asp:TextBox ID="TrainNameTextBox" runat="server" CssClass="form-control" placeholder="Train Name" required="required"></asp:TextBox>
                    <asp:TextBox ID="TrainSeatCountTextBox" runat="server" CssClass="form-control" placeholder="Seat Count" required="required"></asp:TextBox>
                    <asp:Button ID="AddTrainButton" runat="server" Text="Add Train" CssClass="btn btn-primary" OnClick="AddTrainButton_Click" />
                    <asp:Label ID="TrainSuccessMessage" runat="server" CssClass="text-success"></asp:Label>
                    <asp:Label ID="TrainErrorMessage" runat="server" CssClass="text-danger"></asp:Label>
                </div>
            </div>

            <div class="card">
                <div class="card-body">
                    <h4 id="updateTrain">Update Train</h4>
                    <asp:TextBox ID="UpdateTrainIdTextBox" runat="server" CssClass="form-control" placeholder="Train ID" required="required"></asp:TextBox>
                    <asp:TextBox ID="UpdateTrainNameTextBox" runat="server" CssClass="form-control" placeholder="New Train Name"></asp:TextBox>
                    <asp:TextBox ID="UpdateTrainSeatCountTextBox" runat="server" CssClass="form-control" placeholder="New Seat Count"></asp:TextBox>
                    <asp:Button ID="UpdateTrainButton" runat="server" Text="Update Train" CssClass="btn btn-success" OnClick="UpdateTrainButton_Click" />
                    <asp:Label ID="UpdateTrainSuccessMessage" runat="server" CssClass="text-success"></asp:Label>
                    <asp:Label ID="UpdateTrainErrorMessage" runat="server" CssClass="text-danger"></asp:Label>
                </div>
            </div>

            <div class="card">
                <div class="card-body">
                    <h4 id="deleteTrain">Delete Train</h4>
                    <asp:TextBox ID="DeleteTrainIdTextBox" runat="server" CssClass="form-control" placeholder="Train ID" required="required"></asp:TextBox>
                    <asp:Button ID="DeleteTrainButton" runat="server" Text="Delete Train" CssClass="btn btn-danger" OnClick="DeleteTrainButton_Click" />
                    <asp:Label ID="DeleteTrainSuccessMessage" runat="server" CssClass="text-success"></asp:Label>
                    <asp:Label ID="DeleteTrainErrorMessage" runat="server" CssClass="text-danger"></asp:Label>
                </div>
            </div>

            <div class="card">
                <div class="card-body">
                    <h4 id="addStation">Add Station</h4>
                    <asp:TextBox ID="StationNameTextBox" runat="server" CssClass="form-control" placeholder="Station Name" required="required"></asp:TextBox>
                    <asp:Button ID="AddStationButton" runat="server" Text="Add Station" CssClass="btn btn-primary" OnClick="AddStationButton_Click" />
                    <asp:Label ID="StationSuccessMessage" runat="server" CssClass="text-success"></asp:Label>
                    <asp:Label ID="StationErrorMessage" runat="server" CssClass="text-danger"></asp:Label>
                </div>
            </div>

            <div class="card">
                <div class="card-body">
                    <h4 id="updateStation">Update Station</h4>
                    <asp:TextBox ID="UpdateStationIdTextBox" runat="server" CssClass="form-control" placeholder="Station ID" required="required"></asp:TextBox>
                    <asp:TextBox ID="UpdateStationNameTextBox" runat="server" CssClass="form-control" placeholder="New Station Name"></asp:TextBox>
                    <asp:Button ID="UpdateStationButton" runat="server" Text="Update Station" CssClass="btn btn-success" OnClick="UpdateStationButton_Click" />
                    <asp:Label ID="UpdateStationSuccessMessage" runat="server" CssClass="text-success"></asp:Label>
                    <asp:Label ID="UpdateStationErrorMessage" runat="server" CssClass="text-danger"></asp:Label>
                </div>
            </div>

            <div class="card">
                <div class="card-body">
                    <h4 id="deleteStation">Delete Station</h4>
                    <asp:TextBox ID="DeleteStationIdTextBox" runat="server" CssClass="form-control" placeholder="Station ID" required="required"></asp:TextBox>
                    <asp:Button ID="DeleteStationButton" runat="server" Text="Delete Station" CssClass="btn btn-danger" OnClick="DeleteStationButton_Click" />
                    <asp:Label ID="DeleteStationSuccessMessage" runat="server" CssClass="text-success"></asp:Label>
                    <asp:Label ID="DeleteStationErrorMessage" runat="server" CssClass="text-danger"></asp:Label>
                </div>
            </div>

            <div class="card">
                <div class="card-body">
                    <h4 id="addSchedule">Add Schedule</h4>
                    <asp:TextBox ID="AddScheduleTrainIdTextBox" runat="server" CssClass="form-control" placeholder="Train ID"></asp:TextBox>
                    <asp:TextBox ID="DepartureStationIdTextBox" runat="server" CssClass="form-control" placeholder="Departure Station ID"></asp:TextBox>
                    <asp:TextBox ID="ArrivalStationIdTextBox" runat="server" CssClass="form-control" placeholder="Arrival Station ID"></asp:TextBox>
                    <asp:TextBox ID="DepartureTimeTextBox" runat="server" CssClass="form-control" placeholder="Departure Time (yyyy-MM-dd HH:mm)"></asp:TextBox>
                    <asp:TextBox ID="ArrivalTimeTextBox" runat="server" CssClass="form-control" placeholder="Arrival Time (yyyy-MM-dd HH:mm)"></asp:TextBox>
                    <asp:Button ID="AddScheduleButton" runat="server" Text="Add Schedule" CssClass="btn btn-primary" OnClick="AddScheduleButton_Click" />
                    <asp:Label ID="ScheduleSuccessMessage" runat="server" CssClass="text-success"></asp:Label>
                    <asp:Label ID="ScheduleErrorMessage" runat="server" CssClass="text-danger"></asp:Label>
                </div>
            </div>

            <div class="card">
                <div class="card-body">
                    <h4 id="getBookings">Get Bookings</h4>
                    <asp:Button ID="GetBookingsButton" runat="server" Text="Get Bookings" CssClass="btn btn-secondary" OnClick="GetBookingsButton_Click" />
                    <asp:GridView ID="BookingsGridView" runat="server" CssClass="table table-striped mt-3">
                        <Columns>
                            <asp:BoundField DataField="BookingId" HeaderText="Booking ID" />
                            <asp:BoundField DataField="TrainId" HeaderText="Train ID" />
                            <asp:BoundField DataField="ScheduleId" HeaderText="Schedule ID" />
                            <asp:BoundField DataField="NIC" HeaderText="NIC" />
                            <asp:BoundField DataField="SeatCount" HeaderText="Seat Count" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
