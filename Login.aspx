<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PlatonStudentApp.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="UsernameTextBox" runat="server" Placeholder="Username"></asp:TextBox>
            <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password" Placeholder="Password"></asp:TextBox>
            <asp:Button ID="LoginButton" runat="server" Text="Login" OnClick="LoginButton_Click" />
            <asp:Label ID="ErrorMessage" runat="server" ForeColor="Red"></asp:Label>

        </div>
    </form>
</body>
</html>
