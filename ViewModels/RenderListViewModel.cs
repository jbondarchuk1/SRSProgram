using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRSProgramMVC.ViewModels
{
    public class RenderList
    {
        public List<PostVocab> dictionaryList { get; set; }
    }
    public class PostVocab
    {
        public int DictionaryVocabID { get; set; }
        public string Kanji { get; set; }
        public string Hiragana { get; set; }
        public string EnglishMeaning { get; set; }
        public bool doneStudying { get; set; }
    }
}