<%@ Page Title="Grades" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Grades.aspx.cs" Inherits="PlatonStudentApp.Grades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Your Grades</h1>
    <h3>View your grades for the courses you are enrolled in</h3>

    <!-- GridView to Display Grades -->
    <asp:GridView ID="GradesGridView" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
            <asp:BoundField DataField="Grade" HeaderText="Grade" />
            <asp:BoundField DataField="EnrollmentDate" HeaderText="Enrollment Date" DataFormatString="{0:yyyy-MM-dd}" />
        </Columns>
    </asp:GridView>
</asp:Content>
