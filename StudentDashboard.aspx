<%@ Page Title="Student Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StudentDashboard.aspx.cs" Inherits="PlatonStudentApp.StudentDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Welcome to the Student Dashboard</h1>

    <!-- Available Courses -->
<h3>Available Courses</h3>
<asp:GridView ID="CoursesGridView" runat="server" AutoGenerateColumns="False" OnRowCommand="CoursesGridView_RowCommand">
    <Columns>
        <asp:BoundField DataField="CourseID" HeaderText="Course ID" />
        <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
        <asp:BoundField DataField="TeacherName" HeaderText="Assigned Teacher" />
        <asp:ButtonField Text="Enroll" CommandName="Enroll" />
    </Columns>
</asp:GridView>

    <!-- Enrolled Courses with Grades (Read-Only) -->
<h3>Your Enrolled Courses and Grades</h3>
<asp:GridView ID="EnrolledCoursesGridView" runat="server" AutoGenerateColumns="False" OnRowCommand="EnrolledCoursesGridView_RowCommand">
    <Columns>
        <asp:BoundField DataField="CourseID" HeaderText="Course ID" />
        <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
        <asp:BoundField DataField="Grade" HeaderText="Grade" />
        <asp:BoundField DataField="TeacherName" HeaderText="Assigned Teacher" />
        <asp:ButtonField Text="Unenroll" CommandName="Unenroll" />
    </Columns>
</asp:GridView>


    <asp:Label ID="MessageLabel" runat="server" ForeColor="Red"></asp:Label>
</asp:Content>
