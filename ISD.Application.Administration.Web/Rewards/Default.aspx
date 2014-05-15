<%@ Page Title="All Rewards" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ISD.Administration.Web.Rewards.Default" %>

<%@ Register Src="~/UserControls/RewardsManager.ascx" TagPrefix="uc1" TagName="RewardsManager" %>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="content-header">
        <h1>Rewards
                       
                    <small>HAC Rewards</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Content</a></li>
            <li class="active">Rewards</li>
        </ol>
    </section>

    <!-- Main content -->
    <section class="content">
       <uc1:RewardsManager runat="server" ID="RewardsManager" />
    </section>
    

</asp:Content>

