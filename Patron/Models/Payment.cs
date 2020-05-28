using System;
using System.ComponentModel.DataAnnotations;

namespace Patron.Models
{
    public enum Status
    {
        INACTIVE,
        ACTIVE
    };
    public enum Periodicity
    {
        MONTHLY,
        ONE_TIME
    };
    public class Payment
    {
        public int ID { get; set; }
        public int AuthorID { get; set; }
        public int PatronID { get; set; }
        [StringLength(30, MinimumLength = 15, ErrorMessage = "Numer rachunku nie może przekraczać 30 znaków.")]
        public string SourceBankAcc { get; set; }
        public DateTime Date { get; set; }
        public int Value { get; set; }
        public Status Status { get; set; }
        public Periodicity Periodicity { get; set; }

        public virtual Author Author { get; set; }
        public virtual Patron Patron { get; set; }



    }

}