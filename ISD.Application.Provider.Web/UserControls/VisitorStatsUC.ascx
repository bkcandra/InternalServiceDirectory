<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VisitorStatsUC.ascx.cs" Inherits="HealthyClub.Provider.Web.UserControls.VisitorStatsUC" %>
<script src="../Scripts/jquery-1.8.3.js"></script>
<!--[if lte IE 8]><script language="javascript" type="text/javascript" src="~/Scripts/flot/excanvas.min.js"></script><![endif]-->
<script language="javascript" type="text/javascript" src="../Scripts/flot/jquery.flot.min.js"></script>
<script src="../Scripts/flot/jquery.flot.time.js"></script>
<script src="../Scripts/flot/jquery.flot.selection.js"></script>
<script src="../Scripts/jquery.dateFormat-1.0.js"></script>
<script>
    $(document).ready(function () {
        var dataurl = '../api/Chart/' + $("#hdnProviderGUID").val();
        // setup plot

        function weekendAreas(axes) {

            var markings = [],
                d = new Date(axes.xaxis.min);

            // go to the first Saturday

            d.setUTCDate(d.getUTCDate() - ((d.getUTCDay() + 1) % 7))
            d.setUTCSeconds(0);
            d.setUTCMinutes(0);
            d.setUTCHours(0);

            var i = d.getTime();

            // when we don't set yaxis, the rectangle automatically
            // extends to infinity upwards and downwards

            do {
                markings.push({ xaxis: { from: i, to: i + 2 * 24 * 60 * 60 * 1000 } });
                i += 7 * 24 * 60 * 60 * 1000;
            } while (i < axes.xaxis.max);

            return markings;
        }

        $("<div id='tooltip'></div>").css({
            position: "absolute",
            display: "none",
            border: "1px solid #fdd",
            padding: "2px",
            "background-color": "#fee",
            opacity: 0.80
        }).appendTo("body");

        var options = {
            xaxis: {
                mode: "time",
                tickLength: 5
            },
            selection: {
                mode: "x"
            },
            series: {
                lines: {
                    show: true
                },
                points: {
                    show: true
                }
            },
            grid: {
                markings: weekendAreas,
                hoverable: true
            }

        };


        $.getJSON(dataurl, function (data) {
            var plot = $.plot($("#ProviderVisitorChart"), [data], options);
            var overview = $.plot("#ProviderVisitorOverview", [data], {
                series: {
                    lines: {
                        show: true,
                        lineWidth: 1
                    },
                    shadowSize: 0
                },
                xaxis: {
                    ticks: [],
                    mode: "time"
                },
                yaxis: {
                    ticks: [],
                    min: 0,
                    autoscaleMargin: 0.1
                },
                selection: {
                    mode: "x"
                }
            });


            $("#ProviderVisitorChart").bind("plotselected", function (event, ranges) {

                // do the zooming

                plot = $.plot("#ProviderVisitorChart", [data], $.extend(true, {}, options, {
                    xaxis: {
                        min: ranges.xaxis.from,
                        max: ranges.xaxis.to
                    }
                }));

                // don't fire event on the overview to prevent eternal loop

                overview.setSelection(ranges, true);
            });

            $("#ProviderVisitorOverview").bind("plotselected", function (event, ranges) {
                plot.setSelection(ranges);
            });


            $("#ProviderVisitorChart").bind("plothover", function (event, pos, item) {
                if (item) {
                    var x = item.datapoint[0],
                        y = item.datapoint[1];

                    $("#tooltip").html(item.series.label + " on " + $.format.date(new Date(x), "MMM yy") + " = " + y)
                        .css({ top: item.pageY + 5, left: item.pageX + 5 })
                        .fadeIn(200);
                } else {
                    $("#tooltip").hide();
                }

            });
        });




    });


</script>
<div id="content">
    <asp:HiddenField ID="hdnProviderGUID" ClientIDMode="Static" runat="server" />
    <div class="row">
        <div class="stats-date col-md-3">
            <div>Monthly Statistics</div>
            <div class="range">
                <asp:Label ID="lblRange" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <!--/col-->
        <div class="stats col-md-7">

            <div class="stat">
                <div class="left">
                    <div class="title"><span class="color yellow"></span>Pageviews</div>
                    <div class="number yellow">
                        <asp:Label ID="lblPageViews" runat="server" Text=""></asp:Label>
                    </div>

                </div>

            </div>
        </div>
        <!--/col-->
    </div>
    <!--/row-->
    <div class="row">
        <div class="col-xs-12 overflow:hidden">
            <section style="width: 800px; margin: 10px; text-align: center;">
                <div id="ProviderVisitorChart" style="width: 800px; height: 300px;">
                </div>
            </section>
        </div>
        <div class="col-xs-12 overflow:hidden">
            <section style="width: 800px; margin: 10px; text-align: center;">
                <div id="ProviderVisitorOverview" style="width: 800px; height: 125px;">
                </div>
                <h3 style="font-size: 1.4em">Visitor Overview - <%#DateTime.Now.Year.ToString() %> </h3>
            </section>
        </div>
        <!--/col-->

    </div>
    <!--/row-->
</div>
