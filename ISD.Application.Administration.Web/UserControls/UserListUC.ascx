﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserListUC.ascx.cs" Inherits="ISD.Administration.Web.UserControls.UserListUC" %>
<link href="../Content/datatables/dataTables.bootstrap.css" rel="stylesheet" type="text/css" />

<script>
    function fnFormatUserDetails(userTable, userTr) {
        var userData = userTable.fnGetData(userTr);
        var userOut = '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">';
        userOut += '<tr><td style="width:150px">Member #</td><td>' + userData[4] + '</td></tr>';
        userOut += '<tr><td>User GUID</td><td>' + userData[3] + '</td></tr>';
        userOut += '<tr><td>Ref Code</td><td>' + userData[6] + '</td></tr>';
        userOut += '<tr><td>Email</td><td>' + userData[9] + '</td></tr>';
        userOut += '<tr><td>Contact Number</td><td>' + userData[10] + '</td></tr>';
        userOut += '<tr><td>Mobile Number</td><td>' + userData[11] + '</td></tr>';
        userOut += '<tr><td>Modified DateTime</td><td>' + userData[13] + '</td></tr>';
        userOut += '<tr><td>Created DateTime</td><td>' + userData[12] + '</td></tr>';
        userOut += '<tr><td colspan="2" style="text-align:right">' + userData[1] + '</td></tr>';

        userOut += '</table>';

        return userOut;
    }

    $(document).ready(function () {
        /*
     * Insert a 'details' column to the table
     */
        var nCloneTh = document.createElement('th');
        var nCloneTd = document.createElement('td');
        nCloneTd.innerHTML = '<img src="../Content/images/details_open.png">';
        nCloneTd.className = "center";

        $('#tblUser thead tr').each(function () {
            this.insertBefore(nCloneTh, this.childNodes[0]);
        });

        $('#tblUser tbody tr').each(function () {
            this.insertBefore(nCloneTd.cloneNode(true), this.childNodes[0]);
        });


        userTable = $('#tblUser').dataTable({
            "aoColumnDefs": [
                        { "bSearchable": false, "bVisible": false, "aTargets": [1] },
                       { "bSearchable": true, "bVisible": false, "aTargets": [3] },
                       { "bSearchable": true, "bVisible": false, "aTargets": [4] },
                       { "bSearchable": true, "bVisible": false, "aTargets": [10] },
                       { "bSearchable": true, "bVisible": false, "aTargets": [6] },
                       { "bSearchable": false, "aTargets": [2] },
                       { "bSearchable": true, "bVisible": false, "aTargets": [11] },
                       { "bSearchable": false, "bVisible": false, "aTargets": [12] },
                       { "bSearchable": false, "bVisible": false, "aTargets": [13] },
                        { "bSearchable": false, "bVisible": false, "aTargets": [14] }
            ],
            "oLanguage": { "sSearch": "Search Record:" },
            "iDisplayLength": 10,
            "bJQueryUI": true,
            "aaSorting": [[0, "asc"]],
            "sPaginationType": "full_numbers"
        });


        $('#tblUser tbody').on('click', 'td img', function () {
            var userTr = $(this).parents('tr')[0];
            if (userTable.fnIsOpen(userTr)) {
                /* This row is already open - close it */
                this.src = "../Content/images/details_open.png";
                userTable.fnClose(userTr);
            }
            else {
                /* Open this row */
                this.src = "../Content/images/details_close.png";
                userTable.fnOpen(userTr, fnFormatUserDetails(userTable, userTr), 'details');
            }
        });
    });
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div id="divError" runat="server" class="alert alert-danger alert-dismissable" visible="false">
            <i class="fa fa-warning"></i>
            <button class="close" aria-hidden="true" type="button" data-dismiss="alert">×</button>
            <b>Error!</b>
            <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
        </div>
        <div id="divSuccess" runat="server" class="alert alert-success alert-dismissable" visible="false">
            <i class="fa fa-check"></i>
            <button class="close" aria-hidden="true" type="button" data-dismiss="alert">×</button>
            <b>Success!</b>
            <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
        </div>
        

        <div class="margin">
                            <div class="btn-group">
                                 <asp:LinkButton ID="lnkDelete" runat="server" CssClass="btn btn-default" OnClick="lnkDelete_Click" OnClientClick="return confirm('This action will DELETE SELECTED Provider AND ITS related content, Are You Sure ?');">Delete Selected</asp:LinkButton>
      
        <asp:HyperLink ID="lnkExport" runat="server" Target="_blank" CssClass="btn btn-default" NavigateUrl="~/Report/Provider.aspx">Export to csv</asp:HyperLink>
        
  </div>  </div>
       
        <asp:ListView ID="listviewActivities" runat="server" OnItemDataBound="listviewActivities_ItemDataBound" OnItemCommand="listviewActivities_ItemCommand">
            <LayoutTemplate>
                <table id="tblUser" cellpadding="0" cellspacing="0" border="0" class="table table-bordered table-striped">
                    <thead>
                        <tr>
                            <th>&nbsp;</th>
                            <th>&nbsp;</th>
                            <th>&nbsp;</th>
                            <th>Number</th>
                            <th>Username</th>
                            <th>Ref Code</th>
                            <th>Name</th>
                            <th>Confirmed</th>
                            <th>Email</th>
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
                            <th>Username</th>
                            <th>Ref Code</th>
                            <th>Name</th>
                            <th>Confirmed</th>
                            <th>Email</th>
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
                        <asp:HyperLink ID="hlnkUserName" runat="server" Text='<%#Eval("Username") %>'></asp:HyperLink>
                        <asp:HiddenField ID="hdnUserID" runat="server" Value='<%#Eval("Id") %>' />
                    </td>
                    <td>
                        <asp:Label ID="lblRefCode" runat="server" Text='<%#Eval("ReferenceID") %>'></asp:Label>
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
<!-- DATA TABES SCRIPT -->
<script src="../Scripts/datatables/jquery.dataTables.js" type="text/javascript"></script>
<script src="../Scripts/datatables/dataTables.bootstrap.js" type="text/javascript"></script>
<!-- AdminLTE App -->
<script src="../Scripts/AdminLTE/app.js" type="text/javascript"></script>
