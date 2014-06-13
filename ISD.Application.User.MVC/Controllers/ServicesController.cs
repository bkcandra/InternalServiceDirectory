using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using ISD.Data.EDM;
using ISD.Util;
using System.Web.Script.Serialization;

namespace ISD.Application.User.MVC.Controllers
{
    public class ServicesController : Controller
    {


        private ISDEntities db = new ISDEntities();
        // GET: Services

        public async Task<ActionResult> index()
        {
            return View();
        }

        public async Task<ActionResult> GetServices(int? page, int? pageSize, string category, string clinic,string search)
        {
            var services = db.v_ActivityExplorer.OrderBy(x => x.Name).Skip((page ?? 0) * (pageSize ?? 50)).Take((pageSize ?? 50)).ToListAsync();
            return PartialView("_PartialServicesListing", await services);

        }

        [HttpGet]
        public async Task<JsonResult> GetCategoriesJson(string selected)
        {
            string[] selList = null;
            if (!string.IsNullOrEmpty(selected))
            {
                selList = selected.Split('|');
            }

            var categories = await db.v_CategoryExplorer.ToListAsync();
            List<jstree> data = new List<jstree>();

            foreach (var cat in categories)
            {
                var nodeParent = cat.Level1ParentID.ToString();
                var nodeIcon = "fa fa-asterisk";
                bool stateIsDisabled = false;
                bool stateIsOpened = false;
                bool stateIsSelected = false;

                if (cat.Level1ParentID == 0)
                {
                    nodeParent = "#";
                    nodeIcon = "fa fa-folder";
                }
                if (selList != null)
                    if (selList.Contains(cat.ID.ToString()) || selList.Contains(cat.Level1ParentID.ToString()))
                    {
                        stateIsOpened = true;
                        stateIsSelected = true;
                    }

                data.Add(new jstree
                {
                    id = cat.ID.ToString(),
                    text = cat.Name,
                    parent = nodeParent,
                    icon = nodeIcon,
                    state = new jsstate()
                    {
                        disabled = stateIsDisabled,
                        opened = stateIsOpened,
                        selected = stateIsSelected
                    }
                });
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }

    public class jstree
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public jsstate state { get; set; }

    }

    public class jsstate
    {
        public bool opened { get; set; }
        public bool disabled { get; set; }
        public bool selected { get; set; }
    }

    public class ServiceQuery
    {
        public List<int> category { get; set; }
        public List<int> clinic { get; set; }
        public string search { get; set; }


    }
}