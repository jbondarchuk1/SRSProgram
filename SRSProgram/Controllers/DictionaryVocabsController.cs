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
    public class DictionaryVocabsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DictionaryVocabs
        public ActionResult Index()
        {
            return View(db.DictionaryVocabs.ToList());
        }

        // GET: DictionaryVocabs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DictionaryVocab dictionaryVocab = db.DictionaryVocabs.Find(id);
            if (dictionaryVocab == null)
            {
                return HttpNotFound();
            }
            return View(dictionaryVocab);
        }

        // GET: DictionaryVocabs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DictionaryVocabs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DictionaryVocabID,Kanji,Hiragana,EnglishMeaning")] DictionaryVocab dictionaryVocab)
        {
            if (ModelState.IsValid)
            {
                db.DictionaryVocabs.Add(dictionaryVocab);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dictionaryVocab);
        }

        // GET: DictionaryVocabs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DictionaryVocab dictionaryVocab = db.DictionaryVocabs.Find(id);
            if (dictionaryVocab == null)
            {
                return HttpNotFound();
            }
            return View(dictionaryVocab);
        }

        // POST: DictionaryVocabs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DictionaryVocabID,Kanji,Hiragana,EnglishMeaning")] DictionaryVocab dictionaryVocab)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dictionaryVocab).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dictionaryVocab);
        }

        // GET: DictionaryVocabs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DictionaryVocab dictionaryVocab = db.DictionaryVocabs.Find(id);
            if (dictionaryVocab == null)
            {
                return HttpNotFound();
            }
            return View(dictionaryVocab);
        }

        // POST: DictionaryVocabs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DictionaryVocab dictionaryVocab = db.DictionaryVocabs.Find(id);
            db.DictionaryVocabs.Remove(dictionaryVocab);
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
