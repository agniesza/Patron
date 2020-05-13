using System.Collections.Generic;

namespace Patron.Models
{
    public class Patron
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public virtual List<Post> Posts { get; set; }
        public virtual List<Payment> Payments { get; set; }
        public virtual List<AuthorThreshold> AuthorThresholds { get; set; }
    }
}