using ISD.DA;
using ISD.Provider.Web;
using ISD.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;


namespace HealthyClub.Provider.Web.UserControls
{
    public partial class AccountRemovalUC : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (AuthUser())
                {
                    btnDelete.Enabled = txtPassword.Enabled = false;
                    CheckUserStatus();
                }
            }
        }

        private void CheckUserStatus()
        {
            DataAccessComponent dac = new DataAccessComponent();
            var drP = dac.RetrieveProviderProfiles(IdentityHelper.GetUserIdFromRequest(Request));

            if (drP != null)
            {
                if (drP.AccountDeletion)
                {
                    accountCancel.Visible = false;
                    CompleteConfirm.Visible = true;
                }
            }
            else
            {
                Response.Redirect(SystemConstants.CustomerUrl + "Account");
            }
        }

        private bool AuthUser()
        {
            if (!Context.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login.aspx");
                return false;
            }
            else return true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Account");
        }

        protected void chkRemoveAccount_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRemoveAccount.Checked)
                btnDelete.Enabled = txtPassword.Enabled = true;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string usr = Context.User.Identity.Name;

            if (!string.IsNullOrEmpty(txtPassword.Text))
            {

                if (Context.GetOwinContext().GetUserManager<ApplicationUserManager>().Find(usr, txtPassword.Text) != null)
                {
                    DataAccessComponent dac = new DataAccessComponent();

                    string err = "";
                    if (dac.DeactivateUser(usr, IdentityHelper.GetUserIdFromRequest(Request), out err))
                    {
                        accountCancel.Visible = false;
                        CompleteConfirm.Visible = true;
                    }
                    else
                    {
                        lblError.Visible = true;
                        if (err == "Unable to find user")
                        {
                            lblError.Text = "Unknown error, please retry login";
                        }
                    }
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Invalid password";
                }
            }
        }
    }
}