using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PlatonStudentApp
{
    public partial class Grades : System.Web.UI.Page
    {
        private string connectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            // Redirect unauthorized users
            if (Session["Role"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadGrades(); // Load grades on first page load
            }
        }

        private void LoadGrades()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "";

                // Check user role and modify the query accordingly
                if (Session["Role"].ToString() == "Student")
                {
                    // Query for students to view their own grades
                    query = @"
                        SELECT 
                            e.EnrollmentID,
                            c.CourseName,
                            e.Grade,
                            e.EnrollmentDate
                        FROM 
                            Enrollments e
                        INNER JOIN 
                            Courses c ON e.CourseID = c.CourseID
                        WHERE 
                            e.StudentID = @StudentID";
                }
                else if (Session["Role"].ToString() == "Teacher" || Session["Role"].ToString() == "Admin")
                {
                    // Query for teachers or admins to view all grades for courses they manage
                    query = @"
                        SELECT 
                            e.EnrollmentID,
                            c.CourseName,
                            e.Grade,
                            e.EnrollmentDate
                        FROM 
                            Enrollments e
                        INNER JOIN 
                            Courses c ON e.CourseID = c.CourseID
                        INNER JOIN 
                            Users u ON c.TeacherID = u.UserID
                        WHERE 
                            (@Role = 'Admin' OR c.TeacherID = @UserID)";
                }

                SqlCommand cmd = new SqlCommand(query, conn);

                if (Session["Role"].ToString() == "Student")
                {
                    cmd.Parameters.AddWithValue("@StudentID", Convert.ToInt32(Session["StudentID"]));
                }
                else if (Session["Role"].ToString() == "Teacher" || Session["Role"].ToString() == "Admin")
                {
                    cmd.Parameters.AddWithValue("@Role", Session["Role"].ToString());
                    cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(Session["UserID"]));
                }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable gradesTable = new DataTable();

                conn.Open();
                adapter.Fill(gradesTable);

                GradesGridView.DataSource = gradesTable; // Bind data to the GridView
                GradesGridView.DataBind(); // Refresh the GridView
            }
        }

        protected void GradesGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateGrade")
            {
                if (Session["Role"].ToString() != "Teacher" && Session["Role"].ToString() != "Admin")
                {
                    // Only teachers and admins can update grades
                    Response.Redirect("Unauthorized.aspx");
                }

                // Get the EnrollmentID and new grade value
                int enrollmentId = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = (GridViewRow)((Button)e.CommandSource).NamingContainer;
                string newGrade = ((TextBox)row.FindControl("GradeTextBox")).Text;

                // Update the grade in the database
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string updateQuery = @"
                        UPDATE Enrollments
                        SET Grade = @Grade
                        WHERE EnrollmentID = @EnrollmentID";

                    SqlCommand cmd = new SqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@Grade", newGrade);
                    cmd.Parameters.AddWithValue("@EnrollmentID", enrollmentId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                // Reload the GridView
                LoadGrades();
            }
        }
    }
}
