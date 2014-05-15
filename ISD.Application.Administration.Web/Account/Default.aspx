<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="ISD.Administration.Web.UserAccount.Default" %>

<%@ Register Src="../UserControls/AccountManagerUC.ascx" TagName="AccountManagerUC" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content-header">
        <h1>User Account   
                    <small></small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>

            <li class="active">User Account</li>
        </ol>
    </section>

    <!-- Main content -->
    <section class="content">
        <uc1:AccountManagerUC ID="AccountManagerUC1" runat="server" />
    </section>





</asp:Content>
