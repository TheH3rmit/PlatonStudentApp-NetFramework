using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PlatonStudentApp
{
    public partial class ManageUsers : System.Web.UI.Page
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
            }
        }

        private void LoadUsers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT UserID, Username, Email, Role FROM Users";
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
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Users (Username, Password, Email, Role, CreatedDate) VALUES (@Username, @Password, @Email, @Role, GETDATE())";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", UsernameTextBox.Text.Trim());
                cmd.Parameters.AddWithValue("@Password", PasswordTextBox.Text.Trim()); // Hashing?
                cmd.Parameters.AddWithValue("@Email", EmailTextBox.Text.Trim());
                cmd.Parameters.AddWithValue("@Role", RoleDropDown.SelectedValue);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            ResultLabel.Text = "User added successfully!";
            ClearForm();
            LoadUsers(); // Refresh the GridView
        }

        private void ClearForm()
        {
            UsernameTextBox.Text = string.Empty;
            PasswordTextBox.Text = string.Empty;
            EmailTextBox.Text = string.Empty;
            RoleDropDown.SelectedIndex = 0;
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

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Users SET Username = @Username, Email = @Email, Role = @Role WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Role", role);

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