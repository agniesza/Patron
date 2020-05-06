using System;
using System.Collections.Generic;
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
        public string Description { get; set; }
        public string Goals { get; set; }



        public string FacebookLink { get; set; }
        public string InstagramLink { get; set; }
        public string YouTubeLink { get; set; }
        public string OtherLink { get; set; }


        public virtual Category Category { get; set; }
    }
}