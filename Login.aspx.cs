using System;
using System.Configuration;
using System.Data.SqlClient;

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

                    // Store username, role, and userId in the session
                    Session["Username"] = username;
                    Session["Role"] = role;
                    Session["UserID"] = userId;

                    if (role == "Student")
                    {
                        // Retrieve StudentID and store it in the session
                        reader.Close();
                        string studentQuery = "SELECT UserID FROM Users WHERE UserID = @UserID AND Role = 'Student'";
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
}
