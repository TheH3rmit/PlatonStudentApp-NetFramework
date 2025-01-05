using System;
using System.Web.UI;

namespace PlatonStudentApp
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the user is logged in
            if (Session["Role"] != null)
            {
                // Display "Logged in as" information
                LoggedInInfo.Visible = true;
                LoggedInUserLabel.Text = Session["Username"] != null
                    ? Session["Username"].ToString() // Explicitly convert Session["Username"] to string
                    : "Unknown User"; // Fallback for missing username
            }
            else
            {
                // Hide "Logged in as" information for guests
                LoggedInInfo.Visible = false;
            }

            // Role-based navigation visibility
            AdminLinks.Visible = (Session["Role"] != null && Session["Role"].ToString() == "Admin");
            StudentLinks.Visible = (Session["Role"] != null && Session["Role"].ToString() == "Student");
            TeacherLinks.Visible = (Session["Role"] != null && Session["Role"].ToString() == "Teacher");
        }
    }
}
