using System;

namespace Patron.Models
{
    public enum Status
    {
        CANCELLED,
        ACTIVE
    };
    public enum Type
    {
        MONTHLY,
        ONE_TIME
    };
    public class Payment
    {
        public int ID { get; set; }
        public int AuthorID { get; set; }
        public int PatronID { get; set; }
        public string SourceBankAcc { get; set; }
        public DateTime Date { get; set; }
        public int Value { get; set; }
        public Status Status { get; set; }
        public Type Type { get; set; }

        public virtual Author Author { get; set; }
        public virtual Patron Patron { get; set; }



    }

}