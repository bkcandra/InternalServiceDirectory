<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProviderList.ascx.cs" Inherits="ISD.Administration.Web.UserControls.ProviderList" %>
<script src="../Scripts/DataTables-1.9.4/media/js/jquery.dataTables.js"></script>
<style type="text/css" title="currentStyle">
    @import "../Content/DataTables-1.9.4/media/css/demo_page.css";
    @import "../Content/DataTables-1.9.4/media/css/demo_table_jui.css";
    @import "../Content/themes/smoothness/jquery-ui.css";
</style>
<script>
    function fnFormatProvDetails(providerTable, provTr) {
        var providerData = providerTable.fnGetData(provTr);
        var provOut = '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">';
        provOut += '<tr><td style="width:150px">Provider #</td><td>' + providerData[4] + '</td></tr>';
        provOut += '<tr><td>User GUID</td><td>' + providerData[3] + '</td></tr>';

        provOut += '<tr><td>Email</td><td>' + providerData[10] + '</td></tr>';
        provOut += '<tr><td>Contact Number</td><td>' + providerData[12] + '</td></tr>';
        provOut += '<tr><td>Mobile Number</td><td>' + providerData[13] + '</td></tr>';
        provOut += '<tr><td>Modified DateTime</td><td>' + providerData[15] + '</td></tr>';
        provOut += '<tr><td>Created DateTime</td><td>' + providerData[14] + '</td></tr>';
        provOut += '<tr><td colspan="2" style="text-align:right">' + providerData[1] + '</td></tr>';

        provOut += '</table>';

        return provOut;
    }

    $(document).ready(function () {
        /*
     * Insert a 'details' column to the table
     */
        var nCloneTh = document.createElement('th');
        var nCloneTd = document.createElement('td');
        nCloneTd.innerHTML = '<img src="../Content/img/details_open.png">';
        nCloneTd.className = "center";

        $('#tblProvider thead tr').each(function () {
            this.insertBefore(nCloneTh, this.childNodes[0]);
        });

        $('#tblProvider tbody tr').each(function () {
            this.insertBefore(nCloneTd.cloneNode(true), this.childNodes[0]);
        });


        providerTable = $('#tblProvider').dataTable({
            "aoColumnDefs": [
                        { "bSearchable": false, "bVisible": false, "aTargets": [1] },
                       { "bSearchable": true, "bVisible": false, "aTargets": [3] },
                       { "bSearchable": true, "bVisible": false, "aTargets": [4] },
                       { "bSearchable": true, "bVisible": false, "aTargets": [6] },
                       { "bSearchable": true, "bVisible": false, "aTargets": [10] },
                       { "bSearchable": true, "bVisible": false, "aTargets": [12] },
                       { "bSearchable": true, "bVisible": false, "aTargets": [13] },
                       { "bSearchable": false, "bVisible": false, "aTargets": [14] },
                        { "bSearchable": false, "bVisible": false, "aTargets": [15] },
                         { "bSearchable": false, "bVisible": false, "aTargets": [16] },
                       { "bSearchable": false, "aTargets": [2] }
            ],
            "oLanguage": { "sSearch": "Search Record:" },
            "iDisplayLength": 10,
            "bJQueryUI": true,
            "aaSorting": [[0, "asc"]],
            "sPaginationType": "full_numbers"
        });



        $('#tblProvider tbody td img').live('click', function () {
            var provTr = $(this).parents('tr')[0];
            if (providerTable.fnIsOpen(provTr)) {
                /* This row is already open - close it */
                this.src = "../Content/img/details_open.png";
                providerTable.fnClose(provTr);
            }
            else {
                /* Open this row */
                this.src = "../Content/img/details_close.png";
                providerTable.fnOpen(provTr, fnFormatProvDetails(providerTable, provTr), 'details');
            }
        });
    });
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div id="divError" runat="server" class="message error" visible="false">
            <h5>Error!</h5>
            <p>
                <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
            </p>
        </div>
        <div id="divSuccess" runat="server" class="message success" visible="false">
            <h5>Success!</h5>
            <p>
                <asp:Label ID="lblStatus" runat="server" Text="Label"></asp:Label>
            </p>
        </div>

        <asp:LinkButton ID="lnkDelete" runat="server" OnClick="lnkDelete_Click" OnClientClick="return confirm('This action will DELETE SELECTED Provider AND ITS related content, Are You Sure ?');">Delete Selected</asp:LinkButton>
        &nbsp;&nbsp;&nbsp;
        <asp:HyperLink ID="lnkExport" runat="server" Target="_blank" NavigateUrl="~/Report/Provider.aspx">Export to csv</asp:HyperLink>
        <asp:ListView ID="listviewActivities" runat="server" OnItemDataBound="listviewActivities_ItemDataBound" OnItemCommand="listviewActivities_ItemCommand">
            <LayoutTemplate>
                <table id="tblProvider" cellpadding="0" cellspacing="0" border="0" class="display">
                    <thead>
                        <tr>
                            <th>&nbsp;</th>
                            <th>&nbsp;</th>
                            <th>&nbsp;</th>
                            <th>Number</th>
                            <th>Provider Name</th>
                            <th>Ref Code</th>
                            <th>Username</th>
                            <th>Name</th>
                            <th>Confirmed</th>
                            <th>Email</th>
                            <th>Activity Count</th>
                            <th>Contact number</th>
                            <th>Mobile number</th>
                            <th>&nbsp;</th>
                            <th>&nbsp;</th>
                            <th>&nbsp;</th>
                        </tr>
                    </thead>
                    <tbody>
                        <div id="ItemPlaceHolder" runat="server">
                        </div>
                    </tbody>
                    <tfoot>
                        <tr>
                            <th>&nbsp;</th>
                            <th>&nbsp;</th>
                            <th>&nbsp;</th>
                            <th>Number</th>
                            <th>Provider Name</th>
                            <th>Ref Code</th>
                            <th>Username</th>
                            <th>Name</th>
                            <th>Confirmed</th>
                            <th>Email</th>
                            <th>Activity Count</th>
                            <th>Contact number</th>
                            <th>Mobile number</th>
                            <th>&nbsp;</th>
                            <th>&nbsp;</th>
                            <th>&nbsp;</th>
                        </tr>
                    </tfoot>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class="gradeA" style="text-align: center">
                    <td>
                        <asp:HyperLink ID="hlnkEdit" runat="server">Edit</asp:HyperLink>&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkConfirm" CommandName="Confirm" runat="server" ToolTip="Confirm this provider. This action will give this provider permission to log in without confirming their email address">Confirm</asp:LinkButton>&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkResenConfirmation" CommandName="ResendConfrimationEmail" runat="server" ToolTip="Resend confirmation email. Resend an email address containing email confirmation for this provider">Resend Confirmation Email</asp:LinkButton>&nbsp;&nbsp;                  
                    </td>
                    <td>
                        <asp:CheckBox ID="chkSelected" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblUserID" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                    </td>
                    <td>
                       <asp:Label ID="lblID" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblProviderName" runat="server" Text='<%#Eval("ProviderName") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblRefCode" runat="server" Text='<%#Eval("ReferenceID") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:HyperLink ID="hlnkUserName" runat="server" CommandName="Details" Text='<%#Eval("Username") %>'></asp:HyperLink>
                        <asp:HiddenField ID="hdnUserID" runat="server" Value='<%#Eval("Id") %>' />
                    </td>
                    <td>
                        <asp:Label ID="lnkName" runat="server" Text='<%#string.Format("{0} {1}",Eval("FirstName"),Eval("LastName"))%>'></asp:Label>
                    </td>
                    <td>
                        <asp:HiddenField ID="hdnConfirmationToken" runat="server" />
                        <asp:Image ID="imgEmailIcon" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblEmail" runat="server" Text='<%#Eval("Email") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblActCount" runat="server" Text='<%#Eval("ActivityCount") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblContact" runat="server" Text='<%#Eval("PhoneNumber") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblMobile" runat="server" Text='<%#Eval("MobileNumber") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblCreatedDateTime" runat="server" Text='<%#Eval("CreatedDatetime") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblModifiedDateTime" runat="server" Text='<%#Eval("ModifiedDatetime") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblPreferredContact" runat="server" Text='<%#Eval("PreferredContact") %>'></asp:Label>
                    </td>
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
                <div style="color: Red; margin: 10px 2px; font-size: 15px">Could't get provider information.</div>
            </EmptyDataTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="ods" runat="server"></asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>


