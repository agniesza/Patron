using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Patron.Models
{
    public class Author
    {
        public int ID { get; set; }
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        [StringLength(30, MinimumLength = 15, ErrorMessage = "Numer rachunku nie może przekraczać 30 znaków.")]
        public string BankAccount { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(300, MinimumLength = 20, ErrorMessage = "Opis musi posiadać od 50 do 300 znaków.")]
        public string Description { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(200, MinimumLength = 20, ErrorMessage = "Opis celów musi posiadać od 20 do 200 znaków.")]
        public string Goals { get; set; }

        public string City { get; set; }
        public string Avatar { get; set; }
        public int TotalMoney { get; set; }


        public string FacebookLink { get; set; }
        public string InstagramLink { get; set; }
        public string YouTubeLink { get; set; }
        public string TwitterLink { get; set; }
        public string OtherLink { get; set; }


        //public virtual Category Category { get; set; }
        public virtual List<Category> Categories { get; set; }
        public virtual List<Post> Posts { get; set; }
        public virtual List<Payment> Payments { get; set; }
        public virtual List<AuthorThreshold> AuthorThresholds { get; set; }



    }
}