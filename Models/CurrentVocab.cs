using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SRSProgramMVC.Models
{
    public class CurrentVocab
    {
        public int RepCount { get; set; }
        public DateTime DateNextStudy { get; set; }

        [Key]
        [Column (Order = 0)]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [Key]
        [Column(Order = 1)]
        public int DictionaryVocabID { get; set; }
        public virtual DictionaryVocab DictionaryVocab { get; set; }
    }
}