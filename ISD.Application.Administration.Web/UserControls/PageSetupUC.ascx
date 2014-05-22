<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageSetupUC.ascx.cs"
    Inherits="ISD.Administration.Web.UserControls.PageSetupUC" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<div class="row">
    <div class="col-xs-12">
        <div class="box">
            <div class="box-header">
                <h3 class="box-title">Page Info</h3>
            </div>
            <!-- /.box-header -->
            <div class="box-body ">
                <asp:HiddenField ID="hdnEditMode" runat="server" />
                <div id="divError" runat="server" class="alert alert-danger alert-dismissable" visible="false">
                    <i class="fa fa-warning"></i>
                    <button aria-hidden="true" data-dismiss="alert" class="close" type="button">×</button>
                    <b>Error!</b>
                    <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
                </div>

                <div class="alert alert-success alert-dismissable" id="divSuccess" runat="server" visible="false">
                    <i class="fa fa-check"></i>
                    <button aria-hidden="true" data-dismiss="alert" class="close" type="button">×</button>
                    <b>Success!</b>
                    <asp:Label ID="lblSuccess" runat="server" Text=""></asp:Label>

                </div>

                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div id="div1" runat="server">
                            <table>
                                <tr>
                                    <td colspan="3" style="vertical-align: top">Page name will be displayed on page url (ex: www.HealthyAustraliaClub.com/Pages/[Pagename])
                                    </td>

                                </tr>
                                <tr>
                                    <td>Page name&nbsp;
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblName" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtName" runat="server" Height="22px" Width="240px" OnTextChanged="txtName_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <asp:Label ID="lblNameError" runat="server" Text="" Style="color: red"></asp:Label>
                                    </td>

                                </tr>
                                <tr>

                                    <td colspan="3" style="vertical-align: top">&nbsp;
                                    </td>
                                </tr>
                                <tr>

                                    <td colspan="3" style="vertical-align: top">This information below will help search engine to understand what information is inside this page.
                                    </td>
                                </tr>
                                <tr>
                                    <td>Meta title
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTitle" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTitle" runat="server" Height="22px" Width="240px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trDescription" runat="server">
                                    <td>Meta Description
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblDescription" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtDescription" runat="server" Height="22px" Width="240px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trkeyword" runat="server">
                                    <td>Meta keyword
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblKeyword" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtKeyword" runat="server" Height="22px" Width="240px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div id="divCopyTable" runat="server" visible="false" class="margin">
                    <fieldset style="width: 450px">
                        <legend>Copy page from</legend>
                        <table>
                            <tr>
                                <td class="style2">From
                                </td>
                                <td>:
                                </td>
                                <td class="style3">
                                    <asp:DropDownList ID="ddDynamicPage" runat="server">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style2"></td>
                                <td></td>
                                <td class="style3">
                                    <asp:LinkButton ID="lnkCopyFrom" runat="server" OnClick="lnkCopyFrom_Click">Copy Page</asp:LinkButton>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div class="btn-group">
                    <asp:LinkButton ID="lnkEdit" runat="server" OnClick="lnkEdit_Click" CssClass="btn btn-default">Edit</asp:LinkButton>
                    &nbsp;&nbsp;
            <asp:LinkButton ID="lnkDuplicate" runat="server" OnClick="lnkDuplicate_Click" CssClass="btn btn-default">Duplicate This Page</asp:LinkButton>
                    &nbsp;&nbsp;
            <asp:LinkButton ID="lnkSave" runat="server" OnClick="lnkSave_Click" CssClass="btn btn-primary">Save</asp:LinkButton>
                    &nbsp;&nbsp;
            <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click" CssClass="btn btn-default">Cancel</asp:LinkButton>
                </div>
            </div>
            <!-- /.box-body -->
        </div>
        <!-- /.box -->

        <div class="box" id="divContentPreviewHead" runat="server" visible="false">
            <div class="box-header">
                <h3 class="box-title">
                    <asp:Label ID="lblPageSetup" runat="server" Text="Page preview"></asp:Label></h2></h3>
            </div>
            <!-- /.box-header -->
            <div class="box-body">
                <div id="divContentPreview" runat="server" visible="false">
                </div>
                <asp:HiddenField ID="HiddenField1" runat="server" />
                <asp:HiddenField ID="hdnTemplateID" runat="server" />
                <asp:HiddenField ID="hdnEmailType" runat="server" />
            </div>
            <!-- /.box-body -->
        </div>
        <!-- /.box -->

        <div class="box" id="divPageEditor" runat="server" visible="false">
            <div class="box-header">
                <h3 class="box-title">
                    <asp:Label ID="Label1" runat="server" Text="Page Editor"></asp:Label></h2></h3>
            </div>
            <!-- /.box-header -->
            <div class="box-body">

                <CKEditor:CKEditorControl ID="CKEditorControl1" BasePath="~/Scripts/ckeditor/" runat="server">
                </CKEditor:CKEditorControl>
            </div>
            <!-- /.box-body -->
        </div>
        <!-- /.box -->
    </div>
</div>
<asp:HiddenField ID="hdnMode" runat="server" />
<asp:HiddenField ID="hdnPageID" runat="server" />
<asp:HiddenField ID="hdnPageType" runat="server" />
