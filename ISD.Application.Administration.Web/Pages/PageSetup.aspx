<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PageSetup.aspx.cs" Inherits="ISD.Administration.Web.Pages.PageSetup" %>

<%@ Register Src="../UserControls/PageSetupUC.ascx" TagName="PageSetupUC" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <section class="content-header">
        <h1>Page Template     
                    <small></small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>

            <li class="active">Pages</li>
        </ol>
    </section>

    <!-- Main content -->
    <section class="content">
         <uc1:PageSetupUC ID="PageSetupUC1" runat="server" />
    </section>
   
</asp:Content>
