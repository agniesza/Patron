using Microsoft.Ajax.Utilities;
using Patron.DAL;
using Patron.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Patron.Controllers
{
    public class PatronsController : Controller
    {
        private PatronContext db = new PatronContext();

        // GET: Patrons
        [Authorize(Roles ="Admin")]
        public ActionResult Index(string phrase, int? page)
        {
            var patrons = db.Patrons.OrderBy(p => p.UserName);
            if (phrase != null)
            {
                page = 1;
                patrons = patrons.Where(a => a.FirstName.Contains(phrase)
                    || a.LastName.Contains(phrase)
                    || a.UserName.Contains(phrase)).OrderBy(p => p.UserName);
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(patrons.ToPagedList(pageNumber, pageSize));
        }

        // GET: Patrons/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Patron patron = db.Patrons.Find(id);
            if (patron == null)
            {
                return HttpNotFound();
            }
            ViewBag.Avt = patron.Avatar;
            return View(patron);
        }

        //GET
        public ActionResult PatronHomePage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Patron patron = db.Patrons.Find(id);
            if (patron == null)
            {
                return HttpNotFound();
            }
            Status status = (Status)Enum.Parse(typeof(Status), "INACTIVE", true);
            Periodicity period = (Periodicity)Enum.Parse(typeof(Periodicity), "MONTHLY", true);
            ViewBag.ActiveSubscriptions = patron.AuthorThresholds;
             ViewBag.OneTimeSupport = patron.Payments.Where(p => p.Periodicity != period);
            ViewBag.OneTimeSupportCount = (patron.Payments.Where(p => p.Periodicity != period)).Count();//patron.Payments.Where(p => p.Periodicity != period).Count;
            ViewBag.Avt = patron.Avatar;
            ViewBag.CountActiveSub = patron.AuthorThresholds.Count;
            List<Payment> ap = patron.Payments;
            List<Author> aap = new List<Author>();
            foreach (var item in ap)
            {
                aap.Add(item.Author); //autorzy z płatnosci
            }
            List<AuthorThreshold> at = patron.AuthorThresholds;
            List<Author> aat= new List<Author>();
            foreach (var item in at)
            {
                aat.Add(item.Author); //autozy z progow
            }

            List<Author> unsubscribed = new List<Author>();
            bool tmp;
            //wez wszytskie platnosci patrona
            foreach (var payment in patron.Payments)
            {
                tmp = false;
                foreach (var athreshold in patron.AuthorThresholds)
                {
                    if (payment.AuthorID == athreshold.AuthorID)
                        tmp = true;
                }
                if (!tmp)
                {
                    unsubscribed.Add(payment.Author);
                }

            }
            List<Author> unsubscribedDistinct = unsubscribed.GroupBy(a => a.ID)
                .Select(g => g.First()).ToList();

            ViewBag.IncomingPayments = IncomingPayments(patron);
            int sumIncomingPayments = 0;
            foreach (var p in ViewBag.IncomingPayments)
            {
                sumIncomingPayments += p.Value;
            }
            ViewBag.SumIncomingPayments = sumIncomingPayments;
            ViewBag.InActiveSub = unsubscribedDistinct;
            ViewBag.InActiveSubCount = unsubscribedDistinct.Count;
            ViewBag.TotalMonay = patron.Payments.Sum(p => p.Value);
            return View(patron);

        }
        public List<Payment> IncomingPayments(Models.Patron patron)
        {
            Periodicity period = (Periodicity)Enum.Parse(typeof(Periodicity), "MONTHLY", true);

            List<Payment> payments = new List<Payment>();
            foreach (var at in patron.AuthorThresholds)
            {
                foreach (var p in patron.Payments)
                {
                    if(p.AuthorID == at.AuthorID && p.Date.Month == DateTime.Today.AddMonths(-1).Month && p.Date > DateTime.Today.AddMonths(-2) && p.Periodicity == period)
                    {
                        payments.Add(p);
                    }
                }
                {

                }
            }

            return payments;
        }

        // GET: Patrons/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patrons/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ID,UserName,FirstName,LastName")] Models.Patron patron)
        {
            if (ModelState.IsValid)
            {
                db.Patrons.Add(patron);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(patron);
        }
        [Authorize]
        public ActionResult EditProfile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Patron patron = db.Patrons.Find(id);
            if (patron == null)
            {
                return HttpNotFound();
            }
            return View(patron);
        }
        [HttpPost]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public ActionResult EditProfile([Bind(Include = "ID,UserName,FirstName,LastName,Avatar")] Models.Patron patron)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["avatarFile2"];
                if (file != null && file.ContentLength > 0)
                {
                    patron.Avatar = file.FileName;
                    file.SaveAs(HttpContext.Server.MapPath("~/Avatars/") + patron.Avatar);
                }
                db.Entry(patron).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PatronHomePage", new { id = patron.ID });
            }
            return View(patron);
        }
        // GET: Patrons/Edit/5
        [Authorize]
        public ActionResult AddPatronProfile(int? id)
        {
            Author a = db.Authors.Find(id);
            Models.Patron patron = new Models.Patron { UserName = a.UserName };

            db.Patrons.Add(patron);
            db.SaveChanges();

            // return View("~/Views/Patrons/Edit.cshtml", patron);
            return RedirectToAction("Edit", "Patrons", new { id = patron.ID });


        }
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Models.Patron patron = db.Patrons.Find(id);          
           
                if (db.Authors.Any(p => p.UserName.Equals(User.Identity.Name)))
                {
                Author author = db.Authors.Single(p => p.UserName.Equals(User.Identity.Name));
                patron.FirstName = author.FirstName;
                patron.LastName = author.LastName;
                }
            
            if (patron == null)
            {
                return HttpNotFound();
            }
            return View(patron);
        }

        // POST: Patrons/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserName,FirstName,LastName,Avatar")] Models.Patron patron)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["avatarFile"];
                if (file != null && file.ContentLength > 0)
                {
                    patron.Avatar = file.FileName;
                    file.SaveAs(HttpContext.Server.MapPath("~/Avatars/") + patron.Avatar);
                }
                db.Entry(patron).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create", "CreditCards", new { id = patron.ID });
            }
            return View(patron);
        }

        [Authorize]
        public ActionResult AddCreditCard()
        {
            return View();
            /*
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Patron patron = db.Patrons.Find(id);
            CreditCard card = new CreditCard {
                Patron = patron,
                CVV=640
            };
            db.CreditCards.Add(card);
            db.SaveChanges();
            //patron.CreditCard = card;
            
           
            if (patron == null)
            {
                return HttpNotFound();
            }
            ViewBag.thisPatron = patron;
            */
            
        }

        // GET: Patrons/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Patron patron = db.Patrons.Find(id);
            if (patron == null)
            {
                return HttpNotFound();
            }
            return View(patron);
        }

        // POST: Patrons/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Models.Patron patron = db.Patrons.Find(id);
            if (db.CreditCards.Any(card => card.PatronID == patron.ID))
            {
                CreditCard c = db.CreditCards.Single(card => card.PatronID == patron.ID);
                db.CreditCards.Remove(c);
            }
            
            db.Patrons.Remove(patron);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
