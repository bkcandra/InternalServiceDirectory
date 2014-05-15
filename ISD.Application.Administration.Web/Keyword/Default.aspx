<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ISD.Administration.Web.Keyword.Default" %>

<%@ Register Src="../UserControls/KeywordManagementUC.ascx" TagName="KeywordManagementUC" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
               
             <section class="content-header">
        <h1>Keyword
                       
                    <small>Thesaurus or synonim setting</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Content Setting</a></li>
            <li class="active">Keywords</li>
        </ol>
    </section>

    <!-- Main content -->
    <section class="content">
       <uc1:KeywordManagementUC ID="KeywordManagementUC1" runat="server" />
    </section>
</asp:Content>


