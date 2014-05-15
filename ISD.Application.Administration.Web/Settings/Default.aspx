<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ISD.Administration.Web.Settings.Default" %>

<%@ Register Src="../UserControls/WebConfigUC.ascx" TagName="WebConfigUC" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
     <section class="content-header">
        <h1>General Setting
                    <small></small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>

            <li class="active">General Setting</li>
        </ol>
    </section>

    <!-- Main content -->
    <section class="content">
        <uc1:WebConfigUC ID="WebConfigUC1" runat="server" />
    </section>
    
            

        
</asp:Content>
