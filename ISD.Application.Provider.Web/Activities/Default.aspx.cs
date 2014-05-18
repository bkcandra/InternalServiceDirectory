using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ISD.DA;
using Microsoft.AspNet.Identity;

namespace HealthyClub.Providers.Web.Activities
{
    public partial class Default : System.Web.UI.Page
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

                ActivityManagementUC1.ProviderID = Context.User.Identity.GetUserId();

            }
            else
            {
                Response.Redirect("~/Account/Login.aspx");
            }
        }
    }
}