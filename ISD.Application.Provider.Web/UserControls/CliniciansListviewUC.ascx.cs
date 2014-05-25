using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ISD.DA;
using Microsoft.AspNet.Identity;

namespace ISD.Provider.Web.UserControls
{
    public partial class CliniciansListviewUC : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Context.User.Identity.GetUserId() != string.Empty)
            {
                RefreshClinicians();
            }
        }

        private void RefreshClinicians()
        {
            var dt = new DataAccessComponent().RetrieveProviderClinicians(Context.User.Identity.GetUserId());
            if (dt != null)
            {
                listviewClinicians.DataSource = dt;
                       listviewClinicians.DataBind();
            }
     
        }
    }


}
