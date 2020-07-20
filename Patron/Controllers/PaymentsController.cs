using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Patron.DAL;
using Patron.Models;
using PagedList.Mvc;
using PagedList;

namespace Patron.Controllers
{
    public class PaymentsController : Controller
    {
        private PatronContext db = new PatronContext();

        // GET: Payments
        // [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(string sortOrder, string phrase, int? page)
        {
            ViewBag.ValueSortParm = String.IsNullOrEmpty(sortOrder) ? "value_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            var payments = db.Payments.Include(p => p.Author).Include(p => p.Patron);
            switch (sortOrder)
            {
                case "value_desc":
                    payments = payments.OrderByDescending(s => s.Value);
                    break;
                case "Date":
                    payments = payments.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    payments = payments.OrderByDescending(s => s.Date);
                    break;
                default:
                    payments = payments.OrderBy(s => s.Author.UserName);
                    break;
            }
            if (phrase != null)
            {
                page = 1;
                payments = payments.Where(a => a.Author.FirstName.Contains(phrase)
                    || a.Author.LastName.Contains(phrase)
                    || a.Author.UserName.Contains(phrase)
                    || a.Patron.FirstName.Contains(phrase)
                    || a.Patron.LastName.Contains(phrase)
                    || a.Patron.UserName.Contains(phrase)
                    );
            }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
            return View(payments.ToPagedList(pageNumber, pageSize));
        }
        
        [Authorize]
        public ActionResult PatronPayments(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var payments = db.Payments.Where(p => p.PatronID==id).Include(p => p.Author).Include(p => p.Patron).OrderByDescending(p => p.Date);
            ViewBag.patron = db.Patrons.Find(id);
            return View(payments.ToList());
        }

        [Authorize]
        public ActionResult AuthorOneTimePayments(int? id, string sortOrder, string phrase, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.ValueSortParm = String.IsNullOrEmpty(sortOrder) ? "value_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var payments = db.Payments.Where(p => p.AuthorID == id).Include(p => p.Patron).Include(p => p.Author);
            switch (sortOrder)
            {
                case "value_desc":
                    payments = payments.OrderByDescending(s => s.Value);
                    break;
                case "Date":
                    payments = payments.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    payments = payments.OrderByDescending(s => s.Date);
                    break;
                default:
                    payments = payments.OrderBy(s => s.Author.UserName);
                    break;
            }
            if (phrase != null)
            {
                page = 1;
                payments = payments.Where(a => 
                    a.Patron.FirstName.Contains(phrase)
                    || a.Patron.LastName.Contains(phrase)
                    || a.Patron.UserName.Contains(phrase)
                    );
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            ViewBag.author = db.Authors.Find(id);
            return View(payments.ToPagedList(pageNumber, pageSize));
        }
        // GET: Payments/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // GET: Payments/Create

        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.AuthorID = id;
            ViewBag.Author = db.Authors.Find(id);

            Models.Patron p = db.Patrons.Single(pp => pp.UserName == User.Identity.Name);
            ViewBag.PatronID = p.ID;
            return View();
        }

        // POST: Payments/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Payment payment)
        {           
            int Ap = payment.PatronID;
            payment.Patron = db.Patrons.Find(Ap);
            if (ModelState.IsValid)
            {
                payment.Periodicity = (Periodicity)Enum.Parse(typeof(Periodicity), "ONE_TIME", true);
                payment.Date = DateTime.Now;
                db.Payments.Add(payment);
                db.SaveChanges();
                return RedirectToAction("AuthorPage", "Authors", new { id = payment.AuthorID});
            }

            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "UserName", payment.AuthorID);
            ViewBag.PatronID = new SelectList(db.Patrons, "ID", "UserName", payment.PatronID);
            return View(payment);
        }

        // GET: Payments/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "UserName", payment.AuthorID);
            ViewBag.PatronID = new SelectList(db.Patrons, "ID", "UserName", payment.PatronID);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ID,AuthorID,PatronID,Date,Value,Status,Periodicity")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "UserName", payment.AuthorID);
            ViewBag.PatronID = new SelectList(db.Patrons, "ID", "UserName", payment.PatronID);
            return View(payment);
        }

        // GET: Payments/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Payment payment = db.Payments.Find(id);
            db.Payments.Remove(payment);
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
