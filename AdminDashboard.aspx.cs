using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration; 

namespace PlatonStudentApp
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private string connectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            if (!IsPostBack)
            {
                LoadStudents();
            }
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("Unauthorized.aspx"); // Redirect unauthorized users
            }
        }

        private void LoadStudents()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT StudentID, FirstName, LastName, PhoneNumber FROM Students";
                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable studentsTable = new DataTable();

                conn.Open();
                adapter.Fill(studentsTable);

                StudentsGridView.DataSource = studentsTable;
                StudentsGridView.DataBind();
            }
        }

        protected void AddStudentButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Students (FirstName, LastName, PhoneNumber) " +
                               "VALUES (@FirstName, @LastName, @PhoneNumber)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FirstName", FirstNameTextBox.Text);
                cmd.Parameters.AddWithValue("@LastName", LastNameTextBox.Text);
                cmd.Parameters.AddWithValue("@PhoneNumber", PhoneNumberTextBox.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Clear the input fields
            FirstNameTextBox.Text = string.Empty;
            LastNameTextBox.Text = string.Empty;
            PhoneNumberTextBox.Text = string.Empty;

            // Reload the GridView
            LoadStudents();
        }
    }
}
