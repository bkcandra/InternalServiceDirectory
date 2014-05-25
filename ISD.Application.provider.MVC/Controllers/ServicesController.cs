using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ISD.Data.EDM;
using Microsoft.AspNet.Identity;

namespace ISD.Application.provider.MVC.Controllers
{
    public class ServicesController : Controller
    {
        private ISDEntities db = new ISDEntities();
        // GET: Services

        public async Task<ActionResult> Index(int? page, int? pageSize)
        {
            var provID = User.Identity.GetUserId();
            var services = db.v_ActivityExplorer.Where(x => x.ProviderID == provID).OrderBy(x=>x.ModifiedDateTime).Skip((page ?? 0) * pageSize ?? 10).Take(pageSize ?? 10).ToListAsync();
            return View(await services);
        }
    }
}