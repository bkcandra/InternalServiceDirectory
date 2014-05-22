using BCUtility;
using ISD.Util;
using ISD.BF;
using ISD.DA;
using ISD.EDS;
using ISD.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace ISD.Administration.Web.UserControls
{
    public partial class RewardsAddition : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                initDDL();
                Refresh();

            }
        }

        private void initDDL()
        {
            var dt = new DataAccessComponent().RetrieveRewardTypes();
            foreach (var dr in dt)
            {
                ddlRewType.Items.Add(new ListItem(dr.TypeName, dr.Type.ToString()));
            }

            var discActDt = new DataAccessComponent().RetrieveActivities();


            foreach (var discActDr in discActDt)
            {
                ddlDiscAct.Items.Add(new ListItem(discActDr.Name, discActDr.ID.ToString()));
            }

            var reqActDt = new DataAccessComponent().RetrieveActivities();
            foreach (var reqActDr in reqActDt)
            {
                ddlReqActivityName.Items.Add(new ListItem(reqActDr.Name, reqActDr.ID.ToString()));
            }

            var freeActDt = new DataAccessComponent().RetrieveActivities();
            foreach (var freeActDr in freeActDt)
            {
                ddlFreeAct.Items.Add(new ListItem(freeActDr.Name, freeActDr.ID.ToString()));
            }

            var bonusActDt = new DataAccessComponent().RetrieveActivities();
            foreach (var bonusActDr in bonusActDt)
            {
                ddlBonusAct.Items.Add(new ListItem(bonusActDr.Name, bonusActDr.ID.ToString()));
            }

            var allsponsors = new DataAccessComponent().RetrieveSponsorsExplorer();
            foreach (var allsponsor in allsponsors)
            {
                ddlsponsors.Items.Add(new ListItem(allsponsor.Name, allsponsor.ID.ToString()));

            }

        }

        public void Refresh()
        {
            if (select.SelectedIndex == 0)
                selection.Text = "Discount Rate";
            else
                selection.Text = "Money Off";
            if (ddlRewType.SelectedIndex == (int)SystemConstants.RewardType.Gift)
            {
                trDisc1.Visible = trDisc2.Visible = trDisc3.Visible = false;
                troffer1.Visible = troffer2.Visible = false;
                trOther1.Visible = trOther2.Visible = false;
                trgift1.Visible = true;
            }
            else if (ddlRewType.SelectedIndex == (int)SystemConstants.RewardType.Offer)
            {
                troffer1.Visible = troffer2.Visible = true;
                trDisc1.Visible = trDisc2.Visible = trDisc3.Visible = false;

                trOther1.Visible = trOther2.Visible = false;
                trgift1.Visible = false;
            }
            else if (ddlRewType.SelectedIndex == (int)SystemConstants.RewardType.Other)
            {
                trDisc1.Visible = trDisc2.Visible = trDisc3.Visible = false;
                troffer1.Visible = troffer2.Visible = false;
                trOther1.Visible = trOther2.Visible = true;
                trgift1.Visible = false;
            }

            else if (ddlRewType.SelectedIndex == (int)SystemConstants.RewardType.Discount)
            {
                trDisc1.Visible = trDisc2.Visible = trDisc3.Visible = true;
                troffer1.Visible = troffer2.Visible = false;
                trOther1.Visible = trOther2.Visible = false;
                trgift1.Visible = false;
            }



        }

        protected void ddlRewType_SelectedIndexChanged(object sender, EventArgs e)
        {



            Refresh();
        }

        protected void select_index(object sender, EventArgs e)
        {

            Refresh();
        }

        protected void lnkAdd_Click(object sender, EventArgs e)
        {
            if (ValidateRewardDetails())
            {
                DataSetComponent.RewardRow dr = GetDetails();
                DataSetComponent.RewardsDetailsRow drDet = GetRwrdDetails();
                DataSetComponent.RewardImageRow drt = GetImgDetails();

                if (dr != null)
                {
                    if (drt != null)
                    {
                        dr.RewardImage = true;
                    }
                    else
                        dr.RewardImage = false;
                    new BusinessFunctionComponent().SaveRewards(dr, drDet, drt);
                    Response.Redirect("~/Rewards/");
                }
            }
            else
            {
                divError.Visible = true;
                lblError.Text = "Please complete all required field";
            }
        }

        private bool ValidateRewardDetails()
        {
            bool valid = true;

            if (string.IsNullOrEmpty(rname.Text))
                valid = false;
            if (string.IsNullOrEmpty(rdesc.Text))
                valid = false;
            if (string.IsNullOrEmpty(txtCalendarFrom.Text))
                valid = false;
            DateTime dtFrom = SystemConstants.nodate;
            if (!DateTime.TryParse(txtCalendarFrom.Text, out dtFrom))
                valid = false;
            if (ddlRewType.SelectedValue == "0")
                valid = false;
            if (ddlsponsors.SelectedValue == "0")
                valid = false;
            if (string.IsNullOrEmpty(point.Text))
                valid = false;

            return valid;
        }

        private DataSetComponent.RewardImageRow GetImgDetails()
        {

            if (fileUpRewardImage.HasFile)
            {
                var dr = new DataSetComponent.RewardImageDataTable().NewRewardImageRow();
                dr.ID = 0;
                dr.Description = fileUpRewardImage.FileName;
                dr.ImageTitle = dr.Filename = fileUpRewardImage.FileName;
                dr.Filesize = fileUpRewardImage.PostedFile.ContentLength / 1024;
                dr.ImageStream = new ImageHandler().ReadFully(fileUpRewardImage.PostedFile.InputStream);
                return dr;
            }
            else return null;
        }

        protected DataSetComponent.RewardRow GetDetails()
        {
            var dr = new DataSetComponent.RewardDataTable().NewRewardRow();

            dr.ID = 0;
            String spnid = ddlsponsors.SelectedValue;
            dr.ProviderID = spnid;


            dr.RewardsName = rname.Text;
            dr.RewardDescription = rdesc.Text;

            if (!string.IsNullOrEmpty(txtCalendarFrom.Text))
            {
                DateTime RwrdExp = DateTime.Now;

                if (DateTime.TryParse(txtCalendarFrom.Text, out  RwrdExp))
                    dr.RewardExpiryDate = RwrdExp;
                else
                    dr.RewardExpiryDate = DateTime.Now.AddDays(90);
            }
            else
            {
                divError.Visible = true;
                return null;

            }
            dr.RequiredRewardPoint = Convert.ToInt32(point.Text);
            if (ddlRewType.SelectedValue == Convert.ToString((int)SystemConstants.RewardType.Gift))
                dr.RewardType = (int)SystemConstants.RewardType.Gift;
            else if (ddlRewType.SelectedValue == Convert.ToString((int)SystemConstants.RewardType.Offer))
                dr.RewardType = (int)SystemConstants.RewardType.Offer;
            else if (ddlRewType.SelectedValue == Convert.ToString((int)SystemConstants.RewardType.Other))
                dr.RewardType = (int)SystemConstants.RewardType.Other;
            else
                dr.RewardType = (int)SystemConstants.RewardType.Discount;
            dr.CategoryID = 0;

            if (disperc.Text != "")
            {
                if (select.SelectedIndex == 1)
                {
                    dr.Discount = 0;
                    dr.SupportOFF = Convert.ToInt32(disperc.Text);

                }
                else
                {
                    dr.SupportOFF = 0;
                    dr.Discount = Convert.ToInt32(disperc.Text);

                }
            }
            else
            {
                dr.Discount = 0;
                dr.SupportOFF = 0;

            }
            if (ddlReqActivityName.SelectedValue == "")
            {
                dr.RequiredActivityEnroll = 0;
            }
            else
            {
                dr.RequiredActivityEnroll = Convert.ToInt32(ddlReqActivityName.SelectedValue);

            }
            if (Bonuspoint.Text != "")
                dr.BonusPoint = Convert.ToInt32(Bonuspoint.Text);
            else
                dr.BonusPoint = 0;
            if (ddlRewType.SelectedValue == Convert.ToString((int)SystemConstants.RewardType.Gift))
                dr.FreeActivityID = 0;
            else if (ddlRewType.SelectedValue == Convert.ToString((int)SystemConstants.RewardType.Offer))
                dr.FreeActivityID = Convert.ToInt32(ddlFreeAct.SelectedValue);
            else if (ddlRewType.SelectedValue == Convert.ToString((int)SystemConstants.RewardType.Other))
                dr.FreeActivityID = Convert.ToInt32(ddlBonusAct.SelectedValue);
            else
                dr.FreeActivityID = Convert.ToInt32(ddlDiscAct.SelectedValue);



            dr.FreeGift = giftname.Text;
            dr.UsageTimes = Convert.ToInt32(usage.Text);
            dr.NofTimeUsed = 0;
            if (ddlrewsource.SelectedItem.Text == "Internal/Activity Based")
                dr.RewardSource = 1;
            else
                dr.RewardSource = 2;
            return dr;
        }

        public DataSetComponent.RewardsDetailsRow GetRwrdDetails()
        {
            var dt = new DataSetComponent.RewardsDetailsDataTable().NewRewardsDetailsRow();

            dt.RewardID = 0;
            dt.CreatedDateTime = DateTime.Now;

            if (txtCalendarFrom.Text != "")
            {
                DateTime RwrdExp = DateTime.Now;

                if (DateTime.TryParse(txtCalendarFrom.Text, out  RwrdExp))
                    dt.ExpiryDate = RwrdExp;
                else
                    dt.ExpiryDate = DateTime.Now.AddDays(90);
            }
            else
            {
                divError.Visible = true;
                return null;

            }


            dt.ModifiedDateTime = DateTime.Now;

            dt.Keywords = Keyword.Text;
            dt.CreatedBy = Context.User.Identity.Name;
            dt.ModifiedBy = Context.User.Identity.Name;
            return dt;
        }
    }
}