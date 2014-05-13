using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ISD.EDS;
using ISD.Provider.Web;
using ISD.Util;

using ISD.DA;
using ISD.BF;
using Microsoft.AspNet.Identity.EntityFramework;


namespace HealthyClub.Providers.Web.UserControls
{
    public partial class ActivityDetail : System.Web.UI.UserControl
    {
        public int ActivityID
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnActivityID.Value))
                    return Convert.ToInt32(hdnActivityID.Value);
                else return 0;
            }
            set
            {
                hdnActivityID.Value = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Page.RouteData.Values[SystemConstants.route_ActivityID] != null)
                {
                    ActivityID = Convert.ToInt32(Page.RouteData.Values[SystemConstants.route_ActivityID].ToString());
                    if (AuthUser())
                    {
                        lnkEditActivity.Visible = true; ImagesListViewUC2.ActivityID = ActivityID;
                        Refresh();
                    }
                }
                else if (Request.QueryString[SystemConstants.route_ActivityID] != null)
                {
                    ActivityID = Convert.ToInt32(Request.QueryString[SystemConstants.route_ActivityID]);
                    if (AuthUser())
                    {
                        lnkEditActivity.Visible = true; ImagesListViewUC2.ActivityID = ActivityID;
                        Refresh();
                    }
                }
                else
                {
                    lnkEditActivity.Visible = false;
                    divMain.Visible = false;
                    divUnauth.Visible = true;
                    Response.StatusCode = 404;
                }
            }

        }

        private void Refresh()
        {
            var dr = new DataAccessComponent().RetrieveActivityExplorer(ActivityID);

            SetTitle(dr.ProviderID);
            SetActivityInformation(dr);
            SetTimetableInformation();
        }

        private void SetTitle(String providerID)
        {
            DataAccessComponent dac = new DataAccessComponent();

            if (dac.IsUserImageExist(providerID))
            {
                divWithImage.Visible = true;
                divNoImage.Visible = false;
                int ImageID = new BusinessFunctionComponent().getProviderPrimaryImage(providerID);
                if (ImageID != 0)
                    ProviderImage.ImageUrl = "~/ImageHandler.ashx?" + SystemConstants.qs_UserImageID + "=" + ImageID;
                else
                {
                    divWithImage.Visible = false;
                    divNoImage.Visible = true;
                }
            }
            else
            {
                divWithImage.Visible = false;
                divNoImage.Visible = true;
            }
        }

        private void SetTimetableInformation()
        {
            //AcitivityScheduleDetailUC1.ActivityID = ActivityID;
            //AcitivityScheduleDetailUC1.Refresh();
            ScheduleViewerUC1.ActivityID = ActivityID;
            ScheduleViewerUC1.timetableFormat = (int)SystemConstants.TimetableFormat.Seasonal;
        }

        private void SetActivityInformation(DataSetComponent.v_ActivityExplorerRow dr)
        {
            divProductDesc.InnerHtml = dr.FullDescription;
            if (!string.IsNullOrEmpty(dr.Price))
            {
                divPriceDescription.InnerHtml = dr.Price;
            }
            else
            {
                divPriceDescription.InnerHtml = "This is a free activity";
            }
            if (!string.IsNullOrEmpty(dr.eligibilityDescription))
            {
                divEligivibility.InnerHtml = dr.eligibilityDescription;
            }
            else
            {
                trSuitability.Visible = false;
            }

            lblProvider.Text = lblProviderWImage.Text = dr.ProviderName;
            lblContact.Text = dr.PhoneNumber;
            if (string.IsNullOrEmpty(dr.Website))
            {
                lblWebsite.Visible = true;
                lblWebsite.Text = "No website url assigned";
            }
            else
            {
                hlnkWebsite.Text = dr.Website;
                hlnkWebsite.NavigateUrl = "http://" + dr.Website;
            }
            lblUpdate.Text = dr.ModifiedDateTime.Date.ToShortDateString();
            if (!string.IsNullOrEmpty(dr.Suburb))
            {
                lblAddress.Text = dr.Address;
                lblSub.Text = dr.Suburb + ",";
                lblState.Text = dr.StateName;
                lblPostCode.Text = dr.PostCode.ToString();
            }
            else
            {
                lblAddress.Text = "n/a";
                lblSub.Visible = lblState.Visible = lblPostCode.Visible = false;
            }
            lblTitle.Text = lblTitleWImage.Text = dr.Name;
            lblContactName.Text = dr.FirstName + " " + dr.LastName;
            lblEmailAddress.Text = dr.Email;
            if (string.IsNullOrEmpty(dr.Keywords))
                lblKeyword.Text = "No keywords assigned.";
            else
                lblKeyword.Text = dr.Keywords;
        }

        private bool AuthUser()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                var providerID = IdentityHelper.GetUserIdFromRequest(Request);                    
                var ownerLogin = new BusinessFunctionComponent().CheckActivityOwner(ActivityID, providerID);

                return ownerLogin;
            }
            else
            {
                Response.Redirect("~/Account/Login.aspx");
                return false;
            }
        }

        protected void lnkEditActivity_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Activity/EditActivity.aspx?" + SystemConstants.ActivityID + "=" + ActivityID);
        }



    }
}

