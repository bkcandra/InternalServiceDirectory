<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HealthyClub.Administration.Web.Settings.Default" %>

<%@ Register Src="../UserControls/WebConfigUC.ascx" TagName="WebConfigUC" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="grid_2">
        <div class="box sidemenu" style="height: 800px">
            <div class="block" id="section-menu">
                <ul class="section menu">
                    <li>
                        <asp:HyperLink ID="hlnkSiteSetting" runat="server" class="menuitem" NavigateUrl="~/Settings/">Site Setting</asp:HyperLink>
                    </li>
                    <li>
                        <asp:HyperLink ID="hlnkNavigation" runat="server" class="menuitem" NavigateUrl="~/Settings/Navigation.aspx">Navigation</asp:HyperLink>
                    </li>
                    <li>
                        <asp:HyperLink ID="hlnkMailTemplate" runat="server" class="menuitem" NavigateUrl="~/Mail/">Mail Template</asp:HyperLink>
                    </li>
                    <li>
                        <asp:HyperLink ID="hlnkSmtpSetting" runat="server" class="menuitem" NavigateUrl="~/Settings/Mailer.aspx">SMTP Setting</asp:HyperLink>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <div class="grid_10">
        <div class="box sidebox">

            <uc1:WebConfigUC ID="WebConfigUC1" runat="server" />

        </div>
    </div>
</asp:Content>
