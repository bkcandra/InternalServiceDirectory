<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminList.ascx.cs" Inherits="ISD.Administration.Web.UserControls.AdminList" %>
<div>
    <a href="../Account/Registration.aspx">Register New User</a>&nbsp;&nbsp;&nbsp;&nbsp;
    <a href="../Account/ChangePassword.aspx">Change Password</a>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="50%" AllowPaging="True"
        OnRowCommand="GridView1_RowCommand" CellPadding="4" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
        <AlternatingRowStyle BackColor="White" />
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
        <EditRowStyle BackColor="#2461BF" />
        <EmptyDataTemplate>
        </EmptyDataTemplate>
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#4870BE" />
    </asp:GridView>
</div>
