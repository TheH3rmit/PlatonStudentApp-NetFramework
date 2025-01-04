<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentDashboard.aspx.cs" Inherits="PlatonStudentApp.StudentDashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="CoursesGridView" runat="server" AutoGenerateColumns="False">
    <Columns>
        <asp:BoundField DataField="CourseID" HeaderText="Course ID" />
        <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
        <asp:ButtonField Text="Enroll" CommandName="Enroll" />
    </Columns>
</asp:GridView>
        </div>
    </form>
</body>
</html>
