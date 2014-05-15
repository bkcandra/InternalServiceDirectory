﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SuburbSetup.aspx.cs" Inherits="ISD.Administration.Web.Suburb.SuburbSetup" %>

<%@ Register Src="../UserControls/SuburbSetupUC.ascx" TagName="SuburbSetupUC" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content-header">
        <h1>Suburb
                       
                    <small>Create, Edit and Delete suburb</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Content Setting</a></li>
            <li class="active">Suburb</li>
        </ol>
    </section>

    <!-- Main content -->
    <section class="content">
        <uc1:SuburbSetupUC ID="SuburbSetupUC1" runat="server" />
    </section>



</asp:Content>
