using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SRSProgramMVC.Models;

namespace SRSProgramMVC.Controllers
{
    public class CurrentVocabsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        static ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
        private static readonly string currentUserId = user.Id;

        // GET: CurrentVocabs/Index
        [Authorize]
        public ActionResult Index()
        {

            var currentVocabs = db.CurrentVocabs.SqlQuery($"SELECT * FROM dbo.CurrentVocabs WHERE UserId=N\'{currentUserId}\';").ToList();
            return View(currentVocabs);
        }

        // GET: CurrentVocabs/Delete/DictionaryId
        [Authorize]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CurrentVocab currentVocab = db.CurrentVocabs.SqlQuery($"SELECT TOP 1 * FROM dbo.CurrentVocabs WHERE UserId=N\'{currentUserId}\' AND DictionaryVocabID={id};").Single();
            if (currentVocab == null)
            {
                return HttpNotFound();
            }
            return View(currentVocab);
        }

        // POST: CurrentVocabs/Delete/DictionaryId
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CurrentVocab currentVocab = db.CurrentVocabs.SqlQuery($"SELECT TOP 1 * FROM dbo.CurrentVocabs WHERE UserId=N\'{currentUserId}\' AND DictionaryVocabID={id};").Single();
            Debug.WriteLine("Vocab: " + currentVocab.DictionaryVocab);
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
