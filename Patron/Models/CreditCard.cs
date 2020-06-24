using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Patron.Models
{
    public enum CardType
    {
        AmericanExpress,
        MasterCard,
        Visa
    };

    public class CreditCard
    {
       
        public int ID { get; set; }
        [Required]
        public CardType CardType { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Podaj prawidłowy numer karty")]
        public string CardNumber { get; set; }
        [Required]
        [Range(1, 12, ErrorMessage = "Podaj prawidłową wartość w postaci MM")]
        public int ExpirationMonth { get; set; }
        [Required]
        [Range(0, 99, ErrorMessage = "Podaj prawidłową wartość w postaci YY")]
        public int ExpirationYear { get; set; }
        [Required]
        [Range(100, 999, ErrorMessage = "Podaj prawidłową wartość kodu CVV")]
        public int CVV { get; set; }
        public int PatronID { get; set; }

        
        public virtual Patron Patron { get; set; }

    }
}