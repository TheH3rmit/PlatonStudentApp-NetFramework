<%@ Page Title="Course Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Course.aspx.cs" Inherits="PlatonStudentApp.Course" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Manage Courses</h1>

    <!-- GridView to Display Courses -->
    <asp:GridView ID="CoursesGridView" runat="server" AutoGenerateColumns="False" OnRowCommand="CoursesGridView_RowCommand">
        <Columns>
            <asp:BoundField DataField="CourseID" HeaderText="Course ID" />
            <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
            <asp:BoundField DataField="CourseDescription" HeaderText="Description" />
            <asp:BoundField DataField="Credits" HeaderText="Credits" />
            <asp:ButtonField Text="Edit" CommandName="Edit" />
            <asp:ButtonField Text="Delete" CommandName="Delete" />
        </Columns>
    </asp:GridView>

    <br />

    <!-- Input Fields to Add or Update Course -->
    <asp:TextBox ID="CourseNameTextBox" runat="server" Placeholder="Course Name"></asp:TextBox>
    <asp:TextBox ID="CourseDescriptionTextBox" runat="server" Placeholder="Course Description"></asp:TextBox>
    <asp:TextBox ID="CourseCreditsTextBox" runat="server" Placeholder="Credits"></asp:TextBox>
    <asp:Button ID="AddCourseButton" runat="server" Text="Add Course" OnClick="AddCourseButton_Click" />
</asp:Content>
