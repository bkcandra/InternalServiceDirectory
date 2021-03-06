﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ISD.Provider.Web;
using ISD.Util;

using System.IO;
using HealthyClub.Providers.Web.Report;
using ISD.DA;
using ISD.EDS;
using System.Net;
using HealthyClub.Providers.Web;



namespace HealthyClub.Providers.Web.UserControls
{
    public partial class ReportUC : System.Web.UI.UserControl
    {
        public String ProviderID
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnProviderID.Value))
                    return hdnProviderID.Value;
                else return Guid.Empty.ToString();
            }
            set
            {
                hdnProviderID.Value = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetFilter();
            }
        }

        private void SetFilter()
        {
            //setting up categories
            DataSetComponent.CategoryDataTable dtCat = new DataAccessComponent().RetrieveLv0Categories();
            ddlCategories.Items.Clear();

            ListItem liCat = new ListItem("Choose Category", "0");
            ddlCategories.Items.Add(liCat);

            foreach (var dr in dtCat)
            {
                string name = "";
                if (dr.Name != "" && !dr.IsNameNull())
                    name = dr.Name;

                liCat = new ListItem(name, dr.ID.ToString());
                ddlCategories.Items.Add(liCat);
            }
            ddlCategories.SelectedValue = "0";

            //setting up post code
            DataSetComponent.v_SuburbExplorerDataTable dtSuburb = new DataAccessComponent().RetrieveSuburbs();

            ddlSuburbs.Items.Clear();

            ListItem liSub = new ListItem("Suburb", "0");
            ddlSuburbs.Items.Add(liSub);
            foreach (var dr in dtSuburb)
            {
                string name = "";

                if (dr.ID != 0)
                    name = dr.Name;


                liSub = new ListItem(name, dr.ID.ToString());
                ddlSuburbs.Items.Add(liSub);
            }
        }

        protected void Page_init(object sender, EventArgs e)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                
                ProviderID = IdentityHelper.GetUserIdFromRequest(Request);
            }
            else
            {
                Response.Redirect("~/Account/Login.aspx");
            }
        }

        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            GenerateSessionReport();
            Response.Redirect("List.aspx");


            //SaveAsPDF();
        }

        private void SaveAsPDF()
        {
            /* Uri uri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
             string url = uri.AbsoluteUri.Remove(uri.AbsoluteUri.Length - uri.Segments.Last().Length) + "list.aspx";
             string Reporthtml = "";
             Uri html = new Uri(url);
             using (WebClient client = new WebClient())
             {
                 Reporthtml = client.DownloadString(html);
             }

             //byte[] pdfBuf = new Pechkin.Synchronized.SynchronizedPechkin(new Pechkin.GlobalConfig()).Convert(Reporthtml);

             Response.Clear();
             Response.ContentType = "application/pdf";
             Response.AppendHeader("Content-Disposition", "inline;filename=data.pdf");
             Response.BufferOutput = true;
             Response.AddHeader("Content-Length", pdfBuf.Length.ToString());
             Response.BinaryWrite(pdfBuf);
             Response.End();*/
        }

        private void GenerateSessionReport()
        {
            int timetableFormat = 0;
            //NOT USED
            //timetableFormat = (int)SystemConstants.TimetableFormat.Weekly;

            ReportParameterPasser parameters = ReportParameterPasser.GetCurrentParameters();
            parameters.UseTimetable = radWithTimetable.Checked;
            parameters.Title = txtTitle.Text;
            parameters.NameVisible = chkName.Checked;
            parameters.WebsiteVisible = chkWebsite.Checked;
            parameters.ShortDescriptionVisible = chkShortDescription.Checked;
            parameters.AddressVisible = chkAddress.Checked;
            parameters.PriceVisible = chkPrice.Checked;
            parameters.EligibilityVisible = chkEligibility.Checked;
            parameters.TimetableFormat = timetableFormat;
            parameters.ProviderID = ProviderID;
            parameters.CustomReport = false;
            if (!string.IsNullOrEmpty(txtAgeFrom.Text) || !string.IsNullOrEmpty(txtAgeTo.Text))
            {
                parameters.CustomReport = true;
                if (txtAgeFrom.Text != "__")
                    parameters.AgeFrom = Convert.ToInt32(txtAgeFrom.Text);
                if (txtAgeTo.Text != "__")
                    parameters.AgeTo = Convert.ToInt32(txtAgeTo.Text);
            }
            if (!string.IsNullOrEmpty(ddlCategory2.SelectedValue) && !string.IsNullOrEmpty(ddlCategories.SelectedValue))
            {
                if (ddlCategories.SelectedValue != "0" || ddlCategory2.SelectedValue != "0")
                {
                    parameters.CustomReport = true;
                    if (ddlCategory2.SelectedValue != "0")
                    {
                        parameters.CategoryID = Convert.ToInt32(ddlCategory2.SelectedValue);
                    }
                    else if (ddlCategories.SelectedValue != "0")
                    {
                        parameters.CategoryID = Convert.ToInt32(ddlCategories.SelectedValue);
                    }
                }
            }
            if (ddlSuburbs.SelectedValue != "0")
            {
                parameters.CustomReport = true;
                parameters.PostCode = Convert.ToInt32(ddlSuburbs.SelectedValue);
            }
        }

        protected void ddlCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSetComponent.CategoryDataTable dt = new DataAccessComponent().RetrieveLv1Categories(Convert.ToInt32(ddlCategories.SelectedValue));
            ddlCategory2.Items.Clear();
            ddlCategory2.Visible = false;
            if (dt.Count() != 0)
            {
                ddlCategory2.Visible = true;
                ddlCategory2.Items.Clear();

                ListItem li = new ListItem("Choose Category", "0");
                ddlCategory2.Items.Add(li);

                foreach (var dr in dt)
                {
                    string name = "";
                    if (dr.Name != "" && !dr.IsNameNull())
                        name = dr.Name;

                    li = new ListItem(name, dr.ID.ToString());
                    ddlCategory2.Items.Add(li);
                }
                ddlCategory2.SelectedValue = "0";
            }
            else
                ddlCategory2.Visible = false;
        }

    }

}