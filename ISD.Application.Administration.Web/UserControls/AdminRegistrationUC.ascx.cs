using ISD.Administration.Web;
using ISD.Administration.Web.Models;
using ISD.Data.EDM;
using ISD.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;


namespace HealthyClub.Administration.Web.UserControls
{
    public partial class AdminRegistrationUC : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;
            bool isPasswordWeak = true;
            CheckPasswordStrength(out isPasswordWeak);
            if (!isPasswordWeak)
            {
                if (!string.IsNullOrEmpty(txtEmail.Text) && !string.IsNullOrEmpty(txtPassword2.Text) && !string.IsNullOrEmpty(txtUsername.Text))
                {
                    var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    ApplicationUser usr = new ApplicationUser() { UserName = txtUsername.Text, Email = txtEmail.Text };
                    IdentityResult result = manager.Create(usr, txtPassword.Text);
                    manager.GenerateEmailConfirmationToken(usr.Id);

                    var token = manager.GenerateEmailConfirmationToken(usr.Id);

                    RoleManager<IdentityRole> rm =
                        new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ISDEntities()));
                    if (!rm.RoleExists(SystemConstants.AdministratorRole))
                        rm.Create(new IdentityRole(SystemConstants.AdministratorRole));

                    manager.AddToRoleAsync(usr.Id, SystemConstants.AdministratorRole);

                    Response.Redirect("~");

                }
            }
            else
            {
                lblError.Visible = true;
            }
        }

        private void CheckPasswordStrength(out bool isPasswordWeak)
        {
            string Password = txtPassword.Text;
            if (Password.Length >= SystemConstants.MaxRequiredPasswordLength)
            {
                isPasswordWeak = true;
                lblError.Text = "Maximum password length exceeded, Maximum Password Length is " + SystemConstants.MaxRequiredPasswordLength + " Characters";
            }
            else if (Password.Length <= SystemConstants.MinRequiredPasswordLength)
            {
                isPasswordWeak = true;
                lblError.Text = "Password is too weak, Minimum Password Length is " + SystemConstants.MinRequiredPasswordLength + " Characters";
            }

            else { isPasswordWeak = false; }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~");
        }
    }
}