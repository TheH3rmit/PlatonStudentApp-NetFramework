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
            if (Session["Username"] != null)
            {
                LoggedInUserLabel.Text = Session["Username"].ToString();
            }
            else
            {
                LoggedInUserLabel.Text = "Guest";
            }
        }
    }
}