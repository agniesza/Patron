using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Patron.Models
{
    public class Post
    {
        public int ID { get; set; }
        public int AuthorID { get; set; }
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Tytuł musi zawierać od 10 do 100 znaków.")]
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(300, MinimumLength = 10, ErrorMessage = "Treść musi zawierać od 10 do 300 znaków.")]
        public string Content { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public virtual List<Patron> Patrons { get; set; }
        public virtual Author Author { get; set; }

    }
}