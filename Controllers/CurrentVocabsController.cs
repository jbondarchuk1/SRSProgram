using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SRSProgramMVC.Models;

namespace SRSProgramMVC.Controllers
{
    public class CurrentVocabsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CurrentVocabs
        public ActionResult Index()
        {
            return View(db.CurrentVocabs.ToList());
        }

        // GET: CurrentVocabs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CurrentVocab currentVocab = db.CurrentVocabs.Find(id);
            if (currentVocab == null)
            {
                return HttpNotFound();
            }
            return View(currentVocab);
        }

        // GET: CurrentVocabs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CurrentVocabs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CurrentVocabID,RepCount,DateNextStudy")] CurrentVocab currentVocab)
        {
            if (ModelState.IsValid)
            {
                db.CurrentVocabs.Add(currentVocab);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(currentVocab);
        }

        // GET: CurrentVocabs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CurrentVocab currentVocab = db.CurrentVocabs.Find(id);
            if (currentVocab == null)
            {
                return HttpNotFound();
            }
            return View(currentVocab);
        }

        // POST: CurrentVocabs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CurrentVocabID,RepCount,DateNextStudy")] CurrentVocab currentVocab)
        {
            if (ModelState.IsValid)
            {
                db.Entry(currentVocab).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(currentVocab);
        }

        // GET: CurrentVocabs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CurrentVocab currentVocab = db.CurrentVocabs.Find(id);
            if (currentVocab == null)
            {
                return HttpNotFound();
            }
            return View(currentVocab);
        }

        // POST: CurrentVocabs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CurrentVocab currentVocab = db.CurrentVocabs.Find(id);
            db.CurrentVocabs.Remove(currentVocab);
            db.SaveChanges();
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
