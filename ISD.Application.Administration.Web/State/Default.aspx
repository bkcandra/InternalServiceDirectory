<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ISD.Administration.Web.State.Default" %>

<%@ Register Src="../UserControls/StateUC.ascx" TagName="StateUC" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
             
             <section class="content-header">
        <h1>State
                       
                    <small></small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Content Setting</a></li>
            <li class="active">State</li>
        </ol>
    </section>

    <!-- Main content -->
    <section class="content">
         <uc1:StateUC ID="StateUC1" runat="server" />
    </section>
              
         

</asp:Content>
