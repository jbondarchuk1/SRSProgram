using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebSockets;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SRSProgramMVC.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public virtual ICollection<CurrentVocab> CurrentVocabs { get; set; }
        public virtual ICollection<NewVocab> NewVocabs { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new SRSInitializer());
            // Database.SetInitializer<ApplicationDbContext>(new DropCreateDatabaseAlways<ApplicationDbContext>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<SRSProgramMVC.Models.CurrentVocab> CurrentVocabs { get; set; }

        public System.Data.Entity.DbSet<SRSProgramMVC.Models.DictionaryVocab> DictionaryVocabs { get; set; }

        public System.Data.Entity.DbSet<SRSProgramMVC.Models.NewVocab> NewVocabs { get; set; }
    }

    public class SRSInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        public override void InitializeDatabase(ApplicationDbContext context)
        {
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction
                , string.Format("ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", context.Database.Connection.Database));

            base.InitializeDatabase(context);
        }

        protected override void Seed(ApplicationDbContext context)
        {
            // add my user info
            // var users = new List<ApplicationUser>
            // {
            // , PasswordHash = "AE3DuKXsIDhCpD43bv/GY7TklgvQ8cRzZIwGVolqIyfOYg1/NX80UFuRHbjyu1/A3A=="
            var user = new ApplicationUser { UserName = "marfword@gmail.com", Email = "marfword@gmail.com", PasswordHash = "AE3DuKXsIDhCpD43bv/GY7TklgvQ8cRzZIwGVolqIyfOYg1/NX80UFuRHbjyu1/A3A==", EmailConfirmed=true };
            // };

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var result = UserManager.Create(user);
            var JasonId = "";

            // User added successfully, you can safely use the Id now.
            JasonId = user.Id;
            // add vocabulary
            var vocabWords = new List<DictionaryVocab>()
            {
                new DictionaryVocab{DictionaryVocabID = 0, Kanji = "復習", Hiragana = "ふくしゅう", EnglishMeaning = "review" },
                new DictionaryVocab{DictionaryVocabID = 1, Kanji = "天気", Hiragana = "てんき", EnglishMeaning = "weather" },
                new DictionaryVocab{DictionaryVocabID = 2, Kanji = "野菜", Hiragana = "やさい", EnglishMeaning = "vegetable" },
                new DictionaryVocab{DictionaryVocabID = 3, Kanji = "結構", Hiragana = "けっこう", EnglishMeaning = "pretty much" },
                new DictionaryVocab{DictionaryVocabID = 4, Kanji = "結婚", Hiragana = "けっこん", EnglishMeaning = "marriage" },
                new DictionaryVocab{DictionaryVocabID = 5, Kanji = "建物", Hiragana = "たてもの", EnglishMeaning = "building" },
                new DictionaryVocab{DictionaryVocabID = 6, Kanji = "紙", Hiragana = "かみ", EnglishMeaning = "paper" },
                new DictionaryVocab{DictionaryVocabID = 7, Kanji = "帽子", Hiragana = "ぼうし", EnglishMeaning = "hat" },
                new DictionaryVocab{DictionaryVocabID = 8, Kanji = "掃除", Hiragana = "そうじ", EnglishMeaning = "cleaning" },
                new DictionaryVocab{DictionaryVocabID = 9, Kanji = "階段", Hiragana = "かいだん", EnglishMeaning = "stairs" },
                new DictionaryVocab{DictionaryVocabID = 10, Kanji = "絵", Hiragana = "え", EnglishMeaning = "painting/picture" },
                new DictionaryVocab{DictionaryVocabID = 11, Kanji = "毎月", Hiragana = "まいつき", EnglishMeaning = "every month" },
                new DictionaryVocab{DictionaryVocabID = 12, Kanji = "台所", Hiragana = "だいどころ", EnglishMeaning = "kitchen" },
                new DictionaryVocab{DictionaryVocabID = 13, Kanji = "眼鏡", Hiragana = "めがね", EnglishMeaning = "glasses" },
                new DictionaryVocab{DictionaryVocabID = 14, Kanji = "売る", Hiragana = "うる", EnglishMeaning = "to sell" },
                new DictionaryVocab{DictionaryVocabID = 15, Kanji = "昼", Hiragana = "ひる", EnglishMeaning = "noon" },
                new DictionaryVocab{DictionaryVocabID = 16, Kanji = "屋", Hiragana = "や", EnglishMeaning = "shop" },
                new DictionaryVocab{DictionaryVocabID = 17, Kanji = "辞書", Hiragana = "じしょ", EnglishMeaning = "dictionary" },
                new DictionaryVocab{DictionaryVocabID = 18, Kanji = "低い", Hiragana = "ひくい", EnglishMeaning = "low" },
                new DictionaryVocab{DictionaryVocabID = 19, Kanji = "郵便局", Hiragana = "ゆうびんきょく", EnglishMeaning = "post office" },
                new DictionaryVocab{DictionaryVocabID = 20, Kanji = "嫌", Hiragana = "いや（な）", EnglishMeaning = "disagreeable" },
                new DictionaryVocab{DictionaryVocabID = 21, Kanji = "鼻", Hiragana = "はな", EnglishMeaning = "nose" },
                new DictionaryVocab{DictionaryVocabID = 22, Kanji = "砂糖", Hiragana = "さとう", EnglishMeaning = "sugar" },
                new DictionaryVocab{DictionaryVocabID = 23, Kanji = "卵", Hiragana = "たまご", EnglishMeaning = "eggs" },
                new DictionaryVocab{DictionaryVocabID = 24, Kanji = "薄い", Hiragana = "うすい", EnglishMeaning = "thin" },
                new DictionaryVocab{DictionaryVocabID = 25, Kanji = "若い", Hiragana = "わかい", EnglishMeaning = "young" },
                new DictionaryVocab{DictionaryVocabID = 26, Kanji = "重い", Hiragana = "おもい", EnglishMeaning = "heavy" },
                new DictionaryVocab{DictionaryVocabID = 27, Kanji = "短い", Hiragana = "みじかい", EnglishMeaning = "short" },
                new DictionaryVocab{DictionaryVocabID = 28, Kanji = "弱い", Hiragana = "よわい", EnglishMeaning = "weak" },
                new DictionaryVocab{DictionaryVocabID = 29, Kanji = "強い", Hiragana = "つよい", EnglishMeaning = "strong" },
                new DictionaryVocab{DictionaryVocabID = 30, Kanji = "座る", Hiragana = "すわる", EnglishMeaning = "to sit" },
                new DictionaryVocab{DictionaryVocabID = 31, Kanji = "悪い", Hiragana = "わるい", EnglishMeaning = "bad" },
                new DictionaryVocab{DictionaryVocabID = 32, Kanji = "曲がる", Hiragana = "まがる", EnglishMeaning = "to turn" },
                new DictionaryVocab{DictionaryVocabID = 33, Kanji = "並ぶ", Hiragana = "ならぶ", EnglishMeaning = "to line up" },
                new DictionaryVocab{DictionaryVocabID = 34, Kanji = "暖かい", Hiragana = "あたたかい", EnglishMeaning = "warm" },
                new DictionaryVocab{DictionaryVocabID = 35, Kanji = "閉める", Hiragana = "しめる", EnglishMeaning = "to close" },
                new DictionaryVocab{DictionaryVocabID = 36, Kanji = "厚い", Hiragana = "あつい", EnglishMeaning = "thick, deep, heavy" },
                new DictionaryVocab{DictionaryVocabID = 37, Kanji = "疲れる", Hiragana = "つかれる", EnglishMeaning = "to get tired" },
                new DictionaryVocab{DictionaryVocabID = 38, Kanji = "暑い", Hiragana = "あつい", EnglishMeaning = "hot (weather)" },
                new DictionaryVocab{DictionaryVocabID = 39, Kanji = "涼しい", Hiragana = "すずしい", EnglishMeaning = "cool (weather)" },
                new DictionaryVocab{DictionaryVocabID = 40, Kanji = "吸う", Hiragana = "すう", EnglishMeaning = "to smoke" },
                new DictionaryVocab{DictionaryVocabID = 41, Kanji = "消える", Hiragana = "きえる", EnglishMeaning = "to disappear" },
                new DictionaryVocab{DictionaryVocabID = 42, Kanji = "消す", Hiragana = "けす", EnglishMeaning = "to erase" },
                new DictionaryVocab{DictionaryVocabID = 43, Kanji = "お風呂", Hiragana = "おふろ", EnglishMeaning = "bath" },
                new DictionaryVocab{DictionaryVocabID = 44, Kanji = "暗い", Hiragana = "くらい", EnglishMeaning = "dark" },
                new DictionaryVocab{DictionaryVocabID = 45, Kanji = "渡る", Hiragana = "わたる", EnglishMeaning = "to cross over" }
            };
            vocabWords.ForEach(s => context.DictionaryVocabs.Add(s));

            // add New Words
            var newVocab = new List<NewVocab>();
            for (int i = 0; i <= 23; i++)
            {
                NewVocab databaseAddition = new NewVocab { UserId = JasonId, DictionaryVocabID = i };
                context.NewVocabs.Add(databaseAddition);
                // newVocab.Add(databaseAddition);
            }
            // newVocab.ForEach(s => context.NewVocabs.Add(s));

            // context.NewVocabs.Add(new NewVocab { UserId = JasonId, DictionaryVocabID = 10 });


            Debug.WriteLine(JasonId);
            // add currently studying
            Random rnd = new Random();
            var currentVocabList = new List<CurrentVocab>();
            for (int i = 25; i <= 45; i++)
            {
                currentVocabList.Add(new CurrentVocab
                {
                    User = user,
                    UserId = JasonId,
                    DictionaryVocabID = i,
                    DateNextStudy = DateTime.Today,
                    RepCount = rnd.Next(0, 2)
                });
            }
            currentVocabList.ForEach(s => context.CurrentVocabs.Add(s));
        }
    }
}