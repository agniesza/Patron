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

namespace Patron.Controllers
{
    public class PatronsController : Controller
    {
        private PatronContext db = new PatronContext();

        // GET: Patrons
        public ActionResult Index()
        {

            return View(db.Patrons.ToList());
        }

        // GET: Patrons/Details/5
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
            ViewBag.InActiveSubscriptions = patron.Payments.Where(p => p.Status==status && p.Periodicity==period);
            ViewBag.OneTimeSupport = patron.Payments.Where(p => p.Periodicity != period);
            ViewBag.Avt = patron.Avatar;
            ViewBag.CountActiveSub = patron.AuthorThresholds.Count;
            // ViewBag.CountInActiveSub = patron.Payments.DistinctBy(p => p.Author);
            // var paym = patron.Payments.Any(p => p.Author.ID ==;
            List<Payment> ap = patron.Payments;
            List<Author> aap = new List<Author>();
            foreach (var item in ap)
            {
                aap.Add(item.Author);
            }
            List<AuthorThreshold> at = patron.AuthorThresholds;
            List<Author> aat= new List<Author>();
            foreach (var item in at)
            {
                aat.Add(item.Author);
            }
            ViewBag.InActiveSub = aap.Where(p => !aat.Any(p2 => p2.ID == p.ID));
            ViewBag.InActiveSubCount = aap.Where(p => !aat.Any(p2 => p2.ID == p.ID)).Count();

            return View(patron);

        }
        // GET: Patrons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patrons/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: Patrons/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Patrons/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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
                return RedirectToAction("AddCreditCard", new { id = patron.ID });
            }
            return View(patron);
        }
        public ActionResult AddCreditCard(int? id)
        {
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
            return View(card);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddCreditCard(CreditCard creditCard)
        {
            if (ModelState.IsValid)
            {              
                db.Entry(creditCard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        // GET: Patrons/Delete/5
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Models.Patron patron = db.Patrons.Find(id);
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
