using ISD.BF;
using ISD.DA;
using ISD.EDS;
using ISD.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ISD.Administration.Web.UserControls
{
    public partial class KeywordManagementUC : System.Web.UI.UserControl
    {
        public string UIMode
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnType.Value))
                    return hdnType.Value;
                else
                    return SystemConstants.FormMode.New.ToString();
            }
            set
            {
                hdnType.Value = value.ToString();
            }
        }
        public string SortExpression
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnSortParameter.Value))
                    return hdnSortParameter.Value;
                else
                    return "";
            }
            set
            {
                hdnSortParameter.Value = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Refresh();
            }
        }

        private void Refresh()
        {
            if (!IsPostBack)
            {

            }
            SetDataSource();
            GridView1.DataBind();
        }

        private void SetDataSource()
        {
            GridView1.DataSource = ods;
            ods.TypeName = typeof(DataAccessComponent).FullName;
            ods.SelectParameters.Clear();
            ods.SelectParameters.Add("sortExpression", SortExpression);
            ods.EnablePaging = GridView1.AllowPaging;
            ods.SelectMethod = "RetrieveKeyCollections";
            ods.SelectCountMethod = "RetrieveKeyCollectionsCount";
            ods.StartRowIndexParameterName = "startIndex";
            ods.MaximumRowsParameterName = "amount";
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditKey")
            {
                LinkButton lnkRename = e.CommandSource as LinkButton;
                GridViewRow row = lnkRename.Parent.Parent as GridViewRow;
                HiddenField hdnID = row.FindControl("hdnID") as HiddenField;
                LinkButton lnkName = row.FindControl("lnkName") as LinkButton;
                if (e.CommandName == "EditKey")
                {
                    DataSetComponent.v_KeyCollectionViewRow dr = new DataAccessComponent().RetrieveKeyword(Convert.ToInt32(hdnID.Value));
                    txtName.Text = dr.Name;
                    txtDescription.Text = dr.Description;
                    txtSynonims.Text = dr.Keywords;
                    hdnThesaurusID.Value = dr.ID.ToString();
                    hdnThesaurusKeyID.Value = dr.KeywordID.ToString();
                    UIMode = SystemConstants.FormMode.Edit.ToString();
                    ModalPopupExtender1.Show();
                }
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblKeyNumber = e.Row.FindControl("lblKeyNumber") as Label;
                Label LblKeywords = e.Row.FindControl("LblKeywords") as Label;

                String[] keywords = LblKeywords.Text.Split(';');
                lblKeyNumber.Text = keywords.Count().ToString();
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            DataAccessComponent dac = new DataAccessComponent();
            DataSetComponent.KeyCollectionDataTable dt = GetSelected();
            foreach (DataSetComponent.KeyCollectionRow dr in dt)
            {
                dac.DeleteKeyCollection(dr.ID);
            }
            Refresh();
        }

        private DataSetComponent.KeyCollectionDataTable GetSelected()
        {
            DataSetComponent.KeyCollectionDataTable dt = new DataSetComponent.KeyCollectionDataTable();
            foreach (GridViewRow row in GridView1.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox CheckBox1 = row.FindControl("chkboxSelected") as CheckBox;
                    HiddenField hdnID = row.FindControl("hdnID") as HiddenField;
                    var dr = dt.NewKeyCollectionRow();
                    if (CheckBox1.Checked)
                    {
                        dr.ID = Convert.ToInt32(hdnID.Value);
                        dt.AddKeyCollectionRow(dr);
                    }
                }
            }
            return dt;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int mode = 0;
            DataAccessComponent dac = new DataAccessComponent();
            DataSetComponent.KeyCollectionRow drKeyProperties = GetKeyProperties();
            DataSetComponent.KeywordRow drKeywords = GetKeywords();

            BusinessFunctionComponent bfc = new BusinessFunctionComponent();

            if (UIMode == SystemConstants.FormMode.New.ToString())
                mode = (int)SystemConstants.FormMode.New;
            else if (UIMode == SystemConstants.FormMode.Edit.ToString())
                mode = (int)SystemConstants.FormMode.Edit;
            else if (UIMode == SystemConstants.FormMode.View.ToString())
                mode = (int)SystemConstants.FormMode.View;

            bfc.SaveKeywords(drKeyProperties, drKeywords, mode);
            UIMode = SystemConstants.FormMode.New.ToString();

            txtName.Text = "";
            txtDescription.Text = "";
            txtSynonims.Text = "";
            hdnThesaurusID.Value = "";
            hdnThesaurusKeyID.Value = "";
            Refresh();
        }

        private DataSetComponent.KeywordRow GetKeywords()
        {
            DataSetComponent.KeywordRow dr = new DataSetComponent.KeywordDataTable().NewKeywordRow();
            txtSynonims.Text = txtSynonims.Text.Replace('.', ';');
            txtSynonims.Text = txtSynonims.Text.Replace(',', ';');
            
            dr.Keywords = txtSynonims.Text;
            if (hdnThesaurusKeyID.Value != "")
                dr.KeyCollectionID = Convert.ToInt32(hdnThesaurusKeyID.Value);
            else dr.KeyCollectionID = 0;
            return dr;
        }

        private DataSetComponent.KeyCollectionRow GetKeyProperties()
        {
            DataSetComponent.KeyCollectionRow dr = new DataSetComponent.KeyCollectionDataTable().NewKeyCollectionRow();
            dr.Name = txtName.Text;
            dr.Description = txtDescription.Text;
            if (hdnThesaurusID.Value != "")
                dr.ID = Convert.ToInt32(hdnThesaurusID.Value);
            else dr.ID = 0;
            return dr;
        }

        protected void lnkNew_Click1(object sender, EventArgs e)
        {
            newKey();
        }

        public void newKey()
        {
            txtName.Text = txtDescription.Text = txtSynonims.Text = hdnThesaurusID.Value = hdnThesaurusKeyID.Value = string.Empty;
            UIMode = SystemConstants.FormMode.New.ToString();
            ModalPopupExtender1.Show();
        }

    }
}