using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace PlatonStudentApp
{
    public partial class AdminDashboard : System.Web.UI.Page
    {
        private string connectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            // Restrict access to Admins only
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("Unauthorized.aspx");
            }

            if (!IsPostBack)
            {
                LoadUsers(); // Load users into the GridView

                // Display message from Session if it exists
                if (Session["Message"] != null)
                {
                    ResultLabel.Text = Session["Message"].ToString();
                    ResultLabel.ForeColor = System.Drawing.Color.Green;
                    Session.Remove("Message");
                }
            }
        }


        private void LoadUsers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT UserID, Username, Email, Role, FirstName, LastName, PhoneNumber, Address, AdditionalData FROM Users";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable usersTable = new DataTable();
                conn.Open();
                adapter.Fill(usersTable);

                UsersGridView.DataSource = usersTable;
                UsersGridView.DataBind();
            }
        }

        protected void AddUserButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if the username already exists
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Username", UsernameTextBox.Text.Trim());
                        int usernameExists = (int)checkCmd.ExecuteScalar();

                        if (usernameExists > 0)
                        {
                            ResultLabel.ForeColor = System.Drawing.Color.Red;
                            ResultLabel.Text = "Error: Username already exists. Please choose a different username.";
                            return;
                        }
                    }

                    // Insert the new user if username is unique
                    string insertQuery = @"INSERT INTO Users 
                               (Username, Password, Email, Role, FirstName, LastName, PhoneNumber, Address, AdditionalData, CreatedDate) 
                               VALUES 
                               (@Username, @Password, @Email, @Role, @FirstName, @LastName, @PhoneNumber, @Address, @AdditionalData, GETDATE())";
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@Username", UsernameTextBox.Text.Trim());
                        insertCmd.Parameters.AddWithValue("@Password", PasswordTextBox.Text.Trim()); 
                        insertCmd.Parameters.AddWithValue("@Email", EmailTextBox.Text.Trim());
                        insertCmd.Parameters.AddWithValue("@Role", RoleDropDown.SelectedValue);
                        insertCmd.Parameters.AddWithValue("@FirstName", FirstNameTextBox.Text.Trim());
                        insertCmd.Parameters.AddWithValue("@LastName", LastNameTextBox.Text.Trim());
                        insertCmd.Parameters.AddWithValue("@PhoneNumber", PhoneNumberTextBox.Text.Trim());
                        insertCmd.Parameters.AddWithValue("@Address", AddressTextBox.Text.Trim());
                        insertCmd.Parameters.AddWithValue("@AdditionalData", AdditionalDataTextBox.Text.Trim());

                        insertCmd.ExecuteNonQuery();
                    }
                }

                // Set success message in Session and redirect
                Session["Message"] = "User added successfully!";
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch (Exception ex)
            {
                ResultLabel.ForeColor = System.Drawing.Color.Red;
                ResultLabel.Text = "An error occurred while adding the user.";
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
            }
        }


        private void ClearForm()
        {
            UsernameTextBox.Text = string.Empty;
            PasswordTextBox.Text = string.Empty;
            EmailTextBox.Text = string.Empty;
            RoleDropDown.SelectedIndex = 0;
            FirstNameTextBox.Text = string.Empty;
            LastNameTextBox.Text = string.Empty;
            PhoneNumberTextBox.Text = string.Empty;
            AddressTextBox.Text = string.Empty;
            AdditionalDataTextBox.Text = string.Empty;
        }

        protected void UsersGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            UsersGridView.EditIndex = e.NewEditIndex;
            LoadUsers(); // Refresh GridView to enter edit mode
        }

        protected void UsersGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = UsersGridView.Rows[e.RowIndex];
            int userId = Convert.ToInt32(UsersGridView.DataKeys[e.RowIndex].Value);
            string username = ((TextBox)row.Cells[1].Controls[0]).Text;
            string email = ((TextBox)row.Cells[2].Controls[0]).Text;
            string role = ((TextBox)row.Cells[3].Controls[0]).Text;
            string firstName = ((TextBox)row.Cells[4].Controls[0]).Text;
            string lastName = ((TextBox)row.Cells[5].Controls[0]).Text;
            string phoneNumber = ((TextBox)row.Cells[6].Controls[0]).Text;
            string address = ((TextBox)row.Cells[7].Controls[0]).Text;
            string additionalData = ((TextBox)row.Cells[8].Controls[0]).Text;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Users 
                                SET Username = @Username, Email = @Email, Role = @Role, FirstName = @FirstName, LastName = @LastName, 
                                    PhoneNumber = @PhoneNumber, Address = @Address, AdditionalData = @AdditionalData
                                WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Role", role);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@AdditionalData", additionalData);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            UsersGridView.EditIndex = -1; // Exit edit mode
            LoadUsers(); // Refresh the GridView
        }

        protected void UsersGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            UsersGridView.EditIndex = -1;
            LoadUsers(); // Refresh the GridView to cancel edit mode
        }

        protected void UsersGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int userId = Convert.ToInt32(UsersGridView.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Users WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadUsers(); // Refresh the GridView
        }
    }
}