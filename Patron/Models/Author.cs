using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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
        [AllowHtml]
        public string Description { get; set; }

        public string City { get; set; }
        public string Avatar { get; set; }


        public string FacebookLink { get; set; }
        public string InstagramLink { get; set; }
        public string YouTubeLink { get; set; }
        public string TwitterLink { get; set; }
        public string OtherLink { get; set; }


        
        public virtual List<Category> Categories { get; set; }
        public virtual List<Post> Posts { get; set; }
        public virtual List<Payment> Payments { get; set; }
        public virtual List<Threshold> AuthorThresholds { get; set; }
        public virtual List<Patron> Followers { get; set; }
        public virtual List<Goal> Goals { get; set; }
        public virtual List<Comment> Comments { get; set; }



    }
}