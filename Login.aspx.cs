using System;
using System.Collections.Generic;
using System.Configuration;
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
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordTextBox.Text.Trim();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                string query = "SELECT UserID, Role FROM Users WHERE Username = @Username AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    int userId = Convert.ToInt32(reader["UserID"]);
                    string role = reader["Role"].ToString();

                    // Store username and role in the session
                    Session["Username"] = username;
                    Session["Role"] = role;
                    Session["UserID"] = userId; // Store UserID for Teacher or Admin use

                    // Redirect based on role
                    switch (role)
                    {
                        case "Student":
                            // Retrieve StudentID and store it in the session
                            reader.Close();
                            string studentQuery = "SELECT StudentID FROM Students WHERE UserID = @UserID";
                            SqlCommand studentCmd = new SqlCommand(studentQuery, conn);
                            studentCmd.Parameters.AddWithValue("@UserID", userId);

                            object studentId = studentCmd.ExecuteScalar();
                            if (studentId != null)
                            {
                                Session["StudentID"] = Convert.ToInt32(studentId);
                                Response.Redirect("StudentDashboard.aspx");
                            }
                            else
                            {
                                ErrorMessage.Text = "Student record not found. Please contact support.";
                            }
                            break;

                        case "Admin":
                            Response.Redirect("AdminDashboard.aspx");
                            break;

                        case "Teacher":
                            Response.Redirect("TeacherDashboard.aspx");
                            break;

                        default:
                            // Handle unexpected roles
                            ErrorMessage.Text = "Access denied. Invalid role.";
                            break;
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
}