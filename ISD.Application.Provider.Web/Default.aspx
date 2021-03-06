﻿<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ISD.Provider.Web._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <script type="text/javascript" src="assets/js/pages/index.js"></script>
    <div class="fullwidthbanner-container">
        <div class="fullwidthbanner">
            <ul>
                <!-- THE FIRST SLIDE -->
                <li data-transition="3dcurtain-vertical" data-slotamount="1" data-masterspeed="300" data-thumb="assets/img/sliders/revolution/thumbs/thumb1.jpg">
                    <!-- THE MAIN IMAGE IN THE FIRST SLIDE -->
                    <img src="Content/Banner/banner1.jpg" />
                   <!-- <div class="caption large_text sfb bg-black-opacity"
                        data-x="176"
                        data-y="12"
                        data-speed="300"
                        data-start="800"
                        data-easing="easeOutExpo">
                        OVER
                         <span style="color: #ffcc00;">7000</span>
                        SATISFIED CUSTOMERS
                    </div>-->

                </li>

                <!-- THE SECOND SLIDE -->
                <li data-transition="papercut" data-slotamount="1" data-masterspeed="300" data-delay="9400" data-thumb="assets/img/sliders/revolution/thumbs/thumb2.jpg">

                    <img src="Content/Banner/banner2.jpg" />
                    
                </li>

                <!-- THE THIRD SLIDE -->
                <li data-transition="slideleft" data-slotamount="1" data-masterspeed="300" data-thumb="assets/img/sliders/revolution/thumbs/thumb3.jpg">

                    <img src="Content/Banner/banner3.jpg" />
                </li>

                <li data-transition="flyin" data-slotamount="1" data-masterspeed="300" data-thumb="assets/img/sliders/revolution/thumbs/thumb4.jpg">

                    <img src="Content/Banner/banner4.jpg" />
                </li>
                <li data-transition="flyin" data-slotamount="1" data-masterspeed="300" data-thumb="assets/img/sliders/revolution/thumbs/thumb4.jpg">

                    <img src="Content/Banner/banner5.jpg" />
                </li>
                <li data-transition="flyin" data-slotamount="1" data-masterspeed="300" data-thumb="assets/img/sliders/revolution/thumbs/thumb4.jpg">

                    <img src="Content/Banner/banner6.jpg" />
                </li>
                <li data-transition="flyin" data-slotamount="1" data-masterspeed="300" data-thumb="assets/img/sliders/revolution/thumbs/thumb4.jpg">

                    <img src="Content/Banner/banner7.jpg" />
                </li>
                <li data-transition="flyin" data-slotamount="1" data-masterspeed="300" data-thumb="assets/img/sliders/revolution/thumbs/thumb4.jpg">

                    <img src="Content/Banner/banner8.jpg" />
                </li>
            </ul>

            <div class="tp-bannertimer tp-bottom"></div>
        </div>
    </div>
    <!--=== End Slider ===-->

    <!--=== Content Part ===-->
    <div class="container content">
        <div class="clearfix margin-bottom-40"></div>
        <div class="title-box">
            <div class="title-box-text">"We all want good health but sometimes things get in the way"</div>

            <small>CEO, Harry Majewski</small>
        </div>

        <div class="row margin-bottom-20">
            <!-- Welcome Block -->
            <div class="col-md-8 md-margin-bottom-40">
                <div class="headline">
                    <h2>Welcome To Internal Service Directory</h2>
                </div>
                <div class="row">
                    <div class="col-sm-4">
                        <img alt="" src="assets/img/main/6.jpg" class="img-responsive margin-bottom-20">
                    </div>
                    <div class="col-sm-8">
                        <p>This page is for organisations who are interested in being part of the service directory or are already registered as a Service Provider.</p>
                        <ul class="list-unstyled margin-bottom-20">
                            <li><i class="fa fa-check color-green"></i>List services provider by your organisation</li>
                            <li><i class="fa fa-check color-green"></i>Manage clinicians for your services</li>
                            <li><i class="fa fa-check color-green"></i>Powerful reporting tools for organisation</li>
                        </ul>
                    </div>
                </div>
            </div>
            <!--/col-md-8-->

            <!-- Latest Shots -->
            <div class="col-md-4 posts">
                <div class="headline"><h3>Recent Entries</h3></div>
                <dl class="dl-horizontal">
                    <dt><a href="#"><img src="assets/img/sliders/elastislide/6.jpg" alt=""></a></dt>
                    <dd>
                        <p><a href="#">Service #1</a></p>
                    </dd>
                </dl>
                <dl class="dl-horizontal">
                    <dt><a href="#"><img src="assets/img/sliders/elastislide/10.jpg" alt=""></a></dt>
                    <dd>
                        <p><a href="#">Service #2</a></p>
                    </dd>
                </dl>
                <dl class="dl-horizontal">
                    <dt><a href="#"><img src="assets/img/sliders/elastislide/11.jpg" alt=""></a></dt>
                    <dd>
                        <p><a href="#">Service #3</a></p>
                    </dd>
                </dl>
            </div>
            <!--/col-md-4-->
        </div>
        
        <div class="clearfix margin-bottom-30"></div>
        <div class="shadow-wrapper">
            <div class="tag-box tag-box-v1 box-shadow shadow-effect-2">
              
                <p>
                        Currently we're a pilot scheme in Boroondara and are aiming to go national in the future. The Club's philosophy is that good health can be achieved by participating in activities that look after our physical, mental and social health and well-being.
We want organisations who run healthy activities in Boroondara to list them with with on our website. It is free to be part of the Club and you can promote your activity, increase your member enrolments and produced customised reports and timetables.
                    </p>
            </div>
        </div>
    </div>
    <!--/container-->
    <!-- End Our Clients -->
</asp:Content>