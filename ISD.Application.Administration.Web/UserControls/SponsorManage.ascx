<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SponsorManage.ascx.cs" Inherits="ISD.Administration.Web.UserControls.SponsorManage" %>

<style>
    td {
        text-align: left;
        vertical-align: top;
    }

    .required {
        color: red;
    }
</style>
<script>

    function CheckNumber(evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    }

</script>

<div id="showreward" runat="server">
    <table class="form">
        <tr>
            <td class="col1">Sponsor Name
            </td>
            <td>
                <asp:Label ID="lblspnName" runat="server"></asp:Label>
                <asp:TextBox ID="txtspnName" runat="server"></asp:TextBox>


                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtspnName"
                    CssClass="bodyText1" ToolTip="Sponsor Name is required."><span class="required">&nbsp;*</span></asp:RequiredFieldValidator></td>
        </tr>

        <tr>
            <td style="vertical-align: top;">Address
            </td>
            <td colspan="2">
                <asp:Label ID="lbladdress" runat="server" Height="50px" Width="209px"></asp:Label>
                <asp:TextBox ID="txtaddress" runat="server" Height="50px" Width="209px" TextMode="MultiLine"></asp:TextBox>

            </td>
        </tr>

        <tr>
            <td class="col1">Website
            </td>
            <td colspan="2">
                <asp:Label ID="lblwebsite" runat="server"></asp:Label>
                <asp:TextBox ID="txtwebsite" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="col1">Phone Number
            </td>
            <td colspan="2">
                <asp:Label ID="lblnumber" runat="server"></asp:Label>
                <asp:TextBox ID="txtnumber" runat="server" onkeypress="return CheckNumber(event)"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td>Contract Expiry Date
            </td>
            <td>
                <asp:Label ID="lblCalendarFrom" runat="server"></asp:Label>
                <asp:TextBox ID="txtCalendarFrom" runat="server"></asp:TextBox>
                <ajaxToolkit:TextBoxWatermarkExtender ID="txtCalendarFrom_TextBoxWatermarkExtender" runat="server"
                    TargetControlID="txtCalendarFrom" WatermarkText="ExpiryDate" WatermarkCssClass="bodyText2">
                </ajaxToolkit:TextBoxWatermarkExtender>
                <ajaxToolkit:CalendarExtender ID="txtCalendarFrom_CalendarExtender" runat="server" Format="dd/MM/yyyy" TargetControlID="txtCalendarFrom">
                </ajaxToolkit:CalendarExtender>

                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtCalendarFrom"
                    CssClass="bodyText1" ToolTip="Expire date is required."><span class="required">&nbsp;*</span></asp:RequiredFieldValidator></td>
            <asp:Label ID="lbldate" ForeColor="Red" runat="server" Text="Please enter valid date" Visible="false"></asp:Label>
        </tr>

    </table>

    <br />
    <table class="form">
        <tr>
            <td>
                <asp:Button CssClass="btn btn-grey" ID="lnkUpdate" runat="server" OnClick="lnkUpdate_Click" Text="Update"></asp:Button>


                &nbsp;&nbsp;
    <asp:Button CssClass="btn btn-grey" ID="lnkAdd" runat="server" OnClick="lnkAdd_Click" Text="Add"></asp:Button>


                &nbsp;&nbsp;
    <asp:Button CssClass="btn btn-grey" ID="lnkEdit" runat="server" OnClick="lnkEdit_Click" Text="Edit"></asp:Button>
                &nbsp;&nbsp;

            </td>
        </tr>
    </table>


    <br />





    <div id="addeddiv" visible="false" runat="server">
        <asp:Label ID="rewardadded" ForeColor="red" runat="server"></asp:Label>
    </div>
</div>




<asp:HiddenField ID="hdnFormMode" runat="server" />
<asp:HiddenField ID="hdnSponsorID" runat="server" />
