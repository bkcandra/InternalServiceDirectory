<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DashboardSummaryUC.ascx.cs" Inherits="ISD.Administration.Web.UserControls.DashboardSummaryUC" %>
                    
    <div class="row">
        <div class="col-lg-3 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-aqua">
                <div class="inner">
                    <h3>
                        <asp:Label ID="lblReward" runat="server" Text="0"></asp:Label>
                    </h3>
                    <p>
                        Rewards listed
                                   
                    </p>
                </div>
                <div class="icon">
                    <i class="ion ion-bag"></i>
                </div>
                <asp:HyperLink ID="hlnkReward" runat="server" class="small-box-footer" NavigateUrl="~/Rewards/">More info <i class="fa fa-arrow-circle-right"></i></asp:HyperLink>

            </div>
        </div>
        <!-- ./col -->
        <div class="col-lg-3 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-green">
                <div class="inner">
                    <h3>
                        <asp:Label ID="lblActivity" runat="server" Text="0"></asp:Label>
                    </h3>
                    <p>
                        Activity Listed
                                   
                    </p>
                </div>
                <div class="icon">
                    <i class="ion ion-stats-bars"></i>
                </div>
                <asp:HyperLink ID="hlnkActivities" runat="server" class="small-box-footer" NavigateUrl="~/Activities/">More info <i class="fa fa-arrow-circle-right"></i></asp:HyperLink>


            </div>
        </div>
        <!-- ./col -->
        <div class="col-lg-3 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-yellow">
                <div class="inner">
                    <h3>
                        <asp:Label ID="lblUser" runat="server" Text="0"></asp:Label>
                    </h3>
                    <p>
                        User Registered
                                   
                    </p>
                </div>
                <div class="icon">
                    <i class="ion ion-person-add"></i>
                </div>
                <asp:HyperLink ID="hlnkUser" runat="server" class="small-box-footer" NavigateUrl="~/Account/">More info <i class="fa fa-arrow-circle-right"></i></asp:HyperLink>
            </div>
        </div>
        <!-- ./col -->
        <div class="col-lg-3 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-red">
                <div class="inner">
                    <h3>
                        <asp:Label ID="lblVisitor" runat="server" Text="0"></asp:Label>
                    </h3>
                    <p>

                        <asp:Label ID="lblError" runat="server" Text="Unique Visitors"></asp:Label>
                    </p>
                </div>
                <div class="icon">
                    <i class="ion ion-pie-graph"></i>
                </div>
                <a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i>
                </a>
            </div>
        </div>
        <!-- ./col -->
    </div>
    <h4 class="page-header">Activity Stats
                       
        <small>General information on activity stats</small>
    </h4>

    <div class="row">
        <div class="col-lg-2 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-green">
                <div class="inner">
                    <h3>
                        <asp:Label ID="lblTotalActivity" runat="server" Text="0"></asp:Label>
                    </h3>
                    <p>
                        Total Activity
                    </p>
                </div>
            </div>
        </div>
        <div class="col-lg-2 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-green">
                <div class="inner">
                    <h3>
                        <asp:Label ID="lblCat" runat="server" Text="0"></asp:Label>
                    </h3>
                    <p>
                        Categories                                  
                    </p>
                </div>
            </div>
        </div>
        <div class="col-lg-2 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-green">
                <div class="inner">
                    <h3>
                        <asp:Label ID="lblApprovedActivity" runat="server" Text="0"></asp:Label>
                    </h3>
                    <p>
                        Approved activity                                   
                    </p>
                </div>
            </div>
        </div>
        <div class="col-lg-2 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-green">
                <div class="inner">
                    <h3>
                        <asp:Label ID="lblWaitingActivity" runat="server" Text="0"></asp:Label>
                    </h3>
                    <p>
                        Waiting for approval
                    </p>
                </div>
            </div>
        </div>

        <div class="col-lg-2 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-green">
                <div class="inner">
                    <h3>
                        <asp:Label ID="lblDeletedAct" runat="server" Text="0"></asp:Label>
                    </h3>
                    <p>
                        Deleted activity
                    </p>
                </div>
            </div>
        </div>
        <div class="col-lg-2 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-green">
                <div class="inner">
                    <h3>
                        <asp:Label ID="lblExpiredAct" runat="server" Text="0"></asp:Label>
                    </h3>
                    <p>
                        Expired activity                                   
                    </p>
                </div>
            </div>
        </div>
    </div>
     <h4 class="page-header">User Stats
                       
        <small>General information on User stats</small>
    </h4>
    <div class="row">
        <div class="col-lg-3 col-xs-6">
            <!-- small box -->
            <div class="small-box bg-yellow">
                <div class="inner">
                    <h3>
                        <asp:Label ID="lblAdmin" runat="server" Text="0"></asp:Label>
                    </h3>
                    <p>
                        Administrator
                    </p>
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-xs-6">

            <div class="small-box bg-yellow">
                <div class="inner">
                    <h3>
                        <asp:Label ID="lblProviders" runat="server" Text="0"></asp:Label>
                    </h3>
                    <p>
                        Provider                                  
                    </p>
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-xs-6">

            <div class="small-box bg-yellow">
                <div class="inner">
                    <h3>
                        <asp:Label ID="lblSponsor" runat="server" Text="0"></asp:Label>
                    </h3>
                    <p>
                        Sponsor                                
                    </p>
                </div>
            </div>
        </div>
       <div class="col-lg-3 col-xs-6">

            <div class="small-box bg-yellow">
                <div class="inner">
                    <h3>
                        <asp:Label ID="lblMember" runat="server" Text="0"></asp:Label>
                    </h3>
                    <p>
                        Club Member                                
                    </p>
                </div>
            </div>
        </div>

    </div>
