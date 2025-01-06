using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

            // Ensure only Admins or Teachers can access this page
            if (Session["Role"] == null || (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "Teacher"))
            {
                Response.Redirect("Unauthorized.aspx");
            }

            if (!IsPostBack)
            {
                LoadCourses();
                LoadTeachers();
            }
        }

        private void LoadCourses()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT 
                    c.CourseID, 
                    c.CourseName, 
                    c.Description, 
                    c.Credits, 
                    c.StartDate, 
                    c.EndDate, 
                    c.TeacherID, 
                    u.FirstName + ' ' + u.LastName AS TeacherName
                FROM 
                    Courses c
                LEFT JOIN 
                    Users u ON c.TeacherID = u.UserID";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable coursesTable = new DataTable();

                conn.Open();
                adapter.Fill(coursesTable);

                CoursesGridView.DataSource = coursesTable;
                CoursesGridView.DataBind();
            }
        }

        private void LoadTeachers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT UserID, FirstName + ' ' + LastName AS FullName FROM Users WHERE Role = 'Teacher'";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                TeacherDropDown.DataSource = reader;
                TeacherDropDown.DataTextField = "FullName";
                TeacherDropDown.DataValueField = "UserID";
                TeacherDropDown.DataBind();

                TeacherDropDown.Items.Insert(0, new ListItem("-- Select a Teacher --", "0"));
            }
        }

        protected void AddCourseButton_Click(object sender, EventArgs e)
        {
            // Validate fields
            if (string.IsNullOrEmpty(CourseNameTextBox.Text) ||
                string.IsNullOrEmpty(CourseDescriptionTextBox.Text) ||
                string.IsNullOrEmpty(CourseCreditsTextBox.Text) ||
                string.IsNullOrEmpty(StartDateTextBox.Text) ||
                string.IsNullOrEmpty(EndDateTextBox.Text) ||
                TeacherDropDown.SelectedValue == "0")
            {
                MessageLabel.ForeColor = System.Drawing.Color.Red;
                MessageLabel.Text = "All fields are required.";
                return;
            }

            // Add course
            AddCourse();
        }

        private void AddCourse()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                INSERT INTO Courses (CourseName, Description, Credits, StartDate, EndDate, TeacherID) 
                VALUES (@CourseName, @Description, @Credits, @StartDate, @EndDate, @TeacherID)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseName", CourseNameTextBox.Text.Trim());
                cmd.Parameters.AddWithValue("@Description", CourseDescriptionTextBox.Text.Trim());
                cmd.Parameters.AddWithValue("@Credits", int.Parse(CourseCreditsTextBox.Text.Trim()));
                cmd.Parameters.AddWithValue("@StartDate", DateTime.Parse(StartDateTextBox.Text.Trim()));
                cmd.Parameters.AddWithValue("@EndDate", DateTime.Parse(EndDateTextBox.Text.Trim()));
                cmd.Parameters.AddWithValue("@TeacherID", int.Parse(TeacherDropDown.SelectedValue));

                conn.Open();
                cmd.ExecuteNonQuery();

                MessageLabel.ForeColor = System.Drawing.Color.Green;
                MessageLabel.Text = "Course added successfully!";
            }

            // Clear input fields
            ClearInputFields();
            LoadCourses();
        }

        private void DeleteCourse(int courseId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Delete related enrollments
                string deleteEnrollmentsQuery = "DELETE FROM Enrollments WHERE CourseID = @CourseID";
                using (SqlCommand cmd = new SqlCommand(deleteEnrollmentsQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@CourseID", courseId);
                    cmd.ExecuteNonQuery();
                }

                // Delete the course
                string deleteCourseQuery = "DELETE FROM Courses WHERE CourseID = @CourseID";
                using (SqlCommand cmd = new SqlCommand(deleteCourseQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@CourseID", courseId);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageLabel.ForeColor = System.Drawing.Color.Green;
            MessageLabel.Text = "Course and its related enrollments deleted successfully!";
        }


        protected void CoursesGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Set the row in edit mode
            CoursesGridView.EditIndex = e.NewEditIndex;
            LoadCourses();

            // Get the current row being edited
            GridViewRow row = CoursesGridView.Rows[e.NewEditIndex];
            DropDownList teacherDropDown = (DropDownList)row.FindControl("TeacherDropDown");

            if (teacherDropDown != null)
            {
                // Populate the TeacherDropDown with teacher data
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT UserID, FirstName + ' ' + LastName AS FullName FROM Users WHERE Role = 'Teacher'";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    teacherDropDown.DataSource = reader;
                    teacherDropDown.DataTextField = "FullName";
                    teacherDropDown.DataValueField = "UserID";
                    teacherDropDown.DataBind();
                }

                teacherDropDown.Items.Insert(0, new ListItem("-- Select a Teacher --", "0"));

                // Set the selected value to the current teacher
                string teacherId = CoursesGridView.DataKeys[e.NewEditIndex]["TeacherID"]?.ToString();
                if (!string.IsNullOrEmpty(teacherId))
                {
                    teacherDropDown.SelectedValue = teacherId;
                }
            }
        }

        protected void CoursesGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Cancel edit mode and reload courses
            CoursesGridView.EditIndex = -1;
            LoadCourses();
        }

        protected void CoursesGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Get the CourseID of the row being deleted
            int courseId = Convert.ToInt32(CoursesGridView.DataKeys[e.RowIndex].Value);

            // Call the DeleteCourse method to delete the course
            DeleteCourse(courseId);

            // Reload the courses after deletion
            LoadCourses();

            // Show a success message
            MessageLabel.ForeColor = System.Drawing.Color.Green;
            MessageLabel.Text = "Course deleted successfully!";
        }

        protected void CoursesGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Example logic: Handle custom commands like "Delete" or "Edit" here
            if (e.CommandName == "Delete")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int courseId = Convert.ToInt32(CoursesGridView.DataKeys[rowIndex].Value);

                // Call the DeleteCourse method to delete the course
                DeleteCourse(courseId);

                // Reload the courses
                LoadCourses();

                // Show a success message
                MessageLabel.ForeColor = System.Drawing.Color.Green;
                MessageLabel.Text = "Course deleted successfully!";
            }
        }


        protected void CoursesGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Get the current row being updated
            GridViewRow row = CoursesGridView.Rows[e.RowIndex];

            // Retrieve data from controls in the row
            int courseId = Convert.ToInt32(CoursesGridView.DataKeys[e.RowIndex].Value);
            string courseName = ((TextBox)row.Cells[1].Controls[0]).Text.Trim();
            string description = ((TextBox)row.Cells[2].Controls[0]).Text.Trim();
            int credits = int.Parse(((TextBox)row.Cells[3].Controls[0]).Text.Trim());
            DateTime startDate = DateTime.Parse(((TextBox)row.Cells[4].Controls[0]).Text.Trim());
            DateTime endDate = DateTime.Parse(((TextBox)row.Cells[5].Controls[0]).Text.Trim());
            DropDownList teacherDropDown = (DropDownList)row.FindControl("TeacherDropDown");

            if (teacherDropDown == null || string.IsNullOrEmpty(teacherDropDown.SelectedValue) || teacherDropDown.SelectedValue == "0")
            {
                MessageLabel.ForeColor = System.Drawing.Color.Red;
                MessageLabel.Text = "Error: Please select a valid teacher.";
                return;
            }

            int teacherId = int.Parse(teacherDropDown.SelectedValue);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                UPDATE Courses 
                SET CourseName = @CourseName, 
                    Description = @Description, 
                    Credits = @Credits, 
                    StartDate = @StartDate, 
                    EndDate = @EndDate, 
                    TeacherID = @TeacherID 
                WHERE CourseID = @CourseID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", courseId);
                cmd.Parameters.AddWithValue("@CourseName", courseName);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@Credits", credits);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);
                cmd.Parameters.AddWithValue("@TeacherID", teacherId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Reset edit index and reload courses
            CoursesGridView.EditIndex = -1;
            LoadCourses();

            MessageLabel.ForeColor = System.Drawing.Color.Green;
            MessageLabel.Text = "Course updated successfully!";
        }

        private void ClearInputFields()
        {
            CourseNameTextBox.Text = string.Empty;
            CourseDescriptionTextBox.Text = string.Empty;
            CourseCreditsTextBox.Text = string.Empty;
            StartDateTextBox.Text = string.Empty;
            EndDateTextBox.Text = string.Empty;
            TeacherDropDown.SelectedIndex = 0;
        }
    }
}
