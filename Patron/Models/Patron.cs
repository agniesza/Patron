using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Patron.Models
{
    public class Patron
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}