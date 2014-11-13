using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ISD.Data.EDM;
using ISD.Util;

namespace ISD.Application.provider.MVC.Controllers
{
    public class PageController : Controller
    {
        private ISDEntities db = new ISDEntities();

        // GET: Pages
        public async Task<ActionResult> Index(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            var page = await db.Page.Where(x => x.Name == name).FirstOrDefaultAsync();
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
