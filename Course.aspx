<%@ Page Title="Course Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Course.aspx.cs" Inherits="PlatonStudentApp.Course" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Manage Courses</h1>

    <!-- GridView to Display and Edit Courses -->
    <asp:GridView ID="CoursesGridView" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="CourseID,TeacherID" 
        OnRowEditing="CoursesGridView_RowEditing" 
        OnRowCancelingEdit="CoursesGridView_RowCancelingEdit" 
        OnRowUpdating="CoursesGridView_RowUpdating" 
        OnRowDeleting="CoursesGridView_RowDeleting">
        <Columns>
            <asp:BoundField DataField="CourseID" HeaderText="Course ID" ReadOnly="True" />
            <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
            <asp:BoundField DataField="Description" HeaderText="Description" />
            <asp:BoundField DataField="Credits" HeaderText="Credits" />
            <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:TemplateField HeaderText="Teacher">
                <ItemTemplate>
                    <asp:Label ID="TeacherLabel" runat="server" Text='<%# Eval("TeacherName") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="TeacherDropDown" runat="server"></asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ShowEditButton="True" />
            <asp:CommandField ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>

    <br />

    <!-- Input Fields to Add Course -->
    <h3>Add Course</h3>
    <div>
        <asp:TextBox ID="CourseNameTextBox" runat="server" Placeholder="Course Name" CssClass="form-control"></asp:TextBox>
        <br />
        <asp:TextBox ID="CourseDescriptionTextBox" runat="server" Placeholder="Course Description" CssClass="form-control"></asp:TextBox>
        <br />
        <asp:TextBox ID="CourseCreditsTextBox" runat="server" Placeholder="Credits" CssClass="form-control"></asp:TextBox>
        <br />
        <asp:TextBox ID="StartDateTextBox" runat="server" Placeholder="Start Date (YYYY-MM-DD)" CssClass="form-control"></asp:TextBox>
        <br />
        <asp:TextBox ID="EndDateTextBox" runat="server" Placeholder="End Date (YYYY-MM-DD)" CssClass="form-control"></asp:TextBox>
        <br />
        <asp:DropDownList ID="TeacherDropDown" runat="server" CssClass="form-control"></asp:DropDownList>
        <br />
        <asp:Button ID="AddCourseButton" runat="server" Text="Add Course" CssClass="btn btn-primary" OnClick="AddCourseButton_Click" />
    </div>

    <asp:Label ID="MessageLabel" runat="server" ForeColor="Red"></asp:Label>
</asp:Content>
