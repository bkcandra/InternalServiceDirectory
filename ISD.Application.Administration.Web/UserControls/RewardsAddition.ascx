<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RewardsAddition.ascx.cs" Inherits="ISD.Administration.Web.UserControls.RewardsAddition" %>
<link href="../Content/themes/base/jquery-ui.css" rel="stylesheet" />
<script language="javascript">
    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (((charCode > 47) && (charCode < 58)) || (charCode == 8))
            return true;

        return false;
    }

    $(function () {
        $("#datepicker").datepicker();
    });


</script>
<div id="divError" runat="server" class="message error" visible="false">
    <h5>Error!</h5>
    <p>
        <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
    </p>
</div>
<div id="divSuccess" runat="server" class="message success" visible="false">
    <h5>Success!</h5>
    <p>
        <asp:Label ID="lblSuccess" runat="server" Text=""></asp:Label>
    </p>
</div>
<div id="addreward" style="font-weight: bold;">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="form">
                <tr>
                    <td class="col1">
                        <label>
                            Rewards Name</label></td>
                    <td class="col2">
                        <asp:TextBox ID="rname" runat="server" CssClass="medium"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;" class="col1">
                        <label>
                            Reward Description</label>
                    </td>
                    <td>
                        <asp:TextBox ID="rdesc" runat="server" Height="50px" Width="209px" TextMode="MultiLine" CssClass="large"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Reward Expiry Date</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCalendarFrom" runat="server" Width="80px" CssClass="medium"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="txtCalendarFrom_CalendarExtender" runat="server" Format="dd/MM/yyyy" TargetControlID="txtCalendarFrom">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:TextBoxWatermarkExtender ID="txtCalendarFrom_TextBoxWatermarkExtender" runat="server"
                            TargetControlID="txtCalendarFrom" WatermarkText="ExpiryDate">
                        </ajaxToolkit:TextBoxWatermarkExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Reward Type</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRewType" runat="server" OnSelectedIndexChanged="ddlRewType_SelectedIndexChanged" AutoPostBack="True" Width="200px">
                            <asp:ListItem Selected="True" Value="0">Select Type</asp:ListItem>
                        </asp:DropDownList>

                    </td>
                </tr>
                <tr id="tr4" runat="server">
                    <td>
                        <label>
                            Sponsor</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsponsors" runat="server" Width="200px">
                            <asp:ListItem Selected="True" Value="0">Select Sponsor</asp:ListItem>

                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Required Reward Points</label>
                    </td>
                    <td>
                        <asp:TextBox ID="point" runat="server" CssClass="medium" onkeypress="return isNumberKey(event)"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;">
                        <label>
                            Keywords(Enter with semi-colon)</label>
                    </td>
                    <td>
                        <asp:TextBox ID="Keyword" runat="server" Height="50px" Width="212px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Usage Times Available</label>
                    </td>
                    <td>
                        <asp:TextBox ID="usage" runat="server" onkeypress="return isNumberKey(event)" CssClass="medium"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Reward Source</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlrewsource" runat="server" Width="200px">
                            <asp:ListItem>Internal/Activity Based</asp:ListItem>
                            <asp:ListItem>External/Other Products Based</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Reward Image</label>
                    </td>
                    <td style="margin-left: 80px" visible="False">
                        <asp:FileUpload ID="fileUpRewardImage" runat="server" />
                    </td>
                </tr>

                <tr id="trDisc1" runat="server" visible="False">
                    <td class="col1">Discounted Activity Name
                    </td>
                    <td class="col2">
                        <asp:DropDownList ID="ddlDiscAct" runat="server" Width="200px"></asp:DropDownList>

                    </td>
                </tr>
                <tr id="trDisc2" runat="server">
                    <td style="margin-left: 80px">Discount Selection</td>
                    <td style="margin-left: 80px">
                        <asp:DropDownList ID="select" OnSelectedIndexChanged="select_index" runat="server" Width="200px" AutoPostBack="True">
                            <asp:ListItem>Percentage</asp:ListItem>
                            <asp:ListItem>Price Off</asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
                <tr id="trDisc3" runat="server" visible="False">
                    <td style="margin-left: 80px">
                        <asp:Label ID="selection" runat="server"></asp:Label></td>
                    <td style="margin-left: 80px">
                        <asp:TextBox ID="disperc" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox>
                    </td>
                </tr>



                <tr id="troffer1" runat="server" visible="false">
                    <td style="margin-left: 80px">Required Activity Name</td>
                    <td style="margin-left: 80px">
                        <asp:DropDownList ID="ddlReqActivityName" runat="server" Style="margin-left: 0" Width="200px"></asp:DropDownList></td>
                </tr>
                <tr id="troffer2" runat="server" visible="false">
                    <td style="margin-left: 80px">Free Activity Name</td>
                    <td style="margin-left: 80px">
                        <asp:DropDownList ID="ddlFreeAct" runat="server" Style="margin-left: 0" Width="200px"></asp:DropDownList></td>
                </tr>
                <tr id="trgift1" runat="server" visible="false">
                    <td style="margin-left: 80px">Free Gift Name</td>
                    <td style="margin-left: 80px">
                        <asp:TextBox ID="giftname" runat="server" CssClass="medium"></asp:TextBox>
                    </td>
                </tr>
                <tr id="trOther1" visible="false" runat="server">
                    <td style="margin-left: 80px">Bonus Point</td>
                    <td>
                        <asp:TextBox ID="Bonuspoint" runat="server" onkeypress="return isNumberKey(event)" CssClass="medium"></asp:TextBox></td>
                </tr>
                <tr id="trOther2" visible="false" runat="server">
                    <td style="margin-left: 80px">Bonus Activity Name</td>
                    <td>
                        <asp:DropDownList ID="ddlBonusAct" runat="server" Width="200px">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:Button CssClass="btn btn-navy" ID="lnkAdd" runat="server" OnClick="lnkAdd_Click" Text="Add" ></asp:Button>
</div>

