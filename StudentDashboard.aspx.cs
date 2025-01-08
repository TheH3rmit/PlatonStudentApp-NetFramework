using PlatonStudentApp.BusinessLogic;
using System;
using System.Web.UI.WebControls;

namespace PlatonStudentApp
{
    public partial class StudentDashboard : System.Web.UI.Page
    {
        private EnrollmentService enrollmentService;

        protected void Page_Load(object sender, EventArgs e)
        {
            enrollmentService = new EnrollmentService();

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

        private void LoadAvailableCourses()
        {
            int studentId = Convert.ToInt32(Session["StudentID"]);
            CoursesGridView.DataSource = enrollmentService.GetAvailableCoursesForStudent(studentId);
            CoursesGridView.DataBind();
        }

        private void LoadEnrolledCoursesWithGrades()
        {
            int studentId = Convert.ToInt32(Session["StudentID"]);
            EnrolledCoursesGridView.DataSource = enrollmentService.GetEnrolledCoursesWithGrades(studentId);
            EnrolledCoursesGridView.DataBind();
        }

        protected void CoursesGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Enroll")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = CoursesGridView.Rows[rowIndex];

                int courseId = Convert.ToInt32(row.Cells[0].Text);
                int studentId = Convert.ToInt32(Session["StudentID"]);

                bool success = enrollmentService.EnrollStudentInCourse(studentId, courseId);

                if (success)
                {
                    Session["Message"] = "Successfully enrolled in the course.";
                    Session["MessageType"] = "Success";
                }
                else
                {
                    Session["Message"] = "You are already enrolled in this course.";
                    Session["MessageType"] = "Error";
                }

                Response.Redirect(Request.Url.AbsoluteUri);
            }
        }

        protected void EnrolledCoursesGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Unenroll")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = EnrolledCoursesGridView.Rows[rowIndex];

                int courseId = Convert.ToInt32(row.Cells[0].Text);
                int studentId = Convert.ToInt32(Session["StudentID"]);

                bool success = enrollmentService.UnenrollStudentFromCourse(studentId, courseId);

                if (success)
                {
                    Session["Message"] = "Successfully unenrolled from the course.";
                    Session["MessageType"] = "Success";
                }
                else
                {
                    Session["Message"] = "Error: Could not unenroll from the course.";
                    Session["MessageType"] = "Error";
                }

                Response.Redirect(Request.Url.AbsoluteUri);
            }
        }
    }
}
