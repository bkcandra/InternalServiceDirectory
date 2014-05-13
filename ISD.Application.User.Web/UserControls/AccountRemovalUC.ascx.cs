using ISD.DA;
using ISD.User.Web;
using ISD.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ISD.User.Customer.Web.UserControls
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

            // IdentityExtensions.GetUserId()
            // Guid userID = dac.RetrieveUserGUID(IdentityHelper..GetProviderNameFromRequest(Request););
            var drP = dac.RetrieveUserProfiles(IdentityHelper.UserIdKey);

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
                Response.Redirect(SystemConstants.ProviderUrl + "Account");
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

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            string usr = IdentityHelper.GetProviderNameFromRequest(Request); ;
            if (!string.IsNullOrEmpty(txtPassword.Text))
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await manager.FindAsync(Context.User.Identity.Name, txtPassword.Text);
                if (user != null)
                {
                    DataAccessComponent dac = new DataAccessComponent();
                    String userID = dac.RetrieveUserGuid(usr);
                    string err = "";
                    if (dac.DeactivateUser(usr, userID, out err))
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