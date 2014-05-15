using ISD.DA;
using ISD.EDS;
using ISD.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI;
using System.Web.UI.WebControls;

namespace ISD.Administration.Web.Council
{
    public partial class CouncilSetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckSignIn();
                if (Request.QueryString[SystemConstants.CouncilID] != null)
                {
                    int councilID = Convert.ToInt32(Request.QueryString[SystemConstants.CouncilID]);
                    var dac = new DataAccessComponent();
                    DataSetComponent.CouncilRow dr = null;

                    if (councilID != 0)
                    {
                        dr = dac.RetrieveCouncil(councilID);
                        CouncilSetupUC.Mode = SystemConstants.FormMode.Edit;
                        CouncilSetupUC.SetDDL();
                        CouncilSetupUC.SetData(dr);
                    }
                    else
                    {                        
                        CouncilSetupUC.Mode = SystemConstants.FormMode.New;
                    }
                }
               
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