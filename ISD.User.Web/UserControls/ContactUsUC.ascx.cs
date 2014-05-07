﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ISD.EDS;
using ISD.DA;
using System.Web.Security;
using ISD.Util;
using System.Net.Mail;
using BCUtility;

namespace ISD.User.Web.UserControls
{
    public partial class ContactUsUC : System.Web.UI.UserControl
    {
        public bool IsEnquiry
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnIsEnquiry.Value))
                    return Convert.ToBoolean(hdnIsEnquiry.Value);
                else return false;
            }
            set
            {
                hdnIsEnquiry.Value = value.ToString();
            }
        }

        public Guid CustomerID { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Membership.GetUser() != null && Membership.GetUser().IsOnline)
                {

                    var dr = new DataAccessComponent().RetrieveUserProfiles(new Guid(Membership.GetUser().ProviderUserKey.ToString()));
                    if (dr != null)
                        SetData(dr);
                }
                SetButton();

            }
        }

        private void SetData(EntityDataSetComponent.UserProfilesRow dr)
        {
            txtContactPerson.Text = dr.FirstName + " " + dr.LastName;
            txtEmail.Text = dr.Email;
            txtPhone.Text = dr.PhoneNumber;
            CustomerID = dr.UserID;
        }

        private void SetButton()
        {
            btnNextStep.Visible = true;
            trRemarks.Visible = true;
        }

        protected void btnNextStep_Click(object sender, EventArgs e)
        {
            //EntityDataSetComponent.CartRow dr = GetData();

            //EntityDataSetComponent.CartItemDataTable dt = new CartHelper(Session).RetrieveCartItem();
            //EntityDataSetComponent.CartItemDataTable dt = new EntityDataSetComponent.CartItemDataTable();
            //new DataAccessComponent().CreateCart(dr, dt);
            //Session["OrderID"] = dr.ID;

            
            EntityDataSetComponent.WebConfigurationRow drSmtp = new DataAccessComponent().RetrieveEmailServerSetting();
            if (drSmtp != null)
            {
                //SendEmail to HelpDesk
                

                string emailBody = "This email was sent from Healthy Club Notification System to Healthy Club Help Desk.<br/><p>    New query from " + txtContactPerson.Text + "<br />   ";
                emailBody += " Query Information</p><table><tr><td>            </td>            <td >                Contact Person            </td>            <td>                :            </td>            <td>                " + txtContactPerson.Text + "</td>";
                emailBody += "</tr>        <tr>            <td>                &nbsp;            </td>            <td>                Contact Number            </td>            <td>                :            </td>            <td>                " + txtPhone.Text + "</td>";
                emailBody += "</tr>        <tr>            <td>                &nbsp;            </td>            <td>                Email Address            </td>            <td>                :            </td>            <td>                " + txtEmail.Text + "</td>";
                emailBody += "</tr>        <tr>            <td>                &nbsp;</td>            <td>                Message            </td>            <td valign='top'>                :            </td>            <td>                " + txtRemarks.Text + "</td>        </tr>    </table>";

                EmailSender.SendEmail(drSmtp.SMTPAccount, "ISD.User@iechs.org.au", "New Enquiry From " + txtContactPerson.Text + "</br>", emailBody, drSmtp.SMTPHost, drSmtp.SMTPPort, drSmtp.SMTPUserName, drSmtp.SMTPPassword, drSmtp.SMTPSSL, drSmtp.SMTPIIS);

                divContactUs.Visible = false;
                CompleteContactUs.Visible = true;
            }
        }

        private EntityDataSetComponent.ContactUsRow GetData()
        {
            EntityDataSetComponent.ContactUsRow dr = new EntityDataSetComponent.ContactUsDataTable().NewContactUsRow();
            dr.ContactPerson = txtContactPerson.Text;
            dr.Phone = txtPhone.Text;
            dr.Email = txtEmail.Text;
            dr.Remarks = txtRemarks.Text;
            dr.UserID = CustomerID;
            if (radiobyPhone.Checked)
                dr.PreferredContact = (int)SystemConstants.PreferedContact.Phone;
            else
                dr.PreferredContact = (int)SystemConstants.PreferedContact.Email;
            dr.CreatedDatetime = DateTime.Now;
            dr.ModifiedDatetime = DateTime.Now;

            return dr;
        }

        protected void HomeButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~");
        }



    }
}