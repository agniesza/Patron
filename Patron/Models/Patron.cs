﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Patron.Models
{
    public class Patron
    {
        public int ID { get; set; }
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Avatar { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RegistrationDate { get; set; }

        public virtual CreditCard CreditCard { get; set; }
        public virtual List<Post> Posts { get; set; }
        public virtual List<Payment> Payments { get; set; }
        public virtual List<Threshold> AuthorThresholds { get; set; }
        public virtual List<Author> Followed { get; set; }
        public virtual List<Comment> Comments { get; set; }

    }
}