<%@ Page Title="Grades" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Grades.aspx.cs" Inherits="PlatonStudentApp.Grades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Your Grades</h1>
    <h3>View and update grades for the courses</h3>

    <!-- GridView to Display and Update Grades -->
    <asp:GridView ID="GradesGridView" runat="server" AutoGenerateColumns="False" OnRowCommand="GradesGridView_RowCommand">
        <Columns>
            <asp:BoundField DataField="EnrollmentID" HeaderText="Enrollment ID" Visible="False" />
            <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
            <asp:BoundField DataField="Grade" HeaderText="Grade" />
            <asp:BoundField DataField="EnrollmentDate" HeaderText="Enrollment Date" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:TemplateField HeaderText="Update Grade">
                <ItemTemplate>
                    <asp:TextBox ID="GradeTextBox" runat="server" Text='<%# Eval("Grade") %>' CssClass="form-control" />
                    <asp:Button ID="UpdateGradeButton" runat="server" Text="Update" CommandName="UpdateGrade" 
                                CommandArgument='<%# Eval("EnrollmentID") %>' CssClass="btn btn-primary btn-sm" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
