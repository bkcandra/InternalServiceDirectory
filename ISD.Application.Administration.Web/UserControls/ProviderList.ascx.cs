using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BCUtility;
using Google.Apis.Requests;
using ISD.Administration.Web;
using ISD.Administration.Web.Models;
using ISD.EDS;
using ISD.DA;


using ISD.Util;
using ISD.BF;
using ISD.Util;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ISD.Administration.Web.UserControls
{
    public partial class ProviderList : System.Web.UI.UserControl
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
            ods.EnablePaging = true;
            ods.MaximumRowsParameterName = "amount";
            ods.StartRowIndexParameterName = "startIndex";
            ods.SelectMethod = "RetrieveProviderList";
            ods.SelectCountMethod = "RetrieveProviderListCount";


            listviewActivities.DataSourceID = "ods";
            listviewActivities.DataBind();

        }

        private DataSetComponent.ProviderProfilesDataTable GetSelected()
        {
            var dt = new DataSetComponent.ProviderProfilesDataTable();

            foreach (var item in listviewActivities.Items)
            {
                CheckBox chkSelected = item.FindControl("chkSelected") as CheckBox;


                if (chkSelected.Checked)
                {
                    Label lblEmail = item.FindControl("lblEmail") as Label;
                    HyperLink hlnkUserName = item.FindControl("hlnkUserName") as HyperLink;
                    HiddenField hdnUserID = item.FindControl("hdnUserID") as HiddenField;
                    var dr = dt.NewProviderProfilesRow();
                    dr.Username = hlnkUserName.Text;
                    dr.UserID = hdnUserID.Value;
                    dt.AddProviderProfilesRow(dr);
                }
            }
            return dt;
        }

        protected async void lnkDelete_Click(object sender, EventArgs e)
        {
            var mgr =Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var dt = GetSelected();
            foreach (var dr in dt)
            {
                ApplicationUser usr = mgr.FindByName(dr.Username);
                await mgr.DeleteAsync(usr);
            }
            Refresh();
        }


        protected void listviewActivities_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            HiddenField hdnConfirmationToken = e.Item.FindControl("hdnConfirmationToken") as HiddenField;
            HiddenField hdnUserID = e.Item.FindControl("hdnUserID") as HiddenField;
            HyperLink hlnkUserName = e.Item.FindControl("hlnkUserName") as HyperLink;
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

        protected void listviewActivities_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            var mgr = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

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


                hlnkUserName.NavigateUrl = hlnkEdit.NavigateUrl = "~/User/Provider.aspx?" + SystemConstants.ProviderID + "=" + hdnUserID.Value;
                hdnConfirmationToken.Value = IdentityHelper.GetCodeFromRequest(Request);
            }
        }
    }
}

/*
 <asp:GridView ID="gridview1" runat="server" AutoGenerateColumns="False" Width="100%"
                OnRowCommand="gridview1_RowCommand" DataSourceID="ods" AllowPaging="True"
                CellPadding="4" ForeColor="#333333" GridLines="None"
                OnPageIndexChanged="gridview1_PageIndexChanged" OnRowDataBound="gridview1_RowDataBound">
                <EditRowStyle BackColor="#2461BF" />
                <EmptyDataTemplate>
                    Customer Data Not Found
                </EmptyDataTemplate>
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelected" runat="server" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Username">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkUserName" runat="server" CommandName="Details" Text='<%#Eval("Username") %>'></asp:LinkButton>
                            <asp:HiddenField ID="hdnUserID" runat="server" Value='<%#Eval("UserID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkName" runat="server" CommandName="Details" Text='<%#Eval("FirstName") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Email">
                        <ItemTemplate>
                            <asp:Image ID="imgEmailIcon" runat="server" />
                            <asp:Label ID="lblEmail" runat="server" Text='<%#Eval("Email") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Phone">
                        <ItemTemplate>
                            <asp:Label ID="lblJoinDate" runat="server" Text='<%#Eval("PhoneNumber") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
            </asp:GridView>
*/