using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SRSProgramMVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.EnterpriseServices;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using SRSProgramMVC.ViewModels;

namespace SRSProgramMVC.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        static ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
        private static readonly string currentUserId = user.Id;


        [Authorize]
        // GET: Home/Study
        public ActionResult Study()
        {
            NewVocab[] listOfNewVocabulary = new NewVocab[24];
            listOfNewVocabulary = db.NewVocabs.SqlQuery($"SELECT TOP 10 * FROM dbo.NewVocabs WHERE UserId=N\'{currentUserId}\'").ToArray();
            
            // this is the only list that gets passed to the view
            List<CurrentVocab> listCurrentlyStudying = new List<CurrentVocab>();
            listCurrentlyStudying = db.CurrentVocabs.SqlQuery($"SELECT * FROM dbo.CurrentVocabs WHERE DateNextStudy<=\'{DateTime.Today}\'").ToList();

            List<PostVocab> studyDictionaryData = new List<PostVocab>();
            // grabs list of new vocabulary words, removes them from newVocabs table and adds them to CurrentVocabs
            foreach (NewVocab vocab in listOfNewVocabulary)
            {
                CurrentVocab currentVocab = new CurrentVocab
                {
                    UserId = currentUserId,
                    DictionaryVocabID = vocab.DictionaryVocabID,
                    RepCount = 0,
                    DateNextStudy = DateTime.Today
                };

                db.CurrentVocabs.Add(currentVocab);
                listCurrentlyStudying.Add(currentVocab);
                db.NewVocabs.Remove(vocab);
            }
            foreach (CurrentVocab vocab in listCurrentlyStudying)
            {
                // convert dictionary vocab into appropriate postvocab view model
                DictionaryVocab dictionaryForm = db.DictionaryVocabs.Find(vocab.DictionaryVocabID);
                PostVocab postVocab = new PostVocab
                {
                    DictionaryVocabID = dictionaryForm.DictionaryVocabID,
                    Kanji = dictionaryForm.Kanji,
                    Hiragana = dictionaryForm.Hiragana,
                    EnglishMeaning = dictionaryForm.EnglishMeaning,
                    doneStudying = false
                };

                studyDictionaryData.Add(postVocab);
            }

            RenderList renderList = new RenderList()
            {
                dictionaryList = studyDictionaryData
            };
            ModelState.Clear();
            db.SaveChanges();
            return View(renderList);
        }

        // POST: Home/Study
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Study(RenderList? postVocab)
        {
            if (postVocab.dictionaryList == null)
            {
                return View(postVocab);
            } 
                

            // here we need to:
            // update the date of next study for any current vocab with a checkmark next to it
            // pass the same study view back but this time with an updated list only containing vocab with no checkmark
            
            List<PostVocab> studyDictionaryData = new List<PostVocab>();

            foreach (PostVocab v in postVocab.dictionaryList)
            {
                if (v.doneStudying == true)
                {
                    Debug.WriteLine($"{v.DictionaryVocabID}, {currentUserId}");
                    CurrentVocab currentVocab = db.CurrentVocabs.Find(currentUserId, v.DictionaryVocabID);
  
                    if (currentVocab.RepCount < 4)
                    {
                        int newRepCount = currentVocab.RepCount + 1;

                        DateTime dateToNextStudy = DateToNextStudy(newRepCount, DateTime.Today);

                        CurrentVocab updateThis = db.CurrentVocabs.SqlQuery($"SELECT TOP 1 * FROM dbo.CurrentVocabs WHERE DictionaryVocabID={v.DictionaryVocabID} AND UserId=N\'{currentUserId}\';").Single();

                        updateThis.RepCount = newRepCount;
                        updateThis.DateNextStudy = dateToNextStudy;
                        db.SaveChanges();
                    }
                }
                else
                {
                    v.doneStudying = false;
                    Debug.WriteLine($"{v.Kanji}, {v.doneStudying}");
                    studyDictionaryData.Add(v);
                }
                db.SaveChanges();
            }
            foreach (PostVocab p in studyDictionaryData)
            {
                Debug.WriteLine($"{p.DictionaryVocabID},{p.doneStudying}");
            }
            RenderList renderList = new RenderList()
            {
                dictionaryList = studyDictionaryData
            };
            ModelState.Clear();
            // return View(renderList);
            return View(renderList);
        }



            [Authorize]
        // GET: Home/AddVocabulary
        public ActionResult AddVocabulary()
        {
            ViewBag.Message = "Add a new Vocabulary Word";

            return View();
        }
        // POST: Home/AddVocabulary
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult AddVocabulary([Bind(Include = "DictionaryVocabID,Kanji,Hiragana,EnglishMeaning")] DictionaryVocab dictionaryVocab)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;


            if (ModelState.IsValid)
            {
                int? queryDictionaryID = null;
                try
                {
                    DictionaryVocab[] checkVocab = db.DictionaryVocabs.SqlQuery($"SELECT * FROM dbo.DictionaryVocabs WHERE Kanji=N\'{dictionaryVocab.Kanji}\';").ToArray();
                    queryDictionaryID = checkVocab[0].DictionaryVocabID;
                }
                catch
                {
                    Debug.WriteLine("Nothing Returned in SQL Query");
                }

                if (queryDictionaryID == null)
                {
                    DictionaryVocab addedWord;
                    addedWord = db.DictionaryVocabs.Add(dictionaryVocab);

                    NewVocab newVocab = new NewVocab { UserId = currentUserId, DictionaryVocabID = addedWord.DictionaryVocabID };
                    db.NewVocabs.Add(newVocab);
                }
                else
                {
                    NewVocab newVocab = new NewVocab { UserId = currentUserId, DictionaryVocabID = (int)queryDictionaryID };
                    db.NewVocabs.Add(newVocab);
                }

                
                db.SaveChanges();
                return RedirectToAction("AddVocabulary");
            }

            return View(dictionaryVocab);
        }


        #region Helpers
        
        // returns new date of study based on reps or null if reps are >= 4
        public DateTime DateToNextStudy(int numberOfReps, DateTime lastStudyDate)
        {
            
            Dictionary<int,int> SRSTable = new Dictionary<int,int> 
            {
                {1, 2},
                {2, 5},
                {3, 13}
            };
            
            DateTime dateToNextStudy = lastStudyDate.AddDays(Convert.ToDouble(SRSTable[numberOfReps]));
            // SRSTable[numberOfReps]

            return dateToNextStudy;
        }

        #endregion
    }
}