<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ISD.Administration.Web.Council.Default" %>

<%@ Register Src="~/UserControls/CouncilUC.ascx" TagPrefix="uc1" TagName="CouncilUC" %>


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
        <uc1:CouncilUC runat="server" id="CouncilUC" />
    </section>
</asp:Content>
