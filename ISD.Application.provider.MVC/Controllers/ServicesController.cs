using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using ISD.Data.EDM;
using Microsoft.AspNet.Identity;
using ISD.Util;

namespace ISD.Application.provider.MVC.Controllers
{
    public class ServicesController : Controller
    {
        private ISDEntities db = new ISDEntities();
        // GET: Services

        [Authorize(Roles = SystemConstants.ProviderRole)]
        public async Task<ActionResult> Index(int? page, int? pageSize)
        {

            var provID = User.Identity.GetUserId();
            var services = db.v_ActivityExplorer.Where(x => x.ProviderID == provID).OrderByDescending(x => x.ModifiedDateTime).Skip((page ?? 0) * (pageSize ?? 50)).Take((pageSize ?? 50)).ToListAsync();

            return View(await services);
        }
    }
}