<%@ Page Title="Manage Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="ISD.Administration.Web.Account.Manage" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <section class="content-header">
        <h1>Manage your Administrator account
                    </h1>
        <ol class="breadcrumb">
            <li>
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~"><i class="fa fa-dashboard"></i>Home</asp:HyperLink></li>
            <li>Account</li>
            <li class="active">Register</li>
        </ol>
    </section>
    <section class="content">


        <div>
            <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
                <p class="text-success"><%: SuccessMessage %></p>
            </asp:PlaceHolder>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="box box-primary">
                    <asp:PlaceHolder runat="server" ID="changePasswordHolder" Visible="false">
                        <div class="box-header">
                            <h3 class="box-title">Change your password</h3>
                        </div>
                        <div class="box-body">

                            <asp:ValidationSummary runat="server" ShowModelStateErrors="true" CssClass="text-danger" />
                            <div class="form-group">
                                <p>You're logged in as <strong><%: User.Identity.GetUserName() %></strong>.</p>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="CurrentPasswordLabel" AssociatedControlID="CurrentPassword">Current password</asp:Label>

                                <asp:TextBox runat="server" ID="CurrentPassword" TextMode="Password" CssClass="form-control" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="CurrentPassword"
                                    CssClass="text-danger" ErrorMessage="The current password field is required."
                                    ValidationGroup="ChangePassword" />

                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="NewPasswordLabel" AssociatedControlID="NewPassword">New password</asp:Label>

                                <asp:TextBox runat="server" ID="NewPassword" TextMode="Password" CssClass="form-control" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="NewPassword"
                                    CssClass="text-danger" ErrorMessage="The new password is required."
                                    ValidationGroup="ChangePassword" />

                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="ConfirmNewPasswordLabel" AssociatedControlID="ConfirmNewPassword">Confirm new password</asp:Label>

                                <asp:TextBox runat="server" ID="ConfirmNewPassword" TextMode="Password" CssClass="form-control" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmNewPassword"
                                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Confirm new password is required."
                                    ValidationGroup="ChangePassword" />
                                <asp:CompareValidator runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword"
                                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The new password and confirmation password do not match."
                                    ValidationGroup="ChangePassword" />

                            </div>
                        </div>
                        <div class="box-footer">

                            <asp:Button runat="server" Text="Change Password" ValidationGroup="ChangePassword" OnClick="ChangePassword_Click" class="btn btn-primary"/>

                        </div>
                    </asp:PlaceHolder>
                </div>
            </div>
            <section id="passwordForm">
                <div class="box box-primary">
                    <asp:PlaceHolder runat="server" ID="setPassword" Visible="false">


                        <asp:ValidationSummary runat="server" ShowModelStateErrors="true" CssClass="text-danger" />
                        <div class="box-header">
                            <h3 class="box-title">Set your local password</h3>
                        </div>
                        <div class="box-body">
                            <div class="form-group">
                                <p>
                                    You do not have a local password for this site. Add a local
                        password so you can log in without an external login.
                                </p>
                            </div>

                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="password" CssClass="col-md-2 control-label">Password</asp:Label>

                                <asp:TextBox runat="server" ID="password" TextMode="Password" CssClass="form-control" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="password"
                                    CssClass="text-danger" ErrorMessage="The password field is required."
                                    Display="Dynamic" ValidationGroup="SetPassword" />
                                <asp:ModelErrorMessage runat="server" ModelStateKey="NewPassword" AssociatedControlID="password"
                                    CssClass="text-error" SetFocusOnError="true" />

                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="confirmPassword" CssClass="col-md-2 control-label">Confirm password</asp:Label>

                                <asp:TextBox runat="server" ID="confirmPassword" TextMode="Password" CssClass="form-control" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="confirmPassword"
                                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The confirm password field is required."
                                    ValidationGroup="SetPassword" />
                                <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="confirmPassword"
                                    CssClass="text-error" Display="Dynamic" ErrorMessage="The password and confirmation password do not match."
                                    ValidationGroup="SetPassword" />


                            </div>
                        </div>
                        <div class="box-footer">
                            <div class="form-group">

                                <asp:Button runat="server" Text="Set Password" ValidationGroup="SetPassword" OnClick="SetPassword_Click" CssClass="btn btn-default" />

                            </div>
                        </div>



                    </asp:PlaceHolder>
                </div>
            </section>
        </div>
    </section>
</asp:Content>
