using BCUtility;
using ISD.DA;
using ISD.EDS;
using ISD.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace ISD.BF
{
    public class BusinessFunctionComponent
    {
        #region category
        public bool DeleteCategories(int categoryID)
        {
            int activitiesInCategory = new DataAccessComponent().RetrieveActivitiesInCategoryCount(categoryID);
            if (activitiesInCategory != 0)
            {
                return false;
            }
            else
            {
                DataAccessComponent dac = new DataAccessComponent();

                // Delete the children first
                DataSetComponent.CategoryDataTable dt = dac.RetrieveAllSubCategories(categoryID);
                foreach (var dr in dt)
                {
                    dac.DeleteCategory(dr.ID);
                }
                //delete the parents
                dac.DeleteCategory(categoryID);
                return true;
            }
        }

        public void CreateCategory(int parentCategoryID, string userName, DataSetComponent.CategoryRow categoryDR)
        {
            DataAccessComponent dac = new DataAccessComponent();
            ManageCategoryLevel(parentCategoryID, categoryDR);

            categoryDR.CreatedBy = userName;
            categoryDR.CreatedDateTime = DateTime.Now;
            categoryDR.ModifiedBy = "";
            categoryDR.ModifiedDateTime = DateTime.Now;

            dac.CreateCategory(categoryDR);
        }

        private void ManageCategoryLevel(int parentCategoryID, DataSetComponent.CategoryRow categoryDR)
        {
            DataAccessComponent dac = new DataAccessComponent();
            int parentLevel = 0;

            var parentDR = dac.RetrieveCategory(parentCategoryID);

            if (parentDR != null)
            {
                parentLevel = dac.DetermineCategoryLevel(parentDR);
            }
            else
            {
                parentDR = new DataSetComponent.CategoryDataTable().NewCategoryRow();
            }

            if (!parentDR.IsLevel1ParentIDNull())
            {
                categoryDR.Level1ParentID = parentDR.Level1ParentID;
            }
            else
            {
                categoryDR.Level1ParentID = 0;
            }

            if (!parentDR.IsLevel2ParentIDNull())
            {
                categoryDR.Level2ParentID = parentDR.Level2ParentID;
            }
            else
            {
                categoryDR.Level2ParentID = 0;
            }

            switch (parentLevel)
            {
                case 0:
                    categoryDR.Level1ParentID = parentCategoryID;
                    break;
                case 1:
                    categoryDR.Level2ParentID = parentCategoryID;
                    break;
                case 2:
                    throw new Exception(SystemConstants.err_MaximumCategoryLevelExceeded);
            }
        }

        public void MoveCategory(int categoryID, int newParentCategoryID, string userName)
        {
            DataAccessComponent dac = new DataAccessComponent();
            var categoryDR = dac.RetrieveCategory(categoryID);
            ManageCategoryLevel(newParentCategoryID, categoryDR);

            categoryDR.ModifiedBy = userName;
            categoryDR.ModifiedDateTime = DateTime.Now;

            dac.UpdateCategory(categoryDR);

            var childrenDT = dac.RetrieveAllSubCategories(categoryID).OrderBy(p => p.Level);
            if (childrenDT.Count() != 0)
                using (TransactionScope trans = new TransactionScope())
                {
                    foreach (var childDR in childrenDT)
                    {
                        switch (childDR.Level)
                        {
                            case 1:
                                ManageCategoryLevel(childDR.Level1ParentID, childDR);
                                break;
                            case 2:
                                ManageCategoryLevel(childDR.Level2ParentID, childDR);
                                break;
                        }

                        childDR.ModifiedBy = userName;
                        childDR.ModifiedDateTime = DateTime.Now;
                        dac.UpdateCategory(childDR);
                    }

                    trans.Complete();
                }

        }

        public void UpdateCategory(int categoryID, string name, int parentID, string userName)
        {
            DataAccessComponent dac = new DataAccessComponent();
            var dr = dac.RetrieveCategory(categoryID);
            dr.Name = name;

            dr.ModifiedDateTime = DateTime.Now;
            dr.ModifiedBy = userName;

            ManageCategoryLevel(parentID, dr);

            dac.UpdateCategory(dr);
        }
        #endregion

        #region ActivityImage
        public void CreateActivityImage(DataSetComponent.ActivityImageDetailRow dr, out int imageID, int filesize)
        {
            DataAccessComponent dac = new DataAccessComponent();
            var iInfo = dac.RetrieveActivityImageInformation(dr.ActivityID);
            int iiID = 0;
            if (iInfo == null)
            {
                dr.isPrimaryImage = true;

                var ii = new DataSetComponent.ActivityImageDataTable().NewActivityImageRow();
                ii.ActivityID = dr.ActivityID;
                ii.StorageUsed = 0;
                ii.FreeStorage = SystemConstants.MaxActivityImageStorage;
                ii.ImageAmount = 0;
                dac.createActivityImageInformation(ii, out iiID);
            }
            else
                dr.ActivityImageID = iInfo.ID;

            using (TransactionScope trans = new TransactionScope())
            {
                dac.CreateActivityImage(dr, out imageID);

                var ii = dac.RetrieveActivityImageInformation(dr.ActivityID);
                ii.ActivityID = dr.ActivityID;
                ii.StorageUsed = ii.StorageUsed + filesize;
                ii.FreeStorage = ii.FreeStorage - filesize;
                ii.ImageAmount = ii.ImageAmount + 1;

                dac.UpdateImageInformation(dr.ActivityID, iiID, ii);
                trans.Complete();
            }
        }

        public void DeleteActivityImage(int ActivityID, int imageID, int filesize, out string imageThumbVirtualPath, out string imageVirtualPath)
        {
            DataAccessComponent dac = new DataAccessComponent();
            var dr = dac.RetrieveActivityImage(ActivityID, imageID);
            if (dr.isPrimaryImage == true)
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    dac.DeleteActivityImage(ActivityID, imageID, out imageThumbVirtualPath, out imageVirtualPath);

                    var ii = dac.RetrieveActivityImageInformation(dr.ActivityID);
                    ii.ActivityID = dr.ActivityID;
                    ii.StorageUsed = ii.StorageUsed - filesize;
                    ii.FreeStorage = ii.FreeStorage + filesize;
                    ii.ImageAmount = ii.ImageAmount - 1;

                    dac.UpdateImageInformation(dr.ActivityID, ii.ID, ii);
                    trans.Complete();
                }

                var dt = dac.RetrieveActivityImages(ActivityID);
                if (dt.Count() != 0)
                {
                    int ActivityID1 = dt[0].ActivityID;
                    int imageID1 = dt[0].ID;
                    dac.UpdateActivityPrimaryImage(ActivityID1, imageID1);
                }
            }
            else
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    dac.DeleteActivityImage(ActivityID, imageID, out imageThumbVirtualPath, out imageVirtualPath);

                    var ii = dac.RetrieveActivityImageInformation(dr.ActivityID);
                    ii.ActivityID = dr.ActivityID;
                    ii.StorageUsed = ii.StorageUsed - filesize;
                    ii.FreeStorage = ii.FreeStorage + filesize;
                    ii.ImageAmount = ii.ImageAmount - 1;

                    dac.UpdateImageInformation(dr.ActivityID, ii.ID, ii);
                    trans.Complete();
                }
            }
        }

        public string RetrieveImageUrl(int activityID, int imageID)
        {
            var imgDR = new DataAccessComponent().RetrieveActivityImage(activityID, imageID);

            if (!imgDR.IsImageTitleNull())
                return SystemConstants.GetActivityImageURL(activityID, imageID, imgDR.Filename);
            else
                return SystemConstants.ActImageDirectory + "No image.jpg";
        }

        public string RetrieveImageThumbUrl(int activityID, int imageID)
        {
            var imgDR = new DataAccessComponent().RetrieveActivityImage(activityID, imageID);

            if (!imgDR.IsImageTitleNull())
                return SystemConstants.GetActivityImageThumbURL(activityID, imageID, imgDR.Filename);
            else
                return SystemConstants.ActImageDirectory + "No image.jpg";
        }

        public void CreateActivityImages(int ActivityID, DataSetComponent.ActivityImageRow drInfo, DataSetComponent.ActivityImageDetailDataTable dt)
        {
            DataAccessComponent dac = new DataAccessComponent();
            var info = dac.RetrieveActivityImageInformation(ActivityID);
            int count = info.ImageAmount;
            using (TransactionScope trans = new TransactionScope())
            {

                //Insert Images
                foreach (var dr in dt)
                {
                    if (count == 0)
                        dr.isPrimaryImage = true;
                    else dr.isPrimaryImage = false;

                    dac.CreateActivityImage(dr);
                    count++;
                }
                //Update ImageInfo
                dac.UpdateImageInformation(ActivityID, drInfo.ID, drInfo);
                trans.Complete();
            }
        }

        #endregion

        #region Council

        public bool DeleteCouncil(int councilID)
        {
            int SuburbInCouncil = new DataAccessComponent().RetrieveCouncilSuburbsCount(councilID);
            if (SuburbInCouncil != 0)
            {
                return false;
            }
            else
            {
                new DataAccessComponent().DeleteCouncil(councilID);
                return true;
            }

        }

        #endregion

        #region Maintenance

        public void ActivityMaintenance(string User)
        {
            var webconf = new DataAccessComponent().RetrieveWebConfiguration();
            int thisYear = DateTime.Now.Year;
            var DT = new DataAccessComponent().RetrieveActivitiesDontIncludeDeleted();
            int Modifiedindex = 0;
            bool sendEmail = webconf.SendNotificationEmail;

            //Create Maintenance Log
            var WebLog = new DataSetComponent.WebLogDataTable().NewWebLogRow();
            WebLog.Message = SystemConstants.ExecuteActivityMaintenance + "%NL%" + "Datetime:" + DateTime.Now + "Requested by:" + User;
            WebLog.LogCategory = (int)SystemConstants.logType.ActivityMaintenance;
            WebLog.CreatedDateTime = DateTime.Now;
            WebLog.CreatedBy = User;
            WebLog.ReferenceNumber = SystemConstants.ActivityMaintenanceCode + " " + BCUtility.ObjectHandler.GetRandomKey(9);
            WebLog.Note = "";

            //Create Maintenance Log Action
            var webLogActionCollection = new DataSetComponent.WeblLogActionDataTable();

            List<StatusChange> stats = new List<StatusChange>();

            foreach (var dr in DT)
            {
                int today = DateTime.Now.DayOfYear;
                int expiryDays = dr.ExpiryDate.DayOfYear + ((dr.ExpiryDate.Year - thisYear) * 365);

                var status = "";
                if (dr.Status == (int)SystemConstants.ActivityStatus.Expired)
                    status = "EXPIRED";
                if (dr.Status == (int)SystemConstants.ActivityStatus.WillExpire2)
                    status = "EXPIRING";

                if ((expiryDays - today) > webconf.FirstNotificationDays && dr.Status != (int)SystemConstants.ActivityStatus.Active && dr.Status != (int)SystemConstants.ActivityStatus.NotActive && dr.Status != (int)SystemConstants.ActivityStatus.Deleting && dr.Status != (int)SystemConstants.ActivityStatus.Deleted)
                {
                    //new DataAccessComponent().ChangeStatus(dr.ID, (int)SystemConstants.ActivityStatus.Active);
                    StatusChange stat = new StatusChange();
                    stat.ActID = dr.ID;
                    stat.StatusChangeTo = (int)SystemConstants.ActivityStatus.Active;
                    stats.Add(stat);

                    var LogAction = webLogActionCollection.NewWeblLogActionRow();
                    LogAction.ActionType = (int)SystemConstants.LogActionType.NoNotification;
                    LogAction.Message = "Activity ID:" + dr.ID + "%NL%";
                    LogAction.Message += "Flag: invalid Status(" + status + "),  Expiry days is more 14 days.  ";
                    LogAction.Message += "Action Taken: Change status to ACTIVE" + "%NL%";
                    LogAction.LogCategory = (int)SystemConstants.maintenanceCategoryAction.ActivityStatusChange;
                    LogAction.CreatedDateTime = DateTime.Now;
                    LogAction.CreatedBy = User;
                    LogAction.Value = dr.ID.ToString() + "|" + dr.Name + "|" + dr.Status + "|" + dr.ExpiryDate + "|" + dr.ProviderID;

                    webLogActionCollection.AddWeblLogActionRow(LogAction);
                    Modifiedindex++;

                }
                else if ((expiryDays - today) <= webconf.FirstNotificationDays && (expiryDays - today) > webconf.SecondNotificationDays && dr.Status != (int)SystemConstants.ActivityStatus.WillExpire1 && dr.Status != (int)SystemConstants.ActivityStatus.NotActive && dr.Status != (int)SystemConstants.ActivityStatus.Deleting && dr.Status != (int)SystemConstants.ActivityStatus.Deleted)
                {
                    //new DataAccessComponent().ChangeStatus(dr.ID, (int)SystemConstants.ActivityStatus.WillExpire);
                    StatusChange stat = new StatusChange();
                    stat.ActID = dr.ID;
                    stat.StatusChangeTo = (int)SystemConstants.ActivityStatus.WillExpire1;
                    stats.Add(stat);


                    var LogAction = webLogActionCollection.NewWeblLogActionRow();
                    LogAction.ActionType = (int)SystemConstants.LogActionType.Notification1;
                    LogAction.Message = "Activity ID:" + dr.ID + "%NL%";
                    LogAction.Message += "Activity Name:" + dr.Name + "%NL%";
                    LogAction.Message += "Flag: Change Activity Status(" + status + "), Activity expire in 14 days.  ";

                    if (!sendEmail)
                    {
                        LogAction.Message += "Action Taken: Change status to EXPIRING, Sending Notification 1";
                        LogAction.LogCategory = (int)SystemConstants.maintenanceCategoryAction.ActivityStatusChange;
                    }
                    else
                    {
                        LogAction.Message += "Action Taken: Change status to EXPIRING";
                        LogAction.LogCategory = (int)SystemConstants.maintenanceCategoryAction.ActivityStatusChangeAndSendingEmail;
                    }

                    LogAction.CreatedDateTime = DateTime.Now;
                    LogAction.CreatedBy = User;
                    LogAction.Value = dr.ID.ToString() + "|" + dr.Name + "|" + dr.Status + "|" + dr.ExpiryDate + "|" + dr.ProviderID;

                    webLogActionCollection.AddWeblLogActionRow(LogAction);
                    Modifiedindex++;
                }
                else if ((expiryDays - today) <= webconf.SecondNotificationDays && (expiryDays - today) >= 1 && dr.Status != (int)SystemConstants.ActivityStatus.WillExpire2 && dr.Status != (int)SystemConstants.ActivityStatus.NotActive && dr.Status != (int)SystemConstants.ActivityStatus.Deleting && dr.Status != (int)SystemConstants.ActivityStatus.Deleted)
                {
                    // new DataAccessComponent().ChangeStatus(dr.ID, (int)SystemConstants.ActivityStatus.WillExpire);
                    StatusChange stat = new StatusChange();
                    stat.ActID = dr.ID;
                    stat.StatusChangeTo = (int)SystemConstants.ActivityStatus.WillExpire2;
                    stats.Add(stat);

                    var LogAction = webLogActionCollection.NewWeblLogActionRow();
                    LogAction.ActionType = (int)SystemConstants.LogActionType.Notification2;
                    LogAction.Message = "Activity ID:" + dr.ID + "%NL%";
                    LogAction.Message += "Activity Name:" + dr.Name + "%NL%";
                    LogAction.Message += "Flag: Change Activity Status(" + status + "), Activity expire in 7 days.  ";
                    if (!sendEmail)
                    {
                        LogAction.Message += "Action Taken: Change status to EXPIRING, Sending Notification 2";
                        LogAction.LogCategory = (int)SystemConstants.maintenanceCategoryAction.ActivityStatusChange;
                    }
                    else
                    {
                        LogAction.Message += "Action Taken: Change status to EXPIRING";
                        LogAction.LogCategory = (int)SystemConstants.maintenanceCategoryAction.ActivityStatusChangeAndSendingEmail;
                    }

                    LogAction.CreatedDateTime = DateTime.Now;
                    LogAction.CreatedBy = User;
                    LogAction.Value = dr.ID.ToString() + "|" + dr.Name + "|" + dr.Status + "|" + dr.ExpiryDate + "|" + dr.ProviderID;

                    webLogActionCollection.AddWeblLogActionRow(LogAction);
                    Modifiedindex++;
                }
                else if ((expiryDays - today) < 0 && dr.Status != (int)SystemConstants.ActivityStatus.Expired && dr.Status != (int)SystemConstants.ActivityStatus.NotActive && dr.Status != (int)SystemConstants.ActivityStatus.Deleting && dr.Status != (int)SystemConstants.ActivityStatus.Deleted)
                {
                    //new DataAccessComponent().ChangeStatus(dr.ID, (int)SystemConstants.ActivityStatus.Expired);

                    StatusChange stat = new StatusChange();
                    stat.ActID = dr.ID;
                    stat.StatusChangeTo = (int)SystemConstants.ActivityStatus.Expired;
                    stats.Add(stat);

                    var LogAction = webLogActionCollection.NewWeblLogActionRow();
                    LogAction.ActionType = (int)SystemConstants.LogActionType.NotificationExpired;
                    LogAction.Message = "Activity ID:" + dr.ID + "%NL%";
                    LogAction.Message += "Activity Name:" + dr.Name + "%NL%";
                    LogAction.Message += "Flag: invalid Status(" + status + "),  Activity is Expired.  ";
                    LogAction.Message += "Action Taken: Change status to EXPIRED" + "%NL%";

                    LogAction.LogCategory = (int)SystemConstants.logType.ActivityMaintenance;
                    LogAction.CreatedDateTime = DateTime.Now;
                    LogAction.CreatedBy = User;
                    LogAction.Value = dr.ID.ToString() + "|" + dr.Name + "|" + dr.Status + "|" + dr.ExpiryDate + "|" + dr.ProviderID;
                    webLogActionCollection.AddWeblLogActionRow(LogAction);
                    Modifiedindex++;
                }


                if (dr.Status == (int)SystemConstants.ActivityStatus.Deleting && (today - dr.ModifiedDateTime.DayOfYear >= 3))
                {
                    //new DataAccessComponent().ChangeStatus(dr.ID, (int)SystemConstants.ActivityStatus.Deleted);

                    StatusChange stat = new StatusChange();
                    stat.ActID = dr.ID;
                    stat.StatusChangeTo = (int)SystemConstants.ActivityStatus.Deleted;
                    stats.Add(stat);

                    var LogAction = webLogActionCollection.NewWeblLogActionRow();
                    LogAction.ActionType = (int)SystemConstants.LogActionType.DeletingActivity;
                    LogAction.Message = "Activity ID:" + dr.ID + "%NL%";
                    LogAction.Message += "Activity Name:" + dr.Name + "%NL%";
                    LogAction.Message += "Flag: Changing Status(" + status + "),  Activity is now Deleted.  ";
                    LogAction.Message += "Action Taken: Change status to DELETED" + "%NL%";
                    LogAction.LogCategory = (int)SystemConstants.logType.ActivityMaintenance;
                    LogAction.CreatedDateTime = DateTime.Now;
                    LogAction.CreatedBy = User;
                    LogAction.Value = dr.ID.ToString() + "|" + dr.Name + "|" + dr.Status + "|" + dr.ExpiryDate + "|" + dr.ProviderID;
                    webLogActionCollection.AddWeblLogActionRow(LogAction);
                    Modifiedindex++;
                }
            }

            new DataAccessComponent().ChangeStatus(stats);

            //Create Activities Log Action
            var ActivitiesLogGroupCollection = new DataSetComponent.ActivitiesLogGroupDataTable();
            int WebLogId = 0;

            if (Modifiedindex >= 1)
            {
                int ActivityLogGroupID = 0;
                WebLog.Message += " " + Modifiedindex + " activities is affected";
                new DataAccessComponent().SaveLog(WebLog, out WebLogId);

                foreach (var webLogAction in webLogActionCollection)
                {
                    // Check if notification 1 is already sent
                    int emailSent = 0;
                    var refnumber = "";

                    int notificationType = 0;
                    // activityInfo Value Format
                    // [INT actID] | [STRING actName] | [INT actStatus] | [DATETIME actExpiryDate] | [String actProviderID]
                    // ==================================================================================================
                    string[] activityInfo = webLogAction.Value.Split('|');

                    int activityID = Convert.ToInt32(activityInfo[0]);
                    string activityName = activityInfo[1];
                    int activityStatus = Convert.ToInt32(activityInfo[2]);
                    DateTime activityExpDate = Convert.ToDateTime(activityInfo[3]);
                    String activityProviderID = activityInfo[4];



                    if (webLogAction.ActionType == (int)SystemConstants.LogActionType.Notification1)
                    {
                        var newactLogGroup = ActivitiesLogGroupCollection.NewActivitiesLogGroupRow();
                        newactLogGroup.ActivityID = activityID;
                        refnumber = newactLogGroup.ReferenceNumber = SystemConstants.ActivitiesCode + " " + BCUtility.ObjectHandler.GetRandomKey(9);
                        if (sendEmail)
                            emailSent++;
                        newactLogGroup.EmailSentNumber = emailSent;
                        newactLogGroup.LastSent = DateTime.Now;
                        notificationType = newactLogGroup.LastNotificationType = (int)SystemConstants.LogActionType.Notification1;
                        newactLogGroup.ExpiryDate = activityExpDate;
                        newactLogGroup.CreatedDatetime = DateTime.Now;
                        newactLogGroup.CreatedBy = User;
                        new DataAccessComponent().createActivityLogGroup(newactLogGroup, out ActivityLogGroupID);
                    }
                    else if (webLogAction.ActionType == (int)SystemConstants.LogActionType.Notification2)
                    {
                        var actLogGroup = new DataAccessComponent().RetrievePastActivityLogGroup(activityID, (int)SystemConstants.LogActionType.Notification1, activityExpDate.Date);
                        if (actLogGroup != null)
                        {
                            emailSent = actLogGroup.EmailSentNumber;
                            actLogGroup.ActivityID = activityID;
                            if (sendEmail)
                                emailSent++;
                            actLogGroup.EmailSentNumber = emailSent;
                            notificationType = actLogGroup.LastNotificationType = (int)SystemConstants.LogActionType.Notification2;
                            actLogGroup.ExpiryDate = activityExpDate;
                            refnumber = actLogGroup.ReferenceNumber;
                            new DataAccessComponent().UpdateActivityogGroup(actLogGroup, actLogGroup.ID);
                            ActivityLogGroupID = actLogGroup.ID;
                        }
                        else
                        {
                            var newactLogGroup = ActivitiesLogGroupCollection.NewActivitiesLogGroupRow();
                            newactLogGroup.ActivityID = activityID;
                            refnumber = newactLogGroup.ReferenceNumber = SystemConstants.ActivitiesCode + " " + BCUtility.ObjectHandler.GetRandomKey(9);
                            if (sendEmail)
                                emailSent++;
                            newactLogGroup.EmailSentNumber = emailSent;
                            newactLogGroup.LastSent = DateTime.Now;

                            notificationType = newactLogGroup.LastNotificationType = (int)SystemConstants.LogActionType.Notification2;
                            newactLogGroup.ExpiryDate = activityExpDate;
                            newactLogGroup.CreatedDatetime = DateTime.Now;
                            newactLogGroup.CreatedBy = User;
                            new DataAccessComponent().createActivityLogGroup(newactLogGroup, out ActivityLogGroupID);
                        }
                    }
                    else if (webLogAction.ActionType == (int)SystemConstants.LogActionType.NotificationExpired)
                    {
                        var actLogGroup = new DataAccessComponent().RetrievePastActivityLogGroup(activityID, (int)SystemConstants.LogActionType.Notification2, activityExpDate.Date);
                        if (actLogGroup != null)
                        {
                            emailSent = actLogGroup.EmailSentNumber;
                            actLogGroup.ActivityID = activityID;
                            if (sendEmail)
                                emailSent++;
                            actLogGroup.EmailSentNumber = emailSent;

                            notificationType = actLogGroup.LastNotificationType = (int)SystemConstants.LogActionType.NotificationExpired;
                            actLogGroup.ExpiryDate = activityExpDate;
                            refnumber = actLogGroup.ReferenceNumber;
                            new DataAccessComponent().UpdateActivityogGroup(actLogGroup, actLogGroup.ID);
                            ActivityLogGroupID = actLogGroup.ID;
                        }
                        else
                        {
                            var secActLogGroup = new DataAccessComponent().RetrievePastActivityLogGroup(activityID, (int)SystemConstants.LogActionType.Notification1, activityExpDate.Date);
                            if (secActLogGroup != null)
                            {
                                emailSent = secActLogGroup.EmailSentNumber;
                                secActLogGroup.ActivityID = activityID;
                                if (sendEmail)
                                    emailSent++;
                                secActLogGroup.EmailSentNumber = emailSent;

                                notificationType = secActLogGroup.LastNotificationType = (int)SystemConstants.LogActionType.NotificationExpired;
                                secActLogGroup.ExpiryDate = activityExpDate;
                                refnumber = secActLogGroup.ReferenceNumber;
                                new DataAccessComponent().UpdateActivityogGroup(secActLogGroup, secActLogGroup.ID);
                                ActivityLogGroupID = secActLogGroup.ID;
                            }
                            else
                            {
                                var newactLogGroup = ActivitiesLogGroupCollection.NewActivitiesLogGroupRow();
                                newactLogGroup.ActivityID = activityID;
                                refnumber = newactLogGroup.ReferenceNumber = SystemConstants.ActivitiesCode + " " + BCUtility.ObjectHandler.GetRandomKey(9);
                                if (sendEmail)
                                    emailSent++;
                                newactLogGroup.EmailSentNumber = emailSent;

                                newactLogGroup.LastSent = DateTime.Now;
                                notificationType = newactLogGroup.LastNotificationType = (int)SystemConstants.LogActionType.Notification2;
                                newactLogGroup.ExpiryDate = activityExpDate;
                                newactLogGroup.CreatedDatetime = DateTime.Now;
                                newactLogGroup.CreatedBy = User;
                                new DataAccessComponent().createActivityLogGroup(newactLogGroup, out ActivityLogGroupID);
                            }
                        }
                    }
                    else if (webLogAction.ActionType == (int)SystemConstants.LogActionType.DeletingActivity)
                    {
                        var newactLogGroup = ActivitiesLogGroupCollection.NewActivitiesLogGroupRow();
                        newactLogGroup.ActivityID = activityID;
                        refnumber = newactLogGroup.ReferenceNumber = SystemConstants.ActivitiesCode + " " + BCUtility.ObjectHandler.GetRandomKey(9);
                        newactLogGroup.EmailSentNumber = 0;
                        newactLogGroup.LastSent = DateTime.Now;
                        notificationType = newactLogGroup.LastNotificationType = (int)SystemConstants.LogActionType.NoNotification;
                        newactLogGroup.ExpiryDate = activityExpDate;
                        newactLogGroup.CreatedDatetime = DateTime.Now;
                        newactLogGroup.CreatedBy = User;
                        ActivitiesLogGroupCollection.AddActivitiesLogGroupRow(newactLogGroup);
                    }

                    int LogActionID = 0;
                    webLogAction.WebLogID = WebLogId;
                    new DataAccessComponent().SaveWebLogAction(webLogAction, out LogActionID);

                    var actLog = new DataSetComponent.ActivitiesLogDataTable().NewActivitiesLogRow();
                    actLog.LogType = (int)SystemConstants.LogNoteType.Maintenance;
                    actLog.ReferenceNumber = refnumber;
                    actLog.Message = "Activity ID:" + activityID + "%NL%";
                    actLog.Message += "Activity Name:" + activityName + "%NL%";
                    actLog.Note = "";
                    actLog.CreatedBy = SystemConstants.SystemName;
                    actLog.CreatedDatetime = DateTime.Now;
                    actLog.Value = activityExpDate.ToShortDateString();
                    actLog.NotificationNumber = notificationType;
                    actLog.ActivityLogGroupID = ActivityLogGroupID;
                    if (webLogAction.ActionType == (int)SystemConstants.LogActionType.Notification2)
                    {

                        actLog.Message += "Flag: invalid Status(" + activityStatus + "),  Activity is Expiring.  ";
                        actLog.Message += "Action Taken: Change status to EXPIRING" + "%NL%";
                        actLog.Message += "Number of Email sent: " + emailSent + "%NL%";
                        actLog.Message += "Today: " + DateTime.Now.ToShortDateString();
                        if (sendEmail)
                        {
                            var SendEmail = SendNotificationEmail(activityID, actLog.NotificationNumber, actLog.ReferenceNumber, activityProviderID.ToString());
                            actLog.EmailSent = true;
                        }
                        else
                        {
                            actLog.Message += "%NL%" + "Note: Cannot send email, setting is set to Disable sending email";
                            actLog.EmailSent = false;
                        }
                        actLog.LogActionID = LogActionID;
                        new DataAccessComponent().SaveActivityLog(actLog);

                    }
                    else if (webLogAction.ActionType == (int)SystemConstants.LogActionType.Notification1)
                    {

                        actLog.Message += "Flag: invalid Status(" + activityStatus + "),  Activity is Expiring.  ";
                        actLog.Message += "Action Taken: Change status to EXPIRING" + "%NL%";
                        actLog.Message += "Number of Email sent: " + emailSent + "%NL%";
                        actLog.Message += "Today: " + DateTime.Now.ToShortDateString();
                        if (sendEmail)
                        {
                            var SendEmail = SendNotificationEmail(activityID, actLog.NotificationNumber, actLog.ReferenceNumber, activityProviderID.ToString());
                            actLog.EmailSent = true;
                        }
                        else
                        {
                            actLog.Message += "%NL%" + "Note: Cannot send email, setting is set to Disable sending email";
                            actLog.EmailSent = false;
                        }
                        actLog.LogActionID = LogActionID;
                        new DataAccessComponent().SaveActivityLog(actLog);

                    }
                    else if (webLogAction.ActionType == (int)SystemConstants.LogActionType.NotificationExpired)
                    {
                        actLog.Message += "Flag: invalid Status(" + activityStatus + "),  Activity is Expired.  ";
                        actLog.Message += "Action Taken: Change status to EXPIRED" + "%NL%";
                        actLog.Message += "Number of Email sent: " + emailSent + "%NL%";
                        actLog.Message += "Today: " + DateTime.Now.ToShortDateString();
                        if (sendEmail)
                        {
                            var SendEmail = SendNotificationEmail(activityID, actLog.NotificationNumber, actLog.ReferenceNumber, activityProviderID.ToString());
                            actLog.EmailSent = true;
                        }
                        else
                        {
                            actLog.Message += "%NL%" + "Note: Cannot send email, setting is set to Disable sending email";
                            actLog.EmailSent = false;
                        }
                        actLog.LogActionID = LogActionID;
                        new DataAccessComponent().SaveActivityLog(actLog);

                    }
                    else if (webLogAction.ActionType == (int)SystemConstants.LogActionType.DeletingActivity)
                    {
                        actLog.Message += "Flag: Changing Status(" + activityStatus + "),  Activity is Deleted.  ";
                        actLog.Message += "Action Taken: Change status to DELETED" + "%NL%";

                        actLog.Message += "Today: " + DateTime.Now.ToShortDateString();
                        actLog.EmailSent = false;
                        actLog.LogActionID = LogActionID;
                        new DataAccessComponent().SaveActivityLog(actLog);
                    }
                }
                foreach (var actLogGroup in ActivitiesLogGroupCollection)
                {
                    new DataAccessComponent().UpdateActivityLogGroup(actLogGroup.ID, actLogGroup);
                }
            }
            else
            {
                var drLog = new DataSetComponent().WebLog.NewWebLogRow();
                drLog.Message = "Activities Status updated: No changes made %NL%";
                drLog.Message += "Date:" + DateTime.Now + "%NL%";
                drLog.LogCategory = (int)SystemConstants.logType.ActivityMaintenance;
                drLog.CreatedBy = User;
                drLog.CreatedDateTime = DateTime.Now;
                drLog.ReferenceNumber = SystemConstants.ActivityMaintenanceCode + BCUtility.ObjectHandler.GetRandomKey(9);
                drLog.Note = "";
                new DataAccessComponent().SaveLog(drLog, out WebLogId);
            }
        }

        public void RewardsPointMaintenance(string User, out string message)
        {
            DataAccessComponent dac = new DataAccessComponent();
            bool usingDefaultPoint = true;
            int pointAward = SystemConstants.DefaultRewardPoints;
            int WebLogId = 0;

            var setting = dac.RetrieveWebConfiguration();
            if (setting.PointsAwarded != 10)
            {
                usingDefaultPoint = false;
                pointAward = setting.PointsAwarded;
            }

            var WebLog = new DataSetComponent.WebLogDataTable().NewWebLogRow();
            WebLog.Message = SystemConstants.ExecuteRewardMaintenance + "%NL%" + "Datetime:" + DateTime.Now + "Requested by:" + User;
            WebLog.LogCategory = (int)SystemConstants.logType.RewardMaintenance;
            WebLog.CreatedDateTime = DateTime.Now;
            WebLog.CreatedBy = User;
            WebLog.ReferenceNumber = SystemConstants.RewardsMaintenanceCode + " " + BCUtility.ObjectHandler.GetRandomKey(9);
            WebLog.Note = "";

            var unProcAttendance = dac.RetrieveUnprocessedAttendanceRecords();
            new DataAccessComponent().ValidateUserRewards(unProcAttendance.Select(x => x.UserID.ToString()).ToList());
            if (unProcAttendance != null)
            {
                if (unProcAttendance.Count != 0)
                {
                    var awardList = new List<PointAwards>();
                    int totalpoint = 0;
                    foreach (var att in unProcAttendance)
                    {
                        PointAwards aw = awardList.Where(x => x.UserID == att.UserID).FirstOrDefault();
                        if (aw != null)
                        {
                            totalpoint += pointAward;
                            aw.points += pointAward;
                            aw.bonuspoints += 0;
                        }
                        else
                        {
                            aw = new PointAwards();
                            totalpoint += pointAward;
                            aw.points = pointAward;
                            aw.bonuspoints = 0;
                            aw.UserID = att.UserID;
                            awardList.Add(aw);
                        }
                        att.Processed = true;
                        att.ProcesssedDatetime = DateTime.Now;
                    }

                    WebLog.Message += ", " + totalpoint + " points are awarded to " + awardList.Count + " user(s).";
                    using (TransactionScope trans = new TransactionScope())
                    {
                        new DataAccessComponent().SaveLog(WebLog, out WebLogId);
                        new DataAccessComponent().UpdateAttendanceRecords(unProcAttendance);
                        ProcessUserAward(awardList, WebLogId, User);
                        trans.Complete();
                    }

                    message = WebLog.Message;
                    if (usingDefaultPoint)
                        message += "";
                }
                else
                {
                    message = SystemConstants.ErrorAttendanceRecordIsNull;
                }
            }
            else
            {
                message = SystemConstants.ErrorAttendanceRecordIsNull;
            }
        }





        private void ProcessUserAward(List<PointAwards> awardList, int WebLogId, string user)
        {
            //var userRewardLogCollection = new DataSetComponent.UserRewardLogDataTable();
            Dictionary<String, DataSetComponent.UserRewardLogRow> dicRewardLog = new Dictionary<String, DataSetComponent.UserRewardLogRow>();

            foreach (var award in awardList)
            {
                int logActionID = 0;
                var logAction = new DataSetComponent.WeblLogActionDataTable().NewWeblLogActionRow();
                logAction.ActionType = (int)SystemConstants.LogActionType.AwardingRewardPoints;
                logAction.WebLogID = WebLogId;
                logAction.Message = "Awarding " + award.points + " points to " + award.UserID;
                logAction.LogCategory = (int)SystemConstants.logType.RewardMaintenance;
                logAction.CreatedBy = user;
                logAction.CreatedDateTime = DateTime.Now;
                logAction.Value = award.points + "|" + award.bonuspoints + "|" + award.UserID;
                // [INT points] | [INT BONUSPOINTS] | [String USERID] 
                // ================================================


                new DataAccessComponent().SaveWebLogAction(logAction, out logActionID);

                var userRewardLog = new DataSetComponent.UserRewardLogDataTable().NewUserRewardLogRow();

                dicRewardLog.TryGetValue(award.UserID.ToString(), out userRewardLog);
                if (userRewardLog == null)
                {
                    userRewardLog = new DataSetComponent.UserRewardLogDataTable().NewUserRewardLogRow();
                    userRewardLog.UserRewardLogType = (int)SystemConstants.UserRewardlogType.Add;
                    userRewardLog.LogActionID = logActionID;
                    userRewardLog.PointValue = award.points;
                    userRewardLog.BonusPoint = award.bonuspoints;
                    userRewardLog.Message = "";
                    dicRewardLog[award.UserID.ToString()] = userRewardLog;
                }
                else
                {
                    userRewardLog.LogActionID = logActionID;
                    userRewardLog.PointValue = userRewardLog.PointValue + award.points;
                    userRewardLog.BonusPoint = userRewardLog.PointValue + award.bonuspoints;
                    dicRewardLog[award.UserID.ToString()] = userRewardLog;
                }
            }

            new DataAccessComponent().AddAwardPointsToUsers(dicRewardLog);
            new DataAccessComponent().SaveRewardLogs(dicRewardLog);


        }

        /// <summary>
        /// Only call this when there is changes in DB
        /// </summary>
        public void AccountAudit()
        {/*
            var Activities = new DataAccessComponent().RetrieveActivities();
            {
                foreach (var activity in Activities)
                {
                    activity.SecondaryCategoryID1 = activity.SecondaryCategoryID2 = 0;
                    activity.isApproved = activity.isCommenceAnytime = activity.isMembershipRequired = true;
                    new DataAccessComponent().UpdateActivity(activity);
                }
            }

            var Providers = new DataAccessComponent().RetrieveProviderProfiles();
            {
                foreach (var provider in Providers)
                {
                   
                        provider.CreatedBy = provider.ModifiedBy = "System Registration";
                        provider.CreatedDatetime = provider.ModifiedDatetime = DateTime.Now;
                        provider.SecondarySuburb = "";
                    new DataAccessComponent().UpdateProviderProfiles(provider);
                    
                }
            }

            var Users = new DataAccessComponent().RetrieveUserProfiles();
            {
                foreach (var user in Users)
                {
                    
                        user.CreatedBy = user.ModifiedBy = "System Registration";
                        user.CreatedDatetime = user.ModifiedDatetime = DateTime.Now;
                        new DataAccessComponent().UpdateUserProfiles(user);
                    
                }
            }*/
        }

        #endregion

        #region sending email

        private bool SendNotificationEmail(int activityID, int notificationNumber, string referenceNumber, String userID)
        {
            DataAccessComponent dac = new DataAccessComponent();
            var MailConf = dac.RetrieveWebConfiguration();
            var ProviderProfiles = dac.RetrieveProviderProfilesByID(userID);
            if (ProviderProfiles == null || string.IsNullOrEmpty(ProviderProfiles.Email))
            {
                return false;
            }
            else
            {
                DataSetComponent.v_EmailExplorerRow emTemp = new DataSetComponent.v_EmailExplorerDataTable().Newv_EmailExplorerRow();
                if (notificationNumber == 1)
                {

                    emTemp = dac.RetrieveMailTemplate((int)SystemConstants.EmailTemplateType.Expired2week);
                    ParseEmail(emTemp, userID, referenceNumber, (int)SystemConstants.EmailTemplateType.Expired2week, activityID);
                }
                else if (notificationNumber == 2)
                {
                    emTemp = dac.RetrieveMailTemplate((int)SystemConstants.EmailTemplateType.Expired1week);
                    ParseEmail(emTemp, userID, referenceNumber, (int)SystemConstants.EmailTemplateType.Expired1week, activityID);
                }
                else if (notificationNumber == 3)
                {
                    emTemp = dac.RetrieveMailTemplate((int)SystemConstants.EmailTemplateType.Expired);
                    ParseEmail(emTemp, userID, referenceNumber, (int)SystemConstants.EmailTemplateType.Expired, activityID);
                }
                EmailSender.SendEmail(MailConf.SMTPAccount, ProviderProfiles.Email, emTemp.EmailSubject, emTemp.EmailBody, MailConf.SMTPHost, MailConf.SMTPPort, MailConf.SMTPUserName, MailConf.SMTPPassword, MailConf.SMTPSSL, MailConf.SMTPIIS);
                return true;
            }
        }

        public void ParseEmail(DataSetComponent.v_EmailExplorerRow emTemp, String userID, string ConfirmationUrl, int EmailTemplateType, int activityID)
        {
            var dr = new DataAccessComponent().RetrieveUserProfiles(userID);

            if (dr == null)
            {
                var drprov = new DataAccessComponent().RetrieveProviderProfilesByID(userID);
                if (drprov != null)
                {
                    if (EmailTemplateType == (int)SystemConstants.EmailTemplateType.ProviderConfirmationEmail)
                    {
                        //Provider Fullname
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@fullname]", drprov.FirstName + " " + drprov.LastName);
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@loginurl]", SystemConstants.CustomerUrl + "Account/login.aspx");
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@username]", drprov.Username);
                        //Provider ConfirmationTokenuri
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@confirmationTokenwithurl]", SystemConstants.CustomerUrl + "Account/Confirm.aspx?" + SystemConstants.token + "=" + ConfirmationUrl);
                        //Provider ConfirmationToken
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@tokenurl]", ConfirmationUrl);
                        //Provider Confirmationurl
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@confirmationurl]", SystemConstants.CustomerUrl + "Account/Confirm.aspx");
                        //Provider ConfirmationToken
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@cancelregistration]", SystemConstants.CustomerUrl + "Account/CancelAccount.aspx?" + SystemConstants.userID + "=" + userID);
                    }
                    else if (EmailTemplateType == (int)SystemConstants.EmailTemplateType.ConfirmationEmail)
                    {
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@fullname]", drprov.FirstName + " " + drprov.LastName);
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@loginurl]", SystemConstants.CustomerUrl + "Account/login.aspx");
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@username]", drprov.Username);
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@confirmationTokenwithurl]", SystemConstants.CustomerUrl + "Account/Confirm.aspx?" + SystemConstants.token + "=" + ConfirmationUrl);
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@tokenurl]", ConfirmationUrl);
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@confirmationurl]", SystemConstants.CustomerUrl + "Account/Confirm.aspx");
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@cancelregistration]", SystemConstants.CustomerUrl + "Account/CancelAccount.aspx?" + SystemConstants.userID + "=" + userID);
                    }
                    else if (EmailTemplateType == (int)SystemConstants.EmailTemplateType.ForgotPassword)
                    {
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@fullname]", drprov.FirstName + " " + drprov.LastName);
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@username]", drprov.Username);
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@recoverylinkwithtoken]", SystemConstants.CustomerUrl + "Account/PasswordRecovery.aspx?" + SystemConstants.token + "=" + ConfirmationUrl);
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@cancelregistration]", SystemConstants.CustomerUrl + "Account/CancelAccount.aspx?" + SystemConstants.userID + "=" + userID);
                    }
                    else if (EmailTemplateType == (int)SystemConstants.EmailTemplateType.Expired2week)
                    {
                        var activityDR = new DataAccessComponent().RetrieveActivity(activityID);
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@fullname]", drprov.FirstName + " " + drprov.LastName);
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@activityname]", activityDR.Name);
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@activityid]", activityDR.ID.ToString());
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@expirydays]", (activityDR.ExpiryDate.DayOfYear - DateTime.Now.DayOfYear).ToString());
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@providerurl]", SystemConstants.ProviderUrl);
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@expireDate]", activityDR.ExpiryDate.ToShortDateString());
                    }
                    else if (EmailTemplateType == (int)SystemConstants.EmailTemplateType.Expired1week)
                    {
                        var activityDR = new DataAccessComponent().RetrieveActivity(activityID);
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@fullname]", drprov.FirstName + " " + drprov.LastName);
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@activityname]", activityDR.Name);
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@activityid]", activityDR.ID.ToString());
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@expirydays]", (activityDR.ExpiryDate.DayOfYear - DateTime.Now.DayOfYear).ToString());
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@providerurl]", SystemConstants.ProviderUrl);
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@expireDate]", activityDR.ExpiryDate.ToShortDateString());
                    }
                    else if (EmailTemplateType == (int)SystemConstants.EmailTemplateType.Expired)
                    {
                        var activityDR = new DataAccessComponent().RetrieveActivity(activityID);
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@fullname]", drprov.FirstName + " " + drprov.LastName);
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@activityname]", activityDR.Name);
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@activityid]", activityDR.ID.ToString());
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@expirydays]", (DateTime.Now.DayOfYear - activityDR.ExpiryDate.DayOfYear).ToString());
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@providerurl]", SystemConstants.ProviderUrl);
                        emTemp.EmailBody = emTemp.EmailBody.Replace("[@expireDate]", activityDR.ExpiryDate.ToShortDateString());
                    }
                }
            }
            else
            {
                if (EmailTemplateType == (int)SystemConstants.EmailTemplateType.ProviderConfirmationEmail)
                {
                    //Provider Fullname
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@fullname]", dr.FirstName + " " + dr.LastName);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@loginurl]", SystemConstants.CustomerUrl + "Account/login.aspx");
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@username]", dr.Username);
                    //Provider ConfirmationTokenuri
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@confirmationTokenwithurl]", SystemConstants.CustomerUrl + "Account/Confirm.aspx?" + SystemConstants.token + "=" + ConfirmationUrl);
                    //Provider ConfirmationToken
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@tokenurl]", ConfirmationUrl);
                    //Provider Confirmationurl
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@confirmationurl]", SystemConstants.CustomerUrl + "Account/Confirm.aspx");
                    //Provider ConfirmationToken
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@cancelregistration]", SystemConstants.CustomerUrl + "Account/CancelAccount.aspx?" + SystemConstants.userID + "=" + userID);
                }
                else if (EmailTemplateType == (int)SystemConstants.EmailTemplateType.ConfirmationEmail)
                {
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@fullname]", dr.FirstName + " " + dr.LastName);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@loginurl]", SystemConstants.CustomerUrl + "Account/login.aspx");
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@username]", dr.Username);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@confirmationTokenwithurl]", SystemConstants.CustomerUrl + "Account/Confirm.aspx?" + SystemConstants.token + "=" + ConfirmationUrl);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@tokenurl]", ConfirmationUrl);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@confirmationurl]", SystemConstants.CustomerUrl + "Account/Confirm.aspx");
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@cancelregistration]", SystemConstants.CustomerUrl + "Account/CancelAccount.aspx?" + SystemConstants.userID + "=" + userID);
                }
                else if (EmailTemplateType == (int)SystemConstants.EmailTemplateType.ForgotPassword)
                {
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@fullname]", dr.FirstName + " " + dr.LastName);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@username]", dr.Username);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@recoverylinkwithtoken]", SystemConstants.CustomerUrl + "Account/PasswordRecovery.aspx?" + SystemConstants.token + "=" + ConfirmationUrl);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@cancelregistration]", SystemConstants.CustomerUrl + "Account/CancelAccount.aspx?" + SystemConstants.userID + "=" + userID);
                }
                else if (EmailTemplateType == (int)SystemConstants.EmailTemplateType.Expired2week)
                {
                    var activityDR = new DataAccessComponent().RetrieveActivity(activityID);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@fullname]", dr.FirstName + " " + dr.LastName);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@activityname]", activityDR.Name);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@activityid]", activityDR.ID.ToString());
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@expirydays]", (activityDR.ExpiryDate.DayOfYear - DateTime.Now.DayOfYear).ToString());
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@providerurl]", SystemConstants.ProviderUrl);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@expireDate]", activityDR.ExpiryDate.ToShortDateString());
                }
                else if (EmailTemplateType == (int)SystemConstants.EmailTemplateType.Expired1week)
                {
                    var activityDR = new DataAccessComponent().RetrieveActivity(activityID);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@fullname]", dr.FirstName + " " + dr.LastName);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@activityname]", activityDR.Name);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@activityid]", activityDR.ID.ToString());
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@expirydays]", (activityDR.ExpiryDate.DayOfYear - DateTime.Now.DayOfYear).ToString());
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@providerurl]", SystemConstants.ProviderUrl);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@expireDate]", activityDR.ExpiryDate.ToShortDateString());
                }
                else if (EmailTemplateType == (int)SystemConstants.EmailTemplateType.Expired)
                {
                    var activityDR = new DataAccessComponent().RetrieveActivity(activityID);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@fullname]", dr.FirstName + " " + dr.LastName);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@activityname]", activityDR.Name);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@activityid]", activityDR.ID.ToString());
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@expirydays]", (DateTime.Now.DayOfYear - activityDR.ExpiryDate.DayOfYear).ToString());
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@providerurl]", SystemConstants.ProviderUrl);
                    emTemp.EmailBody = emTemp.EmailBody.Replace("[@expireDate]", activityDR.ExpiryDate.ToShortDateString());
                }
            }

        }

        #endregion

        #region Keyword

        public void SaveKeywords(DataSetComponent.KeyCollectionRow drKeyProperties, DataSetComponent.KeywordRow drKeywords, int mode)
        {
            DataAccessComponent dac = new DataAccessComponent();
            int KeyColID = 0;
            using (TransactionScope trans = new TransactionScope())
            {
                if (mode == (int)SystemConstants.FormMode.New)
                {
                    dac.CreateKeyCollection(drKeyProperties, out KeyColID);
                    dac.createKeywords(drKeywords, KeyColID);
                }
                else if (mode == (int)SystemConstants.FormMode.Edit)
                {
                    dac.UpdateKeyCollection(drKeyProperties);
                    dac.UpdateKeywords(drKeywords);
                }
                trans.Complete();
            }
        }


        #endregion

        #region MenuItem

        public void DeleteMenuItem(int MenuItemID)
        {
            DataAccessComponent dac = new DataAccessComponent();
            var childDTs = dac.RetrieveChildMenuItems(MenuItemID);

            int linkID;
            foreach (var childDR in childDTs)
            {
                dac.DeleteMenu(childDR.ID, out linkID);
                dac.DeleteLink(linkID);
            }

            dac.DeleteMenu(MenuItemID, out linkID);
            dac.DeleteLink(linkID);
        }

        public void SortMenuItem(int movedMenuItemID, bool isUp, out bool cannotChangePos)
        {
            DataAccessComponent dac = new DataAccessComponent();

            var movedDR = dac.RetrieveMenu(movedMenuItemID);
            var dt = dac.RetrieveMenus();

            IEnumerable<DataSetComponent.MenuRow> destQuery = null;

            DataSetComponent.MenuRow destDR = null;

            if (isUp)
            {
                destQuery = from dr in dt
                            where dr.ID == movedDR.ID &&
                                    dr.ParentMenuID == movedDR.ParentMenuID &&
                                    dr.Sequence < movedDR.Sequence
                            orderby dr.Sequence
                            select dr;

                destDR = destQuery.LastOrDefault();
            }
            else
            {
                destQuery = from dr in dt
                            where dr.ID == movedDR.ID &&
                                    dr.ParentMenuID == movedDR.ParentMenuID &&
                                    dr.Sequence > movedDR.Sequence
                            orderby dr.Sequence
                            select dr;

                destDR = destQuery.FirstOrDefault();
            }

            if (destDR == null)
            {
                cannotChangePos = true;
                return;
            }

            int temp = destDR.Sequence;

            destDR.Sequence = movedDR.Sequence;
            movedDR.Sequence = temp;

            using (TransactionScope trans = new TransactionScope())
            {
                dac.UpdateMenu(movedDR);
                dac.UpdateMenu(destDR);
                trans.Complete();
            }

            //SwapMenuItem(movedDR, destDR,isUp);
            cannotChangePos = false;
        }

        #endregion

        #region Rewards

        public void DeleteRewards(List<int> RewardID)
        {
            using (TransactionScope trans = new TransactionScope())
            {
                foreach (var item in RewardID)
                {
                    if (item != 0 && item != -1)
                        new DataAccessComponent().DeleteReward(item);
                }
                trans.Complete();

            }
        }

        public void DeleteSponsors(List<String> SponsorID)
        {
            using (TransactionScope trans = new TransactionScope())
            {
                foreach (String item in SponsorID)
                {
                    if (item != null)
                        new DataAccessComponent().DeleteSponsor(item);
                }
                trans.Complete();

            }
        }

        public void SaveRewards(DataSetComponent.RewardRow drReward, DataSetComponent.RewardsDetailsRow drRwrdDet, DataSetComponent.RewardImageRow drRwrdImage)
        {
            using (TransactionScope trans = new TransactionScope())
            {
                int RewardID = 0;
                new DataAccessComponent().SaveReward(drReward, out RewardID);
                if (drRwrdImage != null)
                {
                    drRwrdImage.RewardID = RewardID;
                    new DataAccessComponent().SaveRewardImage(drRwrdImage);
                }
                drRwrdDet.RewardID = RewardID;
                new DataAccessComponent().SaveRewardDetail(drRwrdDet);

                trans.Complete();
            }

        }

        public void UpdateRewards(DataSetComponent.RewardRow drReward, DataSetComponent.RewardsDetailsRow drRwrdDet, DataSetComponent.RewardImageRow drRwrdImage)
        {
            using (TransactionScope trans = new TransactionScope())
            {

                new DataAccessComponent().UpdateReward(drReward);
                if (drRwrdImage != null)
                {

                    new DataAccessComponent().UpdateRewardImage(drRwrdImage);
                }
                if (drRwrdDet != null)

                    new DataAccessComponent().UpdateRewardDetail(drRwrdDet);

                trans.Complete();
            }

        }


        #endregion

        #region Activity

        public static void UpdateActivity(int activityID, DataSetComponent.ActivityRow ActivityDetailDR, DataSetComponent.ActivityContactDetailRow contactDetailsDR, DataSetComponent.ActivityGroupingRow ActGroupingDR, DataSetComponent.ActivityScheduleDataTable ActScheduleDT)
        {
            DataAccessComponent dac = new DataAccessComponent();
            dac.DeleteActivitySchedules(activityID);
            using (TransactionScope trans = new TransactionScope())
            {
                ActivityDetailDR.ID = contactDetailsDR.ActivityID = ActGroupingDR.ActivityID = activityID;
                //Activity was update, Create other
                dac.UpdateActivity(ActivityDetailDR);


                //CreateContactDetails
                dac.UpdateActivityContactDetail(contactDetailsDR);

                //Delete old Schedule before insert new one

                if (ActScheduleDT != null)
                    //Create Schedule
                    foreach (var ActScheduleDR in ActScheduleDT)
                    {
                        ActScheduleDR.ActivityID = activityID;
                        dac.CreateActivitySchedule(ActScheduleDR);
                    }
                //Create Grouping
                dac.UpdateActivityGrouping(ActGroupingDR);


                trans.Complete();
            }
        }


        public DataSetComponent.ActivityScheduleGridDataTable CalculateRecurrence(DataSetComponent.ActivityScheduleDataTable dtSched)
        {
            var timeTableDT = new DataSetComponent.ActivityScheduleGridDataTable();
            foreach (var drSched in dtSched)
            {
                if (drSched.RecurrenceType == (int)SystemConstants.RecurrenceSchedule.NotRecurring)
                {
                    //InsertRecurrenceStartTime
                    DateTime recurStartDate = drSched.ActivityStartDatetime;
                    DateTime recurEndDate = drSched.ActivityEndDatetime;

                    DataSetComponent.ActivityScheduleGridRow dr = timeTableDT.NewActivityScheduleGridRow();
                    dr.StartDateTime = recurStartDate;
                    dr.EndDateTime = recurEndDate;
                    timeTableDT.AddActivityScheduleGridRow(dr);
                }
                else
                {
                    int startYear = drSched.ActivityStartDatetime.Year;
                    int yearDifference = drSched.ActivityExpiryDate.Year - drSched.ActivityStartDatetime.Year;

                    if (drSched.RecurrenceType == (int)SystemConstants.RecurrenceSchedule.Daily)
                    {
                        int dayDifference = (drSched.ActivityExpiryDate.DayOfYear + (yearDifference * 365)) - drSched.ActivityStartDatetime.DayOfYear;
                        int recurTimes = dayDifference / drSched.RecurEvery;

                        for (int i = 0; i <= recurTimes; i++)
                        {
                            //InsertRecurrenceStartTime
                            DateTime recurStartDate = drSched.ActivityStartDatetime.AddDays(drSched.RecurEvery * i);
                            DateTime recurEndDate = drSched.ActivityEndDatetime.AddDays(drSched.RecurEvery * i);

                            DataSetComponent.ActivityScheduleGridRow dr = timeTableDT.NewActivityScheduleGridRow();
                            dr.StartDateTime = recurStartDate;
                            dr.EndDateTime = recurEndDate;
                            timeTableDT.AddActivityScheduleGridRow(dr);
                        }
                    }
                    else if (drSched.RecurrenceType == (int)SystemConstants.RecurrenceSchedule.Weekly)
                    {
                        decimal weekDifference = ((drSched.ActivityExpiryDate.DayOfYear + (yearDifference * 365)) / 7) - (drSched.ActivityStartDatetime.DayOfYear / 7);
                        decimal recurTimes = weekDifference / drSched.RecurEvery;

                        for (decimal i = 1; i <= recurTimes + 1; i++)
                        {
                            for (int weekday = 1; weekday <= 7; weekday++)
                            {
                                if ((drSched.ActivityStartDatetime.DayOfYear + ((drSched.ActivityStartDatetime.Year - startYear) * 365)) > (drSched.ActivityExpiryDate.DayOfYear) + (yearDifference * 365))
                                    break;

                                if (drSched.ActivityStartDatetime.DayOfWeek == DayOfWeek.Monday && drSched.OnMonday ||
                                    drSched.ActivityStartDatetime.DayOfWeek == DayOfWeek.Tuesday && drSched.OnTuesday ||
                                    drSched.ActivityStartDatetime.DayOfWeek == DayOfWeek.Wednesday && drSched.OnWednesday ||
                                    drSched.ActivityStartDatetime.DayOfWeek == DayOfWeek.Thursday && drSched.OnThursday ||
                                    drSched.ActivityStartDatetime.DayOfWeek == DayOfWeek.Friday && drSched.OnFriday ||
                                    drSched.ActivityStartDatetime.DayOfWeek == DayOfWeek.Saturday && drSched.OnSaturday ||
                                    drSched.ActivityStartDatetime.DayOfWeek == DayOfWeek.Sunday && drSched.OnSunday)
                                {
                                    DataSetComponent.ActivityScheduleGridRow dr = timeTableDT.NewActivityScheduleGridRow();
                                    dr.StartDateTime = drSched.ActivityStartDatetime;
                                    dr.EndDateTime = drSched.ActivityEndDatetime;
                                    timeTableDT.AddActivityScheduleGridRow(dr);
                                }


                                drSched.ActivityStartDatetime = drSched.ActivityStartDatetime.AddDays(1);
                                drSched.ActivityEndDatetime = drSched.ActivityEndDatetime.AddDays(1);
                                //recurStartDate = startDateTime.AddDays(1 * i + weekday);
                            }
                            drSched.ActivityStartDatetime = drSched.ActivityStartDatetime.AddDays((7 * Convert.ToInt32(drSched.RecurEvery) - 7));
                            drSched.ActivityEndDatetime = drSched.ActivityEndDatetime.AddDays((7 * Convert.ToInt32(drSched.RecurEvery) - 7));
                        }
                    }
                }

            }
            timeTableDT.DefaultView.Sort = "StartDateTime ASC";
            return timeTableDT;
        }

        public void ConfirmActivities(List<int> activitiesID)
        {
            using (TransactionScope trans = new TransactionScope())
            {
                foreach (var item in activitiesID)
                {
                    if (item != 0 && item != -1)
                        new DataAccessComponent().ConfirmActivity(item);
                }
                trans.Complete();

            }
        }

        public void DeleteActivities(List<int> activitiesID)
        {
            using (TransactionScope trans = new TransactionScope())
            {
                foreach (var item in activitiesID)
                {
                    if (item != 0 && item != -1)
                        new DataAccessComponent().DeleteActivity(item);
                }
                trans.Complete();

            }
        }


        public void ExtendActivitiesExpiryDate(int daysExtended, out int actCount)
        {

            var ActivityDT = new DataAccessComponent().RetrieveExpiredActivities();
            List<int> selectedDT = new DataAccessComponent().RetrieveExpiredActivityIDs();
            var ActivityScheduleDT = new DataAccessComponent().RetrieveActivitySchedulesbyIDs(selectedDT);

            if (ActivityDT != null)
            {
                foreach (var actDr in ActivityDT)
                {
                    actDr.ExpiryDate = actDr.ExpiryDate.AddDays(daysExtended);
                    new DataAccessComponent().UpdateActivity(actDr);
                }
            }
            if (ActivityScheduleDT != null)
            {
                foreach (var schedDr in ActivityScheduleDT)
                {
                    schedDr.ActivityExpiryDate = schedDr.ActivityExpiryDate.AddDays(daysExtended);
                    new DataAccessComponent().UpdateActivitySchedule(schedDr);
                }
            }
            actCount = selectedDT.Count;
        }

        public void ExtendActivitiesExpiryDate(List<int> selectedDT, int daysExtended)
        {
            var ActivityDT = new DataAccessComponent().RetrieveActivitiesbyIDs(selectedDT);
            var ActivityScheduleDT = new DataAccessComponent().RetrieveActivitySchedulesbyIDs(selectedDT);

            if (ActivityDT != null)
            {
                foreach (var actDr in ActivityDT)
                {
                    actDr.ExpiryDate = actDr.ExpiryDate.AddDays(daysExtended);
                    new DataAccessComponent().UpdateActivity(actDr);
                }
            }
            if (ActivityScheduleDT != null)
            {
                foreach (var schedDr in ActivityScheduleDT)
                {
                    schedDr.ActivityExpiryDate = schedDr.ActivityExpiryDate.AddDays(daysExtended);
                    new DataAccessComponent().UpdateActivitySchedule(schedDr);
                }
            }
        }


        #endregion

        #region Page/Asset

        public void DuplicatePage(int OldDynamicPageID, out int newDynamicPageID)
        {
            DataAccessComponent dac = new DataAccessComponent();
            var drOldPage = dac.RetrievePage(OldDynamicPageID);

            drOldPage.ID = 0;
            drOldPage.Name = drOldPage.Name + "_Copy";
            dac.CreatePage(drOldPage);
            newDynamicPageID = drOldPage.ID;

        }

        public void CreateAssetsInformation(DataSetComponent.WebAssetsDataTable dt)
        {
            foreach (var dr in dt)
            {
                new DataAccessComponent().CreateAssetInformation(dr);
            }
        }


        #endregion

        public string GenerateUserRefID(string firstName, string LastName)
        {
            var RefTable = new DataAccessComponent().RetrieveUserReferences();

            string Name = "";
            if (!string.IsNullOrEmpty(LastName))
                Name = LastName.ToUpper();
            else
                Name = firstName.ToUpper();

            StringBuilder sb = new StringBuilder();

            if (Name.Length >= 3)
                sb.Append(Name.Substring(0, 3));
            else
                sb.Append(ObjectHandler.GetRandomKey(3));
            List<string> nameRef = new List<string>(RefTable.Where(x => x.ReferenceID.Substring(0, 3).Equals(sb.ToString())).Select(y => y.ReferenceID.Substring(3, 4)));

            string nextCode = nameRef.Count() == 0 ? sb.ToString() + "0001" : sb.ToString() + (int.Parse(nameRef.OrderByDescending(i => i).First()) + 1).ToString("0000");
            return nextCode;
        }

        public string GenerateUserRefID(string Name)
        {
            var RefTable = new DataAccessComponent().RetrieveUserReferences();

            Name = Name.ToUpper();
            StringBuilder sb = new StringBuilder();

            if (Name.Length >= 3)
                sb.Append(Name.Substring(0, 3));
            else
                sb.Append(ObjectHandler.GetRandomKey(3));
            List<string> nameRef = new List<string>(RefTable.Where(x => x.ReferenceID.Substring(0, 3).Equals(sb.ToString())).Select(y => y.ReferenceID.Substring(3, 4)));

            string nextCode = nameRef.Count() == 0 ? sb.ToString() + "0001" : sb.ToString() + (int.Parse(nameRef.OrderByDescending(i => i).First()) + 1).ToString("0000");
            return nextCode;
        }

        public string GenerateActRefID(string Name)
        {
            var actRef = new DataAccessComponent().RetrieveActivityReferences();
            var RefActIDTable = new HashSet<int>(actRef.Select(x => x.ActivityID));
            var RefCodeTable = new HashSet<string>(actRef.Select(x => x.ReferenceID));


            List<string> exclude = (SystemConstants.Prepositions + SystemConstants.Conjunctions).Split(';').Select(x => x.Trim()).OfType<string>().ToList();
            StringBuilder sb = new StringBuilder();

            List<string> ActName = Name.ToUpper().Split(' ').OfType<string>().ToList();
            HashSet<string> preposHash = new HashSet<string>(exclude.Select(x => x.ToUpper()));
            ActName.RemoveAll(x => preposHash.Contains(x.ToUpper()));


            for (int i = 0; i <= (ActName.Count - 1); i++)
            {
                for (int j = 0; j <= ActName[i].Length; j++)
                {
                    if (Regex.IsMatch(ActName[i], @"^[A-Z]+$"))
                    {
                        sb.Append(ActName[i].Substring(j, 1));

                        if (i == (ActName.Count - 1) && sb.Length != 3)
                        {
                            continue;
                        }
                        else
                            break;
                    }
                }
                if (sb.Length == 3)
                    break;
            }
            if (sb.Length != 3)
            {
                sb.Append(ObjectHandler.GetRandomKey(3 - sb.Length));
            }

            List<string> nameRef = new List<string>(RefCodeTable.Where(x => x.Substring(0, 3).Equals(sb.ToString())).Select(y => y.Substring(3, 4)));
            string nextCode = nameRef.Count() == 0 ? sb.ToString() + "0001" : sb.ToString() + (int.Parse(nameRef.OrderByDescending(i => i).First()) + 1).ToString("0000");
            return nextCode;
        }

        public void CreateActivitiesReference()
        {
            DataSetComponent.ActivityReferenceCodeDataTable actRefDT = new DataSetComponent.ActivityReferenceCodeDataTable();
            var ActRef = new DataAccessComponent().RetrieveActivityReferences();
            var RefActIDTable = new HashSet<int>(ActRef.Select(x => x.ActivityID));
            var RefCodeTable = new HashSet<string>(ActRef.Select(x => x.ReferenceID));

            foreach (var dr in new DataAccessComponent().RetrieveActivities())
            {
                if (!RefActIDTable.Contains(dr.ID))
                {
                    var actRefDR = actRefDT.NewActivityReferenceCodeRow();
                    actRefDR.ActivityGUID = Guid.NewGuid();
                    actRefDR.ActivityID = dr.ID;

                    List<string> exclude = (SystemConstants.Prepositions + SystemConstants.Conjunctions).Split(';').Select(x => x.Trim()).OfType<string>().ToList();
                    StringBuilder sb = new StringBuilder();

                    List<string> ActName = dr.Name.ToUpper().Split(' ').OfType<string>().ToList();
                    HashSet<string> preposHash = new HashSet<string>(exclude.Select(x => x.ToUpper()));
                    ActName.RemoveAll(x => preposHash.Contains(x.ToUpper()));


                    for (int i = 0; i <= (ActName.Count - 1); i++)
                    {
                        for (int j = 0; j <= ActName[i].Length; j++)
                        {
                            if (Regex.IsMatch(ActName[i], @"^[A-Z]+$"))
                            {
                                sb.Append(ActName[i].Substring(j, 1));

                                if (i == (ActName.Count - 1) && sb.Length != 3)
                                {
                                    continue;
                                }
                                else
                                    break;
                            }
                        }
                        if (sb.Length == 3)
                            break;
                    }
                    if (sb.Length != 3)
                    {
                        sb.Append(ObjectHandler.GetRandomKey(3 - sb.Length));
                    }

                    List<string> nameRef = new List<string>(RefCodeTable.Where(x => x.Substring(0, 3).Equals(sb.ToString())).Select(y => y.Substring(3, 4)));
                    string nextCode = nameRef.Count() == 0 ? sb.ToString() + "0001" : sb.ToString() + (int.Parse(nameRef.OrderByDescending(i => i).First()) + 1).ToString("0000");
                    actRefDR.ReferenceID = nextCode;

                    RefActIDTable.Add(actRefDR.ActivityID);
                    RefCodeTable.Add(actRefDR.ReferenceID);

                    actRefDT.AddActivityReferenceCodeRow(actRefDR);
                }
            }

            new DataAccessComponent().insertNewActivitiesReference(actRefDT);

        }

        public void CreateSpecifiedActivitiesReference(List<int> Actlist)
        {
            DataSetComponent.ActivityReferenceCodeDataTable actRefDT = new DataSetComponent.ActivityReferenceCodeDataTable();
            var ActRef = new DataAccessComponent().RetrieveActivityReferences();
            var RefActIDTable = new HashSet<int>(ActRef.Select(x => x.ActivityID));
            var RefCodeTable = new HashSet<string>(ActRef.Select(x => x.ReferenceID));

            foreach (var dr in new DataAccessComponent().RetrieveActivitiesbyIDs(Actlist))
            {
                if (!RefActIDTable.Contains(dr.ID))
                {
                    var actRefDR = actRefDT.NewActivityReferenceCodeRow();
                    actRefDR.ActivityGUID = Guid.NewGuid();
                    actRefDR.ActivityID = dr.ID;

                    List<string> exclude = (SystemConstants.Prepositions + SystemConstants.Conjunctions).Split(';').Select(x => x.Trim()).OfType<string>().ToList();
                    StringBuilder sb = new StringBuilder();

                    List<string> ActName = dr.Name.ToUpper().Split(' ').OfType<string>().ToList();
                    HashSet<string> preposHash = new HashSet<string>(exclude.Select(x => x.ToUpper()));
                    ActName.RemoveAll(x => preposHash.Contains(x.ToUpper()));


                    for (int i = 0; i <= (ActName.Count - 1); i++)
                    {
                        for (int j = 0; j <= ActName[i].Length; j++)
                        {
                            if (Regex.IsMatch(ActName[i], @"^[A-Z]+$"))
                            {
                                sb.Append(ActName[i].Substring(j, 1));

                                if (i == (ActName.Count - 1) && sb.Length != 3)
                                {
                                    continue;
                                }
                                else
                                    break;
                            }
                        }
                        if (sb.Length == 3)
                            break;
                    }
                    if (sb.Length != 3)
                    {
                        sb.Append(ObjectHandler.GetRandomKey(3 - sb.Length));
                    }

                    List<string> nameRef = new List<string>(RefCodeTable.Where(x => x.Substring(0, 3).Equals(sb.ToString())).Select(y => y.Substring(3, 4)));
                    string nextCode = nameRef.Count() == 0 ? sb.ToString() + "0001" : sb.ToString() + (int.Parse(nameRef.OrderByDescending(i => i).First()) + 1).ToString("0000");
                    actRefDR.ReferenceID = nextCode;

                    RefActIDTable.Add(actRefDR.ActivityID);
                    RefCodeTable.Add(actRefDR.ReferenceID);

                    actRefDT.AddActivityReferenceCodeRow(actRefDR);
                }
            }

            new DataAccessComponent().insertNewActivitiesReference(actRefDT);
        }

        public void SaveAttendanceRecords(DataSetComponent.ActivityUserAttendanceDataTable dt)
        {
            using (TransactionScope trans = new TransactionScope())
            {
                new DataAccessComponent().SaveAttendanceRecords(dt);
                trans.Complete();
            }
        }

        public static void UpdateActivity(DataSetComponent.ActivityRow drDetail, DataSetComponent.ActivityContactDetailRow contactDetails, DataSetComponent.ActivityGroupingRow drActGrouping, DataSetComponent.ActivityScheduleDataTable dtActSchedule)
        {
            DataAccessComponent dac = new DataAccessComponent();

            using (TransactionScope trans = new TransactionScope())
            {
                dac.UpdateActivity(drDetail);
                dac.UpdateActivityContactDetail(contactDetails);
                dac.DeleteActivitySchedules(drDetail.ID);
                foreach (var drActSchedule in dtActSchedule)
                    dac.CreateActivitySchedule(drActSchedule);
                dac.UpdateActivityGrouping(drActGrouping);

                trans.Complete();
            }
        }

        public static void SaveActivity(DataSetComponent.ActivityRow ActivityDetailDR, DataSetComponent.ActivityContactDetailRow contactDetailsDR, DataSetComponent.ActivityGroupingRow ActGroupingDR, DataSetComponent.ActivityScheduleDataTable ActScheduleDT)
        {
            DataAccessComponent dac = new DataAccessComponent();

            using (TransactionScope trans = new TransactionScope())
            {
                int activityID;
                dac.CreateActivities(ActivityDetailDR, out activityID);

                //Activity was Created, update all foreign key
                contactDetailsDR.ActivityID = activityID;
                ActGroupingDR.ActivityID = activityID;



                //CreateContactDetails
                dac.CreateActivityContactDetail(contactDetailsDR);

                //Create Schedule
                foreach (var ActScheduleDR in ActScheduleDT)
                {
                    ActScheduleDR.ActivityID = activityID;
                    dac.CreateActivitySchedule(ActScheduleDR);
                }
                //Create Grouping
                dac.CreateActivityGrouping(ActGroupingDR);

                trans.Complete();
            }
        }

        public string RefineSearchKeyreward(string SearchKey)
        {
            if (isImplementAdvanceSearch())
            {
                String SearchPhrase = "";
                var keyCollection = new DataAccessComponent().SearchKeywordCollection(SearchKey);
                if (keyCollection.Count == 0 || keyCollection == null)
                    return SearchKey;
                else
                {
                    foreach (var key in keyCollection)
                    {
                        SearchPhrase = SearchPhrase + key.Keywords + ";";
                    }
                }
                return SearchPhrase;
            }
            else
                return SearchKey;


        }

        public List<string> RefineSearchKey(string InputQueries)
        {
            List<string> SearchParameters = new List<string>();
            String SearchQuery = "";
            String DaysQuery = "";
            String LocationsQuery = "";
            String TimeQuery = "";
            List<TimeSpan> Timelist = new List<TimeSpan>();

            List<string> keywords = InputQueries.ToUpper().Split(' ').OfType<string>().ToList();

            if (isImplementAdvanceSearch())
            {
                /*Delete Prepositions/Postposition from keyword
             * See SystemConstants.Prepositions for list of prepositions             
             */
                List<string> exclude = (SystemConstants.Prepositions + SystemConstants.Conjunctions).Split(';').Select(x => x.Trim()).OfType<string>().ToList();

                HashSet<string> preposHash = new HashSet<string>(exclude.Select(x => x.ToUpper()));
                keywords.RemoveAll(x => preposHash.Contains(x.ToUpper()));

                /*Match and save locations from keyword                
                 First we retrieve available locations from db
                 */
                var suburbs = new DataAccessComponent().RetrieveSuburbs().ToList();

                List<SuburbExplorer> SubsCont = new List<SuburbExplorer>();
                foreach (var suburb in suburbs)
                {
                    string[] subs = suburb.Name.Split(' ');
                    foreach (var sub in subs)
                    {
                        SubsCont.Add(new SuburbExplorer()
                        {
                            ID = suburb.ID,
                            Name = sub
                        });
                    }
                }

                var matchedLocs = SubsCont.Where(x => keywords.Contains(x.Name.ToUpper()));
                HashSet<int> locsID = new HashSet<int>(matchedLocs.Select(x => x.ID));
                IEnumerable<DataSetComponent.v_SuburbExplorerRow> matchedSuburbs = suburbs.Where(x => locsID.Contains(x.ID));

                StringBuilder lq = new StringBuilder();
                foreach (var matchedSuburb in matchedSuburbs)
                    lq.Append(matchedSuburb.Name + ";");
                LocationsQuery = lq.ToString();

                HashSet<string> locationsHash = new HashSet<string>(SubsCont.Select(x => x.Name.ToUpper()));
                keywords.RemoveAll(x => locationsHash.Contains(x.ToUpper()));

                /*Match and save Days from keyword                
                 */
                List<string> dayofweek = new List<string> { DayOfWeek.Monday.ToString(), DayOfWeek.Tuesday.ToString(),
                DayOfWeek.Wednesday.ToString(),DayOfWeek.Thursday.ToString(),DayOfWeek.Friday.ToString(),
                DayOfWeek.Saturday.ToString(),DayOfWeek.Sunday.ToString(),};

                HashSet<string> dayofweekHash = new HashSet<string>(dayofweek.Select(x => x.ToUpper()));
                var days = keywords.Where(x => dayofweekHash.Contains(x.ToUpper()));
                if (days.Count() != 0)
                    DaysQuery = String.Join(";", days);

                keywords.RemoveAll(x => dayofweekHash.Contains(x.ToUpper()));

                foreach (var key in keywords)
                {
                    DateTime dateTime = new DateTime();
                    //12hourformat h:mm tt 	6:30 AM 
                    if (DateTime.TryParseExact(key, @"h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                    {
                        Timelist.Add(dateTime.TimeOfDay);
                    }
                    else if (DateTime.TryParseExact(key, @"h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                    {
                        Timelist.Add(dateTime.TimeOfDay);
                    }
                    if (DateTime.TryParseExact(key, @"hmm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                    {
                        Timelist.Add(dateTime.TimeOfDay);
                    }
                    else if (DateTime.TryParseExact(key, @"hmm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                    {
                        Timelist.Add(dateTime.TimeOfDay);
                    }
                    if (DateTime.TryParseExact(key, @"h:mmtt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                    {
                        Timelist.Add(dateTime.TimeOfDay);
                    }
                    else if (DateTime.TryParseExact(key, @"h:mmtt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                    {
                        Timelist.Add(dateTime.TimeOfDay);
                    }
                    if (DateTime.TryParseExact(key, @"hmmtt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                    {
                        Timelist.Add(dateTime.TimeOfDay);
                    }
                    else if (DateTime.TryParseExact(key, @"hmmtt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                    {
                        Timelist.Add(dateTime.TimeOfDay);
                    }
                    if (DateTime.TryParseExact(key, @"h tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                    {
                        Timelist.Add(dateTime.TimeOfDay);
                    }
                    if (DateTime.TryParseExact(key, @"htt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                    {
                        Timelist.Add(dateTime.TimeOfDay);
                    }
                    //24hourformat HH:mm 14:30
                    else if (DateTime.TryParseExact(key, @"HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                    {
                        Timelist.Add(dateTime.TimeOfDay);
                    }
                    else if (DateTime.TryParseExact(key, @"H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                    {
                        Timelist.Add(dateTime.TimeOfDay);
                    }
                    else if (DateTime.TryParseExact(key, @"HHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                    {
                        Timelist.Add(dateTime.TimeOfDay);
                    }
                    else if (DateTime.TryParseExact(key, @"Hmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                    {
                        Timelist.Add(dateTime.TimeOfDay);
                    }
                    else
                    {
                        var keyCollections = new DataAccessComponent().SearchKeywordCollection(key);
                        if (keyCollections.Count == 0 || keyCollections == null)
                        {
                            foreach (var keycol in keyCollections)
                            {
                                SearchQuery = SearchQuery + keycol.Keywords + ";";
                            }
                        }
                        if (!string.IsNullOrEmpty(SearchQuery))
                        {
                            SearchQuery = SearchQuery + ";" + key;
                        }
                        else
                        {
                            SearchQuery = key;
                        }
                    }
                }
            }
            else
            {
                SearchQuery = InputQueries.Replace(' ', ';');
            }
            if (Timelist.Count != 0)
            {
                Timelist = Timelist.OrderBy(row => row.Hours).ToList();
                if (Timelist.Count >= 2)
                {
                    TimeQuery = Timelist[0].ToString() + "-" + Timelist[Timelist.Count - 1].ToString();
                }
                else if (Timelist.Count == 1)
                {
                    TimeQuery = Timelist[0].ToString();
                }
            }
            SearchQuery = SearchQuery.Replace(" ", string.Empty);
            SearchQuery = SearchQuery.Replace(";;", ";");
            if (SearchQuery.EndsWith(";"))
                SearchQuery = SearchQuery.TrimEnd(';');
            SearchQuery = SearchQuery.Trim();

            SearchParameters.Add(SystemConstants.Query + SearchQuery);
            if (!string.IsNullOrEmpty(LocationsQuery))
                SearchParameters.Add(SystemConstants.Location + LocationsQuery);
            if (!string.IsNullOrEmpty(DaysQuery))
                SearchParameters.Add(SystemConstants.Day + DaysQuery);
            if (!string.IsNullOrEmpty(TimeQuery))
                SearchParameters.Add(SystemConstants.Time + TimeQuery);
            return SearchParameters;
        }

        private bool isweekDay(string key)
        {
            if (key.Equals(DayOfWeek.Monday.ToString()) || key.Equals(DayOfWeek.Tuesday.ToString()) ||
                key.Equals(DayOfWeek.Wednesday.ToString()) || key.Equals(DayOfWeek.Thursday.ToString()) ||
                key.Equals(DayOfWeek.Friday.ToString()) || key.Equals(DayOfWeek.Saturday.ToString()) ||
                key.Equals(DayOfWeek.Sunday.ToString()))
                return true;
            else return false;

        }

        public bool isImplementAdvanceSearch()
        {
            return new DataAccessComponent().CheckAdvanceSearch();
        }

        public bool CheckActivityOwner(int ActivityID, String providerID)
        {
            var dr = new DataAccessComponent().RetrieveActivity(ActivityID);
            if (dr != null)
            {
                if (dr.ProviderID.Equals(providerID))
                    return true;
                else return false;
            }
            else return false;
        }



        public DataSetComponent.ActivityScheduleGridDataTable RetrieveTimetableGrid(int startIndex, int amount, string activityID, string sortExpression)
        {
            var slots = new DataAccessComponent().RetrieveActivitySchedules(Convert.ToInt32(activityID));
            var timeTableDT = CalculateRecurrence(slots);
            timeTableDT.DefaultView.Sort = "StartDateTime ASC";

            if (timeTableDT.Count() != 0)
            {
                var dtTimetable = new DataSetComponent.ActivityScheduleGridDataTable();
                timeTableDT.Skip(startIndex).Take(amount).CopyToDataTable(dtTimetable, LoadOption.PreserveChanges);
                dtTimetable.DefaultView.Sort = "StartDateTime ASC";
                return dtTimetable;
            }
            else
                return null;
        }

        public int RetrieveTimetableGridCount(string activityID)
        {
            var dtSchedule = new DataAccessComponent().RetrieveActivitySchedules(Convert.ToInt32(activityID));
            var timeTableDT = CalculateRecurrence(dtSchedule);


            return timeTableDT.Count();
        }

        public DataSetComponent.ActivityScheduleGridDataTable CalculateRecurrence(DataSetComponent.ActivityScheduleRow drSched)
        {
            var timeTableDT = new DataSetComponent.ActivityScheduleGridDataTable();

            int startYear = drSched.ActivityStartDatetime.Year;
            int yearDifference = drSched.ActivityExpiryDate.Year - drSched.ActivityStartDatetime.Year;

            if (drSched.RecurrenceType == (int)SystemConstants.RecurrenceSchedule.Daily)
            {
                int dayDifference = (drSched.ActivityExpiryDate.DayOfYear + (yearDifference * 365)) - drSched.ActivityStartDatetime.DayOfYear;
                int recurTimes = dayDifference / drSched.RecurEvery;

                for (int i = 0; i <= recurTimes; i++)
                {
                    //InsertRecurrenceStartTime
                    DateTime recurStartDate = drSched.ActivityStartDatetime.AddDays(drSched.RecurEvery * i);
                    DateTime recurEndDate = drSched.ActivityEndDatetime.AddDays(drSched.RecurEvery * i);

                    DataSetComponent.ActivityScheduleGridRow dr = timeTableDT.NewActivityScheduleGridRow();
                    dr.StartDateTime = recurStartDate;
                    dr.EndDateTime = recurEndDate;
                    timeTableDT.AddActivityScheduleGridRow(dr);
                }
            }
            else if (drSched.RecurrenceType == (int)SystemConstants.RecurrenceSchedule.Weekly)
            {
                decimal weekDifference = ((drSched.ActivityExpiryDate.DayOfYear + (yearDifference * 365)) / 7) - (drSched.ActivityStartDatetime.DayOfYear / 7);
                decimal recurTimes = weekDifference / drSched.RecurEvery;

                for (decimal i = 1; i <= recurTimes + 1; i++)
                {
                    for (int weekday = 1; weekday <= 7; weekday++)
                    {
                        if ((drSched.ActivityStartDatetime.DayOfYear + ((drSched.ActivityStartDatetime.Year - startYear) * 365)) > (drSched.ActivityExpiryDate.DayOfYear) + (yearDifference * 365))
                            break;

                        if (drSched.ActivityStartDatetime.DayOfWeek == DayOfWeek.Monday && drSched.OnMonday ||
                            drSched.ActivityStartDatetime.DayOfWeek == DayOfWeek.Tuesday && drSched.OnTuesday ||
                            drSched.ActivityStartDatetime.DayOfWeek == DayOfWeek.Wednesday && drSched.OnWednesday ||
                            drSched.ActivityStartDatetime.DayOfWeek == DayOfWeek.Thursday && drSched.OnThursday ||
                            drSched.ActivityStartDatetime.DayOfWeek == DayOfWeek.Friday && drSched.OnFriday ||
                            drSched.ActivityStartDatetime.DayOfWeek == DayOfWeek.Saturday && drSched.OnSaturday ||
                            drSched.ActivityStartDatetime.DayOfWeek == DayOfWeek.Sunday && drSched.OnSunday)
                        {
                            DataSetComponent.ActivityScheduleGridRow dr = timeTableDT.NewActivityScheduleGridRow();
                            dr.StartDateTime = drSched.ActivityStartDatetime;
                            dr.EndDateTime = drSched.ActivityEndDatetime;
                            timeTableDT.AddActivityScheduleGridRow(dr);
                        }


                        drSched.ActivityStartDatetime = drSched.ActivityStartDatetime.AddDays(1);
                        drSched.ActivityEndDatetime = drSched.ActivityEndDatetime.AddDays(1);
                        //recurStartDate = startDateTime.AddDays(1 * i + weekday);
                    }
                    drSched.ActivityStartDatetime = drSched.ActivityStartDatetime.AddDays((7 * Convert.ToInt32(drSched.RecurEvery) - 7));
                    drSched.ActivityEndDatetime = drSched.ActivityEndDatetime.AddDays((7 * Convert.ToInt32(drSched.RecurEvery) - 7));
                }
            }
            return timeTableDT;
        }

        public int getProviderPrimaryImage(String providerID)
        {
            DataAccessComponent dac = new DataAccessComponent();

            var dt = dac.RetrieveUserImages(providerID);

            foreach (var dr in dt)
            {
                if (dr.isPrimaryImage)
                {
                    return dr.ID;
                }
            }
            return 0;
        }

        public void createRefforAllUser()
        {
            var userProf = new DataAccessComponent().RetrieveUserProfiles();
            var RefTable = new DataAccessComponent().RetrieveUserReferences().Select(x => x.UserId.ToString());

            var usrs = userProf.Where(x => !RefTable.Contains(x.UserID));
            foreach (var usr in usrs)
            {
                var drRef = new DataSetComponent.UserReferenceDataTable().NewUserReferenceRow();
                drRef.UserId = usr.UserID;
                drRef.ReferenceID = GenerateUserRefID(usr.LastName, usr.FirstName);
                new DataAccessComponent().insertNewUserReference(drRef);
            }
        }

        public void SaveActivity(DataSetComponent.ActivityRow ActivityDetailDR, DataSetComponent.ActivityContactDetailRow contactDetailsDR, DataSetComponent.ActivityGroupingRow ActGroupingDR, DataSetComponent.ActivityScheduleDataTable ActScheduleDT, DataSetComponent.ActivityImageRow ImageDetail, DataSetComponent.ActivityImageDetailDataTable Images, out int activityID)
        {
            DataAccessComponent dac = new DataAccessComponent();

            using (TransactionScope trans = new TransactionScope())
            {
                activityID = 0;
                dac.CreateActivities(ActivityDetailDR, out activityID);

                //Activity was Created, Creat other
                contactDetailsDR.ActivityID = activityID;
                ActGroupingDR.ActivityID = activityID;

                //CreateContactDetails
                dac.CreateActivityContactDetail(contactDetailsDR);

                if (ActScheduleDT != null)
                    //Create Schedule
                    foreach (var ActScheduleDR in ActScheduleDT)
                    {
                        ActScheduleDR.ActivityID = activityID;
                        dac.CreateActivitySchedule(ActScheduleDR);
                    }
                //Create Grouping
                dac.CreateActivityGrouping(ActGroupingDR);

                //Create Images
                if (ImageDetail.ImageAmount != 0)
                {
                    ImageDetail.ActivityID = activityID;
                    int imgDetID = 0;
                    dac.createActivityImageInformation(ImageDetail, out imgDetID);
                    int count = 1;
                    foreach (var drImageDetail in Images)
                    {
                        if (count == 1)
                            drImageDetail.isPrimaryImage = true;
                        drImageDetail.ActivityID = activityID;
                        drImageDetail.ActivityImageID = imgDetID;
                        dac.CreateActivityImage(drImageDetail);
                        count++;
                    }
                }

                //Create Reference
                var drRef = new DataSetComponent.ActivityReferenceCodeDataTable().NewActivityReferenceCodeRow();
                drRef.ActivityID = activityID;
                drRef.ActivityGUID = Guid.NewGuid();
                drRef.ReferenceID = GenerateActRefID(ActivityDetailDR.Name);
                dac.insertNewActivityReference(drRef);

                trans.Complete();
            }
        }


        #region UserImage
        public void CreateUserImage(DataSetComponent.UserImageDetailRow dr, out int imageID, int filesize)
        {
            DataAccessComponent dac = new DataAccessComponent();
            var iInfo = dac.RetrieveUserImageInformation(dr.UserID);
            int iiID = 0;
            if (iInfo == null)
            {
                dr.isPrimaryImage = true;

                var ii = new DataSetComponent.UserImageDataTable().NewUserImageRow();
                ii.UserID = dr.UserID;
                ii.StorageUsed = 0;
                ii.FreeStorage = SystemConstants.MaxActivityImageStorage;
                ii.ImageAmount = 0;
                dac.CreateUserImageInformation(ii, out iiID);
            }
            else
                dr.UserImageID = iInfo.ID;

            using (TransactionScope trans = new TransactionScope())
            {
                dac.CreateUserImage(dr, out imageID);

                var ii = dac.RetrieveUserImageInformation(dr.UserID);
                ii.UserID = dr.UserID;
                ii.StorageUsed = ii.StorageUsed + filesize;
                ii.FreeStorage = ii.FreeStorage - filesize;
                ii.ImageAmount = ii.ImageAmount + 1;

                dac.UpdateUserImageInformation(dr.UserID, iiID, ii);
                trans.Complete();
            }
        }

        public void DeleteUserImage(String UserID, int imageID, int filesize, out string imageThumbVirtualPath, out string imageVirtualPath)
        {
            DataAccessComponent dac = new DataAccessComponent();
            var dr = dac.RetrieveUserImage(UserID, imageID);
            if (dr.isPrimaryImage == true)
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    dac.DeleteUserImage(UserID, imageID, out imageThumbVirtualPath, out imageVirtualPath);

                    var ii = dac.RetrieveUserImageInformation(dr.UserID);
                    ii.UserID = dr.UserID;
                    ii.StorageUsed = ii.StorageUsed - filesize;
                    ii.FreeStorage = ii.FreeStorage + filesize;
                    ii.ImageAmount = ii.ImageAmount - 1;

                    dac.UpdateUserImageInformation(dr.UserID, ii.ID, ii);
                    trans.Complete();
                }

                var dt = dac.RetrieveUserImages(UserID);
                if (dt.Count() != 0)
                {
                    String UID1 = dt[0].UserID;
                    int imageID1 = dt[0].ID;
                    dac.UpdateUserPrimaryImage(UID1, imageID1);
                }
            }
            else
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    dac.DeleteUserImage(UserID, imageID, out imageThumbVirtualPath, out imageVirtualPath);

                    var ii = dac.RetrieveUserImageInformation(dr.UserID);
                    ii.UserID = dr.UserID;
                    ii.StorageUsed = ii.StorageUsed - filesize;
                    ii.FreeStorage = ii.FreeStorage + filesize;
                    ii.ImageAmount = ii.ImageAmount - 1;

                    dac.UpdateUserImageInformation(dr.UserID, ii.ID, ii);
                    trans.Complete();
                }
            }
        }

        public string RetrieveUserImageUrl(String userID, int imageID)
        {
            var imgDR = new DataAccessComponent().RetrieveUserImage(userID, imageID);

            if (!imgDR.IsImageTitleNull())
                return SystemConstants.GetUserImageURL(userID, imageID, imgDR.Filename);
            else
                return SystemConstants.UsrImageDirectory + "No image.jpg";
        }

        public string RetrieveUserImageThumbUrl(String userID, int imageID)
        {
            var imgDR = new DataAccessComponent().RetrieveUserImage(userID, imageID);

            if (!imgDR.IsImageTitleNull())
                return SystemConstants.GetUserImageThumbURL(userID, imageID, imgDR.Filename);
            else
                return SystemConstants.ActImageDirectory + "No image.jpg";
        }
        #endregion

        public void CreateEmptyUserImage(String userID)
        {
            DataAccessComponent dac = new DataAccessComponent();
            DataSetComponent.UserImageRow userImage = new DataSetComponent.UserImageDataTable().NewUserImageRow();
            userImage.UserID = userID;
            userImage.StorageUsed = userImage.ImageAmount = 0;
            userImage.FreeStorage = SystemConstants.MaxUserImageStorage;

            int userImageID = 0;
            dac.CreateUserImageInformation(userImage, out userImageID);
        }

        public void CreateNewUserImage(String userID, DataSetComponent.UserImageRow userImage, DataSetComponent.UserImageDetailDataTable userImageDetail)
        {
            DataAccessComponent dac = new DataAccessComponent();
            userImage.UserID = userID;

            int userImageID = 0;
            int notUsed = 0;
            dac.CreateUserImageInformation(userImage, out userImageID);

            foreach (var dr in userImageDetail)
            {
                dr.UserID = userID;
                dr.UserImageID = userImageID;
                dac.CreateUserImage(dr, out  notUsed);
            }
        }

        public void AddNewProviderImages(String ProviderID, DataSetComponent.UserImageDetailDataTable dt, DataSetComponent.UserImageRow usrImagDet)
        {
            DataAccessComponent dac = new DataAccessComponent();
            usrImagDet.UserID = ProviderID;

            int userImageID = 0;
            int notUsed = 0;
            dac.UpdateUserImageInformation(usrImagDet);

            foreach (var dr in dt)
            {
                dr.UserID = ProviderID;
                dr.UserImageID = usrImagDet.ID;
                dac.CreateUserImage(dr, out  notUsed);
            }
        }

        public void ChangeActivityEmailAddress(String providerID, string oldEmailAddress, string newEmailAddress)
        {
            var dt = new DataAccessComponent().RetrieveProviderActivities(providerID, 0, "");
            foreach (var dr in dt)
            {
                if (dr.Email == oldEmailAddress)
                    new DataAccessComponent().ChangeActivityEmailAddress(dr.ID, newEmailAddress);
            }
        }

        public string GenerateRefID(string Name)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var RefTable = new DataAccessComponent().RetrieveUserReferences();

            Name = Name.ToUpper();
            StringBuilder sb = new StringBuilder();

            if (Name.Length >= 3)
                sb.Append(Name.Substring(0, 3));
            else
                sb.Append(ObjectHandler.GetRandomKey(3));
            List<string> nameRef = new List<string>(RefTable.Where(x => x.ReferenceID.Substring(0, 3).Equals(sb.ToString())).Select(y => y.ReferenceID.Substring(3, 4)));

            string nextCode = nameRef.Count() == 0 ? sb.ToString() + "0001" : sb.ToString() + (int.Parse(nameRef.OrderByDescending(i => i).First()) + 1).ToString("0000");
            var a = sw.Elapsed;
            return nextCode;
        }


        #region VisitorInfo
        /*            
         * var MonthlyCount = from visitor in Visits
                              group visitor by visitor.CreatedDatetime
                                  into g
                                  select new VisitorRecords { DateTime = g.Key, Count = g.Count() };
            
            return MonthlyCount;
         */
        public IEnumerable<VisitorRecords> RetrieveProviderVisitorMontlyCount(String ProviderID, out int VisitCount)
        {
            DataAccessComponent dac = new DataAccessComponent();
            var Visits = dac.RetrieveProviderVisitorData(ProviderID, DateTime.Now.AddYears(-1), DateTime.Now);
            VisitCount = Visits.Count;
            //var MonthlyCount = from visitor in Visits
            //                   group visitor by new
            //                   {
            //                       visitor.CreatedDatetime.Year,
            //                       visitor.CreatedDatetime.Month
            //                   }

            //                       into g
            //                       select new { DateTime = g.Key, Count = g.Count() };


            //var MonthlyVisit = Visits
            //  .GroupBy(revision => new { revision.CreatedDatetime.Year, revision.CreatedDatetime.Month })
            //  .Select(group => new { GroupCriteria = group.Key, Count = group.Count() })
            //  .OrderBy(x => x.GroupCriteria.Year)
            //  .ThenBy(x => x.GroupCriteria.Month);

            var GroupedVisits = from year in Enumerable.Range(DateTime.Now.AddYears(-1).Year, 2)
                                from month in Enumerable.Range(1, 12)
                                let key = new { Year = year, Month = month }
                                join revision in Visits on key
                                  equals new
                                  {
                                      revision.CreatedDatetime.Year,
                                      revision.CreatedDatetime.Month
                                  } into g
                                select new { GroupCriteria = key, Count = g.Count() };

            List<VisitorRecords> Records = new List<VisitorRecords>();

            foreach (var GroupedVisit in GroupedVisits)
            {

                VisitorRecords record = new VisitorRecords();
                record.DateTime = new DateTime(GroupedVisit.GroupCriteria.Year, GroupedVisit.GroupCriteria.Month, 1);
                record.Count = GroupedVisit.Count;
                if (record.DateTime >= DateTime.Now)
                    break;
                else
                    Records.Add(record);
            }
            return Records;
        }

        public IEnumerable<VisitorRecords> RetrieveProviderVisitorMontlyCount(String ProviderID, DateTime From, DateTime To, out int VisitCount)
        {
            DataAccessComponent dac = new DataAccessComponent();
            var Visits = dac.RetrieveProviderVisitorData(ProviderID.ToString(), From, To);
            VisitCount = Visits.Count;

            var GroupedVisits = from year in Enumerable.Range(DateTime.Now.AddYears(-1).Year, 2)
                                from month in Enumerable.Range(1, 12)
                                let key = new { Year = year, Month = month }
                                join revision in Visits on key
                                  equals new
                                  {
                                      revision.CreatedDatetime.Year,
                                      revision.CreatedDatetime.Month
                                  } into g
                                select new { GroupCriteria = key, Count = g.Count() };

            List<VisitorRecords> Records = new List<VisitorRecords>();

            foreach (var GroupedVisit in GroupedVisits)
            {

                VisitorRecords record = new VisitorRecords();
                record.DateTime = new DateTime(GroupedVisit.GroupCriteria.Year, GroupedVisit.GroupCriteria.Month, 1);
                record.Count = GroupedVisit.Count;
                if (record.DateTime <= From && record.DateTime >= From)
                    break;
                else
                    Records.Add(record);
            }
            return Records;
        }





        #endregion


    }

    public class Reward
    {
        public int Id { get; set; }
        public int Point { get; set; }
        public string Description { get; set; }
        public DateTime Expiry { get; set; }
        public Reward(int id)
        {

            var dr = new DataAccessComponent().RetrieveRewardInfo(Convert.ToInt32(id));
            if (dr != null)
            {
                this.Point = dr.RequiredRewardPoint;
                this.Description = dr.RewardsName;
                this.Expiry = dr.RewardExpiryDate;
            }

        }
    }

    public class CartItem : IEquatable<CartItem>
    {
        #region Properties

        // A place to store the quantity in the cart  
        // This property has an implicit getter and setter.  
        public int Quantity { get; set; }
        public int newquant { get; set; }
        private int _rewardId;
        public int rewardId
        {
            get { return _rewardId; }
            set
            {
                // To ensure that the Prod object will be re-created  
                _reward = null;
                _rewardId = value;
            }
        }

        private Reward _reward = null;
        public Reward Prod
        {
            get
            {
                // Lazy initialization - the object won't be created until it is needed  
                if (_reward == null)
                {
                    _reward = new Reward(rewardId);
                }
                return _reward;
            }
        }

        public DateTime Expiry
        {

            get { return Prod.Expiry; }
        }
        public string Description
        {
            get { return Prod.Description; }
        }

        public int UnitPoint
        {
            get { return Prod.Point; }
        }


        public int TotalPoint
        {
            get
            {
                if (newquant == 0)
                    return UnitPoint * Quantity;
                else
                    return UnitPoint * newquant;
            }
        }

        #endregion

        // CartItem constructor just needs a rewardId  
        public CartItem(int rewardId)
        {
            this.rewardId = rewardId;
        }

        /** 
         * Equals() - Needed to implement the IEquatable interface 
         *    Tests whether or not this item is equal to the parameter 
         *    This method is called by the Contains() method in the List class 
         *    We used this Contains() method in the ShoppingCart AddItem() method 
         */
        public bool Equals(CartItem item)
        {
            return item.rewardId == this.rewardId;
        }
    }

    public class RewardCart
    {

        #region Properties

        public List<CartItem> Items { get; private set; }

        #endregion

        #region Singleton Implementation
        // Readonly properties can only be set in initialization or in a constructor  
        public static readonly RewardCart Instance;

        // The static constructor is called as soon as the class is loaded into memory  
        static RewardCart()
        {
            // If the cart is not in the session, create one and put it there  
            // Otherwise, get it from the session  
            if (HttpContext.Current.Session["ASPNETRewardCart"] == null)
            {
                Instance = new RewardCart();
                Instance.Items = new List<CartItem>();
                HttpContext.Current.Session["ASPNETRewardCart"] = Instance;
            }
            else
            {
                Instance = (RewardCart)HttpContext.Current.Session["ASPNETRewardCart"];
            }
        }

        // A protected constructor ensures that an object can't be created from outside  

        #endregion

        #region Item Modification Methods
        /** 
     * AddItem() - Adds an item to the shopping  
     */
        public void AddItem(int productId)
        {
            // Create a new item to add to the cart  
            CartItem newItem = new CartItem(productId);

            // If this item already exists in our list of items, increase the quantity  
            // Otherwise, add the new item to the list  
            if (Items.Contains(newItem))
            {
                foreach (CartItem item in Items)
                {
                    if (item.Equals(newItem))
                    {
                        item.Quantity++;
                        return;
                    }
                }
            }
            else
            {
                newItem.Quantity = 1;
                Items.Add(newItem);
            }
        }

        /** 
         * SetItemQuantity() - Changes the quantity of an item in the cart 
         */
        public void SetItemQuantity(int productId, int quantity)
        {
            // If we are setting the quantity to 0, remove the item entirely  
            if (quantity == 0)
            {
                RemoveItem(productId);
                return;
            }

            // Find the item and update the quantity  
            CartItem updatedItem = new CartItem(productId);

            foreach (CartItem item in Items)
            {
                if (item.Equals(updatedItem))
                {
                    item.Quantity = quantity;
                    return;
                }
            }
        }

        public void SetItemnewQuantity(int productId, int quantity)
        {

            // Find the item and update the quantity  
            CartItem updatedItem = new CartItem(productId);

            foreach (CartItem item in Items)
            {
                if (item.Equals(updatedItem))
                {
                    item.newquant = quantity;
                    return;
                }
            }
        }

        public void setquanttozero()
        {

            // Find the item and update the quantity  

            foreach (CartItem item in Items)
            {
                item.newquant = 0;

            }
        }

        /** 
         * RemoveItem() - Removes an item from the shopping cart 
         */
        public void RemoveItem(int productId)
        {
            CartItem removedItem = new CartItem(productId);
            Items.Remove(removedItem);
        }
        public int getItemnewQuantity(int ProductId)
        {

            // Find the item and update the quantity  
            CartItem updatedItem = new CartItem(ProductId);

            foreach (CartItem item in Items)
            {
                if (item.Equals(updatedItem))
                {
                    return item.newquant;
                }
            }
            return 0;
        }

        public int getItemQuantity(int ProductId)
        {

            // Find the item and update the quantity  
            CartItem updatedItem = new CartItem(ProductId);

            foreach (CartItem item in Items)
            {
                if (item.Equals(updatedItem))
                {
                    return item.Quantity;
                }
            }
            return 0;
        }
        #endregion

        #region Reporting Methods
        /** 
     * GetSubTotal() - returns the total price of all of the items 
     *                 before tax, shipping, etc. 
     */
        public int getQuantity()
        {
            int quant = 0;
            foreach (CartItem item in Items)
                quant += item.Quantity;
            return quant;

        }

        public int getitemno()
        {
            int quant = 0;
            foreach (CartItem item in Items)
                quant++;
            return quant;

        }
        public int GetSubTotal()
        {
            int subTotal = 0;
            foreach (CartItem item in Items)
                subTotal += item.TotalPoint;

            return subTotal;
        }

        public string getItems()
        {
            string selected = "";
            string separator = "|";

            foreach (CartItem item in Items)
            {
                if (!String.IsNullOrEmpty(selected))
                    selected += separator;
                selected += Convert.ToString(item.rewardId);
            }
            return selected;
        }

        public string getExpiry()
        {
            string selected = "";
            string separator = "|";

            foreach (CartItem item in Items)
            {
                if (!String.IsNullOrEmpty(selected))
                    selected += separator;
                selected += Convert.ToString(item.Expiry);
            }
            return selected;



        }
        public string getNames()
        {
            string selected = "";
            string separator = "|";

            foreach (CartItem item in Items)
            {
                if (!String.IsNullOrEmpty(selected))
                    selected += separator;
                selected += Convert.ToString(item.Description);
            }
            return selected;



        }

        public int getItemQuant(int reward)
        {
            int quant = 0;
            CartItem newItem = new CartItem(reward);
            foreach (CartItem item in Items)
            {
                if (item.Equals(newItem))
                    quant = item.Quantity;
            }
            return quant;
        }
        #endregion
    }

    
}
