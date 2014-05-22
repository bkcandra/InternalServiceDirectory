<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminList.ascx.cs" Inherits="ISD.Administration.Web.UserControls.AdminList" %>
<div class="btn-group margin">
    <a href="../Account/Registration.aspx" class="btn btn-default">Register New User</a>&nbsp;&nbsp;&nbsp;&nbsp;
    <a href="../Account/ChangePassword.aspx" class="btn btn-default">Change Password</a>
</div>

<div class="box-body table-responsive">

    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="50%" AllowPaging="True"
        OnRowCommand="GridView1_RowCommand" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" class="table table-bordered table-hover">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteUser" OnClientClick='<%# String.Format("return confirm(\"Are you sure want to delete  {0}?\")", Eval("UserName")) %>'>Delete</asp:LinkButton>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ID">
                <ItemTemplate>
                    <asp:Label ID="lblID" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="UserName">
                <ItemTemplate>
                    <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("UserName") %>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            No administrator account found.
        </EmptyDataTemplate>
    </asp:GridView>

</div>
