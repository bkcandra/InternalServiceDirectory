﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ISD.Util;
using ISD.EDS;
using ISD.DA;


namespace ISD.Administration.Web.Category
{
    public partial class CategoriesSetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString[SystemConstants.CategoryID] != null)
                {

                    int categoryID = Convert.ToInt32(Request.QueryString[SystemConstants.CategoryID]);


                    var dac = new DataAccessComponent();

                    DataSetComponent.CategoryRow dr = null;

                    if (categoryID != 0)
                    {
                        dr = dac.RetrieveCategory(categoryID);

                        CategoriesSetupUC1.Mode = SystemConstants.FormMode.Edit;
                    }
                    else
                    {
                        dr = new DataSetComponent.CategoryDataTable().NewCategoryRow();
                        CategoriesSetupUC1.Mode = SystemConstants.FormMode.New;
                    }

                    CategoriesSetupUC1.SetData(dr);
                }
                CheckSignIn();
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