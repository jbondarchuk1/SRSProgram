using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SRSProgramMVC.Models;

namespace SRSProgramMVC.Controllers
{
    public class NewVocabsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        static ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
        private static readonly string currentUserId = user.Id;

        // GET: NewVocabs
        [Authorize]
        public ActionResult Index()
        {
            var newVocabs = db.NewVocabs.SqlQuery($"SELECT * FROM dbo.NewVocabs WHERE UserId=N\'{currentUserId}\';").ToList();
            return View(newVocabs);
        }

        // GET: NewVocabs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CurrentVocab newVocab = db.CurrentVocabs.SqlQuery($"SELECT TOP 1 * FROM dbo.CurrentVocabs WHERE UserId=N\'{currentUserId}\' AND DictionaryVocabID={id};").Single();
            if (newVocab == null)
            {
                return HttpNotFound();
            }
            return View(newVocab);
        }

        // POST: NewVocabs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NewVocab newVocab = db.NewVocabs.SqlQuery($"SELECT TOP 1 * FROM dbo.NewVocabs WHERE UserId=N\'{currentUserId}\' AND DictionaryVocabID={id};").Single();
            db.NewVocabs.Remove(newVocab);
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
