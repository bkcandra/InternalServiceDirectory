<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProviderServicesManagerUC.ascx.cs" Inherits="ISD.Provider.Web.UserControls.ProviderServicesManagerUC" %>
<%@ Register Src="~/UserControls/ScheduleViewerUC.ascx" TagPrefix="uc1" TagName="ScheduleViewerUC" %>

<div class="breadcrumbs">
    <div class="container">
        <h1 class="pull-left">Services manager</h1>
        <ul class="pull-right breadcrumb">
            <li>
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~">Home</asp:HyperLink></li>
            <li class="active">Services Manager</li>
        </ul>
    </div>
</div>
<div class="container content">
    <div class="breadcrumbs">
    </div>
    <div class="row blog-page">
        <!-- Left Sidebar -->
        <div class="col-md-3">
            <ul class="list-group sidebar-nav-v1" id="sidebar-nav">
                <!-- Service -->
                <li class="list-group-item">
                    <asp:HyperLink ID="lnkNewService" runat="server" Text="Add a service"
                        NavigateUrl="~/Service/NewService"></asp:HyperLink></li>
                <li class="list-group-item">
                    <asp:HyperLink ID="lnkNewClinicians" runat="server" Text="Add a clinician"></asp:HyperLink></li>
                <!-- End Service -->
            </ul>
        </div>
        <!-- End Left Sidebar -->
        <!-- Right Sidebar -->
        <div class="col-md-9">
            <table width="100%">
                <tr>
                    <td align="left" style="width: 50%;">Showing:&nbsp;
            <asp:Label ID="lblStartIndex" runat="server"></asp:Label>&nbsp;-
            <asp:Label ID="lblEndIndex"
                runat="server"></asp:Label>
                        &nbsp;of
            <asp:Label ID="lblAmount" runat="server" Text=""></asp:Label>
                        &nbsp;activities
            <asp:Label ID="lblKeyword" runat="server" CssClass="SearchResults" Visible="false"></asp:Label>
                    </td>
                    <td id="tdPager1" runat="server" align="right" style="width: 50%">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="left" style="width: 70%">Sort by&nbsp;
            <asp:DropDownList ID="ddSort" runat="server" Height="18px" Width="200px"
                OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="True">
                <asp:ListItem Value="1">Newly listed</asp:ListItem>
                <%--<asp:ListItem Value="2">Expiry date</asp:ListItem>--%>
                <asp:ListItem Value="3">Expiry date</asp:ListItem>
                <asp:ListItem Value="4">Title: A - Z</asp:ListItem>
                <asp:ListItem Value="5">Title: Z - A</asp:ListItem>
                <asp:ListItem Value="6">Cost: Free - Paid</asp:ListItem>
                <asp:ListItem Value="7">Cost: Paid - Free</asp:ListItem>
            </asp:DropDownList>
                    </td>
                    <td valign="top" align="right" style="width: 30%">
                        <span>Activities per page&nbsp;</span>
                        <asp:DropDownList ID="ddlPagingTop" runat="server" OnSelectedIndexChanged="ddlPagingTop_SelectedIndexChanged" Style="height: 24px"
                            AutoPostBack="true">
                            <asp:ListItem>5</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>20</asp:ListItem>
                            <asp:ListItem>50</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;
                    </td>
                </tr>
            </table>
            <asp:ListView ID="ServicesListView" runat="server" OnItemDataBound="ServicesListView_ItemDataBound">
                <LayoutTemplate>
                    <div id="ItemPlaceHolder" runat="server">
                    </div>
                </LayoutTemplate>
                <ItemTemplate>

                    <div class="funny-boxes funny-boxes-top-sea ">
                        <div class="row" style="text-align: right; float: right">
                            <div class="col-md-12">
                                <asp:LinkButton ID="lnkDeleteAct" runat="server" CausesValidation="False"
                                    CommandName="DeleteAct" Text="Delete" CssClass="btn btn-danger btn-sm"
                                    OnClientClick="return confirm('Are you sure you want to delete this activity?  Once you have deleted an activity it will be removed in three days and cannot be recovered.')" />&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkEditAct" runat="server" CssClass="btn btn-warning btn-sm right">Edit</asp:LinkButton>
                                <asp:HiddenField ID="hdnType" runat="server" Value='<%#Eval("ActivityType") %>' />
                                <asp:HiddenField ID="hdnStatus" runat="server" Value='<%#Eval("Status") %>' />
                                <asp:HiddenField ID="hdnExpiryDate" runat="server" Value='<%#Eval("ExpiryDate") %>' />
                                <asp:HiddenField ID="hdnisApproved" runat="server" Value='<%#Eval("isApproved") %>' />
                                <asp:HiddenField ID="hdnActivityID" runat="server" Value='<%#Eval("ID") %>' />
                                <asp:HiddenField ID="hdnSuburb" runat="server" Value='<%#Eval("SuburbID") %>' />
                                <asp:HiddenField ID="HiddenField3" runat="server" Value='<%#Eval("StateID") %>' />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4 funny-boxes-img">
                                <asp:Image ID="imgServ" runat="server" class="img-responsive" ImageUrl="~/Assets/img/demo.jpg" />
                                <ul class="list-unstyled">
                                    <li><i class="fa fa-briefcase"></i><%# Eval("ProviderName") %></li>
                                    <li><i class="fa fa-map-marker"></i>
                                        <%#Eval("Address") %>, 

                                        <asp:Label ID="lblSub" runat="server" Text='<%# Eval("Suburb") %>'></asp:Label>
                                        <asp:Label ID="lblState" runat="server" Text='<%#Eval("StateName") %>'></asp:Label>
                                        <%#Eval("Postcode") %>
                                    </li>
                                </ul>
                            </div>
                            <div class="col-md-8">
                                <h2>
                                    <asp:HyperLink ID="HlnkActivitiesName" Text='<%#Eval("Name") %>'
                                        runat="server"></asp:HyperLink>
                                </h2>
                                <p>
                                    <asp:Label ID="lblShortDescription" runat="server" Text='<%#Eval("FullDescription").ToString().Length > 250 ? Eval("FullDescription").ToString().Substring(0,250) : Eval("FullDescription").ToString()%>'></asp:Label>
                                    <asp:HyperLink ID="HlnkReadMore" NavigateUrl="#" runat="server">..read more</asp:HyperLink>
                                </p>
                                <p>
                                    <uc1:ScheduleViewerUC runat="server" ID="ScheduleViewerUC" />
                                </p>
                                <p>
                                    <asp:Image ID="imgStatus" runat="server" class="img-responsive right" ImageUrl="~/Assets/img/demo.jpg" />
                                    <asp:Label ID="lblExpiryDate" runat="server" CssClass="right" Text="Expiry on"></asp:Label>
                                    <asp:Label ID="lblStatus" runat="server" Text="" CssClass="right"></asp:Label>
                                    <asp:Label ID="lblType" runat="server" Text="" CssClass="right"></asp:Label>
                                </p>
                            </div>
                            
                        </div>
                    </div>
                </ItemTemplate>

            </asp:ListView>
            <div id="divPager" runat="server">
                <asp:Label ID="lblPageBottom" runat="server" Text="Page:"></asp:Label>
                <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ServicesListView">
                    <Fields>
                        <asp:NextPreviousPagerField ButtonType="Link" PreviousPageText="◄ Previous" ShowFirstPageButton="false"
                            ShowLastPageButton="false" ShowNextPageButton="false" ShowPreviousPageButton="true" />
                        <asp:NumericPagerField />
                        <asp:NextPreviousPagerField ButtonType="Link" NextPageText="Next ►" ShowFirstPageButton="false"
                            ShowLastPageButton="false" ShowNextPageButton="true" ShowPreviousPageButton="false" />
                    </Fields>
                </asp:DataPager>
            </div>
            <div id="ItemCountBottom" runat="server" style="width: 100%; padding: 0.2em 0.5em;">
                Showing:
                <asp:Label ID="lblStartIndexBottom" runat="server"></asp:Label>&nbsp;-&nbsp;<asp:Label ID="lblEndIndexbottom"
                    runat="server"></asp:Label>
                &nbsp;of
                <asp:Label ID="lblAmountBottom" runat="server" Text=""></asp:Label>
                &nbsp;activities
          
            </div>
            <!-- Bordered Funny Boxes -->
        </div>
        <!-- End Right Sidebar -->
    </div>
