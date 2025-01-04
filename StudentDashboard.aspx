<%@ Page Title="Student Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StudentDashboard.aspx.cs" Inherits="PlatonStudentApp.StudentDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Welcome to the Student Dashboard</h1>
    <h3>Available Courses</h3>

    <!-- GridView to Display Courses -->
    <asp:GridView ID="CoursesGridView" runat="server" AutoGenerateColumns="False" OnRowCommand="CoursesGridView_RowCommand">
        <Columns>
            <asp:BoundField DataField="CourseID" HeaderText="Course ID" />
            <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
            <asp:ButtonField Text="Enroll" CommandName="Enroll" />
        </Columns>
    </asp:GridView>
</asp:Content>
