using PlatonStudentApp.BusinessLogic;
using System;

namespace PlatonStudentApp
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordTextBox.Text.Trim();

            UserService userService = new UserService();
            var (userId, role) = userService.ValidateUser(username, password);

            if (userId > 0 && role != null)
            {
                // Store username, role, and userId in the session
                Session["Username"] = username;
                Session["Role"] = role;
                Session["UserID"] = userId;

                if (role == "Student")
                {
                    int? studentId = userService.GetStudentID(userId);
                    if (studentId.HasValue)
                    {
                        Session["StudentID"] = studentId.Value;
                        Response.Redirect("StudentDashboard.aspx");
                    }
                    else
                    {
                        ErrorMessage.Text = "Student record not found. Please contact support.";
                    }
                }
                else if (role == "Admin")
                {
                    Response.Redirect("AdminDashboard.aspx");
                }
                else if (role == "Teacher")
                {
                    Response.Redirect("TeacherDashboard.aspx");
                }
            }
            else
            {
                // Invalid login credentials
                ErrorMessage.Text = "Invalid Username or Password.";
            }
        }
    }
}
