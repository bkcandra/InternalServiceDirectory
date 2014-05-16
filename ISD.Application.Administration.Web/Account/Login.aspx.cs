using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ISD.Administration.Web.Models;
using ISD.Util;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ISD.Administration.Web.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //RegisterHyperLink.NavigateUrl = "Register";
            // Enable this once you have account confirmation enabled for password reset functionality
            // ForgotPasswordHyperLink.NavigateUrl = "Forgot";
            //OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
            //var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            //if (!String.IsNullOrEmpty(returnUrl))
            //{
            //    //RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            //}
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Validate the user password
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                ApplicationUser user = manager.Find(Email.Text, Password.Text);
                if (user != null)
                {
                    if (manager.IsInRole(user.Id, SystemConstants.AdministratorRole))
                    {
                        IdentityHelper.SignIn(manager, user, remember_me.Checked);
                        IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                    }
                    else
                    {
                        FailureText.Text = "User doesn't have permission to log in, please contact Healthy Club Administration for further information.";
                        ErrorMessage.Visible = true;
                    }
                }
                else
                {
                    FailureText.Text = "Invalid username or password.";
                    ErrorMessage.Visible = true;
                }
            }
        }
    }
}