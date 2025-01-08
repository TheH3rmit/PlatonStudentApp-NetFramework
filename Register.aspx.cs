using PlatonStudentApp.BusinessLogic;
using System;
using System.Security.Cryptography;
using System.Text;

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
            UserService userService = new UserService();

            try
            {
                // Check if the username is already taken
                if (userService.IsUsernameTaken(UsernameTextBox.Text.Trim()))
                {
                    Session["Message"] = "Username is already taken. Please choose a different username.";
                    Session["MessageType"] = "Error";
                    Response.Redirect(Request.Url.AbsoluteUri);
                    return;
                }

                // Hash the password (optional for simplicity)
                string hashedPassword = HashPassword(PasswordTextBox.Text.Trim());

                // Register the user
                bool success = userService.RegisterUser(
                    UsernameTextBox.Text.Trim(),
                    hashedPassword,
                    EmailTextBox.Text.Trim(),
                    FirstNameTextBox.Text.Trim(),
                    LastNameTextBox.Text.Trim(),
                    AddressTextBox.Text.Trim(),
                    PhoneNumberTextBox.Text.Trim()
                );

                if (success)
                {
                    ClearForm();
                    Session["Message"] = "Registration successful!";
                    Session["MessageType"] = "Success";
                }
                else
                {
                    Session["Message"] = "An error occurred during registration. Please try again.";
                    Session["MessageType"] = "Error";
                }
            }
            catch (Exception ex)
            {
                // Log the exception and set an error message
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                Session["Message"] = "An unexpected error occurred. Please try again.";
                Session["MessageType"] = "Error";
            }

            // Redirect to avoid form resubmission
            Response.Redirect(Request.Url.AbsoluteUri);
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
