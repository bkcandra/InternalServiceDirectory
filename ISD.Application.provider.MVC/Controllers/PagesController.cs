using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ISD.Data.EDM;
using ISD.Util;
using System.Threading.Tasks;

namespace ISD.Application.provider.MVC.Controllers
{
    public class PagesController : Controller
    {
        private ISDEntities db = new ISDEntities();

        // GET: Pages
        public async Task<ActionResult> Index()
        {
            return View(await db.Page.ToListAsync());
        }

        // GET: Pages/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page pages = await db.Page.FindAsync(id);
            if (pages == null)
            {
                return HttpNotFound();
            }
            return View(pages);
        }

        // GET: Pages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Page pages)
        {
            pages.ModifiedBy = pages.CreatedBy = User.Identity.Name;
            pages.ModifiedDatetime = pages.CreatedDatetime = DateTime.Now;
            if (ModelState.IsValid)
            {
                pages.PageContent = HttpUtility.HtmlDecode(pages.PageContent);
                db.Page.Add(pages);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(pages);
        }

        // GET: Pages/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page pages = await db.Page.FindAsync(id);
            if (pages == null)
            {
                return HttpNotFound();
            }
            return View(pages);
        }

        // POST: Pages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Page pages)
        {
            pages.ModifiedBy = User.Identity.Name;
            pages.ModifiedDatetime = DateTime.Now;
            if (ModelState.IsValid)
            {
                pages.PageContent = HttpUtility.HtmlDecode(pages.PageContent);
                db.Entry(pages).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pages);
        }

        // GET: Pages/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page pages = await db.Page.FindAsync(id);
            if (pages == null)
            {
                return HttpNotFound();
            }
            return View(pages);
        }

        // POST: Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Page pages = await db.Page.FindAsync(id);
            db.Page.Remove(pages);
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

      