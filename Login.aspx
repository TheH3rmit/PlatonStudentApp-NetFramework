<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PlatonStudentApp.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Login</h1>

    <!-- Login Form -->
    <div>
        <asp:TextBox ID="UsernameTextBox" runat="server" CssClass="form-control" Placeholder="Username"></asp:TextBox>
        <br />
        <asp:TextBox ID="PasswordTextBox" runat="server" CssClass="form-control" TextMode="Password" Placeholder="Password"></asp:TextBox>
        <br />
        <asp:Button ID="LoginButton" runat="server" CssClass="btn btn-primary" Text="Login" OnClick="LoginButton_Click" />
        <br />
        <asp:Label ID="ErrorMessage" runat="server" ForeColor="Red"></asp:Label>
    </div>
</asp:Content>
