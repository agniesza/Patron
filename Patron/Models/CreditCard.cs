using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Patron.Models
{
    public enum CardType
    {
        American_Express,
        MasterCard,
        Visa
    };

    public class CreditCard
    {
        public int ID { get; set; }
        public CardType CardType { get; set; }
        public string CardNumber { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationYear { get; set; }
        public int CVV { get; set; }
        public int PatronID { get; set; }

        public virtual Patron Patron { get; set; }

    }
}