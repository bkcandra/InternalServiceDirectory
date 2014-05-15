<%@ Page Title="Pages list" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ISD.Administration.Web.Pages.Default" %>

<%@ Register Src="../UserControls/PagesUC.ascx" TagName="PagesUC" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content-header">
        <h1>Pages     
                    <small></small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>

            <li class="active">Pages</li>
        </ol>
    </section>

    <!-- Main content -->
    <section class="content">
        <asp:HyperLink ID="hlnkNewPage" runat="server" Text="New Page"></asp:HyperLink></li>
          <uc1:PagesUC ID="PagesUC1" runat="server" />
    </section>


</asp:Content>
