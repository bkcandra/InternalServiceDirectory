﻿using ISD.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ISD.User.Web.UserControls
{
    public partial class ScheduleDetailViewerUC : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void GenerateTimeSlot(List<slot> schedules)
        {
            ListView1.DataSource = schedules;
            ListView1.DataBind();
        }

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Label lblStartTime = e.Item.FindControl("lblStartTime") as Label;
            Label lblEndTime = e.Item.FindControl("lblEndTime") as Label;

            slot timeslot = e.Item.DataItem as slot;

            lblStartTime.Text = (timeslot.startTime.ToString("h:mmtt")).ToLower(); ;
            lblEndTime.Text = (timeslot.EndTime.ToString("h:mmtt")).ToLower();
            if (timeslot.EndTime == SystemConstants.nodate)
                lblEndTime.Text = "Finish";
        }
    }
}