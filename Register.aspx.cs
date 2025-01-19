using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace PlatonStudentApp
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Display message from Session if it exists
            if (Session["Message"] != null)
            {
                if (Session["MessageType"] != null && Session["MessageType"].ToString() == "Success")
                {
                    ErrorMessage.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    ErrorMessage.ForeColor = System.Drawing.Color.Red;
                }

                ErrorMessage.Text = Session["Message"].ToString();
                Session.Remove("Message");
                Session.Remove("MessageType");
            }
        }

        protected void RegisterButton_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            try
            {
                // Check if the username is already taken
                if (IsUsernameTaken(UsernameTextBox.Text.Trim(), connectionString))
                {
                    Session["Message"] = "Username is already taken. Please choose a different username.";
                    Session["MessageType"] = "Error";
                    Response.Redirect(Request.Url.AbsoluteUri);
                    return;
                }

                // Useing plain text password (no hashing in this case for simplicity)
                string plainPassword = PasswordTextBox.Text.Trim();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO Users 
                        (Username, Password, Role, Email, FirstName, LastName, Address, PhoneNumber, CreatedDate) 
                        VALUES 
                        (@Username, @Password, 'Student', @Email, @FirstName, @LastName, @Address, @PhoneNumber, GETDATE())";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", UsernameTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", plainPassword); // Save plain text password
                    cmd.Parameters.AddWithValue("@Email", EmailTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@FirstName", FirstNameTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@LastName", LastNameTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", AddressTextBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@PhoneNumber", PhoneNumberTextBox.Text.Trim());

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                // Clear the form and show a success message
                ClearForm();
                Session["Message"] = "Registration successful!";
                Session["MessageType"] = "Success";
            }
            catch (Exception ex)
            {
                // Log the exception and set an error message
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                Session["Message"] = "An error occurred during registration. Please try again.";
                Session["MessageType"] = "Error";
            }

            // Redirect to avoid form resubmission
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        private bool IsUsernameTaken(string username, string connectionString)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();

                return count > 0;
            }
        }

        private void ClearForm()
        {
            FirstNameTextBox.Text = string.Empty;
            LastNameTextBox.Text = string.Empty;
            AddressTextBox.Text = string.Empty;
            PhoneNumberTextBox.Text = string.Empty;
            UsernameTextBox.Text = string.Empty;
            PasswordTextBox.Text = string.Empty;
            EmailTextBox.Text = string.Empty;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
