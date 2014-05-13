<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CategorySetup.aspx.cs" Inherits="HealthyClub.Administration.Web.Category.CategoriesSetup" %>

<%@ Register Src="../UserControls/CategoriesSetupUC.ascx" TagName="CategoriesSetupUC" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="grid_2">
        <div class="box sidemenu" style="height: 800px">
            <div class="block" id="section-menu">
                <ul class="section menu">
                    <li><a class="menuitem">Management</a>
                        <ul class="submenu">
                            <li><a onclick="history.go(-1);">Back </a></li>
                        </ul>
                    </li>

                </ul>
            </div>
        </div>
    </div>
    <div class="grid_10">
        <div class="box sidebox">
            <uc1:CategoriesSetupUC ID="CategoriesSetupUC1" runat="server" />
        </div>
    </div>
</asp:Content>
