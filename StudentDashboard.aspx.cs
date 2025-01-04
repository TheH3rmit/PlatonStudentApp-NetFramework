using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PlatonStudentApp
{
    public partial class StudentDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void CoursesGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Enroll")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = CoursesGridView.Rows[rowIndex];

                int courseId = Convert.ToInt32(row.Cells[0].Text); // Get CourseID

                using (SqlConnection conn = new SqlConnection("YourConnectionString"))
                {
                    string query = "INSERT INTO Enrollments (StudentID, CourseID) VALUES (@StudentID, @CourseID)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StudentID", Session["StudentID"]); // Get StudentID from session
                    cmd.Parameters.AddWithValue("@CourseID", courseId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}