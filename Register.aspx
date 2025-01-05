<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="PlatonStudentApp.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Student Registration</h1>

    <!-- Registration Form -->
    <div>
        <asp:TextBox ID="FirstNameTextBox" runat="server" CssClass="form-control" Placeholder="First Name"></asp:TextBox>
        <br />
        <asp:TextBox ID="LastNameTextBox" runat="server" CssClass="form-control" Placeholder="Last Name"></asp:TextBox>
        <br />
        <asp:TextBox ID="AddressTextBox" runat="server" CssClass="form-control" Placeholder="Address"></asp:TextBox>
        <br />
        <asp:TextBox ID="PhoneNumberTextBox" runat="server" CssClass="form-control" Placeholder="Phone Number"></asp:TextBox>
        <br />
        <asp:TextBox ID="UsernameTextBox" runat="server" CssClass="form-control" Placeholder="Username"></asp:TextBox>
        <br />
        <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password" CssClass="form-control" Placeholder="Password"></asp:TextBox>
        <br />
        <asp:TextBox ID="EmailTextBox" runat="server" CssClass="form-control" Placeholder="Email"></asp:TextBox>
        <br />
        <asp:Button ID="RegisterButton" runat="server" CssClass="btn btn-primary" Text="Register" OnClick="RegisterButton_Click" />
        <br />
        <asp:Label ID="ErrorMessage" runat="server" ForeColor="Red"></asp:Label>
    </div>
</asp:Content>
