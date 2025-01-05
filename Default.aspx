<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PlatonStudentApp._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <!-- Hero Section -->
        <section class="jumbotron text-center">
            <h1 class="display-4">Welcome to the Student Registration System</h1>
            <p class="lead">Streamline student management and course enrollment effortlessly!</p>
            <p>
                <a href="Login.aspx" class="btn btn-primary btn-lg">Login &raquo;</a>
                <a href="Register.aspx" class="btn btn-secondary btn-lg">Register as Student &raquo;</a>
            </p>
        </section>

    </main>

</asp:Content>
