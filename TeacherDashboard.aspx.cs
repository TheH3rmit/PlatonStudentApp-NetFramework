using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PlatonStudentApp
{
    public partial class TeacherDashboard : System.Web.UI.Page
    {
        private string connectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            if (Session["Role"] == null || Session["Role"].ToString() != "Teacher")
            {
                // Redirect to unauthorized page if not a teacher
                Response.Redirect("Unauthorized.aspx");
            }

            if (!IsPostBack)
            {
                LoadCourses(); // Load courses for the teacher on the first page load
            }
        }

        private void LoadCourses()
        {
            int teacherId = Convert.ToInt32(Session["UserID"]); // Get the teacher's UserID from the session

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT c.CourseID, c.CourseName FROM Courses c " +
                               "INNER JOIN CourseTeachers ct ON c.CourseID = ct.CourseID " +
                               "WHERE ct.TeacherID = @TeacherID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TeacherID", teacherId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                CourseDropDown.DataSource = reader;
                CourseDropDown.DataTextField = "CourseName";
                CourseDropDown.DataValueField = "CourseID";
                CourseDropDown.DataBind();

                // Add a default "Select a Course" option
                CourseDropDown.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select a Course --", "0"));
            }
        }

        protected void CourseDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            int courseId = Convert.ToInt32(CourseDropDown.SelectedValue);

            if (courseId > 0)
            {
                LoadStudents(courseId);
            }
            else
            {
                StudentsGridView.DataSource = null;
                StudentsGridView.DataBind();
            }
        }

        private void LoadStudents(int courseId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        s.StudentID,
                        s.FirstName,
                        s.LastName,
                        e.Grade,
                        e.CourseID
                    FROM 
                        Enrollments e
                    INNER JOIN 
                        Students s ON e.StudentID = s.StudentID
                    WHERE 
                        e.CourseID = @CourseID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", courseId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable studentsTable = new DataTable();

                conn.Open();
                adapter.Fill(studentsTable);

                StudentsGridView.DataSource = studentsTable;
                StudentsGridView.DataBind();
            }
        }

        protected void UpdateGrade_Command(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateGrade")
            {
                string[] args = e.CommandArgument.ToString().Split(',');
                int studentId = int.Parse(args[0]);
                int courseId = int.Parse(args[1]);

                GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                TextBox gradeTextBox = (TextBox)row.FindControl("GradeTextBox");
                string grade = gradeTextBox.Text;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Enrollments SET Grade = @Grade WHERE StudentID = @StudentID AND CourseID = @CourseID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Grade", grade);
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.Parameters.AddWithValue("@CourseID", courseId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                Response.Write("<script>alert('Grade updated successfully');</script>");

                // Refresh the grid to reflect the updated grade
                LoadStudents(courseId);
            }
        }
    }
}