<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountManagerUC.ascx.cs" Inherits="ISD.Administration.Web.UserControls.AccountManagerUC" %>
<%@ Register Src="~/UserControls/AdminList.ascx" TagPrefix="uc1" TagName="AdminList" %>
<%@ Register Src="~/UserControls/ProviderList.ascx" TagPrefix="uc1" TagName="ProviderList" %>
<%@ Register Src="~/UserControls/UserListUC.ascx" TagPrefix="uc1" TagName="UserListUC" %>
<link href="../Content/themes/redmond/jquery-ui.css" rel="stylesheet" />

<asp:LinkButton ID="lnkUserRef" runat="server" class="btn-icon btn-navy btn-person" OnClick="lnkUserRef_Click"><span></span> Check User Reference</asp:LinkButton>
<br />
<asp:Label ID="lblStatus" runat="server" Visible="false"></asp:Label>
<br />
<script>
    $(document).ready(function () {
        $("#tabs").tabs();
    });
</script>
<div id="tabs">
    <ul>
        <li><a href="#AdminTab">Administrator Account</a></li>
        <li><a href="#ProviderTab">Provider Accounts</a></li>
        <li><a href="#MemberTab">Customer Accounts</a></li>
    </ul>
    <div id="AdminTab" class="tabs3">
        <p>
            <uc1:AdminList ID="AdminList2" runat="server" />
    </div>
    <div id="ProviderTab" class="tabs3">
        <p>
            <uc1:ProviderList ID="ProviderList2" runat="server" />
    </div>
    <div id="MemberTab" class="tabs3">
        <p>
            <uc1:UserListUC ID="UserListUC2" runat="server" />
    </div>
</div>

