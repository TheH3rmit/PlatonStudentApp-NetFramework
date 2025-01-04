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
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void RegisterButton_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            // Check if the username already exists
            if (IsUsernameTaken(UsernameTextBox.Text, connectionString))
            {
                ErrorMessage.Text = "Username is already taken. Please choose a different username.";
                return;
            }

            // Register the new user
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string userQuery = "INSERT INTO Users (Username, Password, Role, Email) " +
                                   "VALUES (@Username, @Password, 'Student', @Email); SELECT SCOPE_IDENTITY();";
                SqlCommand userCmd = new SqlCommand(userQuery, conn);
                userCmd.Parameters.AddWithValue("@Username", UsernameTextBox.Text);
                userCmd.Parameters.AddWithValue("@Password", PasswordTextBox.Text);
                userCmd.Parameters.AddWithValue("@Email", EmailTextBox.Text);

                conn.Open();

                // Insert into Users table and get the UserID
                int userId = Convert.ToInt32(userCmd.ExecuteScalar());

                // Insert into Students table
                string studentQuery = "INSERT INTO Students (UserID, FirstName, LastName, DateOfBirth, Gender, Address, PhoneNumber) " +
                                      "VALUES (@UserID, @FirstName, @LastName, @DateOfBirth, @Gender, @Address, @PhoneNumber)";
                SqlCommand studentCmd = new SqlCommand(studentQuery, conn);
                studentCmd.Parameters.AddWithValue("@UserID", userId);
                studentCmd.Parameters.AddWithValue("@FirstName", FirstNameTextBox.Text);
                studentCmd.Parameters.AddWithValue("@LastName", LastNameTextBox.Text);
                studentCmd.Parameters.AddWithValue("@DateOfBirth", string.IsNullOrEmpty(DateOfBirthTextBox.Text) ? DBNull.Value : (object)DateOfBirthTextBox.Text);
                studentCmd.Parameters.AddWithValue("@Gender", GenderDropDown.SelectedValue);
                studentCmd.Parameters.AddWithValue("@Address", AddressTextBox.Text);
                studentCmd.Parameters.AddWithValue("@PhoneNumber", PhoneNumberTextBox.Text);

                studentCmd.ExecuteNonQuery();
            }

            // Clear the form and display success message
            ClearForm();
            ErrorMessage.ForeColor = System.Drawing.Color.Green;
            ErrorMessage.Text = "Registration successful!";
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
            DateOfBirthTextBox.Text = string.Empty;
            GenderDropDown.SelectedIndex = 0;
            AddressTextBox.Text = string.Empty;
            PhoneNumberTextBox.Text = string.Empty;
            UsernameTextBox.Text = string.Empty;
            PasswordTextBox.Text = string.Empty;
            EmailTextBox.Text = string.Empty;
        }
    }
}