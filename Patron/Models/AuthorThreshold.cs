using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Patron.Models
{
    public class AuthorThreshold
    {
        public int ID { get; set; }
        public int AuthorID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public int MaxNumberOfPatrons { get; set; }

        public virtual Author Author { get; set; }
        public virtual List<Patron> Patrons { get; set; }
    }
}