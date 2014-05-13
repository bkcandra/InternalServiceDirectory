using System.Data.Entity;
using ISD.Administration.Web;
using ISD.Data.EDM;
using ISD.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace HealthyClub.Administration.Web.UserControls
{
    public partial class AdminLoginUC : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        }

        protected void LoginUser_Authenticate(object sender, AuthenticateEventArgs e)
        {
            var rolemanager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ISDEntities()));
            var um = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            if (um.IsInRole(um.FindByName(LoginUser.UserName).Id, SystemConstants.AdministratorRole))
                e.Authenticated = true;
            else
            {
                e.Authenticated = false;
                LoginUser.FailureText = "Incorrect Username or Password for Administrator Account";
            }
        }
    }
}