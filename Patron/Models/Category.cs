using System.Collections.Generic;

namespace Patron.Models
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }


        public virtual List<Author> Authors { get; set; }

    }
}