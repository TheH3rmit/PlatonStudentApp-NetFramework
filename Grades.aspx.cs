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
    public partial class Grades : System.Web.UI.Page
    {
        private string connectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            if (Session["StudentID"] == null)
            {
                // If no student is logged in, redirect to login
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadGrades(); // Load grades on first page load
            }
        }

        private void LoadGrades()
        {
            int studentId = Convert.ToInt32(Session["StudentID"]); // Get the logged-in student's ID

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Query to get grades and course information for the logged-in student
                string query = @"
                    SELECT 
                        c.CourseName,
                        e.Grade,
                        e.EnrollmentDate
                    FROM 
                        Enrollments e
                    INNER JOIN 
                        Courses c ON e.CourseID = c.CourseID
                    WHERE 
                        e.StudentID = @StudentID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentID", studentId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable gradesTable = new DataTable();

                conn.Open();
                adapter.Fill(gradesTable);

                GradesGridView.DataSource = gradesTable; // Bind data to the GridView
                GradesGridView.DataBind(); // Refresh the GridView
            }
        }
    }
}