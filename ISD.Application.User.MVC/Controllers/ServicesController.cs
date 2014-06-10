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

namespace ISD.Application.User.MVC.Controllers
{
    public class ServicesController : Controller
    {
        private ISDEntities db = new ISDEntities();
        // GET: Services

        public async Task<ActionResult> Index(int? page, int? pageSize)
        {

            var services = db.v_ActivityExplorer.OrderBy(x => x.Name).Skip((page ?? 0) * (pageSize ?? 50)).Take((pageSize ?? 50)).ToListAsync();

            return View(await services);
        }
    }
}