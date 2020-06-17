using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Patron.ViewModels
{
    public class PaymentGroup
    {
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        public int PaymentCount { get; set; }

    }
}