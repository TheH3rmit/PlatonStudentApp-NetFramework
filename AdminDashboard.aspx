<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="PlatonStudentApp.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Admin Dashboard</h1>

    <!-- GridView to Display Students -->
    <asp:GridView ID="StudentsGridView" runat="server" AutoGenerateColumns="False" Height="131px" Width="516px">
        <Columns>
            <asp:BoundField DataField="StudentID" HeaderText="Student ID" />
            <asp:BoundField DataField="FirstName" HeaderText="First Name" />
            <asp:BoundField DataField="LastName" HeaderText="Last Name" />
            <asp:BoundField DataField="PhoneNumber" HeaderText="Phone Number" />
            <asp:ButtonField Text="Edit" CommandName="Edit" />
            <asp:ButtonField Text="Delete" CommandName="Delete" />
        </Columns>
    </asp:GridView>

    <br />

    <!-- Input Fields to Add Students -->
    <asp:TextBox ID="FirstNameTextBox" runat="server" Placeholder="First Name"></asp:TextBox>
    <asp:TextBox ID="LastNameTextBox" runat="server" Placeholder="Last Name"></asp:TextBox>
    <asp:TextBox ID="PhoneNumberTextBox" runat="server" Placeholder="Phone Number"></asp:TextBox>
    <asp:Button ID="AddStudentButton" runat="server" Text="Add Student" OnClick="AddStudentButton_Click" />
</asp:Content>
