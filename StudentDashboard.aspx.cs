using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
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

            if (!IsPostBack)
            {
                LoadCourses(); // Load courses on the first page load
            }
        }

        // Load available courses into the GridView
        private void LoadCourses()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT CourseID, CourseName FROM Courses";
                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable coursesTable = new DataTable();

                conn.Open();
                adapter.Fill(coursesTable);

                CoursesGridView.DataSource = coursesTable; // Bind data to GridView
                CoursesGridView.DataBind(); // Refresh GridView
            }
        }

        // Handle Row Command for Enrollment
        protected void CoursesGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Enroll")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = CoursesGridView.Rows[rowIndex];

                int courseId = Convert.ToInt32(row.Cells[0].Text); // Get CourseID from the GridView
                int studentId = Convert.ToInt32(Session["StudentID"]); // Get StudentID from the session

                EnrollStudentInCourse(studentId, courseId);
            }
        }

        // Enroll a student in a course
        private void EnrollStudentInCourse(int studentId, int courseId)
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
                    Response.Write("<script>alert('You are already enrolled in this course.');</script>");
                }
                else
                {
                    // Enroll the student in the course
                    string enrollQuery = "INSERT INTO Enrollments (StudentID, CourseID) VALUES (@StudentID, @CourseID)";
                    SqlCommand enrollCmd = new SqlCommand(enrollQuery, conn);
                    enrollCmd.Parameters.AddWithValue("@StudentID", studentId);
                    enrollCmd.Parameters.AddWithValue("@CourseID", courseId);

                    enrollCmd.ExecuteNonQuery();

                    // Show success message
                    Response.Write("<script>alert('You have successfully enrolled in the course!');</script>");
                }
            }
        }
    }
}
