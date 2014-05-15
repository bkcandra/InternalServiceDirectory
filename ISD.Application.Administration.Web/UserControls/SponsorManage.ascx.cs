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
    public partial class SponsorManage : System.Web.UI.UserControl
    {
        public int FormMode
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnFormMode.Value))
                {
                    return Convert.ToInt32(hdnFormMode.Value);
                }
                return (int)SystemConstants.FormMode.View;
            }
            set
            {
                if (value == (int)SystemConstants.FormMode.New)
                {
                    lblspnName.Visible = false;
                    lbladdress.Visible = false;
                    lblCalendarFrom.Visible = false;
                    lblwebsite.Visible = false;
                    lblnumber.Visible = false;

                    txtspnName.Visible = true;
                    txtaddress.Visible = true;
                    txtCalendarFrom.Visible = true;
                    txtwebsite.Visible = true;
                    txtnumber.Visible = true;

                    lnkAdd.Visible = true;
                    lnkEdit.Visible = false;
                    lnkUpdate.Visible = false;

                }
                else if (value == (int)SystemConstants.FormMode.Edit)
                {
                    lblspnName.Visible = false;
                    lbladdress.Visible = false;
                    lblCalendarFrom.Visible = false;
                    lblwebsite.Visible = false;
                    lblnumber.Visible = false;

                    txtspnName.Visible = true;
                    txtaddress.Visible = true;
                    txtCalendarFrom.Visible = true;
                    txtwebsite.Visible = true;
                    txtnumber.Visible = true;

                    lnkAdd.Visible = false;
                    lnkEdit.Visible = false;
                    lnkUpdate.Visible = true;

                }
                else if (value == (int)SystemConstants.FormMode.View)
                {
                    lblspnName.Visible = true;
                    lbladdress.Visible = true;
                    lblCalendarFrom.Visible = true;
                    lblwebsite.Visible = true;
                    lblnumber.Visible = true;



                    txtspnName.Visible = false;
                    txtaddress.Visible = false;
                    txtCalendarFrom.Visible = false;
                    txtwebsite.Visible = false;
                    txtnumber.Visible = false;

                    lnkAdd.Visible = false;
                    lnkEdit.Visible = true;
                    lnkUpdate.Visible = false;

                }
                hdnFormMode.Value = value.ToString();
            }

        }

        public string SponsorID
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnSponsorID.Value))
                    return Convert.ToString(hdnSponsorID.Value);
                else return null;
            }
            set
            {
                hdnSponsorID.Value = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Request.QueryString[SystemConstants.route_SponsorsID] != null)
                {
                    FormMode = (int)SystemConstants.FormMode.View;
                    SponsorID = Request.QueryString[SystemConstants.route_SponsorsID];
                    setSponsorsInformation(SponsorID);
                }
                else
                    FormMode = (int)SystemConstants.FormMode.New;

            }
        }

        public void setSponsorsInformation(String Spnid)
        {
            var dr = new DataAccessComponent().RetrieveSponsorDetails(Spnid);
            if (dr != null)
            {
                lblspnName.Text = txtspnName.Text = dr.Name;
                lbladdress.Text = txtaddress.Text = dr.Address;
                lblCalendarFrom.Text = txtCalendarFrom.Text = Convert.ToString(dr.ContractExpiry);
                lblwebsite.Text = txtwebsite.Text = dr.Website;
                lblnumber.Text = txtnumber.Text = Convert.ToString(dr.PhoneNumber);

            }

        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            FormMode = (int)SystemConstants.FormMode.Edit;
        }

        protected void lnkUpdate_Click(object sender, EventArgs e)
        {
            DataSetComponent.SponsorRow sr = GetDetails();
            
            sr.ID = SponsorID;
            new DataAccessComponent().UpdateSponsor(sr);
            setSponsorsInformation(SponsorID);
            FormMode = (int)SystemConstants.FormMode.View;
            addeddiv.Visible = true;
            rewardadded.Text = "Sponsor details are Updated";
        }

        public DataSetComponent.SponsorRow GetDetails()
        {
            var sr = new DataSetComponent.SponsorDataTable().NewSponsorRow();
            sr.Name = txtspnName.Text;
            if (!string.IsNullOrEmpty(txtaddress.Text))
                sr.Address = txtaddress.Text;
            else sr.Address = string.Empty;
            if (!string.IsNullOrEmpty(txtwebsite.Text))
                sr.Website = txtwebsite.Text;
            else sr.Website = string.Empty;
            if (!string.IsNullOrEmpty(txtnumber.Text))
                sr.PhoneNumber = 0;
            else sr.PhoneNumber = 0;

            sr.Status = 1;
            sr.ContractExpiry = Convert.ToDateTime(txtCalendarFrom.Text);

            return sr;

        }
        protected void lnkAdd_Click(object sender, EventArgs e)
        {
            DataSetComponent.SponsorRow sr = GetDetails();
            Guid spnid = Guid.NewGuid();
            sr.ID = spnid.ToString();
            new DataAccessComponent().SaveSponsorDetail(sr);
            setSponsorsInformation(spnid.ToString());
            FormMode = (int)SystemConstants.FormMode.View;
            addeddiv.Visible = true;
            rewardadded.Text = "Sponsor is Added";
        }

    }
}