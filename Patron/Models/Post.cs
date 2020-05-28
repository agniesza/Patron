using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Patron.Models
{
    public class Post
    {
        public int ID { get; set; }
        public int AuthorID { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(300, MinimumLength = 10, ErrorMessage = "Treść musi mieć od 10 do 300 znaków.")]
        public string Content { get; set; }
        public double Raiting { get; set; }
        public int NumberOfRatings { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public virtual List<Patron> Patrons { get; set; }
        public virtual Author Author { get; set; }


        private void CalculateRaiting(int rate)
        {
            double tmp = (Raiting * NumberOfRatings) + rate;
            NumberOfRatings++;
            Raiting = tmp / NumberOfRatings;

        }
    }
}