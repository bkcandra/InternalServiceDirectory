﻿<%@ Page Title="Add Attendance" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OCR.aspx.cs" Inherits="ISD.Administration.Web.Rewards.OCR" %>

<%@ Register Src="../UserControls/AttendanceCSVReader.ascx" TagName="AttendanceCSVReader" TagPrefix="uc1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="grid_2">
        <div class="box sidemenu" style="height: 800px">
            <div class="block" id="section-menu">
                <ul class="section menu">
                    <li><a class="menuitem" href="Default.aspx">Rewards Management</a>
                        <ul class="submenu">
                            <li><a href="Dashboard.aspx">Dashboard</a></li>
                            <li><a href="Add.aspx">Add Reward</a> </li>
                            <li><a href="OCR.aspx">Add Activity Attendance</a></li>
                            <li><a href="Sponsor.aspx">Sponsors Page</a></li>


                        </ul>
                    </li>

                </ul>
            </div>
        </div>
    </div>

    <uc1:AttendanceCSVReader ID="AttendanceCSVReader1" runat="server" />

</asp:Content>




<%--    Please browse a valid BMP or TIFF image file and press&nbsp; a
    button to check the output. <strong>Note</strong>: Arial and Times New Roman (16pt and 32
    pt) fonts are supported.&nbsp;<br />
    
    <table>
        <tr>
            <td align ="left">
                <asp:TextBox ID ="txtname" runat="server" />
            </td>
            <td align="left">
                            <asp:Button ID="storename" runat="server" Text="Add Name" OnClick="storename_Click" />
                </td>

        </tr>
        <tr>
            <td align="left">
                            <asp:Button ID="btnExtractText" runat="server" Text="Extract Text" OnClick="btnExtractText_Click" />
                </td>
        </tr>
        <tr>
            <td align="left" style="height: 30px; color: red;"><%=Session["outMessage"] %>
    <% Session["outMessage"] = ""; %>
            </td>
        </tr>
        <tr>
            <td align="left">
                Extracted OCR Text</td>
        </tr>
        <tr>
            <td align="left" style="width: 100px">
                <asp:TextBox ID="txtExtractedText" runat="server" Height="200px" ReadOnly="True"
                    TextMode="MultiLine" Width="500px"></asp:TextBox></td>
        </tr>
    </table>--%>
