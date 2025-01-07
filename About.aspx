<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="PlatonStudentApp.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2 id="title"><%: Title %>.</h2>
        <h3>About the Project</h3>
        <p>
            This project, developed as part of the course "Projektowanie wielowarstwowych aplikacji internetowych," is a multi-layered web application for managing students, courses, and grades. 
            Built using ASP.NET Web Forms, C#, and SQL Server, the application provides separate dashboards for administrators, students, and teachers, along with features like user management, enrollment, and grading.
        </p>
    </main>
</asp:Content>
