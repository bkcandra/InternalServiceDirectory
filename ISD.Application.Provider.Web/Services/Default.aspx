<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ISD.Provider.Web.Services.Default" %>

<%@ Register Src="~/UserControls/ProviderServicesManagerUC.ascx" TagPrefix="uc1" TagName="ProviderServicesManagerUC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:ProviderServicesManagerUC runat="server" id="ProviderServicesManagerUC" />
</asp:Content>
