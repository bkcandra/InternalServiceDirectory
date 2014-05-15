<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Mailer.aspx.cs" Inherits="ISD.Administration.Web.Settings.Mailer" %>
<%@ Register src="../UserControls/WebMailConfigUC.ascx" tagname="WebMailConfigUC" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
           
              
        <section class="content-header">
        <h1> Mailer Setting
                    <small></small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>

            <li class="active"> Mailer Setting</li>
        </ol>
    </section>

    <!-- Main content -->
    <section class="content">
         <uc1:WebMailConfigUC ID="WebMailConfigUC1" runat="server" />
    </section>
</asp:Content>
