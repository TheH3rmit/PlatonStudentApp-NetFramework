using PlatonStudentApp.BusinessLogic;
using System;
using System.Web.UI.WebControls;

namespace PlatonStudentApp
{
    public partial class AdminDashboard : System.Web.UI.Page
    {
        private UserService userService;

        protected void Page_Load(object sender, EventArgs e)
        {
            userService = new UserService();

            // Restrict access to Admins only
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("Unauthorized.aspx");
            }

            if (!IsPostBack)
            {
                LoadUsers();

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
            UsersGridView.DataSource = userService.GetAllUsers();
            UsersGridView.DataBind();
        }

        protected void AddUserButton_Click(object sender, EventArgs e)
        {
            if (userService.IsUsernameTaken(UsernameTextBox.Text.Trim()))
            {
                ResultLabel.ForeColor = System.Drawing.Color.Red;
                ResultLabel.Text = "Error: Username already exists. Please choose a different username.";
                return;
            }

            bool success = userService.AddUser(
                UsernameTextBox.Text.Trim(),
                PasswordTextBox.Text.Trim(), // Note: Password should be hashed in production
                EmailTextBox.Text.Trim(),
                RoleDropDown.SelectedValue,
                FirstNameTextBox.Text.Trim(),
                LastNameTextBox.Text.Trim(),
                PhoneNumberTextBox.Text.Trim(),
                AddressTextBox.Text.Trim(),
                AdditionalDataTextBox.Text.Trim()
            );

            if (success)
            {
                Session["Message"] = "User added successfully!";
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                ResultLabel.ForeColor = System.Drawing.Color.Red;
                ResultLabel.Text = "An error occurred while adding the user.";
            }
        }

        protected void UsersGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            UsersGridView.EditIndex = e.NewEditIndex;
            LoadUsers();
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

            bool success = userService.UpdateUser(userId, username, email, role, firstName, lastName, phoneNumber, address, additionalData);

            if (success)
            {
                UsersGridView.EditIndex = -1;
                LoadUsers();
            }
            else
            {
                ResultLabel.ForeColor = System.Drawing.Color.Red;
                ResultLabel.Text = "An error occurred while updating the user.";
            }
        }

        protected void UsersGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            UsersGridView.EditIndex = -1;
            LoadUsers();
        }

        protected void UsersGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int userId = Convert.ToInt32(UsersGridView.DataKeys[e.RowIndex].Value);

            if (userService.DeleteUser(userId))
            {
                LoadUsers();
            }
            else
            {
                ResultLabel.ForeColor = System.Drawing.Color.Red;
                ResultLabel.Text = "An error occurred while deleting the user.";
            }
        }
    }
}
