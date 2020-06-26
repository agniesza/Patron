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
            Periodicity period = (Periodicity)Enum.Parse(typeof(Periodicity), "MONTHLY", true);
            foreach (var item in db.Payments)
            {
                if(item.Periodicity == period)
                {
                    if (item.Date == DateTime.Today.AddMonths(-1))                   
                    {// 2-2 itp
                        Payment p = new Payment
                        {
                            Author = item.Author,
                            Patron = item.Patron,
                            Value = item.Value,
                            Periodicity = (Periodicity)Enum.Parse(typeof(Periodicity), "MONTHLY", true),
                            Date = DateTime.Today
                        };
                        db.Payments.Add(p);
                        db.SaveChanges();
                    }
                    else if ((item.Date.Month == 1 || item.Date.Month == 3 || item.Date.Month == 5 
                        || item.Date.Month == 7 || item.Date.Month == 8 || item.Date.Month == 10 || item.Date.Month == 12)
                        && item.Date.Day ==31 && DateTime.Today.Day==30 && item.Date.Month == DateTime.Today.AddMonths(-1).Month)
                    {//31 - 30
                        Payment p = new Payment
                        {
                            Author = item.Author,
                            Patron = item.Patron,
                            Value = item.Value,
                            Periodicity = (Periodicity)Enum.Parse(typeof(Periodicity), "MONTHLY", true),
                            Date = DateTime.Today
                        };
                        db.Payments.Add(p);
                        db.SaveChanges();
                    }
                    else if(item.Date.Month == DateTime.Today.AddMonths(-1).Month
                        && item.Date.Month == 2 && item.Date.Day ==29 )

                    {
                        Payment p = new Payment
                        {
                            Author = item.Author,
                            Patron = item.Patron,
                            Value = item.Value,
                            Periodicity = (Periodicity)Enum.Parse(typeof(Periodicity), "MONTHLY", true),
                            Date = DateTime.Today
                        };
                        db.Payments.Add(p);
                        db.SaveChanges();
                    }
                }
            }
            /*
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
            */
            
            await Console.Out.WriteLineAsync("HelloJob is executing.");
        }
    }
}