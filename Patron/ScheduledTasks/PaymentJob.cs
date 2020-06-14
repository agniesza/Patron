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
            Payment p = new Payment
            {
                Author = db.Authors.Find(1),
                Patron = db.Patrons.Find(1),
                //AuthorThreshold=authorThresholds[0],

                Value = db.AuthorThresholds.Find(1).Value,
                // Status= (Status) Enum.Parse(typeof(Status), "INACTIVE", true),
                Periodicity = (Periodicity)Enum.Parse(typeof(Periodicity), "MONTHLY", true),
                Date = DateTime.Today
            };
            db.Payments.Add(p);
            db.SaveChanges();
            await Console.Out.WriteLineAsync("HelloJob is executing.");
        }
    }
}