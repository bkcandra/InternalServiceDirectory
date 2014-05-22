using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using HealthyClub.Providers.Web.UserControls;
using ISD.BF;
using ISD.DA;
using ISD.Util;
using Microsoft.AspNet.Identity;

namespace ISD.Provider.Web.UserControls
{
    public partial class ProviderServicesManagerUC : System.Web.UI.UserControl
    {
        public delegate void SectionHandler(int ID, int startRow, string type, string sortValue, int pageSize, string searchKey);
        public event ActivitiesListview.SectionHandler RefreshActivitiesSection;

        public int actNo = 0;

        public int CategoryID
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnCategoryID.Value))
                    return Convert.ToInt32(hdnCategoryID.Value);
                else return 0;
            }
            set
            {
                hdnCategoryID.Value = value.ToString();
            }
        }

        public String ProviderId
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnProviderID.Value))
                    return hdnProviderID.Value;
                else return Guid.Empty.ToString();
            }
            set { hdnProviderID.Value = value; }
        }

        public int AgeFrom
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnAgeFrom.Value))
                    return Convert.ToInt32(hdnAgeFrom.Value);
                else return 0;
            }
            set
            {
                hdnAgeFrom.Value = value.ToString();
            }
        }

        public int AgeTo
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnAgeTo.Value))
                    return Convert.ToInt32(hdnAgeTo.Value);
                else return 99;
            }
            set
            {
                hdnAgeTo.Value = value.ToString();
            }
        }

        public bool MonFilter
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnMonFiltered.Value))
                    return Convert.ToBoolean(hdnMonFiltered.Value);
                else return true;
            }
            set
            {
                hdnMonFiltered.Value = value.ToString();
            }
        }

        public bool TueFilter
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnTueFiltered.Value))
                    return Convert.ToBoolean(hdnTueFiltered.Value);
                else return true;
            }
            set
            {
                hdnTueFiltered.Value = value.ToString();
            }
        }

        public bool WedFilter
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnWedFiltered.Value))
                    return Convert.ToBoolean(hdnWedFiltered.Value);
                else return true;
            }
            set
            {
                hdnWedFiltered.Value = value.ToString();
            }
        }

        public bool ThursFilter
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnThuFiltered.Value))
                    return Convert.ToBoolean(hdnThuFiltered.Value);
                else return true;
            }
            set
            {
                hdnThuFiltered.Value = value.ToString();
            }
        }

        public bool FriFilter
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnFriFiltered.Value))
                    return Convert.ToBoolean(hdnFriFiltered.Value);
                else return true;
            }
            set
            {
                hdnFriFiltered.Value = value.ToString();
            }
        }

        public bool SatFilter
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnSatFiltered.Value))
                    return Convert.ToBoolean(hdnSatFiltered.Value);
                else return true;
            }
            set
            {
                hdnSatFiltered.Value = value.ToString();
            }
        }

        public string SuburbID
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnSuburbID.Value))
                    return hdnSuburbID.Value;
                else return "0";
            }
            set
            {
                hdnSuburbID.Value = value.ToString();
            }
        }

        public bool SunFilter
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnSunFiltered.Value))
                    return Convert.ToBoolean(hdnSunFiltered.Value);
                else return true;
            }
            set
            {
                hdnSunFiltered.Value = value.ToString();
            }
        }

        public DateTime dtFrom
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnDateFrom.Value))
                    return Convert.ToDateTime(hdnDateFrom.Value);
                else return SystemConstants.nodate;
            }
            set
            {
                hdnDateFrom.Value = value.ToString();
            }
        }

        public DateTime dtTo
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnDateTo.Value))
                    return Convert.ToDateTime(hdnDateTo.Value);
                else return SystemConstants.nodate;
            }
            set
            {
                hdnDateTo.Value = value.ToString();
            }
        }

        public TimeSpan tmFrom
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnTmFrom.Value))
                    return Convert.ToDateTime(hdnTmFrom.Value).TimeOfDay;
                else return SystemConstants.nodate.TimeOfDay;
            }
            set
            {
                hdnTmFrom.Value = value.ToString();
            }
        }

        public TimeSpan tmTo
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnTmTo.Value))
                    return Convert.ToDateTime(hdnTmTo.Value).TimeOfDay;
                else return SystemConstants.nodate.TimeOfDay;
            }
            set
            {
                hdnTmTo.Value = value.ToString();
            }
        }
        public int StartRow
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnStartRow.Value))
                    return Convert.ToInt32(hdnStartRow.Value);
                else return 0;
            }
            set
            {
                hdnStartRow.Value = value.ToString();
                actNo = StartRow;
            }
        }

        public int PageSize
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnPageSize.Value))
                    return Convert.ToInt32(hdnPageSize.Value);
                else return 10;
            }
            set
            {
                hdnPageSize.Value = value.ToString();
            }
        }

        public string SortValue
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnSortValue.Value))
                    return hdnSortValue.Value;
                else return "1";
            }
            set
            {
                hdnSortValue.Value = value;

            }
        }

        public string SearchKey
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnSearchKey.Value))
                    return hdnSearchKey.Value;
                else return null;
            }
            set
            {
                hdnSearchKey.Value = value;

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                ProviderId = Context.User.Identity.GetUserId();
                Refresh();
            }
            else Response.Redirect("~");
        }

        protected void Page_Init(object sender, EventArgs e)
        {

            if (Request.QueryString[SystemConstants.StartRow] != null)
            {
                StartRow = Convert.ToInt32(Request.QueryString[SystemConstants.StartRow]);
            }


            if (Request.QueryString[SystemConstants.CategoryID] != null)
            {
                CategoryID = Convert.ToInt32(Request.QueryString[SystemConstants.CategoryID]);
            }

            if (Request.QueryString[SystemConstants.SortValue] != null)
            {
                SortValue = Request.QueryString[SystemConstants.SortValue];
            }
            if (Request.QueryString[SystemConstants.SearchKey] != null)
            {
                SearchKey = Request.QueryString[SystemConstants.SearchKey];
            }

            if (Request.QueryString[SystemConstants.PageSize] != null)
            {
                PageSize = Convert.ToInt32(Request.QueryString[SystemConstants.PageSize]);
            }


        }

        public void Refresh()
        {
            ddSort.SelectedValue = SortValue;
            lblKeyword.Visible = false;
            String query = "";


            if (!string.IsNullOrEmpty(SearchKey))
            {
                List<String> parameters = new BusinessFunctionComponent().RefineSearchKey(SearchKey);

                foreach (var parameter in parameters)
                {
                    if (!string.IsNullOrEmpty(parameter))
                    {
                        if (parameter.StartsWith(SystemConstants.Query))
                        {
                            query = parameter.Replace(SystemConstants.Query, string.Empty);
                        }
                        else if (parameter.StartsWith(SystemConstants.Location))
                        {
                            String[] locs = parameter.Replace(SystemConstants.Location, string.Empty).ToUpper().Split(';');
                            var subDT = new DataAccessComponent().RetrieveSuburbs();

                            var suburbs = subDT.Where(x => locs.Contains(x.Name.ToUpper()));

                            foreach (var sub in suburbs)
                            {
                                if (String.IsNullOrEmpty(SuburbID))
                                    SuburbID = sub.ID.ToString();
                                else
                                {
                                    SuburbID += "|" + sub.ID.ToString();
                                }
                            }
                        }
                        else if (parameter.StartsWith(SystemConstants.Day))
                        {
                            MonFilter = TueFilter = WedFilter = ThursFilter = FriFilter = SatFilter = SunFilter = false;
                            string[] days = parameter.Replace(SystemConstants.Day, string.Empty).Split(';');
                            foreach (var day in days)
                            {
                                if (day.ToUpper().Equals(DayOfWeek.Monday.ToString().ToUpper()))
                                    MonFilter = true;
                                if (day.ToUpper().Equals(DayOfWeek.Tuesday.ToString().ToUpper()))
                                    TueFilter = true;
                                if (day.ToUpper().Equals(DayOfWeek.Wednesday.ToString().ToUpper()))
                                    WedFilter = true;
                                if (day.ToUpper().Equals(DayOfWeek.Thursday.ToString().ToUpper()))
                                    ThursFilter = true;
                                if (day.ToUpper().Equals(DayOfWeek.Friday.ToString().ToUpper()))
                                    FriFilter = true;
                                if (day.ToUpper().Equals(DayOfWeek.Saturday.ToString().ToUpper()))
                                    SatFilter = true;
                                if (day.ToUpper().Equals(DayOfWeek.Sunday.ToString().ToUpper()))
                                    SunFilter = true;
                            }
                        }
                        else if (parameter.StartsWith(SystemConstants.Time))
                        {
                            string[] times = parameter.Replace(SystemConstants.Time, string.Empty).Split('-');
                            if (times.Length == 2)
                            {
                                dtFrom = Convert.ToDateTime(SystemConstants.nodate.ToShortDateString() + " " + Convert.ToDateTime(times[0]).ToShortTimeString());
                                dtTo = Convert.ToDateTime(SystemConstants.nodate.ToShortDateString() + " " + Convert.ToDateTime(times[1]).ToShortTimeString());
                            }
                            else if (times.Length == 1)
                            {
                                dtFrom = Convert.ToDateTime(SystemConstants.nodate.ToShortDateString() + " " + Convert.ToDateTime(times[0]).ToShortTimeString());
                            }
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(query))
            {

                SetDataSourcebySearchKey(query);
                int amount = new DataAccessComponent().RetrieveProviderActivitiesbySearchPhraseCount(ProviderId, dtFrom.ToString(), dtTo.ToString(), tmFrom.ToString(), tmTo.ToString(), AgeFrom, AgeTo, SuburbID, CategoryID, query, MonFilter.ToString(), TueFilter.ToString(), WedFilter.ToString(), ThursFilter.ToString(), FriFilter.ToString(), SatFilter.ToString(), SunFilter.ToString());
                lblAmount.Text = amount.ToString();
                if (Convert.ToInt32(lblAmount.Text) <= Convert.ToInt32(PageSize + StartRow))
                {
                    lblEndIndex.Text = lblAmount.Text;
                }
                else
                {
                    lblEndIndex.Text = (StartRow + PageSize).ToString();
                }

                lblStartIndex.Text = (StartRow + 1).ToString();

                if (Convert.ToInt32(lblStartIndex.Text) >= Convert.ToInt32(lblEndIndex.Text))
                {
                    lblStartIndex.Text = lblEndIndex.Text;
                }

                lblEndIndexbottom.Text = lblEndIndex.Text;
                lblStartIndexBottom.Text = lblStartIndex.Text;
                lblAmountBottom.Text = lblAmount.Text;

                lblKeyword.Visible = true;
                if (amount == 0)
                {
                    ItemCountBottom.Visible = false;
                }
                else
                {
                    if (amount <= PageSize)
                    {
                        divPager.Visible = false;
                        ItemCountBottom.Visible = true;
                    }
                    else
                        divPager.Visible = ItemCountBottom.Visible = true;
                }

            }
            else
            {
                SetDataSourcebyProviderCategory();
                int amount = new DataAccessComponent().RetrieveProviderActivitiesCount(ProviderId, dtFrom.ToString(), dtTo.ToString(), tmFrom.ToString(), tmTo.ToString(), AgeFrom, AgeTo, SuburbID, CategoryID, MonFilter.ToString(), TueFilter.ToString(), WedFilter.ToString(), ThursFilter.ToString(), FriFilter.ToString()
                    , SatFilter.ToString(), SunFilter.ToString());
                lblAmount.Text = amount.ToString();

                if (Convert.ToInt32(lblAmount.Text) <= Convert.ToInt32(PageSize + StartRow))
                {
                    lblEndIndex.Text = lblAmount.Text;
                }
                else
                {
                    lblEndIndex.Text = (StartRow + PageSize).ToString();
                }

                lblStartIndex.Text = (StartRow + 1).ToString();

                if (Convert.ToInt32(lblStartIndex.Text) >= Convert.ToInt32(lblEndIndex.Text))
                {
                    lblStartIndex.Text = lblEndIndex.Text;
                }

                lblEndIndexbottom.Text = lblEndIndex.Text;
                lblStartIndexBottom.Text = lblStartIndex.Text;
                lblAmountBottom.Text = lblAmount.Text;


                if (amount == 0)
                {
                    ItemCountBottom.Visible = false;
                }
                else
                {
                    if (amount <= PageSize)
                    {
                        divPager.Visible = false;
                        ItemCountBottom.Visible = true;
                    }
                    else
                        divPager.Visible = ItemCountBottom.Visible = true;
                }

            }
        }

        private void SetDataSourcebySearchKey(String SearchPhrase)
        {
            ods.TypeName = typeof(DataAccessComponent).FullName;
            ods.EnablePaging = true;
            ods.SelectParameters.Clear();
            ods.SelectParameters.Add("searchKey", SearchPhrase);
            ods.SelectParameters.Add("providerID", ProviderId.ToString());
            ods.SelectMethod = "RetrieveProviderActivitiesbySearchPhrase";
            ods.SelectCountMethod = "RetrieveProviderActivitiesbySearchPhraseCount";
            ods.MaximumRowsParameterName = "amount";
            ods.StartRowIndexParameterName = "startIndex";
            ods.SortParameterName = "sortExpression";

            ServicesListView.DataSourceID = "ods";

            SortProducts();
        }

        private void SetDataSourcebyProviderCategory()
        {
            ods.TypeName = typeof(DataAccessComponent).FullName;
            ods.EnablePaging = true;
            ods.SelectParameters.Clear();
            ods.SelectParameters.Add("categoryID", CategoryID.ToString());
            ods.SelectParameters.Add("providerID", ProviderId.ToString());
            ods.SelectMethod = "RetrieveProviderActivities";
            ods.SelectCountMethod = "RetrieveProviderActivitiesCount";
            ods.MaximumRowsParameterName = "amount";
            ods.StartRowIndexParameterName = "startIndex";
            ods.SortParameterName = "sortExpression";

            ServicesListView.DataSourceID = "ods";
            SortProducts();
        }

        private void SortProducts()
        {
            if (SortValue == "1")
            {
                ServicesListView.Sort(SystemConstants.sortLatest, SortDirection.Descending);
            }
            else if (SortValue == "2")
            {
                ServicesListView.Sort(SystemConstants.sortExpiry, SortDirection.Ascending);
            }
            else if (SortValue == "3")
            {
                ServicesListView.Sort(SystemConstants.sortExpiry, SortDirection.Descending);
            }
            else if (SortValue == "4")
            {
                ServicesListView.Sort(SystemConstants.sortName, SortDirection.Ascending);
            }
            else if (SortValue == "5")
            {
                ServicesListView.Sort(SystemConstants.sortName, SortDirection.Descending);
            }
            else if (SortValue == "6")
            {
                ServicesListView.Sort(SystemConstants.sortPrice, SortDirection.Ascending);
            }
            else if (SortValue == "7")
            {
                ServicesListView.Sort(SystemConstants.sortPrice, SortDirection.Descending);
            }
            DataPager1.SetPageProperties(StartRow, PageSize, false);
            SetPageSize();

        }

        private void SetPageSize()
        {
            if (PageSize == 5)
            {
                ddlPagingTop.SelectedValue = "5";
            }
            else if (PageSize == 10)
            {
                ddlPagingTop.SelectedValue = "10";
            }
            else if (PageSize == 20)
            {
                ddlPagingTop.SelectedValue = "20";
            }
            else if (PageSize == 50)
            {
                ddlPagingTop.SelectedValue = "50";

            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortValue = ddSort.SelectedValue;
            Refresh();
        }

        protected void ddlPagingTop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPagingTop.SelectedValue == "5")
            {
                PageSize = 5;
            }
            else if (ddlPagingTop.SelectedValue == "10")
            {
                PageSize = 10;
            }
            else if (ddlPagingTop.SelectedValue == "20")
            {
                PageSize = 20;
            }
            else if (ddlPagingTop.SelectedValue == "50")
            {
                PageSize = 50;
            }
            // Refresh();
            Response.Redirect("~/Activities/Default.aspx?" + SystemConstants.SearchKey + "=" + SearchKey + "&" + SystemConstants.ViewType + "=" + (int)SystemConstants.ActivityViewType.ListView + "&" + SystemConstants.PageSize + "=" + PageSize);
        }

        protected void ServicesListView_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            LinkButton lnkDeleteAct = e.CommandSource as LinkButton;
            ListViewItem item = lnkDeleteAct.Parent as ListViewItem;

            if (e.CommandName == "DeleteAct")
            {
                string Username = Context.User.Identity.Name;
                if (string.IsNullOrEmpty(Username))
                    Username = "ERR_GETUSR";
                HiddenField hdnActivityID = item.FindControl("hdnActivityID") as HiddenField;
                int actID = Convert.ToInt32(hdnActivityID.Value);
                if (actID != 0 || actID != -1)
                {
                    //new DataAccessComponent().DeleteActivity(actID);
                    new DataAccessComponent().ChangeStatus(actID, (int)SystemConstants.ActivityStatus.Deleting, Username);
                }
            }

            //SortValue = ddSort.SelectedValue;
            Refresh();
        }

        protected void ServicesListView_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            if (DataPager1.StartRowIndex == 0)
                DataPager1.Fields[0].Visible = false;

            int ID;
            string type;
            int startRow = e.StartRowIndex;

            if (SearchKey != null)
            {
                ID = 0;
                type = "Search";
            }
            else
            {
                ID = CategoryID;
                type = "Category";
            }
            if (RefreshActivitiesSection != null)
            {
                RefreshActivitiesSection(ID, startRow, type, ddSort.SelectedValue, PageSize, SearchKey);
            }
            Refresh();

        }

        protected void ServicesListView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            actNo++;
            Label lblPhone = e.Item.FindControl("lblPhone") as Label;
            Label lblSub = e.Item.FindControl("lblSub") as Label;
            Label lblAddress = e.Item.FindControl("lblAddress") as Label;
            Label lblState = e.Item.FindControl("lblState") as Label;
            Label lblPostCode = e.Item.FindControl("lblPostCode") as Label;
            Label lblStatus = e.Item.FindControl("lblStatus") as Label;
            Label lblExpiryDate = e.Item.FindControl("lblExpiryDate") as Label;
            Label lblType = e.Item.FindControl("lblType") as Label;
            Label lblShortDescription = e.Item.FindControl("lblShortDescription") as Label;
            HtmlGenericControl divDescription = e.Item.FindControl("divDescription") as HtmlGenericControl;
            LinkButton lnkEditAct = e.Item.FindControl("lnkEditAct") as LinkButton;

            HyperLink HlnkReadMore = e.Item.FindControl("HlnkReadMore") as HyperLink;
            HyperLink HlnkActivitiesName = e.Item.FindControl("HlnkActivitiesName") as HyperLink;
            HiddenField hdnActivityID = e.Item.FindControl("hdnActivityID") as HiddenField;
            HiddenField hdnStatus = e.Item.FindControl("hdnStatus") as HiddenField;
            HiddenField hdnisApproved = e.Item.FindControl("hdnisApproved") as HiddenField;
            HiddenField hdnExpiryDate = e.Item.FindControl("hdnExpiryDate") as HiddenField;
            HiddenField hdnType = e.Item.FindControl("hdnType") as HiddenField;
            HiddenField hdnModified = e.Item.FindControl("hdnModified") as HiddenField;

            System.Web.UI.WebControls.Image imgStatus = e.Item.FindControl("imgStatus") as System.Web.UI.WebControls.Image;

            lnkEditAct.PostBackUrl = "~/Activity/EditActivity.aspx?" + SystemConstants.ActivityID + "=" + hdnActivityID.Value;

            string actName = HlnkActivitiesName.Text;
            actName = actName.Replace(" ", "-");
            HlnkActivitiesName.NavigateUrl = HlnkReadMore.NavigateUrl = "~/Activity/" + hdnActivityID.Value + "/" + actName;

            //lblPhone.Text = "Tel: " + lblPhone.Text;

            if (Regex.IsMatch(lblShortDescription.Text, @"([a-zA-Z]){20,}"))
                divDescription.Attributes.Add("class", "breaking");

            ScheduleViewerUC ScheduleViewerUC1 = e.Item.FindControl("ScheduleViewerUC") as ScheduleViewerUC;

            ScheduleViewerUC1.ActivityID = Convert.ToInt32(hdnActivityID.Value);
            ScheduleViewerUC1.timetableFormat = (int)SystemConstants.TimetableFormat.Seasonal;

            if (string.IsNullOrEmpty(lblSub.Text))
            {
                lblAddress.Visible = false;
                lblSub.Visible = false;
                lblState.Visible = false;
                lblPostCode.Visible = false;

            }
            lblSub.Text = lblSub.Text + ", ";

            lblType.ForeColor = Color.Green;
            imgStatus.ToolTip = "This activity is ";

            //Lets handle to icon first
            if (hdnStatus.Value == ((int)SystemConstants.ActivityStatus.Deleting).ToString())
            {
                imgStatus.ImageUrl = SystemConstants.IconImageUrl + "grey.png";
                imgStatus.ToolTip = "This activity will be deleted";
            }
            else
            {
                if (Convert.ToBoolean(hdnisApproved.Value))
                {
                    imgStatus.ToolTip += "Approved ";

                    if (hdnStatus.Value == ((int)SystemConstants.ActivityStatus.NotActive).ToString())
                    {
                        imgStatus.ImageUrl = SystemConstants.IconImageUrl + "grey.png";
                        imgStatus.ToolTip += "";
                    }
                    else if (hdnStatus.Value == ((int)SystemConstants.ActivityStatus.Active).ToString())
                    {
                        imgStatus.ImageUrl = SystemConstants.IconImageUrl + "green.png";
                        imgStatus.ToolTip += "";
                    }
                    else if (hdnStatus.Value == ((int)SystemConstants.ActivityStatus.WillExpire2).ToString())
                    {
                        imgStatus.ImageUrl = SystemConstants.IconImageUrl + "amber.png";
                        imgStatus.ToolTip += " and expiring.";
                    }
                    else if (hdnStatus.Value == ((int)SystemConstants.ActivityStatus.Expired).ToString())
                    {
                        imgStatus.ImageUrl = SystemConstants.IconImageUrl + "red.png";
                        imgStatus.ToolTip += " and expired.";
                    }

                }
                else
                {
                    imgStatus.ToolTip += "awaiting approval ";
                    imgStatus.ImageUrl = SystemConstants.IconImageUrl + "amber.png";
                    if (hdnStatus.Value == ((int)SystemConstants.ActivityStatus.NotActive).ToString())
                        imgStatus.ToolTip += "";
                    else if (hdnStatus.Value == ((int)SystemConstants.ActivityStatus.Active).ToString())
                        imgStatus.ToolTip += "";
                    else if (hdnStatus.Value == ((int)SystemConstants.ActivityStatus.WillExpire2).ToString())
                        imgStatus.ToolTip += " and expiring.";
                    else if (hdnStatus.Value == ((int)SystemConstants.ActivityStatus.Expired).ToString())
                    {
                        imgStatus.ImageUrl = SystemConstants.IconImageUrl + "red.png";
                        imgStatus.ToolTip += " and expired.";
                    }
                }
            }
            if (hdnStatus.Value == ((int)SystemConstants.ActivityStatus.NotActive).ToString())
            {
                lblStatus.Text = "INACTIVE";
                lblStatus.ForeColor = Color.Gray;
                lblExpiryDate.Text = "Activity inactive";

            }
            else if (hdnStatus.Value == ((int)SystemConstants.ActivityStatus.Active).ToString())
            {
                lblStatus.Text = "ACTIVE";
                lblStatus.ForeColor = Color.Green;
                lblExpiryDate.Text = "Expires on:<br/>" + Convert.ToDateTime(hdnExpiryDate.Value).ToShortDateString();
            }
            else if (hdnStatus.Value == ((int)SystemConstants.ActivityStatus.WillExpire2).ToString())
            {
                lblStatus.Text = "EXPIRES SOON";
                lblStatus.ForeColor = Color.OrangeRed;
                lblExpiryDate.Text = "Expires on:<br/>" + Convert.ToDateTime(hdnExpiryDate.Value).ToShortDateString();
            }
            else if (hdnStatus.Value == ((int)SystemConstants.ActivityStatus.Expired).ToString())
            {
                lblStatus.Text = "EXPIRED";
                lblStatus.ForeColor = Color.Red;
                lblExpiryDate.Text = "Expired on:<br/>" + Convert.ToDateTime(hdnExpiryDate.Value).ToShortDateString();
            }
            else if (hdnStatus.Value == ((int)SystemConstants.ActivityStatus.Deleting).ToString())
            {
                lblStatus.Text = "DELETING";
                lblStatus.ForeColor = Color.Gray;
                DateTime deleted = Convert.ToDateTime(hdnModified.Value);
                deleted = deleted.AddDays(3);
                lblExpiryDate.Text = "Deleted on:<br/>" + deleted.ToShortDateString();
            }



            if (hdnType.Value == ((int)SystemConstants.ActivityFeeCategory.Private_Free).ToString() || hdnType.Value == ((int)SystemConstants.ActivityFeeCategory.Public_Free).ToString())
            {
                lblType.Text = "FREE ACTIVITY";
                lblType.ForeColor = Color.Green;
            }
            else if (hdnType.Value == ((int)SystemConstants.ActivityFeeCategory.Private_Paid).ToString() || hdnType.Value == ((int)SystemConstants.ActivityFeeCategory.Public_Paid).ToString())
            {
                lblType.Text = "PAID ACTIVITY";
                lblType.ForeColor = Color.Blue;
            }
        }

        protected void ServicesListView_ItemDeleted(object sender, ListViewDeletedEventArgs e)
        {

        }

        protected void ServicesListView_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {

        }

        
    }
}