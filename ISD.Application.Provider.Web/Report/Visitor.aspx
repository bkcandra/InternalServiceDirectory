<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Visitor.aspx.cs" Inherits="HealthyClub.Provider.Web.Report.Visitor" %>

<%@ Register Src="~/UserControls/VisitorStatsUC.ascx" TagPrefix="uc1" TagName="VisitorStatsUC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:VisitorStatsUC runat="server" ID="VisitorStatsUC" />
</asp:Content>
