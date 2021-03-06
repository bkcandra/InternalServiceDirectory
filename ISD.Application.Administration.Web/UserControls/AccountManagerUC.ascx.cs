﻿using ISD.Administration.Web.Report;
using ISD.Administration.Web;
using ISD.Administration.Web.Models;
using ISD.BF;
using ISD.DA;
using ISD.EDS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ISD.Util;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace ISD.Administration.Web.UserControls
{
    public partial class AccountManagerUC : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {

        }

        protected void lnkUserRef_Click(object sender, EventArgs e)
        {
            BusinessFunctionComponent bfc = new BusinessFunctionComponent();
            DataAccessComponent dac = new DataAccessComponent();

            var userRefHash = new HashSet<String>(dac.RetrieveUserReferences().Select(x => x.UserId));
            var providers = dac.RetrieveAllproviders();
            var users = dac.RetrieveUserProfiles();

            var identityUsers = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //RoleManager<IdentityRole> rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            //var identityProviders = identityUsers.Where(x => providers.Select(y => y.UserID).Contains(x.Id));
            //var identityCustomer = identityUsers.Where(x => users.Select(y => y.UserID).Contains(x.Id));

            foreach (var user in providers)
            {
                identityUsers.AddToRole(user.UserID, SystemConstants.ProviderRole);
            }
            foreach (var user in users)
            {
                identityUsers.AddToRole(user.UserID, SystemConstants.CustomerRole);
            }

           

            int providerNotValidCount = 0;
            int userNotValidCount = 0;

            if (userRefHash != null && userRefHash.Count != 0)
            {

                var invalidProvs = providers.Where(x => !userRefHash.Contains(x.UserID));

                providerNotValidCount = invalidProvs.Count();

                foreach (var invalidProv in invalidProvs)
                {
                    var drRef = new DataSetComponent.UserReferenceDataTable().NewUserReferenceRow();
                    drRef.UserId = invalidProv.UserID;
                    drRef.ReferenceID = bfc.GenerateUserRefID(invalidProv.ProviderName);
                    dac.insertNewUserReference(drRef);
                }

                var invalidUsers = users.Where(x => !userRefHash.Contains(x.UserID));
                userNotValidCount = invalidUsers.Count();

                foreach (var invalidUser in invalidUsers)
                {
                    var drRef = new DataSetComponent.UserReferenceDataTable().NewUserReferenceRow();
                    drRef.UserId = invalidUser.UserID;
                    drRef.ReferenceID = bfc.GenerateUserRefID(invalidUser.LastName, invalidUser.FirstName);
                    dac.insertNewUserReference(drRef);
                }
                
            }
            else
            {
                foreach (var provider in providers)
                {
                    var drRef = new DataSetComponent.UserReferenceDataTable().NewUserReferenceRow();
                    drRef.UserId = provider.UserID;
                    drRef.ReferenceID = bfc.GenerateUserRefID(provider.ProviderName);
                    dac.insertNewUserReference(drRef);
                }
                providerNotValidCount = providers.Count;

                foreach (var user in users)
                {
                    var drRef = new DataSetComponent.UserReferenceDataTable().NewUserReferenceRow();
                    drRef.UserId = user.UserID;
                    drRef.ReferenceID = bfc.GenerateUserRefID(user.LastName, user.FirstName);
                    dac.insertNewUserReference(drRef);
                }
                userNotValidCount = users.Count;
            }



            if (userNotValidCount == 0 && providerNotValidCount == 0)
                lblStatus.Text = "User reference is checked, No user reference created.";
            else
            {
                lblStatus.Text = "User reference is checked, " + providerNotValidCount + " provider and " + userNotValidCount + "user reference are created";
            }
            lblStatus.Visible = true;

        }
    }
}