using ISD.BF;
using ISD.DA;
using ISD.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI;
using System.Web.UI.WebControls;

namespace HealthyClub.Administration.Web.UserControls
{
    public partial class RewardLog : System.Web.UI.UserControl
    {
        public SystemConstants.FormMode noteMode
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnNoteMode.Value))
                    return (SystemConstants.FormMode)Convert.ToInt32(hdnNoteMode.Value);
                else
                    return SystemConstants.FormMode.View;
            }
            set
            {
                hdnNoteMode.Value = Convert.ToInt32(value).ToString();

                if (value == SystemConstants.FormMode.New)
                {
                    btnNoteSave.Visible = true;
                    btnNoteCancel.Visible = true;

                    btnNoteEdit.Visible = false;
                    lblStatus.Visible = true;
                }
                else if (value == SystemConstants.FormMode.Edit)
                {
                    btnNoteSave.Visible = true;
                    btnNoteCancel.Visible = true;

                    btnNoteEdit.Visible = false;
                    lblStatus.Visible = true;
                }
                else if (value == SystemConstants.FormMode.View)
                {
                    btnNoteSave.Visible = false;
                    btnNoteCancel.Visible = false;

                    btnNoteEdit.Visible = true;
                    lblStatus.Visible = false;

                    lblStatus.Text = "";
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Refresh();
        }

        private void Refresh()
        {
            SetRewardLogDataSource();
        }

        private void SetRewardLogDataSource()
        {
            GridViewReward.DataSource = new DataAccessComponent().RetrieveLogs((int)SystemConstants.logType.RewardMaintenance);
            GridViewReward.DataBind();
        }

        protected void btnProcessAttendance_Click(object sender, EventArgs e)
        {
            string message = "";
            new BusinessFunctionComponent().RewardsPointMaintenance(Context.User.Identity.Name, out message);
            Refresh();
        }

        protected void GridViewReward_ItemCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.CommandName))
            {
                LinkButton lnkDtails = e.CommandSource as LinkButton;
                GridViewRow row = lnkDtails.Parent.Parent as GridViewRow;
                HiddenField hdnID = row.FindControl("hdnID") as HiddenField;
                HiddenField hdnMessage = row.FindControl("hdnMessage") as HiddenField;
                if (e.CommandName == "ShowDetails")
                {
                    hdnMessage.Value = hdnMessage.Value.Replace("%NL%", Environment.NewLine);
                    txtMessage.Text = hdnMessage.Value;
                    GridViewRewardLogAction.DataSource = new DataAccessComponent().RetrieveLogActions(Convert.ToInt32(hdnID.Value));
                    GridViewRewardLogAction.DataBind();
                }
            }
        }

        protected void GridViewReward_ItemDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnCategory = e.Row.FindControl("hdnCategory") as HiddenField;
                Label lblCategory = e.Row.FindControl("lblCategory") as Label;
                if (hdnCategory.Value == ((int)SystemConstants.logType.ActivityMaintenance).ToString())
                    lblCategory.Text = "Activities Maintenance";
            }
        }

        protected void GridViewRewardLogAction_ItemCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.CommandName))
            {
                LinkButton lnkDtails = e.CommandSource as LinkButton;
                GridViewRow row = lnkDtails.Parent.Parent as GridViewRow;
                HiddenField hdnID = row.FindControl("hdnID") as HiddenField;
                HiddenField hdnMessage = row.FindControl("hdnMessage") as HiddenField;
                if (e.CommandName == "ShowDetails")
                {
                    hdnMessage.Value = hdnMessage.Value.Replace("%NL%", Environment.NewLine);
                    txtMessage.Text = hdnMessage.Value;

                }
            }
        }

        protected void GridViewRewardLogAction_ItemDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnActID = e.Row.FindControl("hdnActID") as HiddenField;
                HiddenField hdnValue = e.Row.FindControl("hdnValue") as HiddenField;

                Label lblCreatedDate = e.Row.FindControl("lblCreatedDate") as Label;
                Label lblUser = e.Row.FindControl("lblUser") as Label;
                Label lblPoints = e.Row.FindControl("lblPoints") as Label;
                Label lblBonusPoints = e.Row.FindControl("lblBonusPoints") as Label;


                Label lblRewardLogCategory = e.Row.FindControl("lblRewardLogCategory") as Label;

                string[] WebLogValue = hdnValue.Value.Split('|');
                // [INT points] | [INT BONUSPOINTS] | [GUID USERID] 
                // ================================================

                lblPoints.Text = WebLogValue[0];
                lblBonusPoints.Text = WebLogValue[1];
                String userID = WebLogValue[2];
                
                lblUser.Text = new DataAccessComponent().RetrieveUserProfiles(userID).Username;

                DateTime exp = Convert.ToDateTime(lblCreatedDate.Text);
                lblCreatedDate.Text = exp.ToShortDateString();
            }
        }

        protected void btnNoteEdit_Click(object sender, EventArgs e)
        {

        }

        protected void btnNoteSave_Click(object sender, EventArgs e)
        {

        }

        protected void btnNoteCancel_Click(object sender, EventArgs e)
        {

        }
    }
}