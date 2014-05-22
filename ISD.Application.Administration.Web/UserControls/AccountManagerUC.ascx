<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountManagerUC.ascx.cs" Inherits="ISD.Administration.Web.UserControls.AccountManagerUC" %>
<%@ Register Src="~/UserControls/AdminList.ascx" TagPrefix="uc1" TagName="AdminList" %>
<%@ Register Src="~/UserControls/ProviderList.ascx" TagPrefix="uc1" TagName="ProviderList" %>
<%@ Register Src="~/UserControls/UserListUC.ascx" TagPrefix="uc1" TagName="UserListUC" %>
<div class="row">
    <div class="col-md-12">
        <!-- Custom Tabs -->
        <div class="nav-tabs-custom">
            <ul class="nav nav-tabs">
                <li class="active"><a data-toggle="tab" href="#tab_1">Administrator Account</a></li>
                <li class=""><a data-toggle="tab" href="#tab_2">Provider Accounts</a></li>
                <li class=""><a data-toggle="tab" href="#tab_3">Member Accounts</a></li>
                <li class="pull-right dropdown">
                    <a class="dropdown-toggle" data-toggle="dropdown" href="#">
                        <i class="fa fa-gear"></i>Action<span class="caret"></span>
                    </a>
                    <ul class="dropdown-menu">
                        <li role="presentation">
                            <asp:LinkButton ID="lnkUserRef" runat="server"  tabindex="-1" role="menuitem" OnClick="lnkUserRef_Click"><span></span> Check User Reference</asp:LinkButton>
                        </li>
                    </ul>
                </li>
            </ul>
            <div class="tab-content">
                <div id="tab_1" class="tab-pane active">
                    <uc1:AdminList ID="AdminList2" runat="server" />
                </div>
                <!-- /.tab-pane -->
                <div id="tab_2" class="tab-pane">
                    <uc1:ProviderList ID="ProviderList2" runat="server" />
                </div>
                <!-- /.tab-pane -->
                <div id="tab_3" class="tab-pane">
                    <uc1:UserListUC ID="UserListUC2" runat="server" />
                </div>
            </div>
            <!-- /.tab-content -->
        </div>
        <!-- nav-tabs-custom -->
    </div>
    <!-- /.col -->
</div>




<asp:Label ID="lblStatus" runat="server" Visible="false"></asp:Label>








