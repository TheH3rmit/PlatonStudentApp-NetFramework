using System.Web;
using System.Web.UI;

namespace PlatonStudentApp
{
    public static class Helper
    {
        public static void RedirectToSelf(Page page)
        {
            // Redirect to the same URL to prevent form resubmission
            page.Response.Redirect(page.Request.Url.AbsoluteUri, false);
        }
    }
}
