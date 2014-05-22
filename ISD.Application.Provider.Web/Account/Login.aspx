<%@ Page Title="Log in" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ISD.Provider.Web.Account.Login" Async="true"  %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="breadcrumbs">
        <div class="container">
            <h1 class="pull-left">Login</h1>
            <ul class="pull-right breadcrumb">
                <li>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~">Home</asp:HyperLink></li>

                <li><a href="">Account</a></li>
                <li class="active">Login</li>
            </ul>
        </div>
        <!--/container-->
    </div>
    <div class="container content">
        <div class="row">
            <div class="col-md-4 col-md-offset-4 col-sm-6 col-sm-offset-3">
                <div class="reg-page"> 
                    <div class="reg-header">
                        <h2>Login to your account</h2>
                    </div>
                    <div class="input-group">
                        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                            <p class="text-danger margin-bottom-20">
                                <asp:Literal runat="server" ID="FailureText" />
                            </p>
                        </asp:PlaceHolder>
                    </div>
                    <div class="input-group">
                        <span class="input-group-addon"><i class="fa fa-user"></i></span>
                        <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" placeholder="Username / Email" />

                    </div>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                        CssClass="text-danger" ErrorMessage="The email field is required." />
                    <div class="margin-bottom-20"></div><div class="input-group">
                        <span class="input-group-addon"><i class="fa fa-lock"></i></span>
                        <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" placeholder="Password" />
                    </div>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="The password field is required." />
                    <div class="margin-bottom-20"></div>
                    <div class="row">
                        <div class="col-md-6">
                            <asp:CheckBox runat="server" ID="RememberMe" class="checkbox" Text="Stay signed in" />
                        </div>
                        <div class="col-md-6">
                            <asp:Button runat="server" OnClick="LogIn" Text="Login" class="btn-u pull-right" />

                        </div>
                    </div>
                    <hr>
                    <h4>Forget your Password ?</h4>
                    <p>
                        no worries,
                        <asp:HyperLink runat="server" ID="ForgotPasswordHyperLink" ViewStateMode="Disabled">click here</asp:HyperLink>
                        to reset your password.
                    </p>

                </div>
            </div>
        </div>
        <!--/row-->
    </div>
</asp:Content>
