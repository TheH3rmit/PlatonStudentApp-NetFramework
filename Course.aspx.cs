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
    public partial class Course : System.Web.UI.Page
    {
        private string connectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            if (!IsPostBack)
            {
                LoadCourses();
            }
        }

        // Load courses into the GridView
        private void LoadCourses()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT CourseID, CourseName, CourseDescription, Credits FROM Courses";
                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable coursesTable = new DataTable();

                conn.Open();
                adapter.Fill(coursesTable);

                CoursesGridView.DataSource = coursesTable;
                CoursesGridView.DataBind();
            }
        }

        // Handle Add/Update button click
        protected void AddCourseButton_Click(object sender, EventArgs e)
        {
            if (ViewState["EditingCourseID"] != null)
            {
                // Update the course
                int courseId = (int)ViewState["EditingCourseID"];
                UpdateCourse(courseId);
                ViewState["EditingCourseID"] = null;
                AddCourseButton.Text = "Add Course"; // Reset button text
            }
            else
            {
                // Add a new course
                AddCourse();
            }
        }

        // Add a new course
        private void AddCourse()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Courses (CourseName, CourseDescription, Credits) " +
                               "VALUES (@CourseName, @CourseDescription, @Credits)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseName", CourseNameTextBox.Text);
                cmd.Parameters.AddWithValue("@CourseDescription", CourseDescriptionTextBox.Text);
                cmd.Parameters.AddWithValue("@Credits", CourseCreditsTextBox.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Clear the input fields
            CourseNameTextBox.Text = string.Empty;
            CourseDescriptionTextBox.Text = string.Empty;
            CourseCreditsTextBox.Text = string.Empty;

            // Reload the GridView
            LoadCourses();
        }

        // Update an existing course
        private void UpdateCourse(int courseId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Courses SET CourseName = @CourseName, CourseDescription = @CourseDescription, Credits = @Credits " +
                               "WHERE CourseID = @CourseID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", courseId);
                cmd.Parameters.AddWithValue("@CourseName", CourseNameTextBox.Text);
                cmd.Parameters.AddWithValue("@CourseDescription", CourseDescriptionTextBox.Text);
                cmd.Parameters.AddWithValue("@Credits", CourseCreditsTextBox.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Clear the input fields
            CourseNameTextBox.Text = string.Empty;
            CourseDescriptionTextBox.Text = string.Empty;
            CourseCreditsTextBox.Text = string.Empty;

            // Reload the GridView
            LoadCourses();
        }

        // Delete a course
        private void DeleteCourse(int courseId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Courses WHERE CourseID = @CourseID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", courseId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadCourses();
        }

        // Handle GridView Row Command (Edit/Delete)
        protected void CoursesGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = CoursesGridView.Rows[rowIndex];
            int courseId = Convert.ToInt32(row.Cells[0].Text);

            if (e.CommandName == "Delete")
            {
                DeleteCourse(courseId);
            }
            else if (e.CommandName == "Edit")
            {
                // Populate the input fields with the course data for editing
                CourseNameTextBox.Text = row.Cells[1].Text;
                CourseDescriptionTextBox.Text = row.Cells[2].Text;
                CourseCreditsTextBox.Text = row.Cells[3].Text;
                ViewState["EditingCourseID"] = courseId; // Save the CourseID for editing
                AddCourseButton.Text = "Update Course"; // Change button text
            }

            LoadCourses();
        }
    }
}