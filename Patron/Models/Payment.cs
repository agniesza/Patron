using System;

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
       // public int AuthorThresholdID { get; set; }
        public string SourceBankAcc { get; set; }
        public DateTime Date { get; set; }
        public int Value { get; set; }
        public Status Status { get; set; }
        public Periodicity Periodicity { get; set; }

        public virtual Author Author { get; set; }
        public virtual Patron Patron { get; set; }
       // public virtual AuthorThreshold AuthorThreshold { get; set; }



    }

}