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
using Microsoft.AspNet.Identity;
using ISD.Util;

namespace ISD.Application.provider.MVC.Controllers
{
    public class CliniciansController : Controller
    {
        private ISDEntities db = new ISDEntities();

        [Authorize(Roles = @SystemConstants.ProviderRole)]
        // GET: Clinicians
        public async Task<ActionResult> Index()
        {
            var provID = User.Identity.GetUserId();
            var clinicians = await db.v_ProviderClinicians.Where(c => c.ProviderID == provID).ToListAsync();
            return View(clinicians);
        }
    }
}
