<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ISD.Administration.Web._Default" %>

<%@ Register Src="~/UserControls/DashboardSummaryUC.ascx" TagPrefix="uc1" TagName="DashboardSummaryUC" %>



<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Dashboard
                       
                    <small>Control panel</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
            <li class="active">Dashboard</li>
        </ol>
    </section>

    <!-- Main content -->
    <section class="content">
        <uc1:DashboardSummaryUC runat="server" id="DashboardSummaryUC" />
    </section>
</asp:Content>
