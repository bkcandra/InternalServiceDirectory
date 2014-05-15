using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ISD.Data.EDM;
using ISD.Util;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using ISD.Administration.Web.Models;
using Page = System.Web.UI.Page;

namespace ISD.Administration.Web.Account
{
    public partial class Register : Page
    {
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = new ApplicationUser() { UserName = Email.Text, Email = Email.Text };
            IdentityResult result = manager.Create(user, Password.Text);
            if (result.Succeeded)
            {
                IdentityHelper.SignIn(manager, user, isPersistent: false);

                RoleManager<IdentityRole> rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
                if (!rm.RoleExists(SystemConstants.AdministratorRole))
                    rm.Create(new IdentityRole(SystemConstants.AdministratorRole));
                manager.AddToRole(user.Id, SystemConstants.AdministratorRole);

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                string code = manager.GenerateEmailConfirmationToken(user.Id);
                string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id);
                manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            }
            else
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }
    }
}