using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SRSProgramMVC.Controllers;
using SRSProgramMVC.Models;
using SRSProgramMVC.ViewModels;

namespace SRSProgramTests
{
    [TestClass]
    public class HomeControllerTests
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        HomeController homeController = new HomeController();

        [TestMethod]
        public void DateToNextStudyTest()
        {
            // Arrange
            int reps = 1;
            DateTime previousDate = new DateTime();
            previousDate = DateTime.Today;
            DateTime expected = DateTime.Today.AddDays(1.00);

            // Act
            DateTime actual = homeController.DateToNextStudy(reps, previousDate);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddVocabularyTest()
        {
            // Arrange
            DictionaryVocab expected = new DictionaryVocab()
            {
                Kanji = "感情",
                Hiragana = "かんじょう",
                EnglishMeaning = "deep or strong emotion"
            };

            // Act
            homeController.AddVocabulary(expected);

            // Assert
            DictionaryVocab actual = db.DictionaryVocabs.SqlQuery($"SELECT TOP 1 * FROM dbo.NewVocabs WHERE Kanji=\"{expected.Kanji}\"").Single();
            Assert.AreEqual(expected, actual);
        }

    }
}
