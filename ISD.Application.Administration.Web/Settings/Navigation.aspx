<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Navigation.aspx.cs" Inherits="ISD.Administration.Web.Settings.Navigation" %>

<%@ Register Src="../UserControls/MenuNavigationUC.ascx" TagName="MenuNavigationUC"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <section class="content-header">
        <h1>Navigation
                    <small></small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>

            <li class="active">Navigation Setting</li>
        </ol>
    </section>

    <!-- Main content -->
    <section class="content">
        <uc2:MenuNavigationUC ID="MenuNavigationUC1" runat="server" />
    </section>



</asp:Content>
