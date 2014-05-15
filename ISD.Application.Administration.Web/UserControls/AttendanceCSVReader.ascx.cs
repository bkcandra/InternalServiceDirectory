using ISD.BF;
using ISD.DA;
using ISD.EDS;
using ISD.Util;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web;

using System.Web.UI;
using System.Web.UI.WebControls;


namespace ISD.Administration.Web.UserControls
{
    public partial class AttendanceCSVReader : System.Web.UI.UserControl
    {
        public bool Error
        {
            get
            {
                if (string.IsNullOrEmpty(hdnError.Value))
                    return true;
                else return Convert.ToBoolean(hdnError.Value);
            }
            set
            {
                hdnError.Value = value.ToString();
                btnInsert.Enabled = !Error;
            }
        }

        public int EditedRowIndex
        {
            get
            {
                if (string.IsNullOrEmpty(hdnEditedRowIndex.Value))
                    return -1;
                else return Convert.ToInt32(hdnEditedRowIndex.Value);
            }
            set
            {
                hdnEditedRowIndex.Value = value.ToString();
            }
        }

        DataTable dt
        {
            get;
            set;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSetComponent.ActivityUserAttendanceRow attendanceDR = new DataSetComponent.ActivityUserAttendanceDataTable().NewActivityUserAttendanceRow();

        }

        /*
        protected void imgBtnUpload_Click(object sender, EventArgs e)
        {
            if (fileUpload1.HasFile)     // CHECK IF ANY FILE HAS BEEN SELECTED.
            {
                List<List<String>> AttendanceList = new List<List<string>>();

                DataTable dt = new DataTable();
                List<ListItem> header1 = new List<ListItem>();
                List<ListItem> header2 = new List<ListItem>();
                List<ListItem> header3 = new List<ListItem>();

                string ext = Path.GetExtension(fileUpload1.FileName);
                if (ext != ".csv")
                {
                    lblUploadStatus.Text = "only .csv extension is supported";
                }
                else
                {
                    // open the file "data.csv" which is a CSV file with headers

                    //var acsv = new CsvHelper.CsvReader(new StreamReader(fileUpload1.FileContent));
                    using (var fileReader = new StreamReader(fileUpload1.FileContent))
                    using (var csvReader = new CsvHelper.CsvReader(fileReader))
                    {
                        List<DataRecord> records = csvReader.GetRecords<DataRecord>().ToList();
                        GridviewAttendance.DataSource = records;
                        GridviewAttendance.DataBind();
                    };
                }
                CsvPreview.Visible = CsvSubmit.Visible = true;

            }
            else lblUploadStatus.Text = "No files selected.";
            lblUploadStatus.Visible = true;
        }
        */

