using ISD.Administration.Web;
using ISD.Administration.Web.Models;
using ISD.DA;
using ISD.Data.EDM;
using ISD.EDS;
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
    public partial class AdminList : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Refresh();
            }
        }

        private void Refresh()
        {
            SetDataSource();
        }

        private void SetDataSource()
        {

            //var um = Context.GetOwinContext().GetUserManager<ApplicationUserManager>().Users;
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ISDEntities())).FindByName(SystemConstants.AdministratorRole).Users;
            var dt = rm;

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            LinkButton lnkDelete = e.CommandSource as LinkButton;
            GridViewRow row = lnkDelete.Parent.Parent as GridViewRow;


            Label lblUserName = row.FindControl("lblUserName") as Label;

            if (e.CommandName == "DeleteUser")
            {
              
                var mgr = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

                ApplicationUser usr = mgr.FindByName(lblUserName.Text);
                mgr.Delete(usr);

                Refresh();

            }
            Refresh();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}