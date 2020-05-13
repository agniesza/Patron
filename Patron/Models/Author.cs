using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Patron.Models
{
    public class Author
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CategoryID { get; set; }
        public string BankAccount { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [DataType(DataType.MultilineText)]
        public string Goals { get; set; }
        public string City { get; set; }



        public string FacebookLink { get; set; }
        public string InstagramLink { get; set; }
        public string YouTubeLink { get; set; }
        public string TwitterLink { get; set; }
        public string OtherLink { get; set; }


        public virtual Category Category { get; set; }
        public virtual List<Post> Posts { get; set; }
        public virtual List<Payment> Payments { get; set; }
        public virtual List<AuthorThreshold> AuthorThresholds { get; set; }



    }
}