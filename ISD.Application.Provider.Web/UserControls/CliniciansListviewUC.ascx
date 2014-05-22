<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CliniciansListviewUC.ascx.cs" Inherits="ISD.Provider.Web.UserControls.CliniciansListviewUC" %>
<link href="../Content/datatables/dataTables.bootstrap.css" rel="stylesheet" type="text/css" />

<asp:ListView ID="listviewClinicians" runat="server" OnItemDataBound="listviewActivities_ItemDataBound">
    <LayoutTemplate>
        <table id="tblActivities" cellpadding="0" cellspacing="0" border="0" class="table table-bordered table-striped">
            <thead>
                <tr>
                    <th>&nbsp;</th>
                    <th>Name</th>
                    <th>Clinic</th>
                    <th>Phone</th>
                    <th>Email</th>
                    <th>Is private</th>
                    <th>Days</th>
                </tr>
            </thead>
            <tbody>
                <div id="ItemPlaceHolder" runat="server">
                </div>
            </tbody>
            <tfoot>
                <tr>
                    <th>&nbsp;</th>
                    <th>Name</th>
                    <th>Clinic</th>
                    <th>Phone</th>
                    <th>Email</th>
                    <th>Is private</th>
                    <th>Days</th>
                </tr>
            </tfoot>
        </table>
    </LayoutTemplate>
    <ItemTemplate>
        <tr class="gradeA" style="text-align: center">
            <td>
                <asp:CheckBox ID="chkAct" runat="server" />
            </td>
            <td>
                <asp:HiddenField ID="hdnProviderID" runat="server" Value='<%# Eval("ProviderID") %>' />
                <asp:HiddenField ID="hdnActivityID" runat="server" Value='<%# Eval("ID") %>' />
                <asp:HiddenField ID="hdnPrivate" runat="server" Value='<%# Eval("Type") %>' />
                <asp:HyperLink ID="hlnkProviderID" runat="server"><%# Eval("Name") %></asp:HyperLink>
            </td>
            <td>
                <asp:Label ID="lblLocation" runat="server"></asp:Label>
            </td>
            <td>
                <%# Eval("Phone") %>
            </td>
            <td>
                <%# Eval("Email") %>
                <asp:Label ID="lblType" runat="server" Text=""></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblDays" runat="server" Text=""></asp:Label></td>

        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <div style="color: Red; margin: 10px 2px; font-size: 15px">No activities listed.</div>
    </EmptyDataTemplate>
</asp:ListView>

<!-- DATA TABES SCRIPT -->
<script src="../Scripts/datatables/jquery.dataTables.js" type="text/javascript"></script>
<script src="../Scripts/datatables/dataTables.bootstrap.js" type="text/javascript"></script>
<!-- AdminLTE App -->
<script src="../Scripts/AdminLTE/app.js" type="text/javascript"></script>

<script type="text/javascript">
    $(document).ready(function () {
        $('#tblActivities').dataTable();
        });

   
</script>