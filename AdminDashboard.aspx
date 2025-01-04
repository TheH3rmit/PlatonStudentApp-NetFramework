<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="PlatonStudentApp.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="StudentsGridView" runat="server" AutoGenerateColumns="False" Height="131px" style="margin-bottom: 0px" Width="516px">
    <Columns>
        <asp:BoundField DataField="StudentID" HeaderText="Student ID" />
        <asp:BoundField DataField="FirstName" HeaderText="First Name" />
        <asp:BoundField DataField="LastName" HeaderText="Last Name" />
        <asp:BoundField DataField="PhoneNumber" HeaderText="Phone Number" />
        <asp:ButtonField Text="Edit" CommandName="Edit" />
        <asp:ButtonField Text="Delete" CommandName="Delete" />
    </Columns>
</asp:GridView>

            <asp:TextBox ID="FirstNameTextBox" runat="server" Placeholder="First Name"></asp:TextBox>
            <asp:TextBox ID="LastNameTextBox" runat="server" Placeholder="Last Name"></asp:TextBox>
            <asp:TextBox ID="PhoneNumberTextBox" runat="server" Placeholder="Phone Number"></asp:TextBox>
            <asp:Button ID="AddStudentButton" runat="server" Text="Add Student" OnClick="AddStudentButton_Click" />

        </div>
    </form>
</body>
</html>
