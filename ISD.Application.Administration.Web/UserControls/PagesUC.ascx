<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PagesUC.ascx.cs" Inherits="ISD.Administration.Web.UserControls.PagesUC" %>
<%@ Import Namespace="ISD.Util" %>
<div class="box">
    <div class="box-header">
        <h3 class="box-title">
            
              
        </h3>
    </div>
    <!-- /.box-header -->
    <div class="box-body table-responsive"><asp:HyperLink ID="hlnkNewPage" runat="server" Text="New Page" NavigateUrl= "~/Pages/PageSetup.aspx?Pt=1"></asp:HyperLink>
       
        <asp:GridView ID="gridView1" runat="server" AutoGenerateColumns="False" OnRowCommand="gridView1_RowCommand"
            Width="100%" OnRowDataBound="gridView1_RowDataBound" class="table table-bordered table-hover">
            
    <EmptyDataTemplate>
        No Dynamic Page Available
    </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditDynamicPage" Text="Edit"></asp:LinkButton>
                        <asp:HiddenField ID="hdnPage" runat="server" Value='<%#Eval("ID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDelete" runat="server" OnClientClick="return confirm('Are You Sure Want to Delete Selected Page?');"
                            CommandName="DeleteDynamicPage" Text="Delete"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Page Name">
                    <ItemTemplate>
                        <asp:Label ID="lblPageName" runat="server" Text='<%#Eval("Name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Page Title">
                    <ItemTemplate>
                        <asp:Label ID="lblPageTitle" runat="server" Text='<%#Eval("Title") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Link To Page">
                    <ItemTemplate>
                        <asp:Label ID="lblPagelink" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</div>
<asp:ObjectDataSource ID="ods" runat="server"></asp:ObjectDataSource>
<asp:HiddenField ID="hdnPageType" runat="server" />
