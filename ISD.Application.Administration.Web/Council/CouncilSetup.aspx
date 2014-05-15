﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CouncilSetup.aspx.cs" Inherits="ISD.Administration.Web.Council.CouncilSetup" %>

<%@ Register Src="~/UserControls/CouncilSetupUC.ascx" TagPrefix="uc1" TagName="CouncilSetupUC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
        <section class="content-header">
        <h1>Council
                       
                    <small>HAC Council</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Content Setting</a></li>
            <li class="active">Council</li>
        </ol>
    </section>

    <!-- Main content -->
    <section class="content">
       <uc1:CouncilSetupUC runat="server" id="CouncilSetupUC" />
    </section>

    
</asp:Content>
