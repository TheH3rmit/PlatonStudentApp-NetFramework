<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Unauthorized.aspx.cs" Inherits="PlatonStudentApp.Unauthorized" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Access Denied</h1>
    <p>You do not have permission to access this page. Please ensure you are logged in with the appropriate credentials.</p>
    <a href="Login.aspx" class="btn btn-primary">Go to Login</a>
    <br />
    <a href="Default.aspx">Return to Homepage</a>
</asp:Content>
