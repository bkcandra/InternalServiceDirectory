<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ISD.Administration.Web.Log.Default" %>

<%@ Register Src="~/UserControls/webLog.ascx" TagName="webLog" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/RewardLog.ascx" TagPrefix="uc1" TagName="RewardLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Content/themes/redmond/jquery-ui.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () {
            $("#tabs").tabs();
        });
    </script>
    <div class="grid_2">
        <div class="box sidemenu" style="height: 800px">
            <div class="block" id="section-menu">
                <ul class="section menu">
                    <li><a class="menuitem">Menu</a>
                        <ul class="submenu">
                        </ul>
                    </li>

                </ul>
            </div>
        </div>
    </div>
    <div class="grid_10">
        <div class="box sidebox" style="height: 1000px">
            <h2>Web Log</h2>
            <div class="block">
                <div id="tabs">
                    <ul>
                        <li><a href="#ActLogTab">Activity Log</a></li>
                        <li><a href="#RewardLogTab">Reward Log</a></li>
                    </ul>
                    <div id="ActLogTab" class="tabs3">
                        <p>
                            <uc1:webLog ID="webLog1" runat="server" />
                    </div>
                    <div id="RewardLogTab" class="tabs3">
                        <p>
                            <uc1:RewardLog ID="RewardLog1" runat="server" />
                    </div>

                </div>



            </div>
        </div>
    </div>

</asp:Content>
