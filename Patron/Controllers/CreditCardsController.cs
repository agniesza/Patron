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

namespace Patron.Controllers
{
    public class CreditCardsController : Controller
    {
        private PatronContext db = new PatronContext();

        // GET: CreditCards
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var creditCards = db.CreditCards.Include(c => c.Patron);
            return View(creditCards.ToList());
        }

        // GET: CreditCards/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditCard creditCard = db.CreditCards.Find(id);
            if (creditCard == null)
            {
                return HttpNotFound();
            }
            return View(creditCard);
        }

        // GET: CreditCards/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.ID = new SelectList(db.Patrons, "ID", "UserName");
            ViewBag.FirstName = null;
            ViewBag.LastName = null;
            if (db.Authors.Any(p => p.UserName.Equals(User.Identity.Name)))
            {
                Author author = db.Authors.Single(p => p.UserName.Equals(User.Identity.Name));
                ViewBag.FirstName = author.FirstName;
                ViewBag.LastName = author.LastName;
            }
            return View();
        }

        // POST: CreditCards/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CardType,CardNumber,ExpirationMonth,ExpirationYear,CVV,PatronID")] CreditCard creditCard)
        {
            if (ModelState.IsValid)
            {
                Models.Patron patron = db.Patrons.Single(p => p.UserName == User.Identity.Name);
                creditCard.Patron = patron;
                db.CreditCards.Add(creditCard);
                db.SaveChanges();
                return RedirectToAction("PatronHomePage", "Patrons", new { id = patron.ID });
            }

            ViewBag.ID = new SelectList(db.Patrons, "ID", "UserName", creditCard.ID);
            return View(creditCard);
        }

        // GET: CreditCards/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditCard creditCard = db.CreditCards.Find(id);
            if (creditCard == null)
            {
                return HttpNotFound();
            }

            ViewBag.ID = new SelectList(db.Patrons, "ID", "UserName", creditCard.ID);
            return View(creditCard);
        }

        // POST: CreditCards/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ID,CardType,CardNumber,ExpirationMonth,ExpirationYear,CVV,PatronID")] CreditCard creditCard)
        {
            string currentUserName = User.Identity.Name;
            Models.Patron p = db.Patrons.Single(pp => pp.UserName == currentUserName);
            if (ModelState.IsValid)
            {
                db.Entry(creditCard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PatronHomePage", "Patrons",  new { id = p.ID });
            }
           
            ViewBag.ID = new SelectList(db.Patrons, "ID", "UserName", creditCard.ID);
            return View(creditCard);
        }

        // GET: CreditCards/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditCard creditCard = db.CreditCards.Find(id);
            if (creditCard == null)
            {
                return HttpNotFound();
            }
            return View(creditCard);
        }

        // POST: CreditCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            CreditCard creditCard = db.CreditCards.Find(id);
            db.CreditCards.Remove(creditCard);
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
