<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="PlatonStudentApp.AdminDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Admin Dashboard</h1>

    <!-- GridView to Display and Manage Users -->
    <asp:GridView ID="UsersGridView" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="UserID" 
        OnRowEditing="UsersGridView_RowEditing" 
        OnRowDeleting="UsersGridView_RowDeleting" 
        OnRowUpdating="UsersGridView_RowUpdating" 
        OnRowCancelingEdit="UsersGridView_RowCancelingEdit" 
        CssClass="table table-bordered">
        <Columns>
            <asp:BoundField DataField="UserID" HeaderText="User ID" ReadOnly="True" />
            <asp:BoundField DataField="Username" HeaderText="Username" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="Role" HeaderText="Role" />
            <asp:BoundField DataField="FirstName" HeaderText="First Name" />
            <asp:BoundField DataField="LastName" HeaderText="Last Name" />
            <asp:BoundField DataField="PhoneNumber" HeaderText="Phone Number" />
            <asp:BoundField DataField="Address" HeaderText="Address" />
            <asp:BoundField DataField="AdditionalData" HeaderText="Additional Data" />
            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>

    <br />

    <!-- Form to Add New Users -->
    <h3>Add New User</h3>
    <div>
        <asp:TextBox ID="UsernameTextBox" runat="server" CssClass="form-control" Placeholder="Username"></asp:TextBox>
        <br />
        <asp:TextBox ID="PasswordTextBox" runat="server" CssClass="form-control" Placeholder="Password" TextMode="Password"></asp:TextBox>
        <br />
        <asp:TextBox ID="EmailTextBox" runat="server" CssClass="form-control" Placeholder="Email"></asp:TextBox>
        <br />
        <asp:DropDownList ID="RoleDropDown" runat="server" CssClass="form-control">
            <asp:ListItem Text="Admin" Value="Admin" />
            <asp:ListItem Text="Student" Value="Student" />
            <asp:ListItem Text="Teacher" Value="Teacher" />
        </asp:DropDownList>
        <br />
        <asp:TextBox ID="FirstNameTextBox" runat="server" CssClass="form-control" Placeholder="First Name"></asp:TextBox>
        <br />
        <asp:TextBox ID="LastNameTextBox" runat="server" CssClass="form-control" Placeholder="Last Name"></asp:TextBox>
        <br />
        <asp:TextBox ID="PhoneNumberTextBox" runat="server" CssClass="form-control" Placeholder="Phone Number"></asp:TextBox>
        <br />
        <asp:TextBox ID="AddressTextBox" runat="server" CssClass="form-control" Placeholder="Address"></asp:TextBox>
        <br />
        <asp:TextBox ID="AdditionalDataTextBox" runat="server" CssClass="form-control" TextMode="MultiLine" Placeholder="Additional Data"></asp:TextBox>
        <br />
        <asp:Button ID="AddUserButton" runat="server" CssClass="btn btn-primary" Text="Add User" OnClick="AddUserButton_Click" />
        <asp:Label ID="ResultLabel" runat="server" ForeColor="Green"></asp:Label>
    </div>
</asp:Content>
