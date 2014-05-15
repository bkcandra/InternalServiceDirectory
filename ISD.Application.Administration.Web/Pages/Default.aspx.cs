using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ISD.Util;

namespace ISD.Administration.Web.Pages
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckSignIn();

                hlnkNewPage.NavigateUrl = "~/Pages/PageSetup.aspx?" + SystemConstants.PageType + "=1";
            
            }

        }

        private void CheckSignIn()
        {
            if (!Context.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/login.aspx");
            }
        }
    }
}