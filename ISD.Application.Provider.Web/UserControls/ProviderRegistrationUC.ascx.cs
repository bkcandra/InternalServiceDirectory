using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ISD.DA;
using ISD.EDS;
using System.Web.Services;
using ISD.Provider.Web;
using ISD.Provider.Web.Models;
using ISD.Util;

using ISD.BF;

using System.Data;
using BCUtility;
using System.Text.RegularExpressions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace HealthyClub.Providers.Web.UserControls
{
    public partial class ProviderRegistrationUC : System.Web.UI.UserControl
    {

        bool isSupported
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnisSupported.Value))
                    return Convert.ToBoolean(hdnisSupported.Value);
                else return false;
            }
            set
            {
                hdnisSupported.Value = value.ToString();
                ProviderImageUpload.isSupported = isSupported;
            }
        }

        bool EnableRecaptcha
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnEnableRecap.Value))
                    return Convert.ToBoolean(hdnEnableRecap.Value);
                else return false;
            }
            set
            {
                hdnEnableRecap.Value = value.ToString();
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            var web = new DataAccessComponent().RetrieveWebConfiguration();
            EnableRecaptcha = web.EnableCaptcha;
            if (EnableRecaptcha)
            {
                RecaptchaControl1.PublicKey = System.Configuration.ConfigurationManager.AppSettings[SystemConstants.reCaptchaPublicKey].ToString();
                RecaptchaControl1.PrivateKey = System.Configuration.ConfigurationManager.AppSettings[SystemConstants.reCaptchaPrivateKey].ToString();

            }
            else
            {
                trCaptcha.Visible = EnableRecaptcha;
            }
            isSupported = CheckisBrowserSupported();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                setRegistrationDDL();
                ProviderImageUpload.Name = "Organisation Logo";
            }
            string password = Password.Text;
            Password.Attributes.Add("value", password);

            string verifyPassword = txtPasswordVerify.Text;
            txtPasswordVerify.Attributes.Add("value", verifyPassword);
        }

        private void setRegistrationDDL()
        {
            DataSetComponent.StateDataTable dtState = new DataAccessComponent().RetrieveStates();

            ddlState.Items.Clear();

            ListItem lis = new ListItem("State", "0");
            ddlState.Items.Add(lis);
            foreach (var dr in dtState)
            {
                string name = "";

                if (dr.ID != 0)
                    name = dr.StateName;

                lis = new ListItem(name, dr.ID.ToString());
                ddlState.Items.Add(lis);
            }

        }

        private DataSetComponent.ProviderProfilesRow GetRegistrationData()
        {
            //Reference wizard's controls
            //CreateUserWizardStep ProviderProfiles = CreateNewProvider.FindControl("ProviderProfiles") as CreateUserWizardStep;
            DataSetComponent.ProviderProfilesRow dr = new DataSetComponent.ProviderProfilesDataTable().NewProviderProfilesRow();


            dr.ProviderBranch = txtBranch.Text;
            dr.Username = Username.Text;
            dr.Title = Convert.ToInt32(ddlTitle.SelectedValue);
            dr.FirstName = txtFirstName.Text;
            dr.MiddleName = "";
            dr.SecondarySuburb = getSecondarySuburb();
            if (string.IsNullOrEmpty(txtLastName.Text))
                dr.LastName = "";
            dr.LastName = txtLastName.Text;
            dr.Email = Email.Text.ToLower();
            dr.Address = txtAddress1.Text;
            dr.Suburb = txtSuburb.Text;
            dr.StateID = Convert.ToInt32(ddlState.SelectedValue);
            dr.PhoneNumber = txtContactNumber.Text;
            dr.MobileNumber = txtMobileNumber.Text;
            dr.Aggreement = true;
            dr.ProviderName = txtCompany.Text;
            dr.PostCode = Convert.ToInt32(txtPostCode.Text);
            dr.AccountDeletion = false;
            if (radiobyEMail.Checked)
            {
                dr.PreferredContact = 1;
            }
            else if (radiobyMail.Checked)
            {
                dr.PreferredContact = 3;
            }
            else if (radiobyPhone.Checked)
            {
                dr.PreferredContact = 2;
            }

            dr.CreatedBy = dr.ModifiedBy = Username.Text;
            dr.CreatedDatetime = dr.ModifiedDatetime = DateTime.Now;
            return dr;

        }

        private string getSecondarySuburb()
        {
            String secondarySuburb = "";
            string separator = "|";
            int rowIndex = 0;

            if (ViewState["SuburbTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["SuburbTable"];

                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        TextBox suburb = (TextBox)GridViewSuburb.Rows[rowIndex].Cells[0].FindControl("txtSuburb");
                        rowIndex++;
                        if (String.IsNullOrEmpty(secondarySuburb))
                        {
                            if (!String.IsNullOrEmpty(suburb.Text))
                                secondarySuburb = suburb.Text;
                        }
                        else
                            secondarySuburb += separator + suburb.Text;
                    }
                }
            }
            return secondarySuburb;
        }

        protected void CreateUser(String userID)
        {

            DataSetComponent.ProviderProfilesRow dr = GetRegistrationData();
            dr.UserID = userID;


            if (ProviderImageUpload.isImageValid())
            {
                var userImage = ProviderImageUpload.GetUserImage();
                var userImageDetail = ProviderImageUpload.GetUserImageDetail();
                new BusinessFunctionComponent().CreateNewUserImage(userID, userImage, userImageDetail);
            }
            else
            {
                new BusinessFunctionComponent().CreateEmptyUserImage(userID);
            }

            BusinessFunctionComponent bfc = new BusinessFunctionComponent();
            DataAccessComponent dac = new DataAccessComponent();
            dac.InsertNewProviderProfiles(dr);

            var drRef = new DataSetComponent.UserReferenceDataTable().NewUserReferenceRow();
            drRef.UserId = userID;
            drRef.ReferenceID = bfc.GenerateRefID(dr.ProviderName);
            dac.insertNewUserReference(drRef);
        }

        protected void ValidateUser(out bool error)
        {
            if (EnableRecaptcha)
            {
                RecaptchaControl1.Validate();
                if (!RecaptchaControl1.IsValid)
                {
                    if (!string.IsNullOrEmpty(lblError.Text))
                        lblError.Text += "</br>";
                    lblError.Text += "The security code you entered is not correct.";
                    error = true;
                    lblError.Visible = true;
                }
            }
            error = false;
            lblError.Text = "";


            var reg = @"^.*(?=.{6,18})(?=.*\d)(?=.*[a-zA-Z]).*$";
            var match = Regex.Match(Password.Text, reg, RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                lblError.Text = "Password requirements not met";
                error = true;
            }

            //if (ddlTitle.SelectedValue == "0")
            //{
            //    if (!string.IsNullOrEmpty(lblError.Text))
            //        lblError.Text += "</br>";
            //    lblError.Text += "Title is required.";
            //    error = true;
            //    lblError.Visible = true;
            //}


            if (ddlState.SelectedValue == "0")
            {
                if (!string.IsNullOrEmpty(lblError.Text))
                    lblError.Text += "</br>";
                lblError.Text += "address is required.";
                lblError.Visible = true;
                error = true;
            }

            //if (WebSecurity.UserExists(Username.Text))
            //{
            //    if (!string.IsNullOrEmpty(lblError.Text))
            //        lblError.Text += "</br>";
            //    lblError.Text += SystemConstants.ErrorUsernameTaken;
            //    error = true;
            //    lblError.Visible = true;
            //}
            if (new DataAccessComponent().ProviderNameExist(txtCompany.Text))
            {
                if (!string.IsNullOrEmpty(lblError.Text))
                    lblError.Text += "</br>";
                lblError.Text += SystemConstants.ErrorOrganisationNameTaken;
                error = true;
                lblError.Visible = true;
            }
            if (new DataAccessComponent().isEmailAddressExist(Email.Text.ToLower()))
            {
                if (!string.IsNullOrEmpty(lblError.Text))
                    lblError.Text += "</br>";
                lblError.Text += SystemConstants.ErrorEmailAddressTaken;
                error = true;
                lblError.Visible = true;
            }
            var emailReg = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            var emailmatch = Regex.Match(Email.Text, emailReg, RegexOptions.IgnoreCase);

            if (!emailmatch.Success)
            {
                if (!string.IsNullOrEmpty(lblError.Text))
                    lblError.Text += "</br>";
                lblError.Text += SystemConstants.ErrorInvalidEmail;
                error = true;
                lblError.Visible = true;
            }


        }

        protected void StepNextButton_Click(object sender, EventArgs e)
        {
            bool error = false;
            ValidateUser(out error);
            if (!error)
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = new ApplicationUser() { UserName = Username.Text, Email = Email.Text };


                RoleManager<IdentityRole> rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
                if (!rm.RoleExists(SystemConstants.AdministratorRole))
                    rm.Create(new IdentityRole(SystemConstants.AdministratorRole));
                manager.AddToRole(user.Id, SystemConstants.AdministratorRole);

                string token = manager.GenerateEmailConfirmationToken(user.Id);
                string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(token, user.Id);
                //manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");



                String userID = user.Id;

                if (userID != Guid.Empty.ToString())
                    CreateUser(userID);

                SendConfirmationEmail(userID, token);

                RegistrationStep.Visible = false;
                CompleteStep.Visible = true;

            }
            divError.Visible = error;

        }

        private void SendConfirmationEmail(string userID, string token)
        {
            var MailConf = new DataAccessComponent().RetrieveWebConfiguration();
            var emTemp = new DataAccessComponent().RetrieveMailTemplate((int)SystemConstants.EmailTemplateType.ProviderWelcomeEmail);

            new BusinessFunctionComponent().ParseEmail(emTemp, userID, token, (int)SystemConstants.EmailTemplateType.ProviderConfirmationEmail, 0);

            EmailSender.SendEmail(MailConf.SMTPAccount, Email.Text, emTemp.EmailSubject, emTemp.EmailBody, MailConf.SMTPHost, MailConf.SMTPPort, MailConf.SMTPUserName, MailConf.SMTPPassword, MailConf.SMTPSSL, MailConf.SMTPIIS);
        }

        protected void ContinueButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Activities/");
        }

        protected void HomeButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/Login");
        }

        private void AddNewRowToGrid()
        {
            int rowIndex = 0;
            if (ViewState["SuburbTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["SuburbTable"];
                DataRow drCurrentRow = null;

                if (dtCurrentTable.Rows.Count > 0 && dtCurrentTable.Rows.Count != 2)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        //extract the TextBox values
                        TextBox box1 = (TextBox)GridViewSuburb.Rows[rowIndex].Cells[0].FindControl("txtSuburb");

                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["Column1"] = box1.Text;

                        rowIndex++;
                    }

                    //add new row to DataTable
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    //Store the current data to ViewState
                    ViewState["SuburbTable"] = dtCurrentTable;

                    //Rebind the Grid with the current data
                    GridViewSuburb.DataSource = dtCurrentTable;
                    GridViewSuburb.DataBind();
                }
                else
                {
                    lblErrorSuburb.Text = "Maximum number of suburbs is three";
                }
            }
            else
            {
                DataTable dt = new DataTable();
                DataRow dr = null;
                dt.Columns.Add(new DataColumn("Column1", typeof(string)));

                dr = dt.NewRow();
                dr["Column1"] = string.Empty;
                dt.Rows.Add(dr);

                //Store the DataTable in ViewState
                ViewState["SuburbTable"] = dt;

                GridViewSuburb.DataSource = dt;
                GridViewSuburb.DataBind();
            }

            //Set Previous Data on Postbacks
            SetPreviousData();
        }

        private void SetPreviousData()
        {

            int rowIndex = 0;
            if (ViewState["SuburbTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["SuburbTable"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        TextBox box1 = (TextBox)GridViewSuburb.Rows[rowIndex].Cells[0].FindControl("txtSuburb");
                        box1.Text = dt.Rows[i]["Column1"].ToString();
                        rowIndex++;

                    }
                }
            }
        }

        protected async void Username_TextChanged(object sender, EventArgs e)
        {
            if (Username.Text.Length >= 3)
            {
                var mgr = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await mgr.FindByNameAsync(Username.Text);
                if (user == null)
                    lblUsernameError.Visible = false;
                else lblUsernameError.Visible = true;
            }

        }

        protected void txtCompany_TextChanged(object sender, EventArgs e)
        {
            if (Username.Text.Length >= 3)
            {
                lblOrganisationName.Visible = new DataAccessComponent().ProviderNameExist(txtCompany.Text);
                lblOrganisationName.Text = SystemConstants.ErrorOrganisationNameTaken;
            }
        }

        private bool CheckisBrowserSupported()
        {
            var browser = Request.Browser;
            var version = browser.MajorVersion;
            var name = browser.Browser;

            if (name == "Chrome" && version < SystemConstants.browserChromeVersion)
                return false;
            else if (name == "IE" && version < SystemConstants.browserIEVersion)
                return false;
            else if (name == "Firefox" && version < SystemConstants.browserFirefoxVersion)
                return false;
            else if (name == "Opera" && version < SystemConstants.browserOperaVersion)
                return false;
            else if (name == "Safari" && version < SystemConstants.browserSafariVersion)
                return false;
            else
                return true;
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            AddNewRowToGrid();
        }

        protected void ImageButton1_Click(object sender, EventArgs e)
        {
            AddNewRowToGrid();
        }

        protected void Email_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Email.Text))
            {
                ReqEmailAddress.Visible = true;
                ReqEmailAddress.Validate();
            }
            else
            {
                ReqEmailAddress.Visible = false;
                if (Email.Text.Length >= 5)
                    lblEmailTaken.Visible = new DataAccessComponent().isEmailAddressExist(Email.Text.ToLower());


                var emailReg = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                var match = Regex.Match(Email.Text, emailReg, RegexOptions.IgnoreCase);

                if (!match.Success)
                {
                    lblInvalidEmail.Visible = true;
                }
                else
                    lblInvalidEmail.Visible = false;
            }
        }

        protected void ddlTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckTitle();
        }

        private void CheckTitle()
        {
            if (ddlTitle.SelectedValue == "0")
            {
                txtFirstName_TextBoxWatermarkExtender.WatermarkText = "Name";
                txtLastName.Text = "";
                txtLastName.Visible = false;
                txtFirstName.Width = Unit.Pixel(400);
            }
            else
            {
                txtFirstName_TextBoxWatermarkExtender.WatermarkText = "Given Name";
                txtLastName.Visible = true;
                txtFirstName.Width = Unit.Pixel(200);
            }
        }


    }
}