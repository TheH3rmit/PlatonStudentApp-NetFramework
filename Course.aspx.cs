using PlatonStudentApp.BusinessLogic;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PlatonStudentApp
{
    public partial class Course : System.Web.UI.Page
    {
        private CourseService courseService;

        protected void Page_Load(object sender, EventArgs e)
        {
            courseService = new CourseService();

            // Ensure only Admins or Teachers can access this page
            if (Session["Role"] == null || (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "Teacher"))
            {
                Response.Redirect("Unauthorized.aspx");
            }

            if (!IsPostBack)
            {
                LoadCourses();
                LoadTeachers();

                // Display message from Session if it exists
                if (Session["Message"] != null)
                {
                    MessageLabel.Text = Session["Message"].ToString();
                    MessageLabel.ForeColor = System.Drawing.Color.Green;
                    Session.Remove("Message");
                }
            }
        }

        private void LoadCourses()
        {
            CoursesGridView.DataSource = courseService.GetAllCourses();
            CoursesGridView.DataBind();
        }

        private void LoadTeachers()
        {
            TeacherDropDown.DataSource = courseService.GetTeachers();
            TeacherDropDown.DataTextField = "FullName";
            TeacherDropDown.DataValueField = "UserID";
            TeacherDropDown.DataBind();

            TeacherDropDown.Items.Insert(0, new ListItem("-- Select a Teacher --", "0"));
        }

        protected void AddCourseButton_Click(object sender, EventArgs e)
        {
            try
            {
                courseService.AddCourse(
                    CourseNameTextBox.Text.Trim(),
                    CourseDescriptionTextBox.Text.Trim(),
                    int.Parse(CourseCreditsTextBox.Text.Trim()),
                    DateTime.Parse(StartDateTextBox.Text.Trim()),
                    DateTime.Parse(EndDateTextBox.Text.Trim()),
                    int.Parse(TeacherDropDown.SelectedValue)
                );

                Session["Message"] = "Course added successfully!";
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch (Exception ex)
            {
                MessageLabel.ForeColor = System.Drawing.Color.Red;
                MessageLabel.Text = "An error occurred while adding the course.";
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
            }
        }

        protected void CoursesGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            CoursesGridView.EditIndex = e.NewEditIndex;
            LoadCourses();
        }

        protected void CoursesGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            CoursesGridView.EditIndex = -1;
            LoadCourses();
        }

        protected void CoursesGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = CoursesGridView.Rows[e.RowIndex];

            courseService.UpdateCourse(
                Convert.ToInt32(CoursesGridView.DataKeys[e.RowIndex].Value),
                ((TextBox)row.Cells[1].Controls[0]).Text.Trim(),
                ((TextBox)row.Cells[2].Controls[0]).Text.Trim(),
                int.Parse(((TextBox)row.Cells[3].Controls[0]).Text.Trim()),
                DateTime.Parse(((TextBox)row.Cells[4].Controls[0]).Text.Trim()),
                DateTime.Parse(((TextBox)row.Cells[5].Controls[0]).Text.Trim()),
                int.Parse(((DropDownList)row.FindControl("TeacherDropDown")).SelectedValue)
            );

            CoursesGridView.EditIndex = -1;
            LoadCourses();

            MessageLabel.ForeColor = System.Drawing.Color.Green;
            MessageLabel.Text = "Course updated successfully!";
        }

        protected void CoursesGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            courseService.DeleteCourse(Convert.ToInt32(CoursesGridView.DataKeys[e.RowIndex].Value));

            LoadCourses();

            MessageLabel.ForeColor = System.Drawing.Color.Green;
            MessageLabel.Text = "Course deleted successfully!";
        }
    }
}
