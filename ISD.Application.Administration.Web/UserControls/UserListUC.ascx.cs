using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BCUtility;
using ISD.Administration.Web;
using ISD.Administration.Web.Models;
using ISD.DA;
using ISD.Data.EDM;
using ISD.EDS;
using ISD.BF;
using ISD.Util;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ISD.Administration.Web.UserControls
{
    public partial class UserListUC : System.Web.UI.UserControl
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
            SetDataSource();
        }

        private void SetDataSource()
        {
            ods.TypeName = typeof(DataAccessComponent).FullName;
            ods.SelectParameters.Clear();
            ods.MaximumRowsParameterName = "amount";
            ods.StartRowIndexParameterName = "startIndex";
            ods.SelectMethod = "RetrieveCustomerList";
            ods.SelectCountMethod = "RetrieveCustomerListCount";
            ods.EnablePaging = true;

            listviewActivities.DataSourceID = "ods";
            listviewActivities.DataBind();
        }

        private DataSetComponent.UserProfilesDataTable GetSelected()
        {
            var dt = new DataSetComponent.UserProfilesDataTable();

            foreach (var item in listviewActivities.Items)
            {
                CheckBox chkSelected = item.FindControl("chkSelected") as CheckBox;

                if (chkSelected.Checked)
                {
                    Label lblEmail = item.FindControl("lblEmail") as Label;
                    LinkButton lblUsername = item.FindControl("lnkUserName") as LinkButton;
                    var dr = dt.NewUserProfilesRow();
                    dr.Username = lblUsername.Text;
                    dr.UserID = Guid.Empty.ToString();
                    dt.AddUserProfilesRow(dr);
                }
            }
            return dt;
        }

        private async void DeleteSelected()
        {
            var mgr = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            foreach (var item in listviewActivities.Items)
            {
                CheckBox chkSelected = item.FindControl("chkSelected") as CheckBox;

                if (chkSelected.Checked)
                {
                    Label lblEmail = item.FindControl("lblEmail") as Label;
                    HyperLink lblUsername = item.FindControl("hlnkUserName") as HyperLink;

                    ApplicationUser usr = mgr.FindByName(lblUsername.Text);
                    await mgr.DeleteAsync(usr);
                }
            }
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            DeleteSelected();
            Refresh();
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            SetDataSource();
        }

        protected void listviewActivities_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Image imgEmailIcon = e.Item.FindControl("imgEmailIcon") as Image;
                HyperLink hlnkUserName = e.Item.FindControl("hlnkUserName") as HyperLink;
                LinkButton lnkResenConfirmation = e.Item.FindControl("lnkResenConfirmation") as LinkButton;
                LinkButton lnkConfirm = e.Item.FindControl("lnkConfirm") as LinkButton;
                HyperLink hlnkEdit = e.Item.FindControl("hlnkEdit") as HyperLink;
                HiddenField hdnUserID = e.Item.FindControl("hdnUserID") as HiddenField;
                HiddenField hdnConfirmationToken = e.Item.FindControl("hdnConfirmationToken") as HiddenField;
                Label lblActCount = e.Item.FindControl("lblActCount") as Label;
                Label lblID = e.Item.FindControl("lblID") as Label;
                var mgr = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                if (mgr.IsEmailConfirmed(mgr.FindByName(hlnkUserName.Text).Id))
                {
                    imgEmailIcon.ImageUrl = "~/Content/StyleImages/Check.png";
                    //lnkResenConfirmation.Visible = lnkConfirm.Visible = false;
                }
                else
                {
                    imgEmailIcon.ImageUrl = "~/Content/StyleImages/Cross.png";
                    // lnkResenConfirmation.Visible = lnkConfirm.Visible = true;
                }
                //Response.Redirect("~/User/Member.aspx?" + SystemConstants.UserID + "=" + hdnUserID.Value);
                hlnkUserName.NavigateUrl = hlnkEdit.NavigateUrl = "~/User/Member.aspx?" + SystemConstants.UserID + "=" + hdnUserID.Value;
                hdnConfirmationToken.Value = IdentityHelper.GetCodeFromRequest(Request);
            }
        }

        protected void listviewActivities_ItemCommand(object sender, ListViewCommandEventArgs e)
        {

            HiddenField hdnConfirmationToken = e.Item.FindControl("hdnConfirmationToken") as HiddenField;
            HiddenField hdnUserID = e.Item.FindControl("hdnUserID") as HiddenField;
            LinkButton lnkUserName = e.Item.FindControl("lnkUserName") as LinkButton;
            Label lblEmail = e.Item.FindControl("lblEmail") as Label;
            divSuccess.Visible = false;

            if (e.CommandName == "Confirm")
            {
                if (Context.GetOwinContext().GetUserManager<ApplicationUserManager>().ConfirmEmail(hdnUserID.Value, hdnConfirmationToken.Value).Succeeded)
                    lblStatus.Text = "Account Confirmed - " + DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString();
                else
                    lblStatus.Text = "Failed to Confirm account";
                divSuccess.Visible = true;
            }
            if (e.CommandName == "ResendConfrimationEmail")
            {
                var MailConf = new DataAccessComponent().RetrieveWebConfiguration();
                var emTemp = new DataAccessComponent().RetrieveMailTemplate((int)SystemConstants.EmailTemplateType.ProviderWelcomeEmail);
                new BusinessFunctionComponent().ParseEmail(emTemp, hdnUserID.Value, hdnConfirmationToken.Value, (int)SystemConstants.EmailTemplateType.ProviderConfirmationEmail, 0);
                EmailSender.SendEmail(MailConf.SMTPAccount, lblEmail.Text, emTemp.EmailSubject, emTemp.EmailBody, MailConf.SMTPHost, MailConf.SMTPPort, MailConf.SMTPUserName, MailConf.SMTPPassword, MailConf.SMTPSSL, MailConf.SMTPIIS);
                lblStatus.Text = "Email Sent - " + DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString();
                divSuccess.Visible = true;
            }
        }
    }
}