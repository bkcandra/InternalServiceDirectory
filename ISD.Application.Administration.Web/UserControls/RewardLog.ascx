<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RewardLog.ascx.cs" Inherits="ISD.Administration.Web.UserControls.RewardLog" %>
<div>
    <span style="font-weight: bold">Reward Log</span>
    <hr />
</div>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div style="float: left; width: 600px">
            <asp:GridView ID="GridViewReward" runat="server" OnRowDataBound="GridViewReward_ItemDataBound" OnRowCommand="GridViewReward_ItemCommand" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="Horizontal">
                <EditRowStyle BackColor="#999999" />
                <EmptyDataTemplate>
                    No Data To Display !!
                </EmptyDataTemplate>
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDtails" runat="server" CommandName="ShowDetails" ClientIDMode="AutoID">Show Details</asp:LinkButton>
                            <asp:HiddenField ID="hdnID" runat="server" Value='<%#Eval("ID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Log Category">
                        <ItemTemplate>
                            <asp:Label ID="lblCategory" runat="server" Text="Category"></asp:Label>
                            <asp:HiddenField ID="hdnMessage" runat="server" Value='<%#Eval("Message") %>' />
                            <asp:HiddenField ID="hdnCategory" runat="server" Value='<%#Eval("LogCategory") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Created Date">
                        <ItemTemplate>
                            <asp:Label ID="lblCreatedDate" runat="server" Text='<%#Eval("CreatedDateTime") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="User Name">
                        <ItemTemplate>
                            <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("CreatedBy") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

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
        </div>
        <div style="float: right; width: 250px; height: 200px; text-align: center">
            <div style="top: 70px; position: relative">
                <asp:Button ID="btnProcessAttendance" runat="server" Style="margin-top: 10px;"
                    Text="Process user attendance" OnClick="btnProcessAttendance_Click" />
            </div>
        </div>
        <div style="clear: both"></div>
        <div>
            <span style="font-weight: bold">Reward Log Actions</span>
            <hr />
            <asp:GridView ID="GridViewRewardLogAction" runat="server" AutoGenerateColumns="False" OnRowCommand="GridViewRewardLogAction_ItemCommand" OnRowDataBound="GridViewRewardLogAction_ItemDataBound" CellPadding="4" ForeColor="#333333" GridLines="Both">
                <EditRowStyle BackColor="#999999" />
                <EmptyDataTemplate>
                    No Data To Display !!
                </EmptyDataTemplate>
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkShowDetails" runat="server" CommandName="ShowDetails" ClientIDMode="AutoID">Show Details</asp:LinkButton>
                            <asp:HiddenField ID="hdnRewardLogActionID" runat="server" Value='<%#Eval("ID") %>' />
                            <asp:HiddenField ID="hdnValue" runat="server" Value='<%#Eval("Value") %>' />
                        
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Category">
                        <ItemTemplate>
                            <asp:Label ID="lblRewardLogCategory" runat="server" Text="Category"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Points awarded">
                        <ItemTemplate>
                            <asp:Label ID="lblPoints" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Bonus points awarded">
                        <ItemTemplate>
                            <asp:Label ID="lblBonusPoints" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="user">
                        <ItemTemplate>
                            <asp:Label ID="lblUser" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="log Datetime">
                        <ItemTemplate>
                            <asp:Label ID="lblCreatedDate" runat="server" Text='<%#Eval("CreatedDateTime") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
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
        </div>
        <div style="float: left">
            Details<br />
            <asp:TextBox ID="txtMessage" runat="server" Enabled="false" TextMode="MultiLine"
                Width="425px" Height="100px"></asp:TextBox>
        </div>
        <div style="float: right; text-align: left">
            Note<br />
            <asp:TextBox ID="txtNote" runat="server" Enabled="false" TextMode="MultiLine"
                Width="425px" Height="100px"></asp:TextBox><br />
            <div style="float: left; text-align: center; width: 250px; padding-top: 4px;">
                <asp:Label ID="lblStatus" runat="server" Text="" ForeColor="Red"></asp:Label>
            </div>
            <div style="float: right;">
                <asp:HiddenField ID="hdnNoteMode" runat="server" />
                <asp:HiddenField ID="HdnNoteValue" runat="server" />
                <asp:HiddenField ID="hdnNoteActionID" runat="server" />
                <asp:Button ID="btnNoteEdit" runat="server" Text="Edit" OnClick="btnNoteEdit_Click" />
                &nbsp;
        <asp:Button ID="btnNoteSave" runat="server" Text="Save" OnClick="btnNoteSave_Click" />
                &nbsp;
        <asp:Button ID="btnNoteCancel" runat="server" Text="Cancel" OnClick="btnNoteCancel_Click" />
            </div>
        </div>
        <div style="clear: both"></div>
    </ContentTemplate>
</asp:UpdatePanel>
