using Google.GData.Analytics;
using ISD.Administration.Web;
using ISD.Administration.Web.Models;
using ISD.DA;
using ISD.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;


namespace ISD.Administration.Web.UserControls
{
    public partial class DashboardSummaryUC : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        public string VisitsNumber()
        {

            string visits = string.Empty;
            string username = "healthyaustraliaclub@gmail.com";
            string pass = "HealthyClub";
            string gkey = "?key=AIzaSyCYI0zTx4iGzL4gMqaplbd1TfkF9vMukZs";

            string dataFeedUrl = "https://www.google.com/analytics/feeds/data" + gkey;
            string accountFeedUrl = "https://www.googleapis.com/analytics/v2.4/management/accounts" + gkey;

            Google.GData.Analytics.AnalyticsService service = new Google.GData.Analytics.AnalyticsService("HealthyAustraliaClub");
            service.setUserCredentials(username, pass);

            DataQuery query1 = new DataQuery(dataFeedUrl);

            query1.Ids = "ga:76315108";
            query1.Metrics = "ga:visits";
            query1.Sort = "ga:visits";

            query1.GAStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
            query1.GAEndDate = DateTime.Now.ToString("yyyy-MM-dd");
            query1.StartIndex = 1;

            DataFeed dataFeedVisits = service.Query(query1);

            foreach (DataEntry entry in dataFeedVisits.Entries)
            {
                string st = entry.Title.Text;
                string ss = entry.Metrics[0].Value;
                visits = ss;
            }

            return visits;
        }


        public void initDash()
        {
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            lblAdmin.Text = rm.FindByName(SystemConstants.AdministratorRole).Users.Count.ToString();
            lblProviders.Text = rm.FindByName(SystemConstants.ProviderRole).Users.Count.ToString();
            lblMember.Text = rm.FindByName(SystemConstants.CustomerRole).Users.Count.ToString();
            lblUser.Text = Context.GetOwinContext().GetUserManager<ApplicationUserManager>().Users.Count().ToString();
            DataAccessComponent dac = new DataAccessComponent();

            int actCount = dac.RetrieveActivitiesCount((int)SystemConstants.ActivityStatus.Active, true) + dac.RetrieveActivitiesCount((int)SystemConstants.ActivityStatus.WillExpire2, true);
            lblApprovedActivity.Text = actCount.ToString();
            lblDeletedAct.Text = dac.RetrieveActivitiesCount((int)SystemConstants.ActivityStatus.Deleting, true).ToString();
            lblTotalActivity.Text = lblActivity.Text = dac.RetrieveActivitiesCount().ToString();
            lblCat.Text = dac.RetrieveCategoriesCount().ToString();
            lblWaitingActivity.Text = dac.RetrievePendingActivitiesCount().ToString();
            lblExpiredAct.Text = dac.RetrieveActivitiesCount((int)SystemConstants.ActivityStatus.Expired, true).ToString();
            lblReward.Text = dac.RetrieveRewardsExplorer().Count.ToString();
            try
            {
                lblVisitor.Text = VisitsNumber();
            }
            catch (Exception ex)
            {
                lblVisitor.Text = "ERR";
                lblError.Text = ex.Message;
            }
        }
    }
}