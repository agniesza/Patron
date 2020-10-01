using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Patron.Models
{
    public class Comment
    {
        public int ID { get; set; }

        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public DateTime Date { get; set; }

        public int Rate { get; set; }

        public int PatronID { get; set; }

        public int AuthorID { get; set; }

        public virtual Author Author { get; set; }
        public virtual Patron Patron { get; set; }
    }
}