<%@ Page Title="Manage Users" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageUsers.aspx.cs" Inherits="PlatonStudentApp.ManageUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Manage Users</h1>

    <!-- GridView to List and Manage Users -->
<asp:GridView ID="UsersGridView" runat="server" AutoGenerateColumns="False" 
    OnRowEditing="UsersGridView_RowEditing" 
    OnRowDeleting="UsersGridView_RowDeleting" 
    OnRowUpdating="UsersGridView_RowUpdating" 
    OnRowCancelingEdit="UsersGridView_RowCancelingEdit">
    <Columns>
        <asp:BoundField DataField="UserID" HeaderText="User ID" ReadOnly="True" />
        <asp:BoundField DataField="Username" HeaderText="Username" />
        <asp:BoundField DataField="Email" HeaderText="Email" />
        <asp:BoundField DataField="Role" HeaderText="Role" />
        <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
    </Columns>
</asp:GridView>

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
        <asp:Button ID="AddUserButton" runat="server" CssClass="btn btn-primary" Text="Add User" OnClick="AddUserButton_Click" />
        <asp:Label ID="ResultLabel" runat="server" ForeColor="Green"></asp:Label>
    </div>
</asp:Content>
