using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ISD.BF;
using ISD.EDS;
using ISD.DA;
using ISD.Util;


namespace ISD.Administration.Web.Settings
{
    public partial class Navigation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckSignIn();
            }

        }

        private void CheckSignIn()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/login.aspx");
            }
        }
    }



}