<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ISD.Administration.Web.Account.Login" %>

<html class="bg-black" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <title>Healthy Australia Club | Log in</title>
    <meta content='width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no' name='viewport'>

    <webopt:BundleReference runat="server" Path="~/Content/css" />
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
          <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
          <script src="https://oss.maxcdn.com/libs/respond.js/1.3.0/respond.min.js"></script>
        <![endif]-->
</head>
<body class="bg-black">
    <form id="form1" runat="server">
        <div class="form-box" id="login-box">
            <div class="header">Sign In</div>

            <div class="body bg-gray">
                <div class="form-group">
                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                </div>
                <div class="form-group">
                    <asp:TextBox ID="Email" runat="server" class="form-control" placeholder="Email / Username"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                        CssClass="text-danger" ErrorMessage="The email field is required." />
                </div>
                <div class="form-group">
                    <asp:TextBox ID="Password" runat="server" class="form-control" placeholder="Password" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="The password field is required." />
                </div>
                <div class="form-group">
                    <asp:CheckBox runat="server" ID="remember_me" />
                    Remember me
                </div>
            </div>
            <div class="footer">
                <asp:Button runat="server" OnClick="LogIn" Text="Sign me in" class="btn bg-olive btn-block"/>
                <p><a href="#">I forgot my password</a></p>
            </div>


            <%--  <div class="margin text-center">
                <span>Sign in using social networks</span>
                <br />
                <button class="btn bg-light-blue btn-circle"><i class="fa fa-facebook"></i></button>
                <button class="btn bg-aqua btn-circle"><i class="fa fa-twitter"></i></button>
                <button class="btn bg-red btn-circle"><i class="fa fa-google-plus"></i></button>

            </div>--%>
        </div>
    </form>
</body>
</html>
