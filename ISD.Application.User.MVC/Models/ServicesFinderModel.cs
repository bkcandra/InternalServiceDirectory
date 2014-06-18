using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace ISD.Application.User.MVC.Models
{
    public class ServicesFinderModel
    {
        public string selectedCategory { get; set; }
        public string selectedClinic { get; set; }
        public string searchQuery { get; set; }
        public string page { get; set; }
        public string pageSize { get; set; }
        public ICollection<int> Categories { get; set; }
        public IEnumerable<ISD.Data.EDM.v_ActivityExplorer> services { get; set; }

        public List<ListItem> Show =
       new List<ListItem>
            {
                new ListItem {Value = "", Text = "Show"},
                 new ListItem {Value = "5", Text = "5"},
                 new ListItem {Value = "10", Text = "10"},
                 new ListItem {Value = "25", Text = "25"},
                 new ListItem {Value = "50", Text = "50"}
                };
    }
}