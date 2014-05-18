using ISD.DA;
using ISD.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;


namespace HealthyClub.Provider.Web.UserControls
{
    public partial class VisitorStatsUC : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {

            if (Context.User.Identity.IsAuthenticated)
                hdnProviderGUID.Value = Context.User.Identity.GetUserId();

            else
                Response.Redirect("~/Account/Login.aspx");


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Refresh();
            }
        }

        private void Refresh()
        {
            DataAccessComponent dac = new DataAccessComponent();
            lblRange.Text = DateTime.Now.AddYears(-1).ToShortDateString() + " - " + DateTime.Now.AddYears(-1).ToShortDateString();
            lblPageViews.Text = dac.RetrieveProviderVisitorCount(hdnProviderGUID.Value, DateTime.Now.AddYears(-1),DateTime.Now).ToString();
        }



    }
}