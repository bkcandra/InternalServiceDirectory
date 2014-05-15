<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AttendanceCSVReader.ascx.cs" Inherits="ISD.Administration.Web.UserControls.AttendanceCSVReader" %>
<script src="../Scripts/DataTables-1.9.4/media/js/jquery.js"></script>
<script src="../Scripts/DataTables-1.9.4/media/js/jquery.dataTables.js"></script>
<style type="text/css" title="currentStyle">
    @import "../Content/DataTables-1.9.4/media/css/demo_page.css";
    @import "../Content/DataTables-1.9.4/media/css/demo_table_jui.css";
    @import "../Content/themes/smoothness/jquery-ui.css";
</style>
<script> $(document).ready(function () {
     oTable = $('#<%=GridviewAttendance.ClientID %>').dataTable({
         "oLanguage": { "sSearch": "Search Record:" },
         "iDisplayLength": 10,
         "bJQueryUI": true,
         "aaSorting": [[0, "asc"]],
         "sPaginationType": "full_numbers"
     });

 });
</script>
<style>
    .headerAttendance {
        padding: 0 10px;
    }
    .auto-style1 {
        height: 41px;
    }
</style>
<div class="grid_10">
    <div class="box sidebox">
        <h2>New Attedance</h2>
        <div class="block">
            <div style="width: 100%">
                <div id="Upload" style="border: thin solid #C0C0C0; text-align: center; width: 500px; height: 120px; margin: 20px auto;">
                    <div style="text-align: left; background-color: #E0E0E0; padding: 5px">
                        <asp:Label ID="Label2" runat="server" Text="CSV Reader"></asp:Label>
                    </div>
                    <br />
                    <asp:FileUpload ID="fileUpload1" multiple="true" runat="server" />&nbsp;
        <asp:Button ID="imgBtnUpload" runat="server" Text="Read" OnClick="imgBtnUpload_Click"
            Height="24px" Width="75px" />&nbsp;&nbsp;
        <br />
                    <asp:Label ID="lblUploadStatus" runat="server" Text="Success."
                        Style="margin-top: 5px;" Visible="false"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</div>
<div id="CsvPreview" runat="server" class="grid_6" visible="false">
    <div class="box sidebox">
        <h2>CSV Preview</h2>
        <div class="block" style="overflow: scroll">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:LinkButton ID="lnkCheckedInvalidRecord" CssClass="btn btn-navy" runat="server" OnClick="lnkCheckedInvalidRecord_Click">Select invalid records</asp:LinkButton>
                    &nbsp;&nbsp;
                    <asp:LinkButton ID="lnkSelectAll" runat="server" CssClass="btn btn-navy" OnClick="lnkSelectAll_Click">Select all</asp:LinkButton>
                    &nbsp;&nbsp;
                    <asp:LinkButton ID="lnkDeleteSelectedAct" runat="server" CssClass="btn btn-navy" OnClick="lnkDeleteSelectedAct_Click">Delete selected records</asp:LinkButton>
                    &nbsp;&nbsp;
                    <asp:LinkButton ID="lnkResetFlag" runat="server" OnClick="lnkResetFlag_Click" CssClass="btn btn-navy">Reset Flag</asp:LinkButton>
                    <br /><br />
                    <asp:HiddenField ID="hdnEditedRowIndex" runat="server" />
                    <asp:GridView ID="GridviewAttendance" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%" AutoGenerateColumns="False" OnRowEditing="GridviewAttendance_RowEditing" OnRowCancelingEdit="GridviewAttendance_RowCancelingEdit" OnRowUpdating="GridviewAttendance_RowUpdating">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:CommandField ShowEditButton="True" HeaderText="Action" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelectRecord" runat="server" Checked="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" CssClass="headerAttendance" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                    <asp:GridView ID="gvOriginal" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%" AutoGenerateColumns="False">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:CommandField ShowEditButton="True" />
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </div>
</div>
<div id="CsvSubmit" runat="server" class="grid_4" visible="false">
    <div class="box sidebox">
        <h2>Specify attendance column</h2>
        <div class="block">
            <table class="form">
                <tr>
                    <td colspan="2" style="text-align: left">
                        <label>Step-1 : Specify fields   </label>
                    </td>
                </tr>
                <tr>
                    <td>Specify "ActivityID" Column
                      
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAttendanceActivityID" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Specify "MemberID" Column
                       
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAttendanceMemberID" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Specify "Date" Column
                      
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAttendanceDate" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>

                </tr>
                <tr>
                    <td style="text-align: left" class="auto-style1">
                        <label>Step-2 : Review Records    </label>
                    </td>
                    <td style="text-align: right" class="auto-style1">
                        <asp:LinkButton ID="lnkReview" runat="server" CssClass="btn-icon btn-navy btn-refresh" OnClick="lnkReview_Click"><span></span>Review</asp:LinkButton></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtStatus" runat="server" Enabled="false" TextMode="MultiLine" Width="100%" Text="==Review Results==" Height="75px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left">
                        <label>Step-3 : Submit Attendance </label>
                    </td>
                    <td style="text-align: right">
                        <asp:Button ID="btnInsert" runat="server" Enabled="false" CssClass="btn btn-navy" Text="Submit" OnClick="btnInsert_Click"></asp:Button>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnError" runat="server" />
            &nbsp;&nbsp;&nbsp;
        </div>
    </div>
</div>
