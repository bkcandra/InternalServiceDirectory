using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace ISD.Provider.Web.Models
{
    public class ISDMenu : Menu
    {
        protected override void OnPreRender(EventArgs e)
        {
            // Don't call base OnPreRender
            //base.OnPreRender(e);
        }
    }
}