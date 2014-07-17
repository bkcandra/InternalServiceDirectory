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

namespace ISD.Administration.Web.Controllers
{
    public class RequirementsController : Controller
    {
        private ISDEntities db = new ISDEntities();

        // GET: Requirements
        public async Task<ActionResult> Index()
        {
            return View(await db.Requirements.ToListAsync());
        }

        // GET: Requirements/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requirements requirements = await db.Requirements.FindAsync(id);
            if (requirements == null)
            {
                return HttpNotFound();
            }
            return View(requirements);
        }

        // GET: Requirements/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Requirements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Title,Text,Description,CreatedBy,ModifiedBy,CreatedDatetime,ModifiedDatetime")] Requirements requirements)
        {
            if (ModelState.IsValid)
            {
                db.Requirements.Add(requirements);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(requirements);
        }

        // GET: Requirements/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requirements requirements = await db.Requirements.FindAsync(id);
            if (requirements == null)
            {
                return HttpNotFound();
            }
            return View(requirements);
        }

        // POST: Requirements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Title,Text,Description,CreatedBy,ModifiedBy,CreatedDatetime,ModifiedDatetime")] Requirements requirements)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requirements).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(requirements);
        }

        // GET: Requirements/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requirements requirements = await db.Requirements.FindAsync(id);
            if (requirements == null)
            {
                return HttpNotFound();
            }
            return View(requirements);
        }

        // POST: Requirements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Requirements requirements = await db.Requirements.FindAsync(id);
            db.Requirements.Remove(requirements);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
