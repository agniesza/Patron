using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Patron.Models
{
    public class AuthorThreshold
    {
        public int ID { get; set; }
        public int AuthorID { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(20, ErrorMessage = "Nazwa nie może przekraczać 20 znaków.")]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(100, MinimumLength = 20, ErrorMessage = "Opis musi mieć od 20 do 100 znaków.")]
        public string Description { get; set; }
        public int Value { get; set; }
        public int MaxNumberOfPatrons { get; set; }

        public virtual Author Author { get; set; }
        public virtual List<Patron> Patrons { get; set; }
    }
}