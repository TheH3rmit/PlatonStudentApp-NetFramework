using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PlatonStudentApp
{
    public partial class StudentDashboard : System.Web.UI.Page
    {
        private string connectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            // Ensure only students can access this page
            if (Session["Role"] == null || Session["Role"].ToString() != "Student")
            {
                Response.Redirect("Unauthorized.aspx");
            }

            if (Session["StudentID"] == null)
            {
                MessageLabel.ForeColor = System.Drawing.Color.Red;
                MessageLabel.Text = "Error: Student session is not set. Please log in again.";
                return;
            }

            if (!IsPostBack)
            {
                LoadAvailableCourses();
                LoadEnrolledCoursesWithGrades();
            }
        }

        // Load available courses
        private void LoadAvailableCourses()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                SELECT c.CourseID, c.CourseName, u.FirstName + ' ' + u.LastName AS TeacherName
                FROM Courses c
                LEFT JOIN Users u ON c.TeacherID = u.UserID
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM Enrollments e
                    WHERE e.CourseID = c.CourseID AND e.StudentID = @StudentID
                )";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StudentID", Convert.ToInt32(Session["StudentID"]));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable coursesTable = new DataTable();

                    conn.Open();
                    adapter.Fill(coursesTable);

                    CoursesGridView.DataSource = coursesTable;
                    CoursesGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                MessageLabel.ForeColor = System.Drawing.Color.Red;
                MessageLabel.Text = "Error loading available courses: " + ex.Message;
            }
        }


        // Load enrolled courses with grades (read-only)
        private void LoadEnrolledCoursesWithGrades()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                SELECT e.CourseID, c.CourseName, e.Grade, u.FirstName + ' ' + u.LastName AS TeacherName
                FROM Enrollments e
                INNER JOIN Courses c ON e.CourseID = c.CourseID
                LEFT JOIN Users u ON c.TeacherID = u.UserID
                WHERE e.StudentID = @StudentID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StudentID", Convert.ToInt32(Session["StudentID"]));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable enrolledCoursesTable = new DataTable();

                    conn.Open();
                    adapter.Fill(enrolledCoursesTable);

                    EnrolledCoursesGridView.DataSource = enrolledCoursesTable;
                    EnrolledCoursesGridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                MessageLabel.ForeColor = System.Drawing.Color.Red;
                MessageLabel.Text = "Error loading enrolled courses: " + ex.Message;
            }
        }


        // Handle enrollment
        protected void CoursesGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Enroll")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = CoursesGridView.Rows[rowIndex];

                int courseId = Convert.ToInt32(row.Cells[0].Text); // Get CourseID
                int studentId = Convert.ToInt32(Session["StudentID"]); // Get StudentID from session

                EnrollStudentInCourse(studentId, courseId);
            }
        }

        private void EnrollStudentInCourse(int studentId, int courseId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Check if the student is already enrolled in the course
                    string checkQuery = "SELECT COUNT(*) FROM Enrollments WHERE StudentID = @StudentID AND CourseID = @CourseID";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@StudentID", studentId);
                    checkCmd.Parameters.AddWithValue("@CourseID", courseId);

                    conn.Open();
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        // Student is already enrolled
                        MessageLabel.ForeColor = System.Drawing.Color.Red;
                        MessageLabel.Text = "You are already enrolled in this course.";
                    }
                    else
                    {
                        // Enroll the student in the course
                        string enrollQuery = "INSERT INTO Enrollments (StudentID, CourseID, EnrollmentDate) VALUES (@StudentID, @CourseID, GETDATE())";
                        SqlCommand enrollCmd = new SqlCommand(enrollQuery, conn);
                        enrollCmd.Parameters.AddWithValue("@StudentID", studentId);
                        enrollCmd.Parameters.AddWithValue("@CourseID", courseId);

                        enrollCmd.ExecuteNonQuery();

                        MessageLabel.ForeColor = System.Drawing.Color.Green;
                        MessageLabel.Text = "Successfully enrolled in the course.";
                    }
                }

                // Reload the data
                LoadAvailableCourses();
                LoadEnrolledCoursesWithGrades();
            }
            catch (Exception ex)
            {
                MessageLabel.ForeColor = System.Drawing.Color.Red;
                MessageLabel.Text = "Error enrolling in course: " + ex.Message;
            }
        }

        // Handle unenrollment
        protected void EnrolledCoursesGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Unenroll")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = EnrolledCoursesGridView.Rows[rowIndex];

                int courseId = Convert.ToInt32(row.Cells[0].Text); // Get CourseID
                int studentId = Convert.ToInt32(Session["StudentID"]); // Get StudentID from session

                UnenrollStudentFromCourse(studentId, courseId);
            }
        }

        private void UnenrollStudentFromCourse(int studentId, int courseId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Enrollments WHERE StudentID = @StudentID AND CourseID = @CourseID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.Parameters.AddWithValue("@CourseID", courseId);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageLabel.ForeColor = System.Drawing.Color.Green;
                        MessageLabel.Text = "Successfully unenrolled from the course.";
                    }
                    else
                    {
                        MessageLabel.ForeColor = System.Drawing.Color.Red;
                        MessageLabel.Text = "Error: Could not find the enrollment to delete.";
                    }

                    // Reload the data
                    LoadAvailableCourses();
                    LoadEnrolledCoursesWithGrades();
                }
            }
            catch (Exception ex)
            {
                MessageLabel.ForeColor = System.Drawing.Color.Red;
                MessageLabel.Text = "Error unenrolling from course: " + ex.Message;
            }
        }

    }
}