</div>
<asp:HiddenField ID="hdnSearchKey" runat="server" />
<asp:HiddenField ID="hdnSortValue" runat="server" />
<asp:HiddenField ID="hdnStartRow" runat="server" />
<asp:HiddenField ID="hdnProviderID" runat="server" />
<asp:HiddenField ID="hdnCategoryID" runat="server" />
<asp:HiddenField ID="hdnSuburbID" runat="server" />
<asp:HiddenField ID="hdnAgeFrom" runat="server" />
<asp:HiddenField ID="hdnAgeTo" runat="server" />
<asp:HiddenField ID="hdnPageSize" runat="server" />
<asp:HiddenField ID="hdnDateFrom" runat="server" />
<asp:HiddenField ID="hdnDateTo" runat="server" />
<asp:HiddenField ID="hdnTmTo" runat="server" />
<asp:HiddenField ID="hdnTmFrom" runat="server" />
<asp:HiddenField ID="hdnFiltered" runat="server" />
<asp:HiddenField ID="hdnMonFiltered" runat="server" />
<asp:HiddenField ID="hdnTueFiltered" runat="server" />
<asp:HiddenField ID="hdnWedFiltered" runat="server" />
<asp:HiddenField ID="hdnThuFiltered" runat="server" />
<asp:HiddenField ID="hdnFriFiltered" runat="server" />
<asp:HiddenField ID="hdnSatFiltered" runat="server" />
<asp:HiddenField ID="hdnSavedList" runat="server" />
<asp:HiddenField ID="hdnSunFiltered" runat="server" />
<asp:HiddenField ID="hdnTimespan" runat="server" />
<asp:ObjectDataSource ID="ods" runat="server"></asp:ObjectDataSource>
