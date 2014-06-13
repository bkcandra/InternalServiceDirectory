using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ISD.Application.User.MVC.Models;
using ISD.BF;
using ISD.Data.EDM;
using ISD.EDS;
using ISD.Util;
using System.IO;
using BCUtility;

namespace ISD.Application.User.MVC.Controllers
{
    
    public class ServiceController : Controller
    {
        private ISDEntities db = new ISDEntities();
        // GET: Service
        public async Task<ActionResult> Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dbService = await db.v_ActivityExplorer.Where(x => x.ID == id && x.isPrimary == true).FirstOrDefaultAsync();
            if (dbService == null)
            {
                return HttpNotFound();
            }
            ServiceDetailModel model = new ServiceDetailModel();
            ObjectHandler.CopyPropertyValues(dbService, model);
            model.ImageInfo = await db.ActivityImage.Where(x => x.ActivityID == id).FirstOrDefaultAsync();
            if (model.ImageInfo != null)
                model.Images = await db.ActivityImageDetail.Where(x => x.ActivityImageID == model.ImageInfo.ID).ToListAsync();
            model.Services = await db.v_ActivityExplorer.Where(x => x.PrimaryServiceID == id).ToListAsync();
            model.Clinicians = await db.v_ActivityClinicianExplorer.Where(x => x.ActivityID == id).ToListAsync();
            return View(model);
        }
        public async Task<ActionResult> Detail(string name)
        {
            if (name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dbService = await db.v_ActivityExplorer.Where(x => x.Name == name && x.isPrimary == true).FirstOrDefaultAsync();
            if (dbService == null)
            {
                return HttpNotFound();
            }
            ServiceDetailModel model = new ServiceDetailModel();
            ObjectHandler.CopyPropertyValues(dbService, model);
            model.Images = await db.ActivityImageDetail.Where(x => x.ActivityID == dbService.ID).ToListAsync();
            model.Services = await db.v_ActivityExplorer.Where(x => x.PrimaryServiceID == dbService.ID).ToListAsync();
            model.Clinicians = await db.v_ActivityClinicianExplorer.Where(x => x.ActivityID == dbService.ID).ToListAsync();
            return View(model);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public async Task<ActionResult> Search(string name)
        {
            if (name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dbService = await db.v_ActivityExplorer.Where(x => x.Name == name && x.isPrimary == true).FirstOrDefaultAsync();
            if (dbService == null)
            {
                return HttpNotFound();
            }
            ServiceDetailModel model = new ServiceDetailModel();
            ObjectHandler.CopyPropertyValues(dbService, model);
            model.Images = await db.ActivityImageDetail.Where(x => x.ActivityID == dbService.ID).ToListAsync();
            model.Services = await db.v_ActivityExplorer.Where(x => x.PrimaryServiceID == dbService.ID).ToListAsync();
            model.Clinicians = await db.v_ActivityClinicianExplorer.Where(x => x.ActivityID == dbService.ID).ToListAsync();
            return View(model);
        }
    }
}
