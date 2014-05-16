<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="ISD.Administration.Web.Account.Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <section class="content-header">
        <h1>Register new administrator
                    </h1>
        <ol class="breadcrumb">
            <li>
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~"><i class="fa fa-dashboard"></i>Home</asp:HyperLink></li>
            <li>Account</li>
            <li class="active">Register</li>
        </ol>
    </section>
    <section class="content">
        <div class="row">
            <!-- left column -->
            <div class="col-md-6">
                <!-- general form elements -->
                <div class="box box-primary">
                    <div class="box-header">
                        <h3 class="box-title">Administrator information</h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->

                    <div class="box-body">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="txtUsername">Username</asp:Label>
                            <asp:TextBox runat="server" ID="txtUsername" CssClass="form-control" placeholder="Username" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUsername"
                                CssClass="text-danger" ErrorMessage="The email field is required." />
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="Email">Email address</asp:Label>
                            <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" placeholder="Enter email" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                                CssClass="text-danger" ErrorMessage="The email field is required." />
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="Password">Password</asp:Label>
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" placeholder="Password" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                                CssClass="text-danger" ErrorMessage="The password field is required." />


                        </div>
                        <div class="form-group">

                            <asp:Label runat="server" AssociatedControlID="ConfirmPassword">Confirm password</asp:Label>
                            <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" CssClass="form-control" placeholder="Confirm Password" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                                CssClass="text-danger" Display="Dynamic" ErrorMessage="The confirm password field is required." />
                            <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                                CssClass="text-danger" Display="Dynamic" ErrorMessage="The password and confirmation password do not match." />

                        </div>
                    </div>
                    <!-- /.box -->
                </div>
                <!--/.col (left) -->
                <!-- right column -->

                <!--/.col (right) -->
            </div>
            <div class="col-md-6">
                <div class="box box-primary">
                    <div class="box-header">
                        <h3 class="box-title">Validate your account</h3>
                    </div>
                    <div class="box-body">
                       
                        <div class="form-group">
                            <p class="text-danger">

                                <asp:Literal runat="server" ID="ErrorMessage" />
                            </p>
                        </div>
                        <div class="form-group">
                            <p>You're logged in as <strong><%: User.Identity.GetUserName() %></strong>.</p>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="Password">Password</asp:Label>
                            <asp:TextBox runat="server" ID="txtPasswordCurrentAdmin" TextMode="Password" CssClass="form-control" placeholder="Password" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                                CssClass="text-danger" ErrorMessage="The password field is required." />


                        </div>
                        <div class="form-group">

                            <asp:Label runat="server" AssociatedControlID="txtConfirmPasswordCurrentAdmin">Confirm password</asp:Label>
                            <asp:TextBox runat="server" ID="txtConfirmPasswordCurrentAdmin" TextMode="Password" CssClass="form-control" placeholder="Confirm Password" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                                CssClass="text-danger" Display="Dynamic" ErrorMessage="The confirm password field is required." />
                            <asp:CompareValidator runat="server" ControlToCompare="txtPasswordCurrentAdmin" ControlToValidate="txtConfirmPasswordCurrentAdmin"
                                CssClass="text-danger" Display="Dynamic" ErrorMessage="The password and confirmation password do not match." />

                        </div>
                    </div>
                    <div class="box-footer">
                        <asp:Button ID="btnRegister" runat="server" OnClick="CreateUser_Click" class="btn btn-primary" Text="Submit" />
                    </div>
                </div>
                <!-- /.row -->
            </div>
        </div>
    </section>

</asp:Content>
