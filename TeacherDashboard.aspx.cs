using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

            // Ensure only teachers can access this page
            if (Session["Role"] == null || Session["Role"].ToString() != "Teacher")
            {
                Response.Redirect("Unauthorized.aspx");
            }

            if (!IsPostBack)
            {
                LoadCourses(); // Load the teacher's courses on the first page load

                // Display message from Session if it exists
                if (Session["Message"] != null)
                {
                    if (Session["MessageType"] != null && Session["MessageType"].ToString() == "Success")
                    {
                        MessageLabel.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        MessageLabel.ForeColor = System.Drawing.Color.Red;
                    }

                    MessageLabel.Text = Session["Message"].ToString();
                    Session.Remove("Message");
                    Session.Remove("MessageType");
                }
            }
        }

        private void LoadCourses()
        {
            int teacherId = Convert.ToInt32(Session["UserID"]); // Get the teacher's UserID from the session

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT CourseID, CourseName 
                    FROM Courses 
                    WHERE TeacherID = @TeacherID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TeacherID", teacherId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                CourseDropDown.DataSource = reader;
                CourseDropDown.DataTextField = "CourseName";
                CourseDropDown.DataValueField = "CourseID";
                CourseDropDown.DataBind();

                // Add a default "Select a Course" option
                CourseDropDown.Items.Insert(0, new ListItem("-- Select a Course --", "0"));
            }
        }

        protected void CourseDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            int courseId = Convert.ToInt32(CourseDropDown.SelectedValue);

            if (courseId > 0)
            {
                LoadStudents(courseId); // Load students for the selected course
            }
            else
            {
                StudentsGridView.DataSource = null;
                StudentsGridView.DataBind();
                MessageLabel.Text = ""; // Clear the message label
            }
        }

        private void LoadStudents(int courseId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        e.StudentID,
                        u.FirstName,
                        u.LastName,
                        e.Grade,
                        e.CourseID
                    FROM 
                        Enrollments e
                    INNER JOIN 
                        Users u ON e.StudentID = u.UserID
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
                string grade = gradeTextBox.Text.Trim();

                // Validate the grade
                if (string.IsNullOrEmpty(grade))
                {
                    Session["Message"] = "Grade cannot be empty.";
                    Session["MessageType"] = "Error";
                    Response.Redirect(Request.Url.AbsoluteUri);
                    return;
                }

                if (!decimal.TryParse(grade, out decimal parsedGrade) || parsedGrade < 0 || parsedGrade > 100)
                {
                    Session["Message"] = "Grade must be a number between 0 and 100.";
                    Session["MessageType"] = "Error";
                    Response.Redirect(Request.Url.AbsoluteUri);
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = "UPDATE Enrollments SET Grade = @Grade WHERE StudentID = @StudentID AND CourseID = @CourseID";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Grade", parsedGrade);
                        cmd.Parameters.AddWithValue("@StudentID", studentId);
                        cmd.Parameters.AddWithValue("@CourseID", courseId);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }

                    Session["Message"] = "Grade updated successfully.";
                    Session["MessageType"] = "Success";
                }
                catch (Exception ex)
                {
                    Session["Message"] = "Error updating grade: " + ex.Message;
                    Session["MessageType"] = "Error";
                }

                // Redirect to the same page to avoid form resubmission
                Response.Redirect(Request.Url.AbsoluteUri);
            }
        }
    }
}
