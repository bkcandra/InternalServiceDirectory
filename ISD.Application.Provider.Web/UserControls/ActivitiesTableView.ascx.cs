using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ISD.DA;
using System.Drawing;
using ISD.Util;
using ISD.BF;

namespace HealthyClub.Providers.Web.UserControls
{
    public partial class ActivitiesTableView : System.Web.UI.UserControl
    {
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

        public String ProviderID
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnProviderID.Value))
                    return hdnProviderID.Value;
                else return Guid.Empty.ToString();
            }
            set { hdnProviderID.Value = value; }
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

        public int page
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnPage.Value))
                    return Convert.ToInt32(hdnPage.Value);
                else return 1;
            }
            set
            {
                hdnPage.Value = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        internal void Refresh()
        {
            ddSort.SelectedValue = SortValue;
            lblKeyword.Visible = false;
            String query = "";

            if (SearchKey != null)
            {
                List<String> SearchPhrase = new BusinessFunctionComponent().RefineSearchKey(SearchKey);

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
                            String[] locs =
                                parameter.Replace(SystemConstants.Location, string.Empty).ToUpper().Split(';');
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
                                dtFrom =
                                    Convert.ToDateTime(SystemConstants.nodate.ToShortDateString() + " " +
                                                       Convert.ToDateTime(times[0]).ToShortTimeString());
                                dtTo =
                                    Convert.ToDateTime(SystemConstants.nodate.ToShortDateString() + " " +
                                                       Convert.ToDateTime(times[1]).ToShortTimeString());
                            }
                            else if (times.Length == 1)
                            {
                                dtFrom =
                                    Convert.ToDateTime(SystemConstants.nodate.ToShortDateString() + " " +
                                                       Convert.ToDateTime(times[0]).ToShortTimeString());
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(query))
                {

                    SetDataSourcebySearchKey(query);
                    int amount = new DataAccessComponent().RetrieveProviderActivitiesbySearchPhraseCount(ProviderID,
                        dtFrom.ToString(), dtTo.ToString(), tmFrom.ToString(), tmTo.ToString(), AgeFrom, AgeTo, SuburbID,
                        CategoryID, query, MonFilter.ToString(), TueFilter.ToString(), WedFilter.ToString(),
                        ThursFilter.ToString(), FriFilter.ToString(), SatFilter.ToString(), SunFilter.ToString());
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

                    lblEndIndex1.Text = lblEndIndex.Text;
                    lblStartIndex1.Text = lblStartIndex.Text;
                    lblAmount1.Text = lblAmount.Text;

                    lblKeyword.Visible = true;
                }
                else
                {
                    SetDataSourcebyProviderCategory();
                    int amount = new DataAccessComponent().RetrieveProviderActivitiesCount(ProviderID, dtFrom.ToString(),
                        dtTo.ToString(), tmFrom.ToString(), tmTo.ToString(), AgeFrom, AgeTo, SuburbID, CategoryID,
                        MonFilter.ToString(), TueFilter.ToString(), WedFilter.ToString(), ThursFilter.ToString(),
                        FriFilter.ToString()
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

                    lblEndIndex1.Text = lblEndIndex.Text;
                    lblStartIndex1.Text = lblStartIndex.Text;
                    lblAmount1.Text = lblAmount.Text;
                }
            }
        }

        private void SetDataSourcebySearchKey(String SearchPhrase)
        {
            ods.TypeName = typeof(DataAccessComponent).FullName;
            ods.EnablePaging = true;
            ods.SelectParameters.Clear();
            ods.SelectParameters.Add("searchKey", SearchPhrase);
            ods.SelectParameters.Add("providerID", ProviderID);
            ods.SelectParameters.Add("ageFrom", AgeFrom.ToString());
            ods.SelectParameters.Add("ageTo", AgeTo.ToString());
            ods.SelectParameters.Add("stFrom", dtFrom.ToString());
            ods.SelectParameters.Add("stTo", dtTo.ToString());
            ods.SelectParameters.Add("tmFrom", tmFrom.ToString());
            ods.SelectParameters.Add("tmTo", tmTo.ToString());
            ods.SelectParameters.Add("suburbID", SuburbID.ToString());
            ods.SelectParameters.Add("categoryID", CategoryID.ToString());

            ods.SelectParameters.Add("MonFilter", MonFilter.ToString());
            ods.SelectParameters.Add("TueFilter", TueFilter.ToString());
            ods.SelectParameters.Add("WedFilter", WedFilter.ToString());
            ods.SelectParameters.Add("ThursFilter", ThursFilter.ToString());
            ods.SelectParameters.Add("FriFilter", FriFilter.ToString());
            ods.SelectParameters.Add("SatFilter", SatFilter.ToString());
            ods.SelectParameters.Add("SunFilter", SunFilter.ToString());

            ods.SelectMethod = "RetrieveProviderActivitiesbySearchPhrase";
            ods.SelectCountMethod = "RetrieveProviderActivitiesbySearchPhraseCount";
            ods.MaximumRowsParameterName = "amount";
            ods.StartRowIndexParameterName = "startIndex";
            ods.SortParameterName = "sortExpression";

            GridViewActivities.DataSourceID = "ods";

            SortProducts();
        }

        private void SetDataSourcebyProviderCategory()
        {
            ods.TypeName = typeof(DataAccessComponent).FullName;
            ods.EnablePaging = true;
            ods.SelectParameters.Clear();
            ods.SelectParameters.Add("categoryID", CategoryID.ToString());
            ods.SelectParameters.Add("providerID", ProviderID.ToString());
            ods.SelectParameters.Add("ageFrom", AgeFrom.ToString());
            ods.SelectParameters.Add("ageTo", AgeTo.ToString());
            ods.SelectParameters.Add("stFrom", dtFrom.ToString());
            ods.SelectParameters.Add("stTo", dtTo.ToString());
            ods.SelectParameters.Add("tmFrom", tmFrom.ToString());
            ods.SelectParameters.Add("tmTo", tmTo.ToString());
            ods.SelectParameters.Add("suburbID", SuburbID.ToString());

            ods.SelectParameters.Add("MonFilter", MonFilter.ToString());
            ods.SelectParameters.Add("TueFilter", TueFilter.ToString());
            ods.SelectParameters.Add("WedFilter", WedFilter.ToString());
            ods.SelectParameters.Add("ThursFilter", ThursFilter.ToString());
            ods.SelectParameters.Add("FriFilter", FriFilter.ToString());
            ods.SelectParameters.Add("SatFilter", SatFilter.ToString());
            ods.SelectParameters.Add("SunFilter", SunFilter.ToString());

            ods.SelectMethod = "RetrieveProviderActivities";
            ods.SelectCountMethod = "RetrieveProviderActivitiesCount";
            ods.MaximumRowsParameterName = "amount";
            ods.StartRowIndexParameterName = "startIndex";
            ods.SortParameterName = "sortExpression";

            GridViewActivities.DataSourceID = "ods";
            SortProducts();
        }

        private void SortProducts()
        {
            if (SortValue == "1")
            {
                GridViewActivities.Sort(SystemConstants.sortLatest, SortDirection.Descending);
            }
            else if (SortValue == "2")
            {
                GridViewActivities.Sort(SystemConstants.sortExpiry, SortDirection.Ascending);
            }
            else if (SortValue == "3")
            {
                GridViewActivities.Sort(SystemConstants.sortExpiry, SortDirection.Descending);
            }
            else if (SortValue == "4")
            {
                GridViewActivities.Sort(SystemConstants.sortName, SortDirection.Ascending);
            }
            else if (SortValue == "5")
            {
                GridViewActivities.Sort(SystemConstants.sortName, SortDirection.Descending);
            }
            else if (SortValue == "6")
            {
                GridViewActivities.Sort(SystemConstants.sortPrice, SortDirection.Ascending);
            }
            else if (SortValue == "7")
            {
                GridViewActivities.Sort(SystemConstants.sortPrice, SortDirection.Descending);
            }
            SetPageSize();

        }

        private void SetPageSize()
        {
            if (PageSize == 10)
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
            GridViewActivities.PageIndex = page - 1;
        }

        protected void ddlPagingTop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPagingTop.SelectedValue == "10")
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
            page = 1;
            Refresh();
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

            SortValue = ddSort.SelectedValue;
            Refresh();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.CommandName))
            {
                if (e.CommandName == "ChangeStatus")
                {
                    ImageButton btnImgStatus = e.CommandSource as ImageButton;
                    GridViewRow row = btnImgStatus.Parent.Parent as GridViewRow;
                    radButtonAct.Checked = radButtonInact.Checked = false;
                    HiddenField hdnActivityID = row.FindControl("hdnActivityID") as HiddenField;

                    var dr = new DataAccessComponent().RetrieveActivity(Convert.ToInt32(hdnActivityID.Value));
                    if (dr != null)
                    {
                        hdnCurrentActID.Value = hdnActivityID.Value;
                        if (dr.Status == (int)SystemConstants.ActivityStatus.Active)
                        {
                            radButtonAct.Checked = true;
                        }
                        else if (dr.Status == (int)SystemConstants.ActivityStatus.Expired)
                        {
                            radButtonInact.Checked = true;
                        }
                        else if (dr.Status == (int)SystemConstants.ActivityStatus.NotActive)
                        {
                            radButtonInact.Checked = true;
                        }
                        else if (dr.Status == (int)SystemConstants.ActivityStatus.WillExpire2)
                        {
                            radButtonAct.Checked = true;
                        }
                        hdnStatus.Value = dr.Status.ToString();
                        ModalPopupExtender1.Show();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowPopUp", "ShowPopUp()", true);
                    }
                }
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            int ID;
            string type;
            int startRow = e.NewPageIndex * PageSize;
            int page = e.NewPageIndex;

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
            if (type == "Category")
            {
                Response.Redirect("~/Activities/Default.aspx?" + SystemConstants.CategoryID + "=" + ID + "&" + SystemConstants.SortValue + "=" + ddSort.SelectedValue + "&" + SystemConstants.PageSize + "=" + PageSize + "&" + SystemConstants.ViewType + "=" + (int)SystemConstants.ActivityViewType.TableView + "&" + SystemConstants.Page + "=" + (page + 1));
            }

            else if (type == "Search")
            {
                Response.Redirect("~/Activities/Default.aspx?" + SystemConstants.SearchKey + "=" + SearchKey + "&" + SystemConstants.SortValue + "=" + SortValue + "&" + SystemConstants.PageSize + "=" + PageSize + "&" + SystemConstants.PageSize + "=" + (int)SystemConstants.ActivityViewType.TableView + "&" + SystemConstants.Page + "=" + (page + 1));
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Label lblNo = e.Row.FindControl("lblNo") as Label;
                //Label lblPhone = e.Row.FindControl("lblPhone") as Label;
                //Label lblSub = e.Row.FindControl("lblSub") as Label;
                //HyperLink HlnkReadMore = e.Row.FindControl("HlnkReadMore") as HyperLink;
                HyperLink HlnkActivitiesName = e.Row.FindControl("HlnkActivitiesName") as HyperLink;
                HiddenField hdnActivityID = e.Row.FindControl("hdnActivityID") as HiddenField;

                HlnkActivitiesName.NavigateUrl = "~/Activity/Default.aspx?" + SystemConstants.route_ActivityID + "=" + hdnActivityID.Value;
                string actName = HlnkActivitiesName.Text;
                actName = actName.Replace(" ", "-");
                HlnkActivitiesName.NavigateUrl = "~/Activity/" + hdnActivityID.Value + "/" + actName;

                //lblPhone.Text = "Tel: " + lblPhone.Text;
                //lblSub.Text = lblSub.Text + ", ";

                HiddenField hdnStatus = e.Row.FindControl("hdnStatus") as HiddenField;
                HiddenField hdnExpiryDate = e.Row.FindControl("hdnExpiryDate") as HiddenField;
                HiddenField hdnType = e.Row.FindControl("hdnType") as HiddenField;

                //Image imgStatus = e.Item.FindControl("imgStatus") as Image;
                Label lblStatus = e.Row.FindControl("lblStatus") as Label;
                Label lblExpiryDate = e.Row.FindControl("lblExpiryDate") as Label;
                Label lblType = e.Row.FindControl("lblType") as Label;
                lblType.ForeColor = Color.Green;
                LinkButton lnkEditAct = e.Row.FindControl("lnkEditAct") as LinkButton;
                lnkEditAct.PostBackUrl = "~/Activity/EditActivity.aspx?" + SystemConstants.ActivityID + "=" + hdnActivityID.Value;


                if (hdnStatus.Value == ((int)SystemConstants.ActivityStatus.NotActive).ToString())
                {
                    lblStatus.Text = "INACTIVE";
                    lblStatus.ForeColor = Color.Green;
                    //imgStatus.ImageUrl = SystemConstants.IconImageUrl + SystemConstants.IconActivityHidden;
                    lblExpiryDate.Text = "";
                }
                else if (hdnStatus.Value == ((int)SystemConstants.ActivityStatus.Active).ToString())
                {
                    lblStatus.Text = "ACTIVE";
                    lblStatus.ForeColor = Color.Gray;
                    //imgStatus.ImageUrl = SystemConstants.IconImageUrl + SystemConstants.IconActivityActive;
                    lblExpiryDate.Text = "Expires on: " + Convert.ToDateTime(hdnExpiryDate.Value).ToShortDateString();
                }
                else if (hdnStatus.Value == ((int)SystemConstants.ActivityStatus.WillExpire2).ToString())
                {
                    lblStatus.Text = "EXPIRES SOON";
                    lblStatus.ForeColor = Color.OrangeRed;
                    //imgStatus.ImageUrl = SystemConstants.IconImageUrl + SystemConstants.IconActivityWillExpire;
                    lblExpiryDate.Text = "Expires on: " + Convert.ToDateTime(hdnExpiryDate.Value).ToShortDateString();
                }
                else if (hdnStatus.Value == ((int)SystemConstants.ActivityStatus.Expired).ToString())
                {
                    lblStatus.Text = "EXPIRED";
                    lblStatus.ForeColor = Color.Red;
                    //imgStatus.ImageUrl = SystemConstants.IconImageUrl + SystemConstants.IconActivityExpired;
                    lblExpiryDate.Text = "Expired on: " + Convert.ToDateTime(hdnExpiryDate.Value).ToShortDateString();
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
        }

        protected void btnExecPopUp_Click(object sender, EventArgs e)
        {
            if (hdnStatus.Value == ((int)SystemConstants.ActivityStatus.Expired).ToString())
            {
                lblAlert.Text = SystemConstants.ActivateExpiredActivity;
                ModalPopupExtender2.Show();
            }
            else
            {
                new DataAccessComponent().ChangeStatus(Convert.ToInt32(hdnCurrentActID.Value), radButtonAct.Checked);
                Refresh();
            }
        }


    }
}