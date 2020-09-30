using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Patron.Models
{
    public class Goal
    {
        public int ID { get; set; }

        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string  isAchieved { get; set; }
        public int AuthorID { get; set; }

        public virtual Author Author { get; set; }



    }
}