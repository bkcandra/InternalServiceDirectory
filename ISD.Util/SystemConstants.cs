using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Util
{
    public class SystemConstants
    {

        public static string AdminUrl = "http://administration.healthyaustraliaclub.com.au/";
        public static string CustomerUrl = "http://www.healthyaustraliaclub.com.au/";
        public static string ProviderUrl = "http://provider.healthyaustraliaclub.com.au/";

        public static string ses_Rwdpts = "Rewardpts";
        public static string ses_FName = "FirstName";
        public static string ses_LName = "LastName";
        public static string ses_Role = "Role";
        public static string ses_Email = "Email";
        public static string ses_UserID = "UserID";
        public static string ses_IPAddress = "IPAddress";
        public static string ses_HostName = "IPHostName";

        public static string route_RewardsID = "RewardsID";
        public static string route_SponsorsID = "SponsorsID";
        public static string route_CategoryName = "CategoryName";
        public static string route_CategoryID = "Category";
        public static string route_ActivityID = "ActivityID";
        public static string route_ProviderID = "ProviderID";

        public static string qs_ImageID = "PID";
        public static string qs_RewardThumbImageID = "RTID";
        public static string qs_RewardImageID = "RID";
        public static string qs_ThumbImageID = "PTID";
        public static string qs_AssetID = "AID";
        public static string qs_UserImageID = "UID";
        public static string selectedCategory = "category";
        public static string selectedClinic = "clinic";
        public static string searchQuery = "search";


        public static string RewType = "_Rt";
        public static string sortType = "Reward Type";
        public static string sortPoints = "points";
        public static string SortValue = "_sv";
        public static string AgeTo = "_at";
        public static string AgeFrom = "_af";
        public static string FilterSuburbID = "_si";
        public static string DateTo = "_dt";
        public static string DateFrom = "_df";
        public static string TimeTo = "_tt";
        public static string TimeFrom = "_tf";
        public static string Filtered = "_Fi";
        public static string SuburbID = "_sid";
        public static string ViewType = "_vt";
        public static string PageType = "Pt";
        public static string EmailType = "Et";
        public static string PageID = "Pgid";
        public static string PageName = "Pages";
        public static string MondayisFiltered = "_m";
        public static string TuesdayisFiltered = "_t";
        public static string WedisFiltered = "_w";
        public static string ThurisFiltered = "_th";
        public static string FriisFiltered = "_f";
        public static string SatisFiltered = "_sa";
        public static string SunisFiltered = "_s";
        public static string AnyisFiltered = "_any";

        public static string reCaptchaPublicKey = "reCaptchaPublicKey";
        public static string reCaptchaPrivateKey = "reCaptchaPrivateKey";
        public static string GoogleAPIKey = "GoogleAPIKey";

        public static string Prepositions = "on; in; to; by; for; with; at; of; from; as;a;abaft;aboard;about;above;absent;across;afore;after;against;along;alongside;amid;amidst;among;amongst;an;anenst;apropos;apud;around;as;aside;astride;at;athwart;atop;barring;before;behind;below;beneath;beside;besides;between;betwixt;beyond;but;by;circa;concerning;despite;down;during;except;excluding;failing;following;for;forenenst;given;in;including;inside;into;lest;like;mid;midst;minus;modulo;near;next;of;off;on;onto;opposite;out;outside;over;pace;past;per;plus;pro;qua;regarding;round;sans;save;since;than;through;throughout;till;times;to;toward;towards;under;underneath;unlike;until;unto;up;upon;versus;via;vice;with;within;without;worth;";
        public static string Conjunctions = " for; and; nor; but; or; yet; so";

        public static string Location = "_Loc:";
        public static string Day = "_Day:";
        public static string Time = "_Time:";
        public static string Query = "_Query:";

        public static string FirstStart = "First";
        public static string CategoryID = "CategoryID";
        public static string ActivityID = "ActivityID";
        public static string ActivityName = "ActName";
        public static string UserID = "MemberID";
        public static string ProviderID = "ProviderID";
        public static string StateID = "StateID";
        public static string s_SpecificationPage = "~/Specification/";
        public static string CategoryRoot = "CategoryRoot";
        public static string DevUser = "Developer";
        public const string AdministratorRole = "Administrator";
        public const string ProviderRole = "Provider";
        public static string CustomerRole = "Customer";
        public static string GuestRole = "Guest";
        public static string SearchKey = "Search";
        public static string StartRow = "StartRow";
        public static string PageSize = "PageSize";
        public static string KeyCollectionID = "KeyCollectionID";
        public static string s_CurrentBrandSelected = "CurrentBrandSelected";
        public static string s_CurrentCategorySelected = "CurrentCategorySelected";
        public static DateTime nodate = Convert.ToDateTime("01/01/0001 00:00:00");

        public static long DatetimeMinTimeTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;
        public static string s_ImageType = "ImageType";

        public static string username = "usr";
        public static string ActivityListByCategory = "Activity List By Category";
        public static string ActivityListByProvider = "Activity List By Provider";
        public static string page = "Page";
        public static string externalLink = "externalLink";
        public static string EditMode = "EditMode";
        public static string CouncilID = "CouncilID";

        public static string TemplateID = "TemplateID";
        public static string token = "token";
        public static string userID = "userID";

        public static int ThumbnailWidth = 160;
        public static int ThumbnailHeight = 160;
        public static int MaxFileSize = 1024; // in Kb
        public static int MaxActivityImageStorage = 1024; // in Kb
        public static int intError = -1;
        public static int MaxUserImageStorage = 1024; // in Kb
        public static int MaxRequiredPasswordLength = 20;
        public static int MinRequiredPasswordLength = 6;


        public static string sortPrice = "Fee";
        public static string sortName = "Name";
        public static string sortExpiry = "Expiry";
        public static string sortLatest = "Latest";
        public static string sortNameDesc = "Name DESC";
        public static string sortExpiryDesc = "Expiry DESC";
        public static string sortLatestDesc = "Latest DESC";
        public static string sortPriceDesc = "Fee DESC";
        public static string sortPointsDesc = "points DESC";

        #region Application Message
        public static string EditSuccess = "Your changes has been saved.";
        public static string CreateSuccess = "New record has been saved.";
        public static string DeleteSuccess = "Record has been deleted.";
        public static string FormErrorMessage = "An error has occurred.";
        public static string ErrorTemplateDelete = "Template is currently in used. Please change the setting to avoid empty email delivery.";
        #endregion

        #region IconUrl
        public static string IconImageUrl = "~/Content/StyleImages/";
        #endregion

        #region Error
        public static string ActivateExpiredActivity = "This activity has expired.  Please edit your activity timetable.";
        public static string ErrorAddressnotGiven = "Please contact us on our details below to find out where this activity is held";
        public static string ErrorSchedulenotGiven = "Please contact us for timetable details.";
        public static string ErrorUserNotRegistered = "Username is not registered";

        #region registrationError
        public static string ErrorEmailAddressTaken = "Email address has already been registered.";
        #endregion

        #region LogError
        public static string ErrorUnableTofindActRefNumber = "Unable to find reference Number,New reference number is generated. Activity May have been updated by Provider";
        #endregion

        #region Activity Creation Error Text

        public static string ExecuteActivityMaintenance = "Activity Maintenance, Scanning all activities for obsolete activities and sending notification to user.";
        public static string ExecuteRewardMaintenance = "Rewards Maintenance, Scanning all Attendance Records for unprocessed attendance and awarding points to user.";


        public static string ErrorActivityExpired = "This activity has expired.  Please edit your activity timetable to update your activity details.";
        public static string ErrorActivityTimetableIsZero = "Please complete the activity timetable section";
        public static string ErrorActivityEndDateisNull = "Activity expiry date is required";
        public static string ErrorActivityNameisNull = "Activity name is required";
        public static string ErrorActivityCategoryNotSet = "Activity category is required";
        public static string ErrorActivityTypeNotSet = "Activity type is required";
        public static string FeesDescriptionIsNull = "Fee Description is required";
        public static string ContactDetailsIsNull = "Contact details is required";
        public static string ContactEmailIsNull = "Email address is required";
        public static string ContactAddressIsNull = "Activity address required";
        public static string ErrorAgeFromisNull = "Please enter what ages the activity is suitable for";
        public static string ErrorAgeToisNull = "Please enter what ages the activity is suitable for";
        public static string ErrorOrganisationNameTaken = "This organisation name has already been registered.  Please choose another name.";
        public static string ErrorUsernameTaken = "This username has already been registered.  Please choose another username.";
        public static string ErrorInvalidEmail = "Invalid email address";
        public static string WebAddressIsInvalid = "Website address is invalid";
        public static string ErrorAttendanceRecordIsNull = "Aborting rewards maintenance. Reason : Attendance Records is NULL or no unprocessed attendance records found.";

        #endregion
        #endregion

        #region IconName
        public static string IconActivityHidden = "Hidden.png";
        public static string IconActivityActive = "active.png";
        public static string IconActivityWillExpire = "expire.png";
        public static string IconActivityExpired = "expired.png";
        #endregion

        #region enum
        public enum FormMessageId { CreateSuccess, EditSuccess, DeleteSuccess, Error, ErrorTemplateDelete }
        public enum FormMode { View = 1, Edit = 2, New = 0 }
        public enum ActivityFeeCategory { Public_Free = 1, Public_Paid = 2, Private_Free = 3, Private_Paid = 4 }
        public enum ActivityStatus
        {
            /// <summary>
            /// Activity with this status is inactive therefore is hidden from user. Activity will not be checked for expiry date.
            /// </summary>
            NotActive = 0,
            /// <summary>
            /// Activity with this status is active therefore is visible to user. Activity will checked for expiry date and it's expiry date must be higher than first notification date(Default: x > 14 days).
            /// </summary>
            Active = 1,
            /// <summary>
            /// Activity with this status is active therefore is visible to user. Activity will checked for expiry date and it's expiry is between the first notification days and higher than second notification days(Default: 14 days => x > 7 days).
            /// </summary>
            WillExpire1 = 2,
            /// <summary>
            /// Activity with this status is active therefore is visible to user. Activity will checked for expiry date and it's expiry is between the second notification days and higher than expiry date(Default: 7 days => x > 0 days).
            /// </summary>
            WillExpire2 = 3,
            /// <summary>
            /// Activity with this status is inactive therefore is hidden from user. Activity will checked for expiry date and it's expiry is later than expiry date(Default:  0 days => x).
            /// </summary>
            Expired = 4,
            /// <summary>
            ///  Activity with this status is in the process of deletion.
            /// </summary>
            Deleting = 5,
            /// <summary>
            /// Activity with this status is deleted and can only recovered by admin.
            /// </summary>
            Deleted = 6
        }
        public enum ServiceRequirementsType { AND = 1, OR = 2 };
        public enum ServiceRequirementsValue { Medicare = 1, Pensioner = 2, HealthCare = 3, COBoroondara = 4, CIYarra = 5, HACC = 6 };
        public enum LogActionType { Notification1 = 1, Notification2 = 2, NotificationExpired = 3, NoNotification = 4, DeletingActivity = 5, AwardingRewardPoints = 6 }
        public enum ActivityViewType { ListView = 1, TableView = 2 }
        public enum RewardViewType { ListView = 1, TileView = 2 }
        public enum LogNoteType
        {
            Maintenance = 1,
            ActivityLog = 2
        }
        public enum TimetableFormat { notVisible = 1, Weekly = 2, Seasonal = 3 }
        public enum ScheduleDetailType { additionalDay = 1, ExceptionDay = 2 }
        public enum RecurrenceSchedule { NotRecurring = 1, Daily = 2, Weekly = 3, Fortnightly = 4, Monthly = 5 }
        public enum ScheduleViewFormat { Datagrid = 1, noTimetable = 2 }
        public enum PreferedContact { Email = 1, Phone = 2, Brochure = 3 }
        public enum Report { }
        public enum PageOrientation { Horizontal = 1, Vertical = 2 }
        public enum Gender { Male = 1, Female = 2 }
        public enum logType
        {
            /// <summary>
            /// Setting and fixing activity status. Called frequently on activity maintenance
            /// </summary>
            ActivityMaintenance = 1,
            /// <summary>
            /// Scanning activity attendance and awarding reward points. Called frequently on activity maintenance
            /// </summary>
            RewardMaintenance = 3
        }
        public enum UserRewardlogType
        {
            /// <summary>
            /// add reward points
            /// </summary>
            Add = 1,
            /// <summary>
            /// Substract reward points
            /// </summary>
            Redeem = 2
        }
        public enum maintenanceCategoryAction { ActivityStatusChange = 1, ActivityStatusChangeAndSendingEmail = 2 }
        public enum MenuTargetType { Category = 1, Provider = 2, Activity = 3, Page = 4, Tag = 5, ExternalLink = 6 }
        public enum ItemType { Standard = 1, Tab = 2, Sidebar = 3, Header = 4, Footer = 5, Brand = 6 }
        public enum EmailTemplateType { WelcomeEmail = 1, ProviderWelcomeEmail = 2, ConfirmationEmail = 3, ProviderConfirmationEmail = 4, ForgotPassword = 5, Expired2week = 6, Expired1week = 7, Expired = 8 }
        public enum MenuType { MemberMenu = 1, ProviderMenu = 2 }
        public enum userImageType { UserImage = 1, ProviderIcon = 2 }
        public enum userTitle { NoTitle = 0, Mr = 1, Mrs = 2, Ms = 3, Miss = 4, Dr = 5, Rev = 6 }
        public enum SavedListType { Activity = 1, Reward = 2 }
        public enum RewardType { Gift = 1, Offer = 2, Other = 3, Discount = 4 }
        public enum ListingNavigationType { search = 1, filter = 2, category = 3 }
        public enum UserRole { Provider = 1, Customer = 2, Guest = 3 }

        public enum CliniciansType { Public = 1, Private = 2, Both = 3 }
        #endregion

        #region ErrorMessage
        public static string err_MaximumCategorySelected
        {
            get
            {
                return "Maximum number of categories you can choose is five";
            }
        }
        public static string err_MaximumCategoryLevelExceeded
        {
            get
            {
                return "Maximum category hierarchy is 2!";
            }
        }

        #endregion

        #region globalAsax
        public static string str_ImageDirectory = "imageDirectory";
        public static string str_usrImageDirectory = "usrImageDirectory";
        public static string str_ImageThumbDirectory = "imageThumbDirectory";
        public static string str_TagImageDirectory = "TagImageDirectory";
        public static string str_UserFilesPath = "userFilesPath";
        public static string str_UploadedFilesPath = "uploadedFilesPath";
        public static string str_TemplatePath = "templatePath";
        public static string str_ScriptsPath = "scriptsPath";
        public static string str_AdminPath = "adminPath";

        public static string AdminPath = "";
        public static string ActImageDirectory = "~/UserFiles/ActImage/";
        public static string UsrImageDirectory = "~/UserFiles/UsrImage/";
        public static string TmpActImageDirectory = "~/UserFiles/TempImage/";
        public static string ImageThumbDirectory = "thumb/";
        public static string Thumbnail = "Thumbnail/";
        public static string TagImageDirectory = "~/UserFiles/Tags/";
        public static string UserFilesPath = "/userfiles/";

        public static string TemplatePath = "~/Templates/";
        public static string ScriptPath = "http://server1/furniture/Scripts/";
        public static string AdmintemplatePath = "";
        #endregion

        #region globalAsax
        public static string ServerImageDir = "";
        public static string ServerTagImageDir = "";

        public static string str_Logo = "Logo_";
        public static string str_Banner = "Banner_";
        public static string srt_FileSize = "MaxFileSize";
        #endregion

        #region ReturnURI

        public static string GetUserImageURL(String ProviderID, int imageID, string imageName)
        {
            return UsrImageDirectory + ProviderID + "/" + imageID + "_" + imageName;
        }

        public static string GetUserImageThumbURL(String ProviderID, int imageID, string imageName)
        {
            return UsrImageDirectory + ProviderID + "/" + ImageThumbDirectory + imageID + "_" + imageName;
        }

        public static string GetActivityImageURL(int activityID, int imageID, string imageName)
        {
            return ActImageDirectory + activityID + "/" + activityID + "_" + imageID + "_" + imageName;
        }

        public static string GetActivityImageThumbURL(int activityID, int imageID, string imageName)
        {
            return ActImageDirectory + activityID + "/" + ImageThumbDirectory + activityID + "_" + imageID + "_" + imageName;
        }

        public static string GetTempActImageDirectory(string ProviderID, string ActionKey)
        {
            return TmpActImageDirectory + ProviderID + "/" + ActionKey + "/";
        }
        public static string GetTempActivityImageThumbDirectory(string ProviderID, string ActionKey)
        {
            return TmpActImageDirectory + ProviderID + "/" + ActionKey + "/" + ImageThumbDirectory;
        }

        public static string GetTempActImageDirectoryURL(string ProviderID, string ActionKey, string imageName)
        {
            return TmpActImageDirectory + ProviderID + "/" + ActionKey + "/" + imageName;
        }

        public static string GetTempActivityImageThumbURL(string ProviderID, string ActionKey, string imageName)
        {
            return TmpActImageDirectory + ProviderID + "/" + ActionKey + "/" + ImageThumbDirectory + imageName;
        }

        #endregion


        public static string ActivityMaintenanceCode = "MNT";
        public static string RewardsMaintenanceCode = "REW";
        public static string ActivitiesCode = "ACT";
        public static string ProviderCode = "PRV";
        public static string UserCode = "USR";
        public static string SystemName = "System";

        public static int NotificationOneDays = 14; //Days from Expiry Date
        public static int NotificationTwoDays = 7; //Days from Expiry Date
        // Next Notification is send on expiry date


        public static int DefaultRewardPoints = 10;

        public static double browserFirefoxVersion = 10.0;
        public static double browserSafariVersion = 3.0;
        public static double browserOperaVersion = 8.0;
        public static double browserChromeVersion = 9.0;
        public static double browserIEVersion = 8.5;


        public static int ImageLimitInByte()
        {
            //4.5 Mb * 1024
            return 4608 * 1024;
        }
    }

    public class SuburbExplorer
    {
        public String Name { get; set; }
        public int ID { get; set; }
    }

    public class VisitorRecords
    {
        public DateTime DateTime { get; set; }
        public int Count { get; set; }

    }

    public class StatusChange
    {
        public int ActID { get; set; }
        public int StatusChangeTo { get; set; }
    }

    public class PointAwards
    {
        public String UserID { get; set; }
        public int points { get; set; }
        public int bonuspoints { get; set; }
    }
}
