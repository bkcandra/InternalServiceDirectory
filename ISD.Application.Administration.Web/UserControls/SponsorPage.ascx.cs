using ISD.BF;
using ISD.DA;
using ISD.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace ISD.Administration.Web.UserControls
{
    public partial class SponsorPage : System.Web.UI.UserControl
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
            SetSponsorsTable();
        }

        private void SetSponsorsTable()
        {
            var Sponsors = new DataAccessComponent().RetrieveSponsorsExplorer();

            rptSponsors.DataSource = Sponsors;
            rptSponsors.DataBind();

        }
        protected void rptSponsors_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnstatus = e.Item.FindControl("hdnstatus") as HiddenField;
                Label lblStatus = e.Item.FindControl("lblStatus") as Label;
                if (hdnstatus.Value == "1")
                    lblStatus.Text = "Active";
                else
                    lblStatus.Text = "Inactive";

                HiddenField hdnSponsorID = e.Item.FindControl("hdnSponsorsID") as HiddenField;
                HyperLink hlnkSponsorName = e.Item.FindControl("hlnkSponsorName") as HyperLink;
                hlnkSponsorName.NavigateUrl = "~/Rewards/SponsorTools.aspx?" + SystemConstants.route_SponsorsID + "=" + hdnSponsorID.Value;

            }
        }

        protected void lnkSelectAll_Click(object sender, EventArgs e)
        {
            ModifyCheckbox(true, false);
        }

        protected void lnkUnselectAll_Click(object sender, EventArgs e)
        {
            ModifyCheckbox(false, false);
        }


        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            var dt = GetSelectedSponsorID();
            if (dt.Count != 0)
            {
                new BusinessFunctionComponent().DeleteSponsors(dt);
            }
            Refresh();
        }
        protected void lnkAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("SponsorTools.aspx");
        }

        protected void lnkSelectExp_Click(object sender, EventArgs e)
        {
            ModifyCheckbox(false, true);
        }

        private void ModifyCheckbox(bool selectAll, bool selectExpired)
        {
            foreach (RepeaterItem item in rptSponsors.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox chkAct = item.FindControl("chkAct") as CheckBox;

                    HiddenField hdnRewardsID = item.FindControl("hdnSPonsorsID") as HiddenField;
                    Label lblStatus = item.FindControl("lblStatus") as Label;

                    chkAct.Checked = selectAll;
                    if (selectExpired && lblStatus.Text == SystemConstants.ActivityStatus.Expired.ToString())
                        chkAct.Checked = selectExpired;
                }
            }
        }
        private List<String> GetSelectedSponsorID()
        {
            List<String> Selected = new List<String>();

            foreach (RepeaterItem item in rptSponsors.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox chkAct = item.FindControl("chkAct") as CheckBox;
                   
                    HiddenField hdnSponsorsID = item.FindControl("hdnSponsorsID") as HiddenField;
                    

                    if (chkAct.Checked)
                    {
                        String ID = hdnSponsorsID.Value;
                        Selected.Add(ID);
                    }
                }
            }
            return Selected;
        }


    }
}