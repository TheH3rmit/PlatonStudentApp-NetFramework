<%@ Page Title="Teacher Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TeacherDashboard.aspx.cs" Inherits="PlatonStudentApp.TeacherDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Teacher Dashboard</h1>
    <h3>Manage Grades for Your Courses</h3>

    <!-- Course Selection -->
    <asp:DropDownList ID="CourseDropDown" runat="server" AutoPostBack="True" OnSelectedIndexChanged="CourseDropDown_SelectedIndexChanged" CssClass="form-control">
    </asp:DropDownList>
    <br />

    <!-- GridView to Manage Student Grades -->
    <asp:GridView ID="StudentsGridView" runat="server" AutoGenerateColumns="False" OnRowCommand="UpdateGrade_Command">
        <Columns>
            <asp:BoundField DataField="StudentID" HeaderText="Student ID" />
            <asp:BoundField DataField="FirstName" HeaderText="First Name" />
            <asp:BoundField DataField="LastName" HeaderText="Last Name" />
            <asp:BoundField DataField="Grade" HeaderText="Grade" />
            <asp:TemplateField HeaderText="Update Grade">
                <ItemTemplate>
                    <asp:TextBox ID="GradeTextBox" runat="server" Text='<%# Eval("Grade") %>' CssClass="form-control" />
                    <asp:Button ID="UpdateGradeButton" runat="server" Text="Update" CommandName="UpdateGrade" CommandArgument='<%# Eval("StudentID") + "," + Eval("CourseID") %>' CssClass="btn btn-primary btn-sm mt-1" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <asp:Label ID="MessageLabel" runat="server" ForeColor="Green" />
</asp:Content>
