using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ISD.User.Web;
using ISD.Util;
using ISD.EDS;
using System.Web.Security;
using ISD.BF;
using ISD.DA;
using System.Net;
using System.Xml.Linq;
using Microsoft.AspNet.Identity;
using WebMatrix.WebData;


namespace ISD.User.Customer.Web.UserControls
{
    public partial class RewardDetail : System.Web.UI.UserControl
    {

        public int RewardID
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnRewardID.Value))
                    return Convert.ToInt32(hdnRewardID.Value);
                else return 0;
            }
            set
            {
                hdnRewardID.Value = value.ToString();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblerror.Visible = false;
                if (Request.QueryString[SystemConstants.route_RewardsID] != null)
                {
                    RewardID = Convert.ToInt32(Request.QueryString[SystemConstants.route_RewardsID]);


                    Refresh();


                }
                if (WebSecurity.IsAuthenticated)
                {
                  
                    string fName = "Anonymous";
                    string lName = "User";
                    string rName = "Anonymous";
                    string email = "AnonymousUser";

                    if (Session[SystemConstants.ses_FName] != null)
                        fName = (String)(Session[SystemConstants.ses_FName]);
                    if (Session[SystemConstants.ses_LName] != null)
                        lName = (String)(Session[SystemConstants.ses_LName]);
                    if (Session[SystemConstants.ses_Role] != null)
                        rName = (String)(Session[SystemConstants.ses_Role]);
                    if (Session[SystemConstants.ses_Email] != null)
                        email = (String)(Session[SystemConstants.ses_Email]);

                    /*Analytics.Client.Identify("019mr8mf4r", new Traits() {
                    { "name",fName +" "+ lName },
                    { "email", email },
                    { "role", rName },
                    { "friendCount", 29 }
                    });*/
                }
            }
        }
        void Refresh()
        {
            Addtocart.CommandName = Convert.ToString(RewardID);
            AddtocartNcheckout.CommandName = Convert.ToString(RewardID);
            var dr = new DataAccessComponent().RetrieveRewardInfo(Convert.ToInt32(RewardID));
            if (dr != null)
            {
                string check = Convert.ToString(dr.RewardImage);
                if (!string.IsNullOrEmpty(check))
                {
                    if (Convert.ToBoolean(check))
                    {
                        var drr = new DataAccessComponent().RetrieveRewardPrimaryImage(Convert.ToInt32(RewardID));
                        if (drr != null || drr.ImageStream != null)
                            //Convert byte directly, while its easier, its not suppose to be 
                            //imgPreview.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(dr.ImageStream);
                            imgPreview.ImageUrl = "~/ImageHandler.ashx?" + SystemConstants.qs_RewardThumbImageID + "=" + drr.ID;
                    }
                    else
                        imgPreview.ImageUrl = "~/Images/gift.jpg";
                }

                lblsearch.Text = "Reward '" + dr.RewardsName + "'";
                lblpoints.Text = Convert.ToString(dr.RequiredRewardPoint);
                lblLongDescription.Text = dr.RewardDescription;
                RwdExpiry.Text = Convert.ToString(dr.RewardExpiryDate);
                lblName.Text = dr.RewardsName;
            }
            else
            {
                imgPreview.Visible = false;
                lblpoints.Visible = false;
                lblLongDescription.Visible = false;
                lblName.Visible = false;
                RwdExpiry.Visible = false;
            }

        }

        protected void Addtocart_Click(object sender, EventArgs e)
        {
            if (WebSecurity.IsAuthenticated)
            {

                if (Session != null)
                {
                    Button ClickedButton = (Button)sender;
                    int RewardPts = Convert.ToInt32(lblpoints.Text);
                    bool check = checkRewards(RewardPts);
                    if (check == true)
                    {
                        int RewardID = Convert.ToInt32(ClickedButton.CommandName);
                        RewardCart.Instance.AddItem(RewardID);
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                        lblerror.Visible = true;

                }


            }
            else
                Response.Redirect("Performance.aspx");

        }

        protected void AddtocartNcheckout_Click(object sender, EventArgs e)
        {
            if (WebSecurity.IsAuthenticated)
            {

                if (Session != null)
                {
                    Button ClickedButton = (Button)sender;
                    int RewardPts = Convert.ToInt32(lblpoints.Text);
                    bool check = checkRewards(RewardPts);
                    if (check == true)
                    {
                        int RewardID = Convert.ToInt32(ClickedButton.CommandName);
                        RewardCart.Instance.AddItem(RewardID);
                        Response.Redirect("Redeem.aspx");
                    }
                    else
                        lblerror.Visible = true;
                }

            }
            else
                Response.Redirect("Performance.aspx");
        }

        protected bool checkRewards(int pts)
        {
            int currrwd = Convert.ToInt32(Session[SystemConstants.ses_Rwdpts]);
            int subtotal = RewardCart.Instance.GetSubTotal() + pts;
            if (currrwd >= subtotal)
                return true;
            return false;


        }

    }
}