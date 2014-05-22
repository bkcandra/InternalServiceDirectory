using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using ISD.Util;
using System.IO;
using System.Web.UI.HtmlControls;
using ISD.EDS;
using ISD.DA;
using System.Data;
using ISD.BF;


namespace HealthyClub.Providers.Web.UserControls
{
    public partial class ActivityRegistrationImageUC : System.Web.UI.UserControl
    {
        public bool isSupported
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnisSupported.Value))
                    return Convert.ToBoolean(hdnisSupported.Value);
                else return false;
            }
            set
            {
                hdnisSupported.Value = value.ToString();
            }
        }

        public int ImageUploaded
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnImgUploaded.Value))
                    return Convert.ToInt32(hdnImgUploaded.Value);
                else return 0;
            }
            set
            {
                hdnImgUploaded.Value = value.ToString();
            }
        }

        public int SizeUploaded
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnSizeUploaded.Value))
                    return Convert.ToInt32(hdnSizeUploaded.Value);
                else return 0;
            }
            set
            {
                hdnSizeUploaded.Value = value.ToString();
            }
        }

        public string ActionKey
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnActionCode.Value))
                    return hdnActionCode.Value;
                else return "ABCDE123";
            }
            set
            {
                hdnActionCode.Value = value.ToString();
            }
        }

        public string ProviderID
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnProviderID.Value))
                    return hdnProviderID.Value;
                else return Guid.Empty.ToString();
            }
            set
            {
                hdnProviderID.Value = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            isSupported = CheckisBrowserSupported();
        }

        protected void imgBtnUpload_Click(object sender, EventArgs e)
        {
            var dt = GetImages();
            if (dt == null)
            {
                dt = new DataSetComponent.ActivityImageDetailDataTable();
            }
            if (fileUpload1.HasFile)     // CHECK IF ANY FILE HAS BEEN SELECTED.
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(fileUpload1.PostedFile.ContentType, "image/\\S+"))
                {
                    string ext = fileUpload1.PostedFile.ContentType;
                    if (ext == "bmp" || ext == ".bmp")
                    {
                        lblUploadStatus.Text = ".bmp extension is not supported";
                    }
                    else
                    {
                        int iFailedCnt = 0;
                        HttpFileCollection hfc = Request.Files;
                        lblUploadStatus.Text = "Select <b>" + hfc.Count + "</b> file(s)";

                        if (hfc.Count <= 10)    // 10 FILES RESTRICTION.
                        {
                            for (int i = 0; i <= hfc.Count - 1; i++)
                            {
                                HttpPostedFile hpf = hfc[i];
                                if (hpf.ContentLength > 0)
                                {
                                    string sFileName = Path.GetFileName(hpf.FileName);
                                    SizeUploaded = SizeUploaded + Convert.ToInt32(hpf.ContentLength / 1024);
                                    if (SizeUploaded <= SystemConstants.MaxActivityImageStorage)
                                    {
                                        var dr = dt.NewActivityImageDetailRow();

                                        dr.ActivityID = 0;
                                        dr.ActivityImageID = 0;
                                        dr.ImageStream = new BCUtility.ImageHandler().ReadFully(hpf.InputStream);
                                        dr.Filename = sFileName;
                                        dr.Filesize = hpf.ContentLength / 1024;
                                        dt.AddActivityImageDetailRow(dr);

                                        ImageUploaded += 1;
                                    }
                                    else
                                    {
                                        iFailedCnt += 1;
                                        lblUploadStatus.Text += "</br><b>" + iFailedCnt + "</b> file(s) could not be uploaded. Maximum Size per activity is" + SystemConstants.MaxActivityImageStorage + " Kb";
                                    }
                                }
                            }
                            lblUploadStatus.Text = "<b>" + ImageUploaded + "</b> file(s) uploaded.";
                        }
                        else lblUploadStatus.Text = "Max. 10 files allowed.";
                    }
                }
                else
                {
                    lblUploadStatus.Text = "Only image files are accepted.";
                }

            }
            else lblUploadStatus.Text = "No files selected.";
            lblUploadStatus.Visible = true;
            if (ImageUploaded > 0)
            {
                SetDataSource(dt);
                divUploadSuccessfull.Visible = true;
            }

        }

        internal void initUploader(string providerID, string key)
        {
            ActionKey = key;
            ProviderID = providerID;
            // string Path = SystemConstants.ImageDirectory + "/" + activityID + "/" + activityID + "_" + imageID + "_" + prodImage.Filename;
            //Ensure Directory is exist
            //var path = System.IO.Directory.CreateDirectory(Server.MapPath(@SystemConstants.TmpActImageDirectory + "/" + ProviderID + "/"));
            //Create ImageDir provider and key

            //System.IO.Directory.CreateDirectory(Server.MapPath(@SystemConstants.TmpActImageDirectory + "/" + ProviderID + "/" + ActionKey + "/"));
            //Create ImageThumbDir
            //System.IO.Directory.CreateDirectory(Server.MapPath(@SystemConstants.ActImageDirectory + "/" + ProviderID + "/" + ActionKey + "/" + @SystemConstants.ImageThumbDirectory));

        }

        private void SetDataSource(DataSetComponent.ActivityImageDetailDataTable dt)
        {
            dtImageLib.DataSource = dt;
            dtImageLib.DataBind();
        }

        protected void dtImageLib_ItemCommand(object source, DataListCommandEventArgs e)
        {

            if (e.CommandName == "RemoveImage")
            {
                HiddenField hdnID = e.Item.FindControl("hdnID") as HiddenField;
                var dt = GetImages(Convert.ToInt32(hdnID.Value));

                SetDataSource(dt);
                ImageUploaded = dt.Count;
                lblUploadStatus.Text = "<b>" + ImageUploaded + "</b> file(s) uploaded.";
                if (ImageUploaded == 0)
                    divUploadSuccessfull.Visible = false;
            }
        }

        protected void dtImageLib_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Image imgUpload = e.Item.FindControl("imgUpload") as Image;
                HiddenField hdnImageStream = e.Item.FindControl("hdnImageStream") as HiddenField;

                DataRowView drView = e.Item.DataItem as DataRowView;
                var dr = drView.Row as DataSetComponent.ActivityImageDetailRow;

                hdnImageStream.Value = Convert.ToBase64String(dr.ImageStream);

                if (isSupported)
                {
                    imgUpload.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(dr.ImageStream);
                }
                else
                {
                    imgUpload.Visible = false;
                }
            }
        }

        public DataSetComponent.ActivityImageRow GetImageDetail()
        {
            var dr = new DataSetComponent.ActivityImageDataTable().NewActivityImageRow();
            dr.ActivityID = 0;
            dr.StorageUsed = SizeUploaded;
            dr.FreeStorage = SystemConstants.MaxActivityImageStorage - SizeUploaded;
            dr.ImageAmount = ImageUploaded;
            return dr;
        }

        public DataSetComponent.ActivityImageDetailDataTable GetImages()
        {
            if (ImageUploaded != 0)
            {
                var dt = new DataSetComponent.ActivityImageDetailDataTable();
                foreach (DataListItem item in dtImageLib.Items)
                {
                    var dr = dt.NewActivityImageDetailRow();
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        Image imgUpload = item.FindControl("imgUpload") as Image;
                        HiddenField hdnImageStream = item.FindControl("hdnImageStream") as HiddenField;
                        HiddenField hdnImageSize = item.FindControl("hdnImageSize") as HiddenField;
                        HiddenField hdnID = item.FindControl("hdnID") as HiddenField;

                        Label lblName = item.FindControl("lblName") as Label;
                        dr.ID = Convert.ToInt32(hdnID.Value);
                        dr.ActivityID = 0;
                        dr.ActivityImageID = 0;
                        dr.ImageStream = Convert.FromBase64String(hdnImageStream.Value);
                        dr.Filename = lblName.Text;
                        dr.ImageTitle = "";
                        dr.Description = "";
                        dr.isPrimaryImage = false;
                        dr.Filesize = Convert.ToInt32(hdnImageSize.Value);
                        dt.AddActivityImageDetailRow(dr);
                    }
                }
                return dt;
            }
            else return null;
        }

        public DataSetComponent.ActivityImageDetailDataTable GetImages(int Removed)
        {
            if (ImageUploaded != 0)
            {
                var dt = new DataSetComponent.ActivityImageDetailDataTable();
                foreach (DataListItem item in dtImageLib.Items)
                {
                    var dr = dt.NewActivityImageDetailRow();
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        HiddenField hdnID = item.FindControl("hdnID") as HiddenField;
                        if (Convert.ToInt32(hdnID.Value) != Removed)
                        {
                            Image imgUpload = item.FindControl("imgUpload") as Image;
                            HiddenField hdnImageStream = item.FindControl("hdnImageStream") as HiddenField;
                            HiddenField hdnImageSize = item.FindControl("hdnImageSize") as HiddenField;

                            Label lblName = item.FindControl("lblName") as Label;

                            dr.ID = Convert.ToInt32(hdnID.Value);
                            dr.ActivityID = 0;
                            dr.ActivityImageID = 0;
                            dr.ImageStream = Convert.FromBase64String(hdnImageStream.Value);
                            dr.Filename = lblName.Text;
                            dr.ImageTitle = "";
                            dr.Description = "";
                            dr.isPrimaryImage = false;
                            dr.Filesize = Convert.ToInt32(hdnImageSize.Value);
                            dt.AddActivityImageDetailRow(dr);
                        }
                    }
                }
                return dt;
            }
            else return null;
        }

        private bool CheckisBrowserSupported()
        {
            var browser = Request.Browser;
            var version = browser.MajorVersion;
            var name = browser.Browser;

            if (name == "Chrome" && version < SystemConstants.browserChromeVersion)
                return false;
            else if (name == "IE" && version < SystemConstants.browserIEVersion)
                return false;
            else if (name == "Firefox" && version < SystemConstants.browserFirefoxVersion)
                return false;
            else if (name == "Opera" && version < SystemConstants.browserOperaVersion)
                return false;
            else if (name == "Safari" && version < SystemConstants.browserSafariVersion)
                return false;
            else
                return true;
        }
    }

}