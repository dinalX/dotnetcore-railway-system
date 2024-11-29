<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TrainBooking.aspx.cs" Inherits="BookingWebApplication.TrainBooking" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Train Booking</title>
    <link rel="stylesheet" type="text/css" href="~/Content/bootstrap.min.css" />
    <script src="~/Scripts/jquery-3.7.1.min.js"></script>
    <script src="~/Scripts/bootstrap.min.js"></script>
    <style>
        body {
            background-color: #f8f9fa;
        }
        .container {
            margin-top: 20px;
            padding: 20px;
            background-color: #ffffff;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            position: relative; 
        }
        h2 {
            text-align: center;
            margin-bottom: 20px;
        }
        .form-group label {
            font-weight: bold;
        }
        .btn-primary, .btn-success {
            width: 100%;
        }
        .text-success, .text-danger {
            text-align: center;
            margin-top: 10px;
        }
        .hidden-field {
            display: none;
        }
        .grid-container {
            margin-top: 20px;
        }
        .admin-button {
            position: absolute; 
            top: 20px;
            right: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div class="container">
            <h2>Train Booking</h2>

            <!-- Button to navigate to Admin Page -->
            <asp:Button ID="AdminButton" runat="server" Text="Admin Page" CssClass="btn btn-secondary admin-button" OnClick="AdminButton_Click" />

            <div class="row">
                <div class="col-md-6 mb-3">
                    <label for="DepartureStationDropDownList">Departure Station:</label>
                    <asp:DropDownList ID="DepartureStationDropDownList" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>

                <div class="col-md-6 mb-3">
                    <label for="ArrivalStationDropDownList">Arrival Station:</label>
                    <asp:DropDownList ID="ArrivalStationDropDownList" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>

            <div class="row">
                <div class="col-md-6 mb-3">
                    <label for="TravelDateTextBox">Travel Date:</label>
                    <asp:TextBox ID="TravelDateTextBox" runat="server" CssClass="form-control" placeholder="YYYY-MM-DD"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TravelDateTextBox" Format="yyyy-MM-dd" />
                </div>
            </div>

            <div class="form-group">
                <asp:Button ID="SearchButton" runat="server" Text="Search Trains" CssClass="btn btn-primary" OnClick="SearchButton_Click" />
            </div>

            <div class="grid-container">
                <asp:GridView ID="ScheduleGridView" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" OnRowCommand="ScheduleGridView_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="ScheduleId" HeaderText="Schedule ID" Visible="False" />
                        <asp:BoundField DataField="TrainName" HeaderText="Train Name" /> 
                        <asp:BoundField DataField="DepartureTime" HeaderText="Departure Time" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                        <asp:BoundField DataField="ArrivalTime" HeaderText="Arrival Time" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                        <asp:ButtonField ButtonType="Button" Text="Select" CommandName="Select" />
                    </Columns>
                </asp:GridView>
            </div>

            <asp:HiddenField ID="ScheduleIdHiddenField" runat="server" />

            <div class="hidden-field">
                <label for="ScheduleIdTextBox">Schedule ID:</label>
                <asp:TextBox ID="ScheduleIdTextBox" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>

            <div class="form-group mb-3">
                <label for="NICTextBox">NIC:</label>
                <asp:TextBox ID="NICTextBox" runat="server" CssClass="form-control" style="padding: 10px;"></asp:TextBox>
            </div>

            <div class="form-group mb-3">
                <label for="SeatCountDropDownList">Seat Count:</label>
                <asp:DropDownList ID="SeatCountDropDownList" runat="server" CssClass="form-control">
                    <asp:ListItem Value="1">1</asp:ListItem>
                    <asp:ListItem Value="2">2</asp:ListItem>
                    <asp:ListItem Value="3">3</asp:ListItem>
                    <asp:ListItem Value="4">4</asp:ListItem>
                    <asp:ListItem Value="5">5</asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="form-group mb-3">
                <label for="PassengerNameTextBox">Passenger Name:</label>
                <asp:TextBox ID="PassengerNameTextBox" runat="server" CssClass="form-control" style="padding: 10px;"></asp:TextBox>
            </div>

            <div class="form-group mb-3">
                <label for="ContactInfoTextBox">Contact Info:</label>
                <asp:TextBox ID="ContactInfoTextBox" runat="server" CssClass="form-control" style="padding: 10px;"></asp:TextBox>
            </div>

            <div class="form-group mb-3">
                <asp:Button ID="BookButton" runat="server" Text="Book Ticket" CssClass="btn btn-success" OnClick="BookButton_Click" style="padding: 10px;" />
            </div>

            <asp:Label ID="SuccessMessage" runat="server" CssClass="text-success mb-3"></asp:Label>
            <asp:Label ID="ErrorMessage" runat="server" CssClass="text-danger mb-3"></asp:Label>

        </div>
    </form>
</body>
</html>