        protected void imgBtnUpload_Click(object sender, EventArgs e)
        {
            if (fileUpload1.HasFile)     // CHECK IF ANY FILE HAS BEEN SELECTED.
            {

                dt = new DataTable();
                dt.Clear();
                List<ListItem> header1 = new List<ListItem>();
                ListItem fli1 = new ListItem("Select column", "0");
                header1.Add(fli1);
                List<ListItem> header2 = new List<ListItem>();
                ListItem fli2 = new ListItem("Select column", "0");
                header2.Add(fli2);
                List<ListItem> header3 = new List<ListItem>();
                ListItem fli3 = new ListItem("Select column", "0");
                header3.Add(fli3);


                string ext = Path.GetExtension(fileUpload1.FileName);
                if (ext != ".csv")
                {
                    lblUploadStatus.Text = "only .csv extension is supported";
                }
                else
                {
                    // open the file "data.csv" which is a CSV file with headers
                    using (CsvReader csv =
                           new CsvReader(new StreamReader(fileUpload1.FileContent), true))
                    {
                        int fieldCount = csv.FieldCount;
                        string[] headers = csv.GetFieldHeaders();
                        dt.Columns.Clear();
                        foreach (var header in headers)
                        {
                            BoundField boundfield = new BoundField();
                            boundfield.DataField = header;
                            boundfield.HeaderText = header;
                            GridviewAttendance.Columns.Add(boundfield);
                            dt.Columns.Add(header);

                            ListItem li1 = new ListItem(header);
                            header1.Add(li1);

                            ListItem li2 = new ListItem(header);
                            header2.Add(li2);

                            ListItem li3 = new ListItem(header);
                            header3.Add(li3);


                        }
                        while (csv.ReadNextRecord())
                        {
                            DataRow dr = dt.NewRow();
                            List<string> AttendanceRow = new List<String>();

                            for (int i = 0; i < fieldCount; i++)
                            {
                                dr[headers[i]] = csv[i].ToString();
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }
                GridviewAttendance.DataSource = dt;
                GridviewAttendance.DataBind();
                GridviewAttendance.HeaderRow.TableSection = TableRowSection.TableHeader;

                ddlAttendanceActivityID.DataSource = header1;
                ddlAttendanceDate.DataSource = header2;
                ddlAttendanceMemberID.DataSource = header3;

                ddlAttendanceActivityID.DataBind();
                ddlAttendanceDate.DataBind();
                ddlAttendanceMemberID.DataBind();

                CsvPreview.Visible = CsvSubmit.Visible = true;

            }
            else lblUploadStatus.Text = "No files selected.";
            lblUploadStatus.Visible = true;
        }

        protected void GridviewAttendance_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (EditedRowIndex != -1)
            {
                CancelEditGridView();
            }
            EditedRowIndex = GridviewAttendance.EditIndex = e.NewEditIndex;
            dt = getGridViewData(e.NewEditIndex);
            GridviewAttendance.DataSource = dt;
            GridviewAttendance.HeaderRow.TableSection = TableRowSection.TableHeader;
            GridviewAttendance.DataBind();
        }

        private DataTable getGridViewData(int editIndex)
        {

            DataTable dtU = new DataTable();
            DataTable dtOri = new DataTable();
            if (GridviewAttendance.HeaderRow != null)
            {

                for (int i = 0; i < GridviewAttendance.HeaderRow.Cells.Count; i++)
                {
                    dtU.Columns.Add(GridviewAttendance.HeaderRow.Cells[i].Text);
                }
            }

            foreach (GridViewRow row in GridviewAttendance.Rows)
            {
                DataRow dr = dtU.NewRow();
                if (row.RowIndex == editIndex)
                {
                    DataRow drOri = dtOri.NewRow();
                    gvOriginal.Columns.Clear();
                    for (int i = 0; i < GridviewAttendance.HeaderRow.Cells.Count; i++)
                    {
                        BoundField boundfield = new BoundField();
                        boundfield.DataField = GridviewAttendance.HeaderRow.Cells[i].Text;
                        boundfield.HeaderText = GridviewAttendance.HeaderRow.Cells[i].Text;
                        gvOriginal.Columns.Add(boundfield);
                        dtOri.Columns.Add(GridviewAttendance.HeaderRow.Cells[i].Text);

                    }
                    for (int i = 0; i < row.Cells.Count; i++)
                    {

                        string text = Regex.Replace(row.Cells[i].Text, @"<[^>]+>|&nbsp;", "").Trim();
                        dr[i] = text;
                        drOri[i] = text;

                    }
                    dtOri.Rows.Add(drOri);
                    gvOriginal.DataSource = dtOri;
                    gvOriginal.DataBind();
                }
                else
                {
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        dr[i] = Regex.Replace(row.Cells[i].Text, @"<[^>]+>|&nbsp;", "").Trim();
                    }
                }
                dtU.Rows.Add(dr);
            }
            return dtU;
        }

        private DataTable getGridViewData(bool deleteSelected)
        {
            DataTable dtU = new DataTable();
            if (GridviewAttendance.HeaderRow != null)
            {

                for (int i = 0; i < GridviewAttendance.HeaderRow.Cells.Count; i++)
                {
                    dtU.Columns.Add(GridviewAttendance.HeaderRow.Cells[i].Text);
                }
            }

            foreach (GridViewRow row in GridviewAttendance.Rows)
            {
                if (deleteSelected)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkSelectRecord = row.FindControl("chkSelectRecord") as CheckBox;

                        if (!chkSelectRecord.Checked)
                        {
                            DataRow dr = dtU.NewRow();
                            for (int i = 0; i < row.Cells.Count; i++)
                            {
                                dr[i] = Regex.Replace(row.Cells[i].Text, @"<[^>]+>|&nbsp;", "").Trim();
                            }
                            dtU.Rows.Add(dr);
                        }
                    }
                }
                else
                {
                    DataRow dr = dtU.NewRow();
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        dr[i] = Regex.Replace(row.Cells[i].Text, @"<[^>]+>|&nbsp;", "").Trim();
                    }
                    dtU.Rows.Add(dr);
                }

            }
            return dtU;
        }

        private DataTable CancelGridViewData(int editIndex)
        {
            DataTable dtU = new DataTable();

            if (GridviewAttendance.HeaderRow != null)
            {
                for (int i = 0; i < GridviewAttendance.HeaderRow.Cells.Count; i++)
                {
                    dtU.Columns.Add(GridviewAttendance.HeaderRow.Cells[i].Text);
                }
            }

            foreach (GridViewRow row in GridviewAttendance.Rows)
            {
                DataRow dr = dtU.NewRow();
                if (row.RowIndex == editIndex)
                {
                    foreach (GridViewRow rowOri in gvOriginal.Rows)
                    {
                        if (rowOri.RowIndex == 0)
                            for (int i = 0; i < rowOri.Cells.Count; i++)
                            {
                                dr[i] = Regex.Replace(rowOri.Cells[i].Text, @"<[^>]+>|&nbsp;", "").Trim();
                            }
                    }
                }
                else
                {
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        dr[i] = Regex.Replace(row.Cells[i].Text, @"<[^>]+>|&nbsp;", "").Trim();

                    }
                }
                dtU.Rows.Add(dr);
            }
            gvOriginal.DataSource = null;
            gvOriginal.DataBind();
            return dtU;
        }

        private DataTable UpdateGridViewData(int editIndex)
        {
            DataTable dtU = new DataTable();

            if (GridviewAttendance.HeaderRow != null)
            {
                for (int i = 0; i < GridviewAttendance.HeaderRow.Cells.Count; i++)
                {
                    dtU.Columns.Add(GridviewAttendance.HeaderRow.Cells[i].Text);
                }
            }

            foreach (GridViewRow row in GridviewAttendance.Rows)
            {

                DataRow dr = dtU.NewRow();
                if (row.RowIndex == editIndex)
                {
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        if (i != 0 && i != 1)
                        {
                            TextBox txtUpdate = ((TextBox)row.Cells[i].Controls[0]);
                            if (txtUpdate != null)
                                dr[i] = Regex.Replace(txtUpdate.Text, @"<[^>]+>|&nbsp;", "").Trim();
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        dr[i] = Regex.Replace(row.Cells[i].Text, @"<[^>]+>|&nbsp;", "").Trim();
                    }
                }
                dtU.Rows.Add(dr);
            }
            gvOriginal.DataSource = null;
            gvOriginal.DataBind();
            return dtU;
        }

        protected void GridviewAttendance_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            CancelEditGridView();
        }

        private void CancelEditGridView()
        {
            dt = CancelGridViewData(EditedRowIndex);
            EditedRowIndex = GridviewAttendance.EditIndex = -1;
            GridviewAttendance.DataSource = dt;
            GridviewAttendance.HeaderRow.TableSection = TableRowSection.TableHeader;
            GridviewAttendance.DataBind();
        }

        protected void GridviewAttendance_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            dt = UpdateGridViewData(EditedRowIndex);
            EditedRowIndex = GridviewAttendance.EditIndex = -1;
            GridviewAttendance.DataSource = dt;
            GridviewAttendance.HeaderRow.TableSection = TableRowSection.TableHeader;
            GridviewAttendance.DataBind();
        }

        protected void lnkReview_Click(object sender, EventArgs e)
        {
            bool valid = false;
            List<ReviewRecord> invalidUsrs, invalidActs = new List<ReviewRecord>();

            List<ReviewRecord> allrecords = GetRecords();
            if (allrecords != null)
            {
                CheckReference(allrecords, out invalidUsrs, out invalidActs, out valid);
                if (valid)
                {
                    Error = false;
                    txtStatus.Text = "=Review Status= \r\n\nAttendance records has been reviewed. You can now continue to submit the records.";
                }
                else
                {
                    txtStatus.Text = "=Review Status= \r\n\nOops, I have found invalid record(s) on the records and have flagged it for you. Consider to edit or remove before you can submit the records ";
                    Error = true;
                    FlagAttendanceRecords(invalidActs, invalidUsrs);
                }
            }
            else
            {
                txtStatus.Text = "=Review Status= \r\n\nOops, No records found. Did you delete all of the records? please reupload the attendance list";
                Error = true;
            }
        }

        private void CheckReference(List<ReviewRecord> allrecords, out List<ReviewRecord> invalidUsrs, out List<ReviewRecord> invalidActs, out bool Valid)
        {
            var UserRefTable = new DataAccessComponent().RetrieveUserReferences();
            HashSet<string> usrRefHash = new HashSet<string>(UserRefTable.Select(x => x.ReferenceID));
            invalidUsrs = allrecords.Where(x => !usrRefHash.Contains(x.UserRefID)).ToList();

            var ActReftable = new DataAccessComponent().RetrieveActivityReferences();
            HashSet<string> actRefHash = new HashSet<string>(ActReftable.Select(x => x.ReferenceID));
            invalidActs = allrecords.Where(x => !actRefHash.Contains(x.ActRefID)).ToList();

            if (invalidActs.Count() == 0 && invalidUsrs.Count() == 0)
                Valid = true;
            else
                Valid = false;

        }

        private void FlagAttendanceRecords(List<ReviewRecord> invalidActRecords, List<ReviewRecord> invalidUsrRecords)
        {
            HashSet<int> invalidActRecordsIndex = new HashSet<int>(invalidActRecords.Select(x => x.RecordNumber));
            HashSet<int> invalidUsrRecordsIndex = new HashSet<int>(invalidUsrRecords.Select(x => x.RecordNumber));

            foreach (GridViewRow row in GridviewAttendance.Rows)
            {
                if (invalidUsrRecordsIndex.Contains(row.RowIndex) || invalidActRecordsIndex.Contains(row.RowIndex))
                    row.BackColor = Color.Red;
            }

        }

        private List<ReviewRecord> GetRecords()
        {
            String ActivityIDHeaderText = ddlAttendanceActivityID.SelectedItem.Text;
            String MemberIDHeaderText = ddlAttendanceMemberID.SelectedItem.Text;
            String DateHeaderText = ddlAttendanceDate.SelectedItem.Text;

            int MemberIDCell = 0;
            int ActIDCell = 0;
            int DateCell = 0;

            for (int i = 0; i < GridviewAttendance.HeaderRow.Cells.Count; i++)
            {
                if (GridviewAttendance.HeaderRow.Cells[i].Text == ActivityIDHeaderText)
                    ActIDCell = i;
                else if (GridviewAttendance.HeaderRow.Cells[i].Text == MemberIDHeaderText)
                    MemberIDCell = i;
                else if (GridviewAttendance.HeaderRow.Cells[i].Text == DateHeaderText)
                    DateCell = i;
            }

            List<ReviewRecord> AllRecords = new List<ReviewRecord>();
            foreach (GridViewRow row in GridviewAttendance.Rows)
            {
                ReviewRecord record = new ReviewRecord();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if (i == MemberIDCell)
                        record.UserRefID = Regex.Replace(row.Cells[i].Text, @"<[^>]+>|&nbsp;", "").Trim();
                    else if (i == ActIDCell)
                        record.ActRefID = Regex.Replace(row.Cells[i].Text, @"<[^>]+>|&nbsp;", "").Trim();
                    else if (i == DateCell)
                    {
                        DateTime attDate = DateTime.Now;
                        string date = Regex.Replace(row.Cells[i].Text, @"<[^>]+>|&nbsp;", "").Trim();
                        if (DateTime.TryParse(date, out attDate))
                        {
                            record.AttendanceDate = attDate;
                        }
                        else
                            record.AttendanceDate = DateTime.Now;
                    }
                    record.RecordNumber = row.RowIndex;

                }
                AllRecords.Add(record);
            }
            return AllRecords;
        }

        protected void lnkResetFlag_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridviewAttendance.Rows)
            {
                row.BackColor = Color.White;
            }
        }

        protected void lnkCheckedInvalidRecord_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridviewAttendance.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    if (row.BackColor == Color.Red)
                    {
                        CheckBox chkSelectRecord = row.FindControl("chkSelectRecord") as CheckBox;
                        chkSelectRecord.Checked = true;
                    }

                }
            }
        }

        protected void lnkSelectAll_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridviewAttendance.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkSelectRecord = row.FindControl("chkSelectRecord") as CheckBox;
                    chkSelectRecord.Checked = true;
                }
            }
        }

        protected void lnkDeleteSelectedAct_Click(object sender, EventArgs e)
        {
            dt = getGridViewData(true);
            GridviewAttendance.DataSource = dt;
            GridviewAttendance.HeaderRow.TableSection = TableRowSection.TableHeader;
            GridviewAttendance.DataBind();
        }

        private List<int> GetSelectedRecord()
        {
            List<int> index = new List<int>();
            foreach (GridViewRow row in GridviewAttendance.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkSelectRecord = row.FindControl("chkSelectRecord") as CheckBox;
                    if (chkSelectRecord.Checked)
                    {
                        index.Add(row.RowIndex);
                    }
                }
            }
            return index;
        }

        private void ResetForm()
        {
            GridviewAttendance.DataSource = ddlAttendanceActivityID.DataSource = ddlAttendanceDate.DataSource = ddlAttendanceMemberID.DataSource = null;
            GridviewAttendance.HeaderRow.TableSection = TableRowSection.TableHeader;
            GridviewAttendance.DataBind();
            ddlAttendanceActivityID.DataBind();
            ddlAttendanceDate.DataBind();
            ddlAttendanceMemberID.DataBind();
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            //Review
            bool valid = false;
            List<ReviewRecord> invalidUsrs, invalidActs = new List<ReviewRecord>();

            List<ReviewRecord> allrecords = GetRecords();
            if (allrecords != null)
            {
                CheckReference(allrecords, out invalidUsrs, out invalidActs, out valid);
                if (valid)
                {
                    DataSetComponent.ActivityUserAttendanceDataTable dt = new DataSetComponent.ActivityUserAttendanceDataTable();
                    var UserRefTable = new DataAccessComponent().RetrieveUserReferences();
                    var ActReftable = new DataAccessComponent().RetrieveActivityReferences();

                    foreach (var record in allrecords)
                    {
                        DataSetComponent.ActivityUserAttendanceRow dr = dt.NewActivityUserAttendanceRow();
                        dr.UserID = UserRefTable.Where(x => x.ReferenceID == record.UserRefID).Select(y => y.UserId).FirstOrDefault();
                        dr.ActivityID = ActReftable.Where(x => x.ReferenceID == record.ActRefID).Select(y => y.ActivityID).FirstOrDefault();
                        dr.AttendanceDatetime = record.AttendanceDate;
                        dr.CreatedBy = SystemConstants.SystemName;
                        dr.CreatedDateTime = DateTime.Now;
                        dr.Processed = false;
                        dr.ProcesssedDatetime = SystemConstants.nodate;
                        dt.AddActivityUserAttendanceRow(dr);
                    
                    }
                    
                    //SAVE
                    new BusinessFunctionComponent().SaveAttendanceRecords(dt);
                    ResetForm();
                    txtStatus.Text = "= SUCCESS inserting records = \r\n\n " + allrecords.Count + " record(s) was saved.";

                }
                else
                {
                    txtStatus.Text = "=Failed inserting records= \r\n\nOops, I have found invalid record(s) on the records and have flagged it for you. Consider to edit or remove before you can submit the records ";
                    Error = true;
                    FlagAttendanceRecords(invalidActs, invalidUsrs);
                }
            }
            else
            {
                txtStatus.Text = "=Failed inserting records= \r\n\nOops, No records found. Did you delete all of the records? please reupload the attendance list";
                Error = true;
            }

        }



    }
    public class ReviewRecord
    {
        public string ActRefID { get; set; }
        public string UserRefID { get; set; }
        public DateTime AttendanceDate { get; set; }
        public int ActID { get; set; }
        public int RecordNumber { get; set; }
    }
}