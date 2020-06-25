using Patron.DAL;
using Patron.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace Patron.ScheduledTasks
{
    public class PaymentJob : IJob
    {
        private PatronContext db = new PatronContext();

        async Task IJob.Execute(IJobExecutionContext context)
        {
            foreach (var at in db.AuthorThresholds)
            {
                foreach (var patr in at.Patrons)
                {
                    Payment p = new Payment
                    {
                        Author = at.Author,
                        Patron = patr,
                        Value = at.Value,
                       Periodicity = (Periodicity)Enum.Parse(typeof(Periodicity), "MONTHLY", true),
                        Date = DateTime.Now
                    };
                    db.Payments.Add(p);
                    db.SaveChanges();
                }
            }
            
            await Console.Out.WriteLineAsync("HelloJob is executing.");
        }
    }
}