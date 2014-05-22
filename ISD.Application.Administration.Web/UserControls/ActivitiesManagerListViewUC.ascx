<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActivitiesManagerListViewUC.ascx.cs" Inherits="ISD.Administration.Web.UserControls.ActivitiesManagerListViewUC1" %>
<!-- DATA TABLES -->
<link href="../Content/datatables/dataTables.bootstrap.css" rel="stylesheet" type="text/css" />




<div class="row">
    <div class="col-xs-12">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="box">
                   <div class="box-header">
                        <h3 class="box-title"></h3>
                    </div>
                     <!-- /.box-header -->
                    <div class="box-body table-responsive">
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
                            <asp:Label ID="lblSuccess" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="alert alert-info alert-dismissable" id="PrintForm" runat="server" visible="false">
                            <i class="fa fa-info"></i>
                            <button class="close" aria-hidden="true" type="button" data-dismiss="alert">×</button>
                            <b>Alert!</b> Forms are generated..<a href="javascript:void(0)" onclick="return PrintPanel();">Click Here</a>&nbsp;to print your attendance forms.
                                   
                        </div>

                        <div class="margin">
                            <div class="btn-group">
                                <asp:LinkButton ID="lnkSelectAll" runat="server" class="btn btn-default" OnClick="lnkSelectAll_Click">Select All</asp:LinkButton>
                                <asp:LinkButton ID="lnkUnselectAll" runat="server" class="btn btn-default" OnClick="lnkUnselectAll_Click">Unselect All</asp:LinkButton>
                                <asp:LinkButton ID="lnkConfirm" runat="server" Class="btn btn-default" OnClick="lnkConfirm_Click">Confirm</asp:LinkButton>
                                <asp:LinkButton ID="lnkDelete" runat="server" class="btn btn-default" OnClick="lnkDelete_Click">Delete</asp:LinkButton>
                                <asp:LinkButton ID="lnkPrint" runat="server" class="btn btn-default" OnClick="lnkPrint_Click">Get Form</asp:LinkButton>
                                <asp:LinkButton CssClass="btn btn-default" ID="lnkExtend" runat="server" Text="Extend Expired Activities" data-toggle="modal" data-target="#myModal"></asp:LinkButton>
                                <button class="btn btn-default" data-toggle="modal" data-target="#myModal">
                                    Extend expiry date
                                </button>

                                <asp:Button CssClass="btn btn-default" Style="display: none" ID="btnExtend" runat="server" OnClick="lnkExtendActivities_Click" Text="Extend Expired Activities"></asp:Button>
                                <asp:LinkButton CssClass="btn btn-default" ID="btnCheckReference" runat="server"  OnClick="lnkCheckReference_Click">Check activities reference codes</asp:LinkButton>
                            </div>
                        </div>
                        <asp:ListView ID="listviewActivities" runat="server" OnItemDataBound="listviewActivities_ItemDataBound">
                            <LayoutTemplate>
                                <table id="tblActivities" cellpadding="0" cellspacing="0" border="0" class="table table-bordered table-striped">
                                    <thead>
                                        <tr>
                                            <th>&nbsp;</th>
                                            <th>Activity Provider</th>
                                            <th>Activity Name</th>
                                            <th>Active</th>
                                            <th>Approved</th>
                                            <th>Modified Date</th>
                                            <th>Expiry Date</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <div id="ItemPlaceHolder" runat="server">
                                        </div>
                                    </tbody>
                                    <tfoot>
                                        <tr>
                                            <th>&nbsp;</th>
                                            <th>Activity Provider</th>
                                            <th>Activity Name</th>
                                            <th>Active</th>
                                            <th>Approved</th>
                                            <th>Modified Date</th>
                                            <th>Expiry Date</th>
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
                                        <asp:HyperLink ID="hlnkProviderID" runat="server"><%# Eval("ProviderName") %></asp:HyperLink>
                                    </td>
                                    <td>
                                        <asp:HiddenField ID="hdnActivityID" runat="server" Value='<%# Eval("ID") %>' />
                                        <asp:Label ID="lblActID" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                        <asp:HyperLink ID="hlnkActivityName" runat="server"><%# Eval("Name") %></asp:HyperLink>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
                                        <asp:HiddenField ID="hdnStatus" runat="server" Value='<%# Eval("Status") %>' />
                                    </td>
                                    <td>
                                        <asp:HiddenField ID="hdnIsApproved" runat="server" Value='<%# Eval("isApproved") %>' />
                                        <asp:Label ID="lblApproved" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td><%# Eval("ModifiedDateTime") %></td>
                                    <td><%# Eval("ExpiryDate") %></td>
                                </tr>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                <div style="color: Red; margin: 10px 2px; font-size: 15px">No activities listed.</div>
                            </EmptyDataTemplate>
                        </asp:ListView>
                    </div>
                    <div id="printsheet" runat="server">

                        <style>
                            table.empty {
                                display: inline-table;
                            }


                            table td.empty {
                                width: 20px;
                            }
                        </style>
                        <asp:ListView ID="Attendanceview" runat="server">
                            <LayoutTemplate>
                                <div id="ItemPlaceHolder" runat="server">
                                </div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <div id="AttSheet" runat="server" style="padding-top: 10px; width: 550px; padding-bottom: 10px; border: solid 1px; height: 930px;">
                                    <div id="head">
                                        <span style="font-size: large; padding-left: 150px"><strong>Activity Attendance Report</strong></span>
                                    </div>
                                    <br />
                                    <div class="upper" style="padding-left: 10px;">
                                        <table>
                                            <tbody>

                                                <tr>
                                                    <td>
                                                        <asp:Image ID="QRcode" Style="width: 150px; max-height: 150px" runat="server" />

                                                    </td>
                                                    <td></td>
                                                    <td align="left" style="text-align: justify; padding: 5px">
                                                        <span><strong>Activity ID:
                                        <asp:Label ID="lblActivityID" Text='<%# Eval("actid") %>' runat="server"></asp:Label></strong></span>
                                                        <br />
                                                        <span><strong>Activity Name: </strong>
                                                            <asp:Label ID="lblActname" Text='<%# Eval("ActName") %>' runat="server"></asp:Label></span>

                                                        <br />
                                                        <span style="display: inline;"><strong>Date: </strong>
                                                            <table style="display: inline-table;">
                                                                <tr>
                                                                    <td class="empty">&nbsp;</td>
                                                                    <td class="empty">&nbsp;</td>
                                                                    <td>/</td>
                                                                    <td class="empty">&nbsp;</td>
                                                                    <td class="empty">&nbsp;</td>
                                                                    <td>/</td>
                                                                    <td class="empty">&nbsp;</td>
                                                                    <td class="empty">&nbsp;</td>
                                                                </tr>
                                                            </table>

                                                        </span>

                                                    </td>
                                                </tr>

                                            </tbody>
                                        </table>
                                    </div>
                                    <hr />
                                    <div id="lowerleft" style="padding-left: 90px; float: left;">
                                        <strong>IDs of the members attended this activity</strong>
                                        <br />
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr style="border: solid 1px;" runat="server">
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="lowerright" style="padding-left: 300px; margin-top: 28px;">
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table style="border: solid 1px; margin-bottom: 2px;">
                                            <tr>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                                <td style="width: 20px;">&nbsp;</td>
                                            </tr>
                                        </table>

                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
<div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">

                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">

                                    <h4 class="modal-title" id="myModalLabel">Extend Expiry date</h4>
                                </div>
                                <div class="modal-body">
                                    Extend expired activities by
        <asp:TextBox ID="txtExtend" runat="server" TextMode="Number" Width="100px" MaxLength="2"></asp:TextBox>
                                    &nbsp;Day(s)
                                    
                                </div>
                                <div class="modal-footer">

                                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                    <asp:Button CssClass="btn btn-default"  ID="Button1" runat="server" OnClick="lnkExtendActivities_Click" Text="Extend Expired Activities"></asp:Button>
                               

                                </div>
                            </div>
                        </div>
                    </div>


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
