using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PlatonStudentApp
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the user is logged in
            if (Session["Role"] != null)
            {
                // Set the "Logged in as" information
                LoggedInInfo.Visible = true; // Show the LoggedInInfo PlaceHolder
                LoggedInUserLabel.Text = Session["Username"] != null
                    ? Session["Username"].ToString()
                    : "Unknown User"; // Fallback for missing username
            }
            else
            {
                // Hide the "Logged in as" information for guests
                LoggedInInfo.Visible = false;
            }

            // Role-based visibility for menu links
            AdminLinks.Visible = (Session["Role"] != null && Session["Role"].ToString() == "Admin");
            StudentLinks.Visible = (Session["Role"] != null && Session["Role"].ToString() == "Student");
        }
    }
}