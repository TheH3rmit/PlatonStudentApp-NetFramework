using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PlatonStudentApp
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string username = UsernameTextBox.Text; // Get username from the form
            string password = PasswordTextBox.Text; // Get password from the form

            // SQL connection string
            using (SqlConnection conn = new SqlConnection("YourConnectionString"))
            {
                // SQL query to check username and password
                string query = "SELECT Role FROM Users WHERE Username = @Username AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                conn.Open();
                var role = cmd.ExecuteScalar(); // Executes the query and gets the Role (Admin/Student)

                if (role != null) // If a matching user is found
                {
                    // Save the username and role in the session
                    Session["Username"] = username;
                    Session["Role"] = role.ToString();

                    // Redirect based on role
                    if (role.ToString() == "Admin")
                    {
                        Response.Redirect("AdminDashboard.aspx");
                    }
                    else
                    {
                        Response.Redirect("StudentDashboard.aspx");
                    }
                }
                else
                {
                    // Show error message
                    ErrorMessage.Text = "Invalid Username or Password.";
                }
            }
        }

    }
}