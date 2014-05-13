using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ISD.Util;
using ISD.DA;
using ISD.EDS;


namespace HealthyClub.Administration.Web.Suburb
{
    public partial class SuburbSetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString[SystemConstants.SuburbID] != null)
                {
                    int suburbID = Convert.ToInt32(Request.QueryString[SystemConstants.SuburbID]);
                    var dac = new DataAccessComponent();
                    DataSetComponent.v_SuburbExplorerRow dr = null;

                    if (suburbID != 0)
                    {
                        dr = dac.RetrieveSuburb(suburbID);
                        SuburbSetupUC1.Mode = SystemConstants.FormMode.Edit;
                        SuburbSetupUC1.SetData(dr);
                    }
                    else
                    {
                        dr = new DataSetComponent.v_SuburbExplorerDataTable().Newv_SuburbExplorerRow();
                        SuburbSetupUC1.Mode = SystemConstants.FormMode.New;
                    }
                }
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