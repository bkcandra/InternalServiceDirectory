using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ISD.Util;
using ISD.DA;

namespace HealthyClub.Administration.Web.UserControls
{
    public partial class MailTemplateListUC : System.Web.UI.UserControl
    {
        public int EmailType
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnEmailType.Value))
                    return Convert.ToInt32(hdnEmailType.Value);
                else return 1;
            }
            set
            {
                hdnEmailType.Value = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Refresh();
        }

        private void Refresh()
        {
            SetDataSource();
            SetHyperlink();
        }

        private void SetHyperlink()
        {
            if (EmailType == 1)
                hlnkNewTemplate.Text = "New Template";

            hlnkNewTemplate.NavigateUrl = "~/Mail/EmailTemplate.aspx?" + SystemConstants.EmailType + "=" + EmailType;
        }

        private void SetDataSource()
        {
            ods.TypeName = typeof(DataAccessComponent).FullName;
            ods.SelectParameters.Clear();
            ods.SelectParameters.Add("EmailType", EmailType.ToString());
            ods.SelectMethod = "RetrieveMailTemplates";
            ods.EnablePaging = true;
            ods.SelectCountMethod = "RetrieveMailTemplatesCount";
            ods.StartRowIndexParameterName = "startIndex";
            ods.MaximumRowsParameterName = "amount";

            gridView1.DataSourceID = "ods";
            gridView1.DataBind();
        }

        protected void gridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteDynamicTemplate" || e.CommandName == "EditDynamicTemplate")
            {
                var lnkEdit = e.CommandSource as Control;
                GridViewRow Row = lnkEdit.Parent.Parent as GridViewRow;
                HiddenField hdnEmailTemplateID = Row.FindControl("hdnEmailTemplateID") as HiddenField;
                bool isUsed = false;

                if (e.CommandName == "DeleteDynamicTemplate")
                {
                    var dt = new DataAccessComponent().RetrieveEmailSettings();
                    foreach (var dr in dt)
                    {
                        if (dr.EmailTemplateID == Convert.ToInt32(hdnEmailTemplateID.Value))
                            isUsed = true;
                    }

                    if (!isUsed)
                    {
                        new DataAccessComponent().DeleteEmailTemplate(Convert.ToInt32(hdnEmailTemplateID.Value));
                        Refresh();
                    }
                    else if (isUsed)
                    {
                        lblWarning.Visible = true;
                        lblWarning.Text = "Cannot delete selected template. You are currently using this template in one or more setting. Consider to change this setting before you delete this template";
                    }
                }
                else if (e.CommandName == "EditDynamicTemplate")
                {
                    Response.Redirect("~/Mail/EmailTemplate.aspx?" + SystemConstants.TemplateID + "=" + hdnEmailTemplateID.Value + "&" + SystemConstants.EmailType + "=" + EmailType);
                }
            }
        }

    }
}