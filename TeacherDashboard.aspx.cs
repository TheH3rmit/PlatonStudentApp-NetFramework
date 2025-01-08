using PlatonStudentApp.BusinessLogic;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PlatonStudentApp
{
    public partial class TeacherDashboard : System.Web.UI.Page
    {
        private TeacherService teacherService;

        protected void Page_Load(object sender, EventArgs e)
        {
            teacherService = new TeacherService();

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
            int teacherId = Convert.ToInt32(Session["UserID"]);
            CourseDropDown.DataSource = teacherService.GetCoursesForTeacher(teacherId);
            CourseDropDown.DataTextField = "CourseName";
            CourseDropDown.DataValueField = "CourseID";
            CourseDropDown.DataBind();

            // Add a default "Select a Course" option
            CourseDropDown.Items.Insert(0, new ListItem("-- Select a Course --", "0"));
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
                MessageLabel.Text = ""; // Clear the message label
            }
        }

        private void LoadStudents(int courseId)
        {
            StudentsGridView.DataSource = teacherService.GetStudentsForCourse(courseId);
            StudentsGridView.DataBind();
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

                bool success = teacherService.UpdateStudentGrade(studentId, courseId, parsedGrade);

                if (success)
                {
                    Session["Message"] = "Grade updated successfully.";
                    Session["MessageType"] = "Success";
                }
                else
                {
                    Session["Message"] = "Error updating grade.";
                    Session["MessageType"] = "Error";
                }

                // Redirect to avoid form resubmission
                Response.Redirect(Request.Url.AbsoluteUri);
            }
        }
    }
}
