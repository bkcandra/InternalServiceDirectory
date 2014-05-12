<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ISD.User.Web.Test.Default" %>
<%@ Register src="../UserControls/SavedListUC.ascx" tagname="SavedListUC" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:SavedListUC ID="SavedListUC1" runat="server" />
</asp:Content>
