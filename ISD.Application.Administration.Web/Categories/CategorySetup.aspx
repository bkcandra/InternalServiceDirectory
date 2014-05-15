<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CategorySetup.aspx.cs" Inherits="ISD.Administration.Web.Category.CategoriesSetup" %>

<%@ Register Src="../UserControls/CategoriesSetupUC.ascx" TagName="CategoriesSetupUC" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <section class="content-header">
        <h1>Category setup
                       
                    <small>Create, edit or delete a category</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Categories</a></li>
            <li class="active">Setup</li>
        </ol>
    </section>

    <!-- Main content -->
    <section class="content">
        <uc1:CategoriesSetupUC ID="CategoriesSetupUC1" runat="server" />
    </section>
           
        
</asp:Content>
