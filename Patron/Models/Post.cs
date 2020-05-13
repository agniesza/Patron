using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Patron.Models
{
    public class Post
    {
        public int ID { get; set; }
        public int AuthorID { get; set; }
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        public double Raiting { get; set; }
        public int NumberOfRatings { get; set; }

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