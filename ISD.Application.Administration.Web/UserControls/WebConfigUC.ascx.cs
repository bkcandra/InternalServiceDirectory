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
    public partial class WebConfigUC : System.Web.UI.UserControl
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
            var dr = new DataAccessComponent().RetrieveWebConfiguration();
            SetForm(dr);
        }

        private DataSetComponent.WebConfigurationRow GetForm()
        {
            var dr = new DataSetComponent.WebConfigurationDataTable().NewWebConfigurationRow();
            dr.ModifiedBy = Context.User.Identity.Name;
            dr.ModifiedDatetime = DateTime.Now;

            if (radAdvSearchTrue.Checked)
                dr.AdvancedSearch = true;
            else if (radAdvSearchFalse.Checked)
                dr.AdvancedSearch = false;

            dr.AdministratorSiteUrl = hdnAdminurl.Value;
            dr.CustomerSiteUrl = hdnCusturl.Value;
            dr.ProviderSiteUrl = hdnProvurl.Value;
            dr.ProviderImageUrl = hdnImageurl.Value;
            dr.SMTPAccount = hdnSMPTAccount.Value;
            dr.SMTPPassword = hdnSMPTPassword.Value;
            dr.SMTPUserName = hdnSMPTUserName.Value;
            dr.SMTPHost = hdnSMPTHost.Value;
            dr.SMTPPort = Convert.ToInt32(hdnSMPTPort.Value);
            dr.SMTPSSL = Convert.ToBoolean(hdnSMPTSSL.Value);
            dr.SMTPIIS = Convert.ToBoolean(hdnSMPTIIS.Value);

            if (radCaptchaTrue.Checked)
                dr.EnableCaptcha = true;
            else if (radCaptchaFalse.Checked)
                dr.EnableCaptcha = false;

            if (radEnableExpTrue.Checked)
                dr.EnableExpiryProcessing = true;
            else if (radEnableExpfalse.Checked)
                dr.EnableExpiryProcessing = false;

            if (radEnableExpNotiTrue.Checked)
                dr.EnableFirstNotification = true;
            else if (radEnableExpNotiFalse.Checked)
                dr.EnableFirstNotification = false;

            dr.FirstNotificationDays = Convert.ToInt32(txtFirstNotiDays.Text);

            if (radEnableExpNoti2True.Checked)
                dr.EnableSecondNotification = true;
            else if (radEnableExpNoti2False.Checked)
                dr.EnableSecondNotification = false;

            dr.SecondNotificationDays = Convert.ToInt32(txtSecondNotiDays.Text);

            if (radSendEmailTrue.Checked)
                dr.SendNotificationEmail = true;
            else if (radSendEmailFalse.Checked)
                dr.SendNotificationEmail = false;

            if (radRewardProcessTrue.Checked)
                dr.EnableRewardProcessing = true;
            else if (radRewardProcessFalse.Checked)
                dr.EnableRewardProcessing = false;

            dr.PointsAwarded = Convert.ToInt32(txtRewardPoints.Text);

            return dr;
        }

        private void SetForm(DataSetComponent.WebConfigurationRow dr)
        {
            
            if (dr.AdvancedSearch != null)
            {
                radAdvSearchTrue.Checked = dr.AdvancedSearch;
                radAdvSearchFalse.Checked = !dr.AdvancedSearch;
            }
            hdnAdminurl.Value = dr.AdministratorSiteUrl;
            hdnCusturl.Value = dr.CustomerSiteUrl;
            hdnProvurl.Value = dr.ProviderSiteUrl;
            hdnImageurl.Value = dr.ProviderImageUrl;
            hdnSMPTAccount.Value = dr.SMTPAccount;
            hdnSMPTPassword.Value = dr.SMTPPassword;
            hdnSMPTUserName.Value = dr.SMTPUserName;
            hdnSMPTHost.Value = dr.SMTPHost;
            hdnSMPTPort.Value = dr.SMTPPort.ToString();
            hdnSMPTSSL.Value = dr.SMTPSSL.ToString();
            hdnSMPTIIS.Value = dr.SMTPIIS.ToString();

            if (dr.EnableCaptcha != null)
            {
                radCaptchaTrue.Checked = dr.EnableCaptcha;
                radCaptchaFalse.Checked = !dr.EnableCaptcha;
            }
            if (dr.EnableExpiryProcessing != null)
            {
                radEnableExpTrue.Checked = dr.EnableExpiryProcessing;
                radEnableExpfalse.Checked = !dr.EnableExpiryProcessing;
            }
            if (dr.EnableFirstNotification != null)
            {
                radEnableExpNotiTrue.Checked = dr.EnableFirstNotification;
                radEnableExpNotiFalse.Checked = !dr.EnableFirstNotification;
            }
            if (dr.FirstNotificationDays != null)
            {
                txtFirstNotiDays.Text = dr.FirstNotificationDays.ToString();
            }
            else
                txtFirstNotiDays.Text = SystemConstants.NotificationOneDays.ToString();

            if (dr.EnableSecondNotification != null)
            {
                radEnableExpNoti2True.Checked = dr.EnableSecondNotification;
                radEnableExpNoti2False.Checked = !dr.EnableSecondNotification;
            }
            if (dr.SecondNotificationDays != null)
            {
                txtSecondNotiDays.Text = dr.SecondNotificationDays.ToString();
            }
            else
                txtFirstNotiDays.Text = SystemConstants.NotificationTwoDays.ToString();
            if (dr.SendNotificationEmail != null)
            {
                radSendEmailTrue.Checked = dr.SendNotificationEmail;
                radSendEmailFalse.Checked = !dr.SendNotificationEmail;
            }
            if (dr.EnableRewardProcessing != null)
            {
                radRewardProcessTrue.Checked = dr.EnableRewardProcessing;
                radRewardProcessFalse.Checked = !dr.EnableRewardProcessing;
            }
            if (dr.PointsAwarded != null)
            {
                txtRewardPoints.Text = dr.PointsAwarded.ToString();
            }
            else
            {
                txtRewardPoints.Text = SystemConstants.DefaultRewardPoints.ToString();
            }
        }

        private bool DataIsValid(DataSetComponent.WebConfigurationRow dr, out string message)
        {
            // ADD VALIDATION CODE HERE 
            message = "";
            return true;
        }

        protected void lnkSubmit_Click(object sender, EventArgs e)
        {

            divSuccess.Visible = divWarning.Visible = divError.Visible = false;
            var dr = GetForm();
            string errorMessage = "";
            if (DataIsValid(dr, out errorMessage))
            {
                new DataAccessComponent().UpdateWebConfiguration(dr);
                lblSuccess.Text = "Web Configuration saved - " + DateTime.Now.ToShortTimeString();
                divSuccess.Visible = true;
            }
            else
            {
                lblError.Text = errorMessage + " - " + DateTime.Now.ToShortTimeString();
                divError.Visible = true;
            }

        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            divSuccess.Visible = divWarning.Visible = divError.Visible = false;
            Refresh();
        }
    }
}