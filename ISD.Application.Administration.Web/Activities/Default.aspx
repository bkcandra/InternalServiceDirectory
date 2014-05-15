<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ISD.Administration.Web.Activities.Default" %>

<%@ Register Src="~/UserControls/ActivitiesManagerUC.ascx" TagPrefix="uc1" TagName="ActivitiesManagerUC" %>
<%@ Register Src="~/UserControls/ActivitiesManagerListViewUC.ascx" TagPrefix="uc1" TagName="ActivitiesManagerListViewUC" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Activities
                       
                    <small>List of activity in HAC Site</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Content</a></li>
            <li class="active">Activities</li>
        </ol>
    </section>

    <!-- Main content -->
    <section class="content">
        <uc1:ActivitiesManagerListViewUC runat="server" ID="ActivitiesManagerListViewUC" />
    </section>




</asp:Content>
