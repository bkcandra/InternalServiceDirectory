﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace HealthyClub.Administration.Web.Keyword
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
            CheckSignIn();
            }

        }

        private void CheckSignIn()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/login.aspx");
            }
        }

        protected void lnkNewKeyword_Click(object sender, EventArgs e)
        {
            KeywordManagementUC1.newKey();
        }
    }
}