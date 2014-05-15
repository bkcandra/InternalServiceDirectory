using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ISD.Util;
using ISD.DA;
using ISD.EDS;


namespace ISD.Administration.Web.State
{
    public partial class StateSetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckSignIn();
                if (Request.QueryString[SystemConstants.StateID] != null)
                {
                    int stateID = Convert.ToInt32(Request.QueryString[SystemConstants.StateID]);
                    var dac = new DataAccessComponent();
                    DataSetComponent.StateRow dr = null;

                    if (stateID != 0)
                    {
                        dr = dac.RetrieveState(stateID);
                        StateSetupUC1.Mode = SystemConstants.FormMode.Edit;
                        StateSetupUC1.SetData(dr);
                    }
                    else
                    {
                        dr = new DataSetComponent.StateDataTable().NewStateRow();
                        StateSetupUC1.Mode = SystemConstants.FormMode.New;
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