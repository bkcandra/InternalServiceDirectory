using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ISD.Util;
using ISD.EDS;
using System.Web.Security;
using ISD.BF;
using ISD.DA;
using System.Net;
using System.Xml.Linq;
using WebMatrix.WebData;



namespace ISD.User.Web.UserControls
{
    public partial class ActivityDetail : System.Web.UI.UserControl
    {
        public string Address
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnAddress.Value))
                    return hdnAddress.Value;
                else return "";
            }
            set
            {
                hdnAddress.Value = value.ToString();
                setGMapLoc();
            }
        }

        private void setGMapLoc()
        {
            double Lat = GetCoordinates(Address).Latitude;
            double Lng = GetCoordinates(Address).Longitude;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "setGMap", "FindLocation('" + Lat + "','" + Lng + "','" + Address + "')", true);

        }

        static string baseUri = "http://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false";
        string location = string.Empty;

        public static Coordinate GetCoordinates(string Address)
        {
            using (var client = new WebClient())
            {
                var requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", Uri.EscapeDataString(Address));

                var request = WebRequest.Create(requestUri);

                var response = request.GetResponse();
                var xdoc = XDocument.Load(response.GetResponseStream());
                var result = xdoc.Element("GeocodeResponse").Element("result");
                var locationElement = result.Element("geometry").Element("location");
                var lat = locationElement.Element("lat");
                var lng = locationElement.Element("lng");


                return new Coordinate(Convert.ToDouble(lat.Value), Convert.ToDouble(lng.Value));
            }
        }

        public struct Coordinate
        {
            private double lat;
            private double lng;

            public Coordinate(double latitude, double longitude)
            {
                lat = latitude;
                lng = longitude;

            }

            public double Latitude { get { return lat; } set { lat = value; } }
            public double Longitude { get { return lng; } set { lng = value; } }

        }

        //Reverse Gecoding
        public static void RetrieveFormatedAddress(string lat, string lng)
        {
            string requestUri = string.Format(baseUri, lat, lng);

            using (WebClient wc = new WebClient())
            {
                string result = wc.DownloadString(requestUri);
                var xmlElm = XElement.Parse(result);
                var status = (from elm in xmlElm.Descendants() where elm.Name == "status" select elm).FirstOrDefault();
                if (status.Value.ToLower() == "ok")
                {
                    var res = (from elm in xmlElm.Descendants() where elm.Name == "formatted_address" select elm).FirstOrDefault();
                    requestUri = res.Value;
                }
            }
        }

        public int ActivityID
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnActivityID.Value))
                    return Convert.ToInt32(hdnActivityID.Value);
                else return 0;
            }
            set
            {
                hdnActivityID.Value = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Page.RouteData.Values[SystemConstants.route_ActivityID] != null)
                    ActivityID = Convert.ToInt32(Page.RouteData.Values[SystemConstants.route_ActivityID].ToString());
                if (Request.QueryString[SystemConstants.route_ActivityID] != null)
                    ActivityID = Convert.ToInt32(Request.QueryString[SystemConstants.route_ActivityID]);
                if (ActivityID != 0)
                {
                    Refresh();
                }
                else
                {
                    Response.Redirect("~/Activities/");
                }
            }
        }



        private void LogVisitor()
        {
            string info = string.Empty;
            var dr = new DataSetComponent.ActivityVisitorDataTable().NewActivityVisitorRow();
            dr.ActivityID = ActivityID;
            if (!string.IsNullOrEmpty((string)Session[SystemConstants.ses_IPAddress]))
            {
                dr.IPAddress = (string)Session[SystemConstants.ses_IPAddress];
            }
            else
            {
                dr.IPAddress = "-";
            }

            if ((string)Session[SystemConstants.ses_Role] == SystemConstants.GuestRole)
            {
                dr.UserType = (int)SystemConstants.UserRole.Guest;
                info += "Name:Guest;";
                info += "UserHost:" + (string)Session[SystemConstants.ses_HostName] + ";";
            }
            else if ((string)Session[SystemConstants.ses_Role] == SystemConstants.ProviderRole)
            {
                dr.UserType = (int)SystemConstants.UserRole.Provider;
                info += "Name:" + (string)Session[SystemConstants.ses_FName] + " " + (string)Session[SystemConstants.ses_LName] + ";";
                info += "UserID:" + (string)Session[SystemConstants.ses_UserID] + ";";
                info += "UserHost:" + (string)Session[SystemConstants.ses_HostName] + ";";
            }
            else
            {
                dr.UserType = (int)SystemConstants.UserRole.Customer;
                info += "Name:" + (string)Session[SystemConstants.ses_FName] + " " + (string)Session[SystemConstants.ses_LName] + ";";
                info += "UserID:" + (string)Session[SystemConstants.ses_UserID] + ";";
                info += "UserHost:" + (string)Session[SystemConstants.ses_HostName] + ";";
            }
            dr.OtherInformation = info;
            dr.CreatedBy = SystemConstants.SystemName;
            dr.CreatedDatetime = DateTime.Now;

            new DataAccessComponent().InsertVisitor(dr);
        }

        private void LogVisitors(int count)
        {
            var dt = new DataSetComponent.ActivityVisitorDataTable();
            DateTime start = DateTime.Now.AddYears(-1);
            Random gen = new Random();

            HashSet<int> actIDs = new DataAccessComponent().RetrieveActivitiesIDs();
            for (int i = 0; i <= count; i++)
            {
                var dr = dt.NewActivityVisitorRow();
                string info = string.Empty;

                int curValue = gen.Next(62, 426);

                while (!actIDs.Contains(curValue))
                {
                    curValue = gen.Next(62, 426);
                }
                dr.ActivityID = curValue;
                //
                if (!string.IsNullOrEmpty((string)Session[SystemConstants.ses_IPAddress]))
                {
                    dr.IPAddress = (string)Session[SystemConstants.ses_IPAddress];
                }
                else
                {
                    dr.IPAddress = "-";
                }

                if ((string)Session[SystemConstants.ses_Role] == SystemConstants.GuestRole)
                {
                    dr.UserType = (int)SystemConstants.UserRole.Guest;
                    info += "Name:Guest;";
                    info += "UserHost:" + (string)Session[SystemConstants.ses_HostName] + ";";
                }
                else if ((string)Session[SystemConstants.ses_Role] == SystemConstants.ProviderRole)
                {
                    dr.UserType = (int)SystemConstants.UserRole.Provider;
                    info += "Name:" + (string)Session[SystemConstants.ses_FName] + " " + (string)Session[SystemConstants.ses_LName] + ";";
                    info += "UserID:" + (string)Session[SystemConstants.ses_UserID] + ";";
                    info += "UserHost:" + (string)Session[SystemConstants.ses_HostName] + ";";
                }
                else
                {
                    dr.UserType = (int)SystemConstants.UserRole.Customer;
                    info += "Name:" + (string)Session[SystemConstants.ses_FName] + " " + (string)Session[SystemConstants.ses_LName] + ";";
                    info += "UserID:" + (string)Session[SystemConstants.ses_UserID] + ";";
                    info += "UserHost:" + (string)Session[SystemConstants.ses_HostName] + ";";
                }
                dr.OtherInformation = info;
                dr.CreatedBy = SystemConstants.SystemName;


                int range = (DateTime.Today - start).Days;
                dr.CreatedDatetime = start.AddDays(gen.Next(range));

                dt.AddActivityVisitorRow(dr);
            }
            new DataAccessComponent().InsertVisitors(dt);
        }
        private void Refresh()
        {
            var dr = new DataAccessComponent().RetrieveActivityExplorer(ActivityID);
            if (dr != null)
            {
                LogVisitor();
                SetTitle(dr.ProviderID);
                SetActivityInformation(dr);
                SetTimetableInformation();
                ActivityNavigationUC1.SetNavigation(dr.Name, dr.ID, dr.CategoryID, dr.CategoryName);

                var drimages = new DataAccessComponent().RetrieveActivityImages(ActivityID);
                if (drimages != null)
                    ImagesListViewUC1.ActivityID = ActivityID;
                else
                    gallery.Visible = false;
            }
            else
            {
                Response.Redirect("~/Activities");
            }
        }

        private void SetTitle(Guid providerID)
        {
            DataAccessComponent dac = new DataAccessComponent();

            if (dac.IsUserImageExist(providerID))
            {
                divWithImage.Visible = true;
                divNoImage.Visible = false;
                int ImageID = new BusinessFunctionComponent().getProviderPrimaryImage(providerID);
                if (ImageID != 0)
                    ProviderImage.ImageUrl = "~/ImageHandler.ashx?" + SystemConstants.qs_UserImageID + "=" + ImageID;
                else
                {
                    divWithImage.Visible = false;
                    divNoImage.Visible = true;
                }
            }
            else
            {
                divWithImage.Visible = false;
                divNoImage.Visible = true;
            }

        }

        private void SetTimetableInformation()
        {
            ScheduleViewerUC1.ActivityID = Convert.ToInt32(hdnActivityID.Value);
            ScheduleViewerUC1.timetableFormat = (int)SystemConstants.TimetableFormat.Seasonal;
        }

        private void SetActivityInformation(DataSetComponent.v_ActivityExplorerRow dr)
        {
            divProductDesc.InnerHtml = dr.FullDescription;
            if (!string.IsNullOrEmpty(dr.Price))
            {
                divPriceDescription.InnerHtml = dr.Price;
            }
            else
            {
                divPriceDescription.InnerHtml = "This is a free activity";
            }
            if (!string.IsNullOrEmpty(dr.eligibilityDescription))
            {
                divEligivibility.InnerHtml = dr.eligibilityDescription;
            }

            trSuitability.Visible = !string.IsNullOrEmpty(dr.eligibilityDescription);
            lblContact.Text = dr.PhoneNumber;
            hlnkWebsite.Text = dr.Website;
            hlnkWebsite.NavigateUrl = "http://" + dr.Website;
            lblUpdate.Text = dr.ModifiedDateTime.Date.ToShortDateString();
            if (!string.IsNullOrEmpty(dr.Suburb))
            {
                lblAddress.Text = dr.Address;
                lblSub.Text = dr.Suburb + ",";
                lblState.Text = dr.StateName;
                lblPostCode.Text = dr.PostCode.ToString();
            }
            else
            {
                lblAddress.Text = SystemConstants.ErrorAddressnotGiven;
                lblSub.Visible = lblState.Visible = lblPostCode.Visible = false;
            }
            lblProvider.Text = lblProviderWImage.Text = dr.ProviderName;
            lblTitle.Text = lblTitleWImage.Text = dr.Name;
            lblContactName.Text = dr.FirstName + " " + dr.LastName;
            lblEmailAddress.Text = dr.Email;
            Address = dr.Address + " " + dr.Suburb + " " + dr.StateName + " " + dr.PostCode;

            Page.Title = dr.ProviderName + " - " + dr.Name + ", Healthy Australia Club";

            trAddress.Visible = !String.IsNullOrEmpty(Address);
            trCP.Visible = !String.IsNullOrEmpty(lblContactName.Text);
            trPhone.Visible = !String.IsNullOrEmpty(lblContact.Text);
            trEmail.Visible = !String.IsNullOrEmpty(dr.Email);
            trWebsite.Visible = !String.IsNullOrEmpty(dr.Website);
            trDescription.Visible = !String.IsNullOrEmpty(dr.FullDescription);

        }

        private bool AuthUser()
        {
            if (Membership.GetUser() != null)
            {
                var providerID = new Guid(Membership.GetUser().ProviderUserKey.ToString());
                var ownerLogin = new BusinessFunctionComponent().CheckActivityOwner(ActivityID, providerID);

                return ownerLogin;
            }
            else
            {
                Response.Redirect("~/Account/Login.aspx");
                return false;
            }
        }

        protected void lnkEditActivity_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Activity/EditActivity.aspx?" + SystemConstants.ActivityID + "=" + ActivityID);
        }
    }
}