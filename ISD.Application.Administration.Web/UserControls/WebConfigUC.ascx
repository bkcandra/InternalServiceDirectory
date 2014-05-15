<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebConfigUC.ascx.cs" Inherits="ISD.Administration.Web.UserControls.WebConfigUC" %>
<link href="../Content/themes/redmond/jquery-ui.css" rel="stylesheet" />
<script>
    $(function () {
        $("#accordion").accordion({
            collapsible: false,
            heightStyle: "content"
        });
        $(document).tooltip();
    });

    function CheckNumber(evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    }
</script>
<style type="text/css">
    .auto-style1 {
        height: 30px;
    }

    .auto-style2 {
        height: 30px;
        width: 175px;
    }

    .auto-style3 {
        width: 175px;
    }
</style>
<h2>Site Setting</h2>
<div class="block">

    <div id="divSuccess" runat="server" class="message success" visible="false">
        <h5>Success!</h5>
        <p>
            <asp:Label ID="lblSuccess" runat="server" Text=""></asp:Label>
        </p>
    </div>
    <div id="divWarning" runat="server" class="message warning" visible="false">
        <h5>Warning!</h5>
        <p>
            <asp:Label ID="lblWarning" runat="server" Text=""></asp:Label>
        </p>
    </div>
    <div id="divError" runat="server" class="message error" visible="false">
        <h5>Error!</h5>
        <p>
            <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
        </p>
    </div>
    <div id="accordion">
        <h3>General Setting</h3>
        <div>
            <table class="form">
                <tr>
                    <td>
                        <label title="Specify if you want to enable advance search on activity listing.">
                            Enable Advance Search
                        </label>
                    </td>
                    <td>
                        <asp:RadioButton ID="radAdvSearchTrue" Checked="true" Text="True" runat="server" GroupName="EnableADV" />
                        <asp:RadioButton ID="radAdvSearchFalse" Text="False" runat="server" GroupName="EnableADV" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <label title="Specify if you want to implement captcha on user registration process. This is useful when tester  wants to test functionality of the website.">
                            Enable Captcha
                        </label>
                    </td>
                    <td>
                        <asp:RadioButton ID="radCaptchaTrue" Checked="true" Text="Enable Captcha" runat="server" GroupName="EnableCaptcha" />
                        <asp:RadioButton ID="radCaptchaFalse" Text="Disable Captcha" runat="server" GroupName="EnableCaptcha" />

                    </td>
                </tr>
            </table>

        </div>
        <h3>Expiry Processing</h3>
        <div>
            <table class="form">
                <tr>
                    <td class="auto-style2">
                        <label title="Specify if you want to implement captcha on user registration process. This is useful when tester  wants to test functionality of the website.">
                            Expiry processing
                        </label>
                    </td>
                    <td class="auto-style1">
                        <asp:RadioButton ID="radEnableExpTrue" Checked="true" Text="Enable expiry processing" runat="server" GroupName="EnableExp" />
                        <asp:RadioButton ID="radEnableExpfalse" Text="Disable expiry processing" runat="server" GroupName="EnableExp" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        <label title="Specify if you want to send email to activity owner during expiry processing.">
                            Send email on
                        </label>
                    </td>
                    <td class="auto-style1">
                        <asp:RadioButton ID="radSendEmailTrue" Checked="true" Text="Send email" runat="server" GroupName="SendEmail" />
                        <asp:RadioButton ID="radSendEmailFalse" Text="Don't send email" runat="server" GroupName="SendEmail" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2"></td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        <label title="Specify if you want to enable first notification processing.">
                            First notification
                        </label>
                    </td>
                    <td class="auto-style1">
                        <asp:RadioButton ID="radEnableExpNotiTrue" Checked="true" Text="Enable first notification" runat="server" GroupName="EnableExpNoti1" />
                        <asp:RadioButton ID="radEnableExpNotiFalse" Text="Disable first notification" runat="server" GroupName="EnableExpNoti1" />

                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <label title="Specify how many days before first notification is processed">
                            First notification days
                        </label>
                    </td>
                    <td class="col2">
                        <asp:TextBox ID="txtFirstNotiDays" runat="server" onkeypress="return CheckNumber(event)" Width="70px" MaxLength="2" Text="14"></asp:TextBox>&nbsp; Default:
                        14 days</td>
                </tr>
                <tr>
                    <td class="auto-style2"></td>
                </tr>

                <tr>
                    <td class="auto-style2">
                        <label title="Specify if you want toenable first notification processing.">
                            Enable second notification
                        </label>
                    </td>
                    <td class="auto-style1">
                        <asp:RadioButton ID="radEnableExpNoti2True" Checked="true" Text="Enable second notification" runat="server" GroupName="EnableExpNoti2" />
                        <asp:RadioButton ID="radEnableExpNoti2False" Text="Disable second notification" runat="server" GroupName="EnableExpNoti2" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <label title="Specify how many days before second notification is processed">
                            second notification days
                        </label>
                    </td>
                    <td class="col2">
                        <asp:TextBox ID="txtSecondNotiDays" runat="server" onkeypress="return CheckNumber(event)" Width="70px" MaxLength="2" Text="7"></asp:TextBox>
                        &nbsp;Default: 7 days</td>
                </tr>
            </table>
        </div>
        <h3>Reward Processing</h3>
        <div>
            <table class="form">
                <tr>
                    <td class="auto-style2">
                        <label title="Specify if you want to process user reward points,if enable the system will process user attendance and grants point to user attending an activity.">
                            Reward processing
                        </label>
                    </td>
                    <td class="auto-style1">
                        <asp:RadioButton ID="radRewardProcessTrue" Checked="true" Text="Enable reward processing" runat="server" GroupName="EnableRew" />
                        <asp:RadioButton ID="radRewardProcessFalse" Text="Disable reward processing" runat="server" GroupName="EnableRew" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <label title="Specify how many points is given to user attending an activity.">
                            Points granted per attendance
                        </label>
                    </td>
                    <td class="col2">
                        <asp:TextBox ID="txtRewardPoints" runat="server" onkeypress="return CheckNumber(event)" Width="70px" MaxLength="3" Text="10"></asp:TextBox>
                        &nbsp;Default: 10 points</td>
                </tr>
            </table>
        </div>
    </div>
</div>
<asp:HiddenField ID="hdnAdminurl" runat="server" />
<asp:HiddenField ID="hdnCusturl" runat="server" />
<asp:HiddenField ID="hdnProvurl" runat="server" />
<asp:HiddenField ID="hdnImageurl" runat="server" />
<asp:HiddenField ID="hdnSMPTAccount" runat="server" />
<asp:HiddenField ID="hdnSMPTPassword" runat="server" />
<asp:HiddenField ID="hdnSMPTUserName" runat="server" />
<asp:HiddenField ID="hdnSMPTHost" runat="server" />
<asp:HiddenField ID="hdnSMPTPort" runat="server" />
<asp:HiddenField ID="hdnSMPTSSL" runat="server" />
<asp:HiddenField ID="hdnSMPTIIS" runat="server" />
<br />
<asp:LinkButton ID="lnkSubmit" class="btn-icon btn-navy btn-check" runat="server" Text="" OnClick="lnkSubmit_Click"><span></span>Save</asp:LinkButton>
&nbsp;&nbsp;
<asp:LinkButton ID="lnkCancel" class="btn-icon btn-navy btn-cross" runat="server" Text="" OnClick="lnkCancel_Click"><span></span>Cancel</asp:LinkButton>
